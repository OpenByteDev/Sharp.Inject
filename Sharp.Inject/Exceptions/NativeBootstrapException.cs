using System;

namespace Sharp.Inject.Bootstrap {
    public class NativeBootstrapException : BootstrapException {
        public NativeBootstrapException() {
        }

        public NativeBootstrapException(string? message)
            : base(message) {
        }

        public NativeBootstrapException(string? message, Exception? innerException)
            : base(message, innerException) {
        }
    }
}
