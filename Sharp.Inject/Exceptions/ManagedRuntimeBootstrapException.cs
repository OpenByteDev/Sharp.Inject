using System;

namespace Sharp.Inject.Bootstrap {
    public class ManagedRuntimeBootstrapException : BootstrapException {
        public ManagedRuntimeBootstrapException() {
        }

        public ManagedRuntimeBootstrapException(string? message)
            : base(message) {
        }

        public ManagedRuntimeBootstrapException(string? message, Exception? innerException)
            : base(message, innerException) {
        }
    }
}
