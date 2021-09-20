using System;
using System.Diagnostics;
using System.IO;
using ReloadedInjector = Reloaded.Injector.Injector;

namespace Sharp.Inject {
    public class Injector : IDisposable {
        internal ReloadedInjector UnderlyingInjector { get; init; }
        public Process Process { get; init; }

        public Injector(Process process) {
            Process = process;
            UnderlyingInjector = new(process);
        }

        public InjectedBootstrapper InjectBootstrapper() {
            Debug.Assert(File.Exists(InjectedBootstrapper.BootstrapperModulePath));
            var moduleHandle = UnderlyingInjector.Inject(InjectedBootstrapper.BootstrapperModulePath);
            if (moduleHandle == 0) {
                throw new Exception("Injection failed");
            }
            return new InjectedBootstrapper(this);
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            UnderlyingInjector.Dispose();
        }
    }
}
