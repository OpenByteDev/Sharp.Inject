@0x9a9f58e1905ee420;

interface InjectorService {
    notifyError @0 (error :ErrorInfo);
    getManagedPayloadInfo @1 () -> (info :ManagedPayloadInfo);
    putPayloadService @2 (service: PayloadService);

    struct ErrorInfo {
        union {
            native :union {
                hosting @0 :Text;
                panic @1 :Text;
                other @2 :Text;
            }
            managed @3 :ManagedExceptionInfo;
        }
    }

    struct ManagedExceptionInfo {
        message @0 :Text;
        typeName @1 :Text;
        stackTrace @2 :Option(Text);
        inner @3 :Option(ManagedExceptionInfo);
    }

    struct ManagedPayloadInfo {
        assemblyPath @0 :Text;
        runtimeConfigPath @1 :Text;
    }
}

interface PayloadService {
}

struct Option(T) {
    union {
        some @0 :T;
        none @1 :Void;
    }
}
