using Sharp.Inject;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Test {
    public static class Program {
        public static async Task Main() {
            var process = Process.Start(new ProcessStartInfo() {
                FileName = Path.GetFullPath("Dummy.exe"),
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal
            });
            using var injector = new Injector(process);
            using var payload = injector.Inject("Payload.dll");
            await payload.InvokeEntryPoint(
                "Payload.runtimeconfig.json",
                "Payload.dll"
            );
            
            Console.Read();
            
            payload.Eject();
            process.Kill();
        }
    }
}
