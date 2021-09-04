using System;

namespace Sharp.Inject.Bootstrap {
    public class UnreachableException : NotImplementedException {
        public UnreachableException()
            : base("This method is used for expression creation only") {
        }
    }
}
