using System;

namespace Sharp.Inject.Bootstrap {
    public class BootstrapException : Exception {
        public BootstrapException() {
        }

        public BootstrapException(string? message)
            : base(message) {
        }

        public BootstrapException(string? message, Exception? innerException)
            : base(message, innerException) {
        }
    }
}
