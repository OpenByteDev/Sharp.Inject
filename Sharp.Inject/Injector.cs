using System;
using System.Diagnostics;
using System.IO;
using ReloadedInjector = Reloaded.Injector.Injector;

namespace Sharp.Inject {
    public class Injector : IDisposable {
        internal static readonly string BootstrapperModulePath = Path.GetFullPath("Sharp.Inject.Bootstrap.Native.dll");

        private readonly ReloadedInjector underlyingInjector;
        private readonly Process process;

        public Injector(Process process) {
            this.process = process;
            this.underlyingInjector = new(process);
        }

        public InjectedModule Inject(string modulePath) {
            modulePath = Path.GetFullPath(modulePath);
            var moduleHandle = this.underlyingInjector.Inject(BootstrapperModulePath);
            if (moduleHandle == 0) {
                throw new Exception("Injection failed");
            }
            return new InjectedModule(modulePath, this.underlyingInjector);
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            this.underlyingInjector.Dispose();
        }
    }
}
