use netcorehost::{
    cast_managed_fn,
    nethost, pdcstr,
    pdcstring::PdCString,
};
use std::{convert::TryInto, net::{Ipv4Addr, SocketAddrV4}, panic::{self, AssertUnwindSafe}, str::FromStr};
use capnp_rpc::{RpcSystem, rpc_twoparty_capnp, twoparty};
use futures::FutureExt;
use futures::AsyncReadExt;

#[derive(Debug)]
#[repr(i32)]
enum BootstrapResult {
    Success = 0,
    BootstrapError = 1,
    FatalError = 3,
}

pub mod sharp_inject_native_capnp {
    include!(concat!(env!("OUT_DIR"), "/sharp_inject_native_capnp.rs"));
}

#[no_mangle]
extern "system" fn bootstrap(port: i64) -> BootstrapResult {
    match panic::catch_unwind(|| bootstrap_real(port.try_into().unwrap())).unwrap_or_else(|e| Err(BootstrapError::Panic(e))) {
        Ok(_) => BootstrapResult::Success,
        Err(BootstrapError::Capnp(_)) => {
            BootstrapResult::FatalError
        }
        Err(_) => {
            BootstrapResult::BootstrapError
        }
    }
}

#[tokio::main]
async fn bootstrap_real(port: u16) -> Result<(), BootstrapError> {
    tokio::task::LocalSet::new().run_until(async move {
        let address = SocketAddrV4::new(Ipv4Addr::LOCALHOST, port);
        let stream = tokio::net::TcpStream::connect(address).await.unwrap();
        stream.set_nodelay(true).unwrap();

        let (reader, writer) = tokio_util::compat::TokioAsyncReadCompatExt::compat(stream).split();
        let rpc_network = Box::new(twoparty::VatNetwork::new(
            reader,
            writer,
            rpc_twoparty_capnp::Side::Client,
            Default::default(),
        ));
        let mut rpc_system = RpcSystem::new(rpc_network, None);
        let client: sharp_inject_native_capnp::native_injector_service::Client =
            rpc_system.bootstrap(rpc_twoparty_capnp::Side::Server);
        tokio::task::spawn_local(Box::pin(rpc_system.map(|_| ())));

        let result = AssertUnwindSafe(async {
            let request = client.get_managed_payload_info_request();
            let response = request.send().promise.await?;

            let payload_info = response.get()?.get_info()?;
            
            host_managed_assembly(
                port,
                payload_info.get_runtime_config_path()?,
                payload_info.get_assembly_path()?
            ).map(|_| ())
        }).catch_unwind().await.unwrap_or_else(|e| Err(BootstrapError::Panic(e)));

        if let Err(error) = &result {
            let mut request = client.set_native_bootstrap_error_request();
            match error {
                BootstrapError::Hosting(hosting_error) => request.get().get_error_info()?.set_hosting(hosting_error.to_string().as_str()),
                BootstrapError::Capnp(capnp_error) => request.get().get_error_info()?.set_other(capnp_error.to_string().as_str()),
                BootstrapError::Other(other_error) => request.get().get_error_info()?.set_other(other_error.to_string().as_str()),
                BootstrapError::Panic(panic_error) => request.get().get_error_info()?.set_panic(panic_error.downcast_ref::<&str>().unwrap_or(&"Unknown fatal error.")),
            }
            request.send().promise.await?;
        }

        result
    }).await
}


fn host_managed_assembly<TArg>(port: TArg, runtime_config_path: &str, assembly_path: &str) -> Result<i32, BootstrapError> {
    // locate and load hostfxr
    let hostfxr = nethost::load_hostfxr()?;

    // setup context
    let context = hostfxr
        .initialize_for_runtime_config(PdCString::from_str(runtime_config_path)?)?;

    // load managed entry point
    let loader = context
        .get_delegate_loader_for_assembly(PdCString::from_str(assembly_path)?)?;
    let managed_entry_point = loader
        .get_function_pointer_for_unmanaged_callers_only_method(
            pdcstr!("Sharp.Inject.Bootstrap.Bootstrapper, Sharp.Inject.Bootstrap"),
            pdcstr!("ManagedEntryPoint"),
        )?;
    let managed_entry_point =
        unsafe { cast_managed_fn!(managed_entry_point, fn(TArg) -> i32) };

    // call entry point
    let result = managed_entry_point(port);

    Ok(result)
}

#[derive(Debug)]
enum BootstrapError {
    Hosting(netcorehost::error::Error),
    Capnp(capnp::Error),
    Other(Box<dyn std::error::Error>),
    Panic(Box<dyn std::any::Any + std::marker::Send>),
}

impl From<netcorehost::error::Error> for BootstrapError {
    fn from(error: netcorehost::error::Error) -> Self {
        BootstrapError::Hosting(error)
    }
}

impl From<netcorehost::pdcstring::NulError> for BootstrapError {
    fn from(error: netcorehost::pdcstring::NulError) -> Self {
        BootstrapError::Other(error.into())
    }
}

impl From<capnp::Error> for BootstrapError {
    fn from(error: capnp::Error) -> Self {
        BootstrapError::Capnp(error)
    }
}

impl From<netcorehost::hostfxr::GetFunctionPointerError> for BootstrapError {
    fn from(error: netcorehost::hostfxr::GetFunctionPointerError) -> Self {
        Self::from(netcorehost::error::Error::from(error))
    }
}

impl From<netcorehost::error::HostingError> for BootstrapError {
    fn from(error: netcorehost::error::HostingError) -> Self {
        Self::from(netcorehost::error::Error::from(error))
    }
}

impl From<netcorehost::nethost::LoadHostfxrError> for BootstrapError {
    fn from(error: netcorehost::nethost::LoadHostfxrError) -> Self {
        Self::from(netcorehost::error::Error::from(error))
    }
}
