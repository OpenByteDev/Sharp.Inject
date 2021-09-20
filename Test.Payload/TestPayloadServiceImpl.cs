using CapnpGen;
using Sharp.Inject;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Payload {
    [PayloadService]
    public sealed class TestPayloadServiceImpl : ITestPayloadService {
        public Task<string> Echo(string message, CancellationToken cancellationToken = default) {
            return Task.FromResult(message);
        }

        public Task Log(string message, CancellationToken cancellationToken = default) {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }

        public void Dispose() {}
    }
}
