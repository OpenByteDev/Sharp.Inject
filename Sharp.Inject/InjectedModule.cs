using Capnp.Rpc;
using CapnpGen;
using Sharp.Inject.Bootstrap;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Sharp.Inject {
    public class InjectedModule : IDisposable {
        private readonly string _ModulePath;
        private readonly Reloaded.Injector.Injector _UnderlyingInjector;
        private readonly TcpRpcServer _RpcServer;
        private InjectorServiceImpl? _RpcService;

        internal InjectedModule(string modulePath, Reloaded.Injector.Injector underlyingInjector) {
            _ModulePath = modulePath;
            _UnderlyingInjector = underlyingInjector;
            _RpcServer = new TcpRpcServer();
            _RpcServer.AddBuffering(1024);
        }

        public Task<TService>? InvokeEntryPoint<TService>(string runtimeConfigPath, string assemblyPath) {
            var tcpPort = GetAvailablePort(42898)!.Value;

            Debug.Assert(_RpcService == null);
            _RpcService = new InjectorServiceImpl(runtimeConfigPath, assemblyPath);

            _RpcServer.Main = _RpcService;
            _RpcServer.StartAccepting(IPAddress.Loopback, tcpPort);

            var result = _UnderlyingInjector.CallFunction(Injector.BootstrapperModulePath, "bootstrap", (long)tcpPort);
            Console.WriteLine(result);

            return _RpcService.RemoteServiceTaskSource.Task as Task<TService>;
        }

        private static int? GetAvailablePort(int startingPort) {
            var properties = IPGlobalProperties.GetIPGlobalProperties();

            // Ignore active connections
            var portsFromActiveConnections = properties.GetActiveTcpConnections()
                .Select(c => c.LocalEndPoint.Port);

            // Ignore active tcp listners
            var portsFromActiveListeners = properties.GetActiveTcpListeners()
                .Select(l => l.Port);

            var usedCandidatePorts = portsFromActiveConnections.Concat(portsFromActiveListeners).Where(p => startingPort >= p);

            return Enumerable.Range(startingPort, ushort.MaxValue - startingPort)
                .Except(usedCandidatePorts.OrderBy(e => e))
                .Select(p => (int?)p)
                .FirstOrDefault();
        }

        public void Eject() {
            _UnderlyingInjector.Eject(_ModulePath);
        }

        #region IDisposable
        private bool isDisposed;

        protected virtual void Dispose(bool disposing) {
            if (!isDisposed) {
                if (disposing) {
                    _RpcServer.Dispose();
                    _RpcService?.Dispose();
                    Eject();
                }

                isDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


        private sealed class InjectorServiceImpl : IInjectorService {
            public string RuntimeConfigPath;
            public string AssemblyPath;
            public TaskCompletionSource<object> RemoteServiceTaskSource = new();

            public InjectorServiceImpl(string runtimeConfigPath, string assemblyPath) {
                RuntimeConfigPath = runtimeConfigPath;
                AssemblyPath = assemblyPath;
            }

            public Task<InjectorService.ManagedPayloadInfo> GetManagedPayloadInfo(CancellationToken cancellationToken = default) {
                var payloadInfo = new InjectorService.ManagedPayloadInfo();
                payloadInfo.RuntimeConfigPath = RuntimeConfigPath;
                payloadInfo.AssemblyPath = AssemblyPath;
                return Task.FromResult(payloadInfo);
            }

            public Task NotifyError(InjectorService.ErrorInfo error, CancellationToken cancellationToken = default) {
                Exception? exception = error.which switch {
                    InjectorService.ErrorInfo.WHICH.Native => error.Native.which switch {
                        InjectorService.ErrorInfo.native.WHICH.Hosting => new ManagedRuntimeBootstrapException(error.Native.Hosting),
                        InjectorService.ErrorInfo.native.WHICH.Panic => new NativeBootstrapException(error.Native.Panic),
                        InjectorService.ErrorInfo.native.WHICH.Other => new BootstrapException(error.Native.Other),
                        _ => null
                    },
                    InjectorService.ErrorInfo.WHICH.Managed => new PayloadException("An exception occured inside the payload entry point.", TryReconstructException(error?.Managed)),
                    _ => null
                };

                RemoteServiceTaskSource.TrySetException(exception ?? new BootstrapException("An unknown error occured while injecting the payload or executing its entry point."));

                return Task.CompletedTask;
            }

            private static Exception? TryReconstructException([NotNullIfNotNull("exceptionInfo")] InjectorService.ManagedExceptionInfo? exceptionInfo) {
                if (exceptionInfo is null) {
                    return null;
                }

                var exceptionType = Type.GetType(exceptionInfo.TypeName);
                var innerException = TryReconstructException(exceptionInfo.Inner?.Some);
                Exception? exceptionInstance = null;
                if (exceptionType is not null) {
                    try {
                        exceptionInstance = Activator.CreateInstance(exceptionType, exceptionInfo.Message, innerException) as Exception;
                    } catch (Exception) { }
                }
                if (exceptionInstance is null) {
                    exceptionInstance = new InnerPayloadException(exceptionInfo.Message, innerException);
                }

                return exceptionInstance;
            }

            public Task PutPayloadService(IPayloadService service, CancellationToken cancellationToken = default) {
                RemoteServiceTaskSource.TrySetResult(service);
                return Task.CompletedTask;
            }

            #region IDisposable
            private bool isDisposed;

            private void Dispose(bool disposing) {
                if (!isDisposed) {
                    if (disposing) {
                        // Nothing to do (IInjectorService requires IDisposable)
                    }

                    isDisposed = true;
                }
            }

            public void Dispose() {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
            #endregion IDisposable
        }

    }
}
