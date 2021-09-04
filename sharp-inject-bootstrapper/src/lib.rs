use netcorehost::{
    cast_managed_fn,
    error::Error,
    nethost, pdcstr,
    pdcstring::{PdCStr, PdCString},
};
use std::{cmp, ffi::c_void, panic::{self, AssertUnwindSafe}, slice, str::FromStr};
use widestring::U16CStr;

#[derive(Debug)]
#[repr(i32)]
enum BootstrapResult {
    Success = 0,
    BootstrapError = 1,
    ManagedEntryPointException = 2,
    FatalError = 3,
}

#[no_mangle]
extern "system" fn bootstrap(args_ptr: *mut BootstrapArgs) -> BootstrapResult {
    let args = unsafe { &mut *args_ptr };

    match panic::catch_unwind(AssertUnwindSafe(|| bootstrap_real(args))) {
        Ok(Ok(_)) => BootstrapResult::Success,
        Ok(Err(e)) => {
            match e {
                BootstrapError::Hosting(e) => {
                    write_error(args, e.to_string().as_str());
                    BootstrapResult::BootstrapError
                }
                BootstrapError::EntryPointException => BootstrapResult::ManagedEntryPointException,
                BootstrapError::InvalidAssemblyName => {
                    write_error(args, "Invalid assembly name specified.");
                    BootstrapResult::BootstrapError
                }
            }
        }
        Err(e) => {
            if let Some(message) = e.downcast_ref::<&str>() {
                write_error(args, message);
            } else {
                write_error(args, "Unknown error.");
            }
            BootstrapResult::FatalError
        }
    }
}

fn write_error(args: &mut BootstrapArgs, error: &str) {
    let error_buffer = args as *mut _ as *mut c_void;
    let size_ptr = error_buffer.cast::<usize>();
    let data_ptr = unsafe { size_ptr.offset(1).cast::<u16>() };
    let data = unsafe { slice::from_raw_parts_mut(data_ptr, args.reserved_bytes) };
    let wide_error = PdCString::from_str(error).unwrap().into_vec();
    let len = cmp::min(data.len(), wide_error.len()); 
    data[..len].copy_from_slice(&wide_error[..len]);
    unsafe { *size_ptr = len };
}

fn bootstrap_real(args: &mut BootstrapArgs) -> Result<i32, BootstrapError> {
    // convert arguments
    let reserved_bytes = args.reserved_bytes;
    let module_path = unsafe { args.module_path.as_pdcstr() };
    let runtime_config_path = unsafe { args.runtime_config_path.as_pdcstr() };
    let assembly_name = unsafe { args.assembly_name.as_pdcstr() };
    let user_argument = args.user_argument;
    let user_argument_size = args.user_argument_size;

    // use netcorehost to load module and get entry point.
    let hostfxr = nethost::load_hostfxr().map_err(|e| BootstrapError::Hosting(e.into()))?;
    let context = hostfxr
        .initialize_for_runtime_config(runtime_config_path)
        .map_err(|e| BootstrapError::Hosting(e.into()))?;
    let loader = context
        .get_delegate_loader_for_assembly(module_path)
        .map_err(|e| BootstrapError::Hosting(e.into()))?;
    let managed_entry_point = loader
        .get_function_pointer_for_unmanaged_callers_only_method(
            PdCString::from_str(
                format!(
                    "Sharp.Inject.Bootstrap.Bootstrapper, {}",
                    assembly_name
                        .to_string()
                        .map_err(|_| BootstrapError::InvalidAssemblyName)?
                )
                .as_str(),
            )
            .map_err(|_| BootstrapError::InvalidAssemblyName)?,
            pdcstr!("ManagedEntryPoint"),
        )
        .map_err(|e| BootstrapError::Hosting(e.into()))?;
    let managed_entry_point =
        unsafe { cast_managed_fn!(managed_entry_point, fn(*const ManagedBootstrapArgs) -> i32) };

    // call entry point
    let managed_args = unsafe {
        ManagedBootstrapArgs {
            user_argument,
            user_argument_size,
            error_message_buffer: (args as *mut BootstrapArgs)
                .cast::<usize>()
                .offset(1)
                .cast(), // we reuse the argument buffer for the error message.
            error_message_len: (args as *mut BootstrapArgs).cast::<usize>(),
            error_message_buffer_size: reserved_bytes,
        }
    };

    let result = managed_entry_point(&managed_args);

    if unsafe { *managed_args.error_message_len } != reserved_bytes {
        return Err(BootstrapError::EntryPointException);
    }

    Ok(result)
}

#[derive(Debug)]
#[repr(C)]
struct BootstrapArgs {
    reserved_bytes: usize,
    module_path: LengthPrefixedString,
    runtime_config_path: LengthPrefixedString,
    assembly_name: LengthPrefixedString,
    user_argument_size: usize,
    user_argument: *const c_void,
}

#[derive(Debug)]
#[repr(C)]
struct LengthPrefixedString {
    length: usize,
    chars: *const u16,
}

impl LengthPrefixedString {
    pub unsafe fn as_u16cstr(&self) -> &U16CStr {
        U16CStr::from_ptr_with_nul(self.chars, self.length)
    }
    pub unsafe fn as_pdcstr(&self) -> &PdCStr {
        let s = self.as_u16cstr();
        if cfg!(windows) {
            PdCStr::from_u16_c_str(s)
        } else {
            todo!()
        }
    }
}

#[derive(Debug)]
#[repr(C)]
struct ManagedBootstrapArgs {
    user_argument: *const c_void,
    user_argument_size: usize,
    error_message_buffer: *mut u16,
    error_message_len: *mut usize,
    error_message_buffer_size: usize,
}

#[derive(Debug)]
enum BootstrapError {
    Hosting(Error),
    EntryPointException,
    InvalidAssemblyName,
}
