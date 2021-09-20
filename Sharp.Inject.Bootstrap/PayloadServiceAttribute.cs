using System;

namespace Sharp.Inject {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PayloadServiceAttribute: Attribute {
    }
}
