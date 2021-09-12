using Capnp.Rpc;
using CapnpGen;
using System;
using System.Runtime.InteropServices;
using Exception = System.Exception;

namespace Sharp.Inject.Bootstrap {
    public static class Bootstrapper {
        [UnmanagedCallersOnly]
        public static void ManagedEntryPoint(int port) {
            try {
                using var client = new TcpRpcClient();
                client.AddBuffering(1024);
                client.Connect("localhost", port);
                using var injectorService = client.GetMain<IInjectorService>();
                try {
                    // object payloadServiceType = Type
                    // injectorService.PutPayloadService();
                } catch (Exception e) {
                    var errorInfo = new InjectorService.ErrorInfo {
                        Managed = SerializeException(e),
                        which = InjectorService.ErrorInfo.WHICH.Managed
                    };
                    injectorService.NotifyError(errorInfo);
                }
            } catch (Exception e) {
                // TODO:
                Console.WriteLine(e.Message);
            }
        }

        private static InjectorService.ManagedExceptionInfo SerializeException(Exception e) {
            var info = new InjectorService.ManagedExceptionInfo();
            info.Message = e.Message;
            info.TypeName = e.GetType().AssemblyQualifiedName ?? e.GetType().FullName ?? e.GetType().Name;

            info.StackTrace = new();
            if (e.StackTrace is not null) {
                info.StackTrace.Some = e.StackTrace;
            } else {
                info.StackTrace.which = Option<string>.WHICH.None;
            }

            info.Inner = new();
            if (e.InnerException is not null) {
                info.Inner.Some = SerializeException(e.InnerException);
            } else {
                info.Inner.which = Option<InjectorService.ManagedExceptionInfo>.WHICH.None;
            }

            return info;
        }
    }
}
