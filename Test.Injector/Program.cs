using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Test.Injector {
    public static class Program {
        public static async Task Main() {
            using var process = Process.Start(new ProcessStartInfo() {
                FileName = Path.GetFullPath("Test.ManagedDummy.exe"),
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal
            });
            using var injector = new Sharp.Inject.Injector(process);
            using var bootstrapper = injector.InjectBootstrapper();
            await bootstrapper.Bootstrap(
                "Test.Payload.runtimeconfig.json",
                "Test.Payload.dll"
            );
            using var service = await bootstrapper.GetService<CapnpGen.ITestPayloadService>();
            await service.Log("Test1");
            Console.WriteLine(await service.Echo("Test2"));
            
            Console.Read();
            
            bootstrapper.Eject();
            process.Kill();
        }

    }
}
