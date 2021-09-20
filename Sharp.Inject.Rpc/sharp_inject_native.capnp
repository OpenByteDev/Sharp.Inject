@0x9a9f58e1905ee420;

interface NativeInjectorService {
    setNativeBootstrapError @0 (errorInfo :NativeErrorInfo);
    getManagedPayloadInfo @1 () -> (info :ManagedPayloadInfo);

    struct NativeErrorInfo {
        union {
            hosting @0 :Text;
            panic @1 :Text;
            other @2 :Text;
        }
    }

    struct ManagedPayloadInfo {
        assemblyPath @0 :Text;
        runtimeConfigPath @1 :Text;
    }
}
