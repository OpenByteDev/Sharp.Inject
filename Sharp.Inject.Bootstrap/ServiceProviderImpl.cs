using Capnp;
using CapnpGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp.Inject.Bootstrap {
    internal sealed class ServiceProviderImpl : ManagedInjectorService.IServiceProvider {
        private Dictionary<ulong, object> Services { get; } = new();
        private Dictionary<ulong, Type> ServiceTypes { get; } = new();
        private readonly object _serviceLock = new();

        public void ScanAssemblies(IEnumerable<Assembly> assemblies) {
            var frameworkToken = typeof(object).Assembly.GetName().GetPublicKeyToken();

            foreach (var (type, id) in assemblies
                .Where(assembly => !StructuralComparisons.StructuralEqualityComparer.Equals(assembly.GetName().GetPublicKeyToken(), frameworkToken))
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    Attribute.IsDefined(type, typeof(PayloadServiceAttribute))
                )
                .SelectMany(type => type.GetInterfaces().Select(i => (Type: type, Id: i.GetCustomAttribute<TypeIdAttribute>()?.Id)))) {
                if (id is not null) {
                    ServiceTypes.Add(id.Value, type);
                }
            }
        }

        public Task<object> GetService(ulong serviceId, CancellationToken cancellationToken = default) {
            if (Services.TryGetValue(serviceId, out var service)) {
                return Task.FromResult(service);
            }

            lock (_serviceLock) {
                if (Services.TryGetValue(serviceId, out service)) {
                    return Task.FromResult(service);
                }

                var serviceImplType = ServiceTypes.GetValueOrDefault(serviceId);
                var serviceImplInstance = Activator.CreateInstance(serviceImplType);
                Services.Add(serviceId, serviceImplInstance);
                return Task.FromResult(serviceImplInstance);
            }
        }

        public void Dispose() {}
    }
}