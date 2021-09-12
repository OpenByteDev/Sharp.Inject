using System;

namespace Sharp.Inject {
    public class InnerPayloadException : PayloadException {

        public string? OriginalTypeName { get; }
        public string? OriginalStackTrace { get; }

        public InnerPayloadException() : base() { }
        public InnerPayloadException(string? message) : base(message) { }
        public InnerPayloadException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
