using Sharp.Inject;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Test {
    class Program {
        static unsafe void Main(string[] args) {
            var process = Process.Start(new ProcessStartInfo() {
                FileName = Path.GetFullPath("Dummy.exe"),
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal
            });
            using var injector = new Injector(process);
            var payload = injector.Inject("../../../../Payload/bin/Debug/net6.0/Payload.dll");
            payload.InvokeEntryPoint(
                string.Join(' ', Enumerable.Repeat("Test", 1000)),
                "../../../../Payload/bin/Debug/net6.0/Payload.runtimeconfig.json"
            );
            
            Console.Read();
            
            payload.Eject();
            process.Kill();
        }
    }
}
