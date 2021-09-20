using Capnp.Rpc;
using CapnpGen;
using Sharp.Inject.Bootstrap;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Sharp.Inject {
    public class InjectedBootstrapper : IDisposable {
        internal static readonly string BootstrapperModulePath = Path.GetFullPath("Sharp.Inject.Bootstrap.Native.dll");
        private readonly Injector _Injector;
        private readonly TcpRpcServer _TcpRpcServer = new();
        private ManagedInjectorService.IServiceProvider? _ServiceProvider;

        internal InjectedBootstrapper(Injector injector) {
            _Injector = injector;
        }

        public async Task Bootstrap(string runtimeConfigPath, string assemblyPath) {
            if (_ServiceProvider is not null || _TcpRpcServer.IsAlive)
                throw new InvalidOperationException("The payload has already been bootstrapped.");

            runtimeConfigPath = Path.GetFullPath(runtimeConfigPath);
            assemblyPath = Path.GetFullPath(assemblyPath);

            var tcpPort = GetAvailablePort(42898)!.Value;

            using var rpcService = new InjectorServiceImpl(runtimeConfigPath, assemblyPath);

            _TcpRpcServer.Main = rpcService;
            _TcpRpcServer.StartAccepting(IPAddress.Loopback, tcpPort);

            _ = Task.Run(() => {
                var result = _Injector.UnderlyingInjector.CallFunction(BootstrapperModulePath, "bootstrap", (long)tcpPort);
                Console.WriteLine(result);
            });

            _ServiceProvider = await rpcService.RemoteServiceProviderTaskSource.Task;
        }

        public async Task<TService> GetService<TService>() where TService : class {
            return await TryGetService<TService>().ConfigureAwait(false)
                ?? throw new NotSupportedException($"The given {nameof(TService)}={typeof(TService).FullName} is not supported by the payload.");
        }

        public async Task<TService?> TryGetService<TService>() where TService : class {
            var typeId = typeof(TService).GetCustomAttribute<Capnp.TypeIdAttribute>()?.Id;
            if (typeId is null)
                throw new ArgumentException(nameof(TService), $"{nameof(TService)} is not a Capn'n Proto interface.");

            var bareService = await _ServiceProvider.GetService(typeId.Value).ConfigureAwait(false) as BareProxy;
            return bareService?.Cast<TService>(false);
        }

        public async Task<BareProxy> GetService(ulong serviceInterfaceId) {
            return await TryGetService(serviceInterfaceId).ConfigureAwait(false) ??
                throw new NotSupportedException($"The service interface with id 0x{serviceInterfaceId:X} is not supported by the payload.");
        }

        public async Task<BareProxy?> TryGetService(ulong serviceInterfaceId) {
            if (_ServiceProvider is null)
                throw new InvalidOperationException("The payload has not yet been bootstrapped.");

            return await _ServiceProvider.GetService(serviceInterfaceId).ConfigureAwait(false) as BareProxy;
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
            _Injector.UnderlyingInjector.Eject(BootstrapperModulePath);
        }

        #region IDisposable
        private bool isDisposed;

        protected virtual void Dispose(bool disposing) {
            if (!isDisposed) {
                if (disposing) {
                    _ServiceProvider?.Dispose();
                    _TcpRpcServer.Dispose();
                }
                Eject();

                isDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


        private sealed class InjectorServiceImpl : IManagedInjectorService, INativeInjectorService {
            public string RuntimeConfigPath;
            public string AssemblyPath;
            public TaskCompletionSource<ManagedInjectorService.IServiceProvider> RemoteServiceProviderTaskSource = new();

            public InjectorServiceImpl(string runtimeConfigPath, string assemblyPath) {
                RuntimeConfigPath = runtimeConfigPath;
                AssemblyPath = assemblyPath;
            }

            Task<NativeInjectorService.ManagedPayloadInfo> INativeInjectorService.GetManagedPayloadInfo(CancellationToken cancellationToken) {
                return Task.FromResult(new NativeInjectorService.ManagedPayloadInfo() {
                    RuntimeConfigPath = RuntimeConfigPath,
                    AssemblyPath = AssemblyPath,
                });
            }

            public Task<string> GetPayloadAssemblyPath(CancellationToken cancellationToken = default) {
                return Task.FromResult(AssemblyPath);
            }

            public Task SetPayloadServiceProvider(ManagedInjectorService.IServiceProvider provider, CancellationToken cancellationToken = default) {
                RemoteServiceProviderTaskSource.TrySetResult(provider);
                return Task.CompletedTask;
            }

            public Task SetManagedExceptionInfo(ManagedInjectorService.ManagedExceptionInfo exceptionInfo, CancellationToken cancellationToken = default) {
                RemoteServiceProviderTaskSource.TrySetException(new PayloadException("An exception occured inside the payload entry point.", TryReconstructException(exceptionInfo)));
                return Task.CompletedTask;
            }

            private static Exception? TryReconstructException([NotNullIfNotNull("exceptionInfo")] ManagedInjectorService.ManagedExceptionInfo? exceptionInfo) {
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

            public Task SetNativeBootstrapError(NativeInjectorService.NativeErrorInfo errorInfo, CancellationToken cancellationToken = default) {
                RemoteServiceProviderTaskSource.TrySetException(errorInfo.which switch {
                    NativeInjectorService.NativeErrorInfo.WHICH.Hosting => new ManagedRuntimeBootstrapException(errorInfo.Hosting),
                    NativeInjectorService.NativeErrorInfo.WHICH.Panic => new NativeBootstrapException(errorInfo.Panic),
                    NativeInjectorService.NativeErrorInfo.WHICH.Other => new BootstrapException(errorInfo.Other),
                    _ => new BootstrapException("An unknown error occured while injecting the payload or bootstrapping the runtime.")
                });
                return Task.CompletedTask;
            }

            #region IDisposable
            private bool isDisposed;

            private void Dispose(bool disposing) {
                if (!isDisposed) {
                    if (disposing) {
                        RemoteServiceProviderTaskSource.TrySetCanceled();
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
