use netcorehost::{
    cast_managed_fn,
    nethost, pdcstr,
    pdcstring::PdCString,
};
use std::{convert::TryInto, net::{Ipv4Addr, SocketAddrV4}, panic::{self, AssertUnwindSafe}, str::FromStr};
use capnp_rpc::{rpc_twoparty_capnp, twoparty, RpcSystem};
use futures::FutureExt;
use futures::AsyncReadExt;

#[derive(Debug)]
#[repr(i32)]
enum BootstrapResult {
    Success = 0,
    BootstrapError = 1,
    FatalError = 3,
}

pub mod sharp_inject_capnp {
    include!(concat!(env!("OUT_DIR"), "/sharp_inject_capnp.rs"));
}

#[no_mangle]
extern "system" fn bootstrap(port: i64) -> BootstrapResult {
    match panic::catch_unwind(|| bootstrap_real(port.try_into().unwrap())) {
        Ok(Ok(Ok(_))) => BootstrapResult::Success,
        Ok(Ok(Err(_))) => {
            BootstrapResult::BootstrapError
        },
        Ok(Err(_)) | Err(_) => {
            BootstrapResult::FatalError
        }
    }
}

#[tokio::main]
async fn bootstrap_real(port: u16) -> Result<Result<(), Box<dyn std::error::Error>>, Box<dyn std::any::Any + std::marker::Send>> {
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
        let client: sharp_inject_capnp::injector_service::Client =
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
        }).catch_unwind().await;

        match &result {
            Ok(Ok(_)) => {},
            Ok(Err(err)) => {
                let request = client.notify_error_request();
                let error_info = request.get().get_error()?.init_native();
                if err.downcast_ref::<netcorehost::error::Error>()
                let _response = request.send().promise.await.unwrap();
            },
            Err(panic) => {

            }
        }

        result
    }).await
}


fn host_managed_assembly(port: u16, runtime_config_path: &str, assembly_path: &str) -> Result<i32, Box<dyn std::error::Error>> {
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
        unsafe { cast_managed_fn!(managed_entry_point, fn(i32) -> i32) };


    panic!("Test panic");
    // call entry point
    let result = managed_entry_point(port as i32);

    Ok(result)
}

