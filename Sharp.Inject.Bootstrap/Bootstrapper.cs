using Capnp.Rpc;
using CapnpGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Sharp.Inject.Bootstrap {
    public sealed class Bootstrapper : IDisposable {
        internal TcpRpcClient RpcClient { get; }
        internal ServiceProviderImpl ServiceProvider { get; }

        public Bootstrapper() {
            RpcClient = new();
            ServiceProvider = new();
        }

        private static Lazy<Bootstrapper> _instance = new(() => new());
        public static Bootstrapper Instance => _instance.Value;

        [UnmanagedCallersOnly]
        public static void ManagedEntryPoint(int port) {
            try {
                Instance.Setup(port).GetAwaiter().GetResult();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private async Task Setup(int port) {
            RpcClient.Connect("localhost", port);
            using var injectorService = RpcClient.GetMain<IManagedInjectorService>();

            try {
                var payloadAssemblyPath = await injectorService.GetPayloadAssemblyPath();
                var loadedAssemblies = LoadAssemblyWithDependencies(payloadAssemblyPath);
                ServiceProvider.ScanAssemblies(loadedAssemblies);
                await injectorService.SetPayloadServiceProvider(ServiceProvider);
            } catch (Exception e) {
                await injectorService.SetManagedExceptionInfo(SerializeException(e));
            }
        }

        private static List<Assembly> LoadAssemblyWithDependencies(string assemblyPath) {
            var allLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(e => e.Location);
            var dependencyResolver = new AssemblyDependencyResolver(assemblyPath);
            var assembliesToLoad = new HashSet<string>() { assemblyPath };
            var loadedAssemblies = new List<Assembly>();

            while (assembliesToLoad.Count > 0) {
                assemblyPath = assembliesToLoad.First();
                assembliesToLoad.Remove(assemblyPath);

                var assembly = LoadAssemblyByPath(assemblyPath);
                loadedAssemblies.Add(assembly);

                foreach (var dependencyName in assembly.GetReferencedAssemblies()) {
                    var dependencyPath = dependencyResolver.ResolveAssemblyToPath(dependencyName);
                    if (dependencyPath == null)
                        continue;

                    if (allLoadedAssemblies.TryGetValue(dependencyPath, out var dependencyAssembly))
                        loadedAssemblies.Add(dependencyAssembly);
                    else assembliesToLoad.Add(dependencyPath);
                }
            }

            return loadedAssemblies;
        }

        private static Assembly LoadAssemblyByPath(string assemblyPath) {
            var assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(assemblyPath)) {
                CodeBase = Path.GetDirectoryName(assemblyPath)
            };
            return Assembly.Load(assemblyName);
        }

        private static ManagedInjectorService.ManagedExceptionInfo SerializeException(Exception e) {
            var info = new ManagedInjectorService.ManagedExceptionInfo();
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
                info.Inner.which = Option<ManagedInjectorService.ManagedExceptionInfo>.WHICH.None;
            }

            return info;
        }

        public void Dispose() {
            ServiceProvider.Dispose();
            RpcClient.Dispose();
        }
    }
}
