@0x89ff0f235d41ac2b;

interface ManagedInjectorService {
    setManagedExceptionInfo @0 (exceptionInfo :ManagedExceptionInfo);
    getPayloadAssemblyPath @1 () -> (path :Text);
    setPayloadServiceProvider @2 (provider :ServiceProvider);

    struct ManagedExceptionInfo {
        message @0 :Text;
        typeName @1 :Text;
        stackTrace @2 :Option(Text);
        inner @3 :Option(ManagedExceptionInfo);
    }

    interface ServiceProvider {
        getService @0 (typeName :UInt64) -> (service :AnyPointer);
    }
}

struct Option(T) {
    union {
        some @0 :T;
        none @1 :Void;
    }
}
