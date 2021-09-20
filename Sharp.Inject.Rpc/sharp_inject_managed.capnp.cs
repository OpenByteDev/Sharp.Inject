using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5022d822edbb081UL), Proxy(typeof(ManagedInjectorService_Proxy)), Skeleton(typeof(ManagedInjectorService_Skeleton))]
    public interface IManagedInjectorService : IDisposable
    {
        Task SetManagedExceptionInfo(CapnpGen.ManagedInjectorService.ManagedExceptionInfo exceptionInfo, CancellationToken cancellationToken_ = default);
        Task<string> GetPayloadAssemblyPath(CancellationToken cancellationToken_ = default);
        Task SetPayloadServiceProvider(CapnpGen.ManagedInjectorService.IServiceProvider provider, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5022d822edbb081UL)]
    public class ManagedInjectorService_Proxy : Proxy, IManagedInjectorService
    {
        public async Task SetManagedExceptionInfo(CapnpGen.ManagedInjectorService.ManagedExceptionInfo exceptionInfo, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.Params_SetManagedExceptionInfo.WRITER>();
            var arg_ = new CapnpGen.ManagedInjectorService.Params_SetManagedExceptionInfo()
            {ExceptionInfo = exceptionInfo};
            arg_?.serialize(in_);
            using (var d_ = await Call(16501802021794066561UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.Result_SetManagedExceptionInfo>(d_);
                return;
            }
        }

        public async Task<string> GetPayloadAssemblyPath(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.Params_GetPayloadAssemblyPath.WRITER>();
            var arg_ = new CapnpGen.ManagedInjectorService.Params_GetPayloadAssemblyPath()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16501802021794066561UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.Result_GetPayloadAssemblyPath>(d_);
                return (r_.Path);
            }
        }

        public async Task SetPayloadServiceProvider(CapnpGen.ManagedInjectorService.IServiceProvider provider, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.Params_SetPayloadServiceProvider.WRITER>();
            var arg_ = new CapnpGen.ManagedInjectorService.Params_SetPayloadServiceProvider()
            {Provider = provider};
            arg_?.serialize(in_);
            using (var d_ = await Call(16501802021794066561UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.Result_SetPayloadServiceProvider>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5022d822edbb081UL)]
    public class ManagedInjectorService_Skeleton : Skeleton<IManagedInjectorService>
    {
        public ManagedInjectorService_Skeleton()
        {
            SetMethodTable(SetManagedExceptionInfo, GetPayloadAssemblyPath, SetPayloadServiceProvider);
        }

        public override ulong InterfaceId => 16501802021794066561UL;
        async Task<AnswerOrCounterquestion> SetManagedExceptionInfo(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.Params_SetManagedExceptionInfo>(d_);
                await Impl.SetManagedExceptionInfo(in_.ExceptionInfo, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.Result_SetManagedExceptionInfo.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetPayloadAssemblyPath(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetPayloadAssemblyPath(cancellationToken_), path =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.Result_GetPayloadAssemblyPath.WRITER>();
                    var r_ = new CapnpGen.ManagedInjectorService.Result_GetPayloadAssemblyPath{Path = path};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetPayloadServiceProvider(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.Params_SetPayloadServiceProvider>(d_);
                await Impl.SetPayloadServiceProvider(in_.Provider, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.Result_SetPayloadServiceProvider.WRITER>();
                return s_;
            }
        }
    }

    public static class ManagedInjectorService
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd346794a60f14e1bUL)]
        public class ManagedExceptionInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd346794a60f14e1bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Message = reader.Message;
                TypeName = reader.TypeName;
                StackTrace = CapnpSerializable.Create<CapnpGen.Option<string>>(reader.StackTrace);
                Inner = CapnpSerializable.Create<CapnpGen.Option<CapnpGen.ManagedInjectorService.ManagedExceptionInfo>>(reader.Inner);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Message = Message;
                writer.TypeName = TypeName;
                StackTrace?.serialize(writer.StackTrace);
                Inner?.serialize(writer.Inner);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Message
            {
                get;
                set;
            }

            public string TypeName
            {
                get;
                set;
            }

            public CapnpGen.Option<string> StackTrace
            {
                get;
                set;
            }

            public CapnpGen.Option<CapnpGen.ManagedInjectorService.ManagedExceptionInfo> Inner
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Message => ctx.ReadText(0, null);
                public string TypeName => ctx.ReadText(1, null);
                public CapnpGen.Option<string>.READER StackTrace => ctx.ReadStruct(2, CapnpGen.Option<string>.READER.create);
                public CapnpGen.Option<CapnpGen.ManagedInjectorService.ManagedExceptionInfo>.READER Inner => ctx.ReadStruct(3, CapnpGen.Option<CapnpGen.ManagedInjectorService.ManagedExceptionInfo>.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 4);
                }

                public string Message
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public string TypeName
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
                }

                public CapnpGen.Option<string>.WRITER StackTrace
                {
                    get => BuildPointer<CapnpGen.Option<string>.WRITER>(2);
                    set => Link(2, value);
                }

                public CapnpGen.Option<CapnpGen.ManagedInjectorService.ManagedExceptionInfo>.WRITER Inner
                {
                    get => BuildPointer<CapnpGen.Option<CapnpGen.ManagedInjectorService.ManagedExceptionInfo>.WRITER>(3);
                    set => Link(3, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8358f59905bae706UL), Proxy(typeof(ServiceProvider_Proxy)), Skeleton(typeof(ServiceProvider_Skeleton))]
        public interface IServiceProvider : IDisposable
        {
            Task<object> GetService(ulong typeName, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8358f59905bae706UL)]
        public class ServiceProvider_Proxy : Proxy, IServiceProvider
        {
            public Task<object> GetService(ulong typeName, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.ServiceProvider.Params_GetService.WRITER>();
                var arg_ = new CapnpGen.ManagedInjectorService.ServiceProvider.Params_GetService()
                {TypeName = typeName};
                arg_?.serialize(in_);
                return Impatient.MakePipelineAware(Call(9464584654494033670UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    using (d_)
                    {
                        var r_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.ServiceProvider.Result_GetService>(d_);
                        return (r_.Service);
                    }
                }

                );
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8358f59905bae706UL)]
        public class ServiceProvider_Skeleton : Skeleton<IServiceProvider>
        {
            public ServiceProvider_Skeleton()
            {
                SetMethodTable(GetService);
            }

            public override ulong InterfaceId => 9464584654494033670UL;
            Task<AnswerOrCounterquestion> GetService(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.ServiceProvider.Params_GetService>(d_);
                    return Impatient.MaybeTailCall(Impl.GetService(in_.TypeName, cancellationToken_), service =>
                    {
                        var s_ = SerializerState.CreateForRpc<CapnpGen.ManagedInjectorService.ServiceProvider.Result_GetService.WRITER>();
                        var r_ = new CapnpGen.ManagedInjectorService.ServiceProvider.Result_GetService{Service = service};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class ServiceProvider
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd03eb64c6195e465UL)]
            public class Params_GetService : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd03eb64c6195e465UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    TypeName = reader.TypeName;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.TypeName = TypeName;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public ulong TypeName
                {
                    get;
                    set;
                }

                public struct READER
                {
                    readonly DeserializerState ctx;
                    public READER(DeserializerState ctx)
                    {
                        this.ctx = ctx;
                    }

                    public static READER create(DeserializerState ctx) => new READER(ctx);
                    public static implicit operator DeserializerState(READER reader) => reader.ctx;
                    public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                    public ulong TypeName => ctx.ReadDataULong(0UL, 0UL);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public ulong TypeName
                    {
                        get => this.ReadDataULong(0UL, 0UL);
                        set => this.WriteData(0UL, value, 0UL);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf1a2aa1d724793e2UL)]
            public class Result_GetService : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf1a2aa1d724793e2UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Service = CapnpSerializable.Create<object>(reader.Service);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Service.SetObject(Service);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public object Service
                {
                    get;
                    set;
                }

                public struct READER
                {
                    readonly DeserializerState ctx;
                    public READER(DeserializerState ctx)
                    {
                        this.ctx = ctx;
                    }

                    public static READER create(DeserializerState ctx) => new READER(ctx);
                    public static implicit operator DeserializerState(READER reader) => reader.ctx;
                    public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                    public DeserializerState Service => ctx.StructReadPointer(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public DynamicSerializerState Service
                    {
                        get => BuildPointer<DynamicSerializerState>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7d1a523c40ef2a3UL)]
        public class Params_SetManagedExceptionInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf7d1a523c40ef2a3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                ExceptionInfo = CapnpSerializable.Create<CapnpGen.ManagedInjectorService.ManagedExceptionInfo>(reader.ExceptionInfo);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                ExceptionInfo?.serialize(writer.ExceptionInfo);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.ManagedInjectorService.ManagedExceptionInfo ExceptionInfo
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.ManagedInjectorService.ManagedExceptionInfo.READER ExceptionInfo => ctx.ReadStruct(0, CapnpGen.ManagedInjectorService.ManagedExceptionInfo.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.ManagedInjectorService.ManagedExceptionInfo.WRITER ExceptionInfo
                {
                    get => BuildPointer<CapnpGen.ManagedInjectorService.ManagedExceptionInfo.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd418ead77bd6eaa6UL)]
        public class Result_SetManagedExceptionInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd418ead77bd6eaa6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x94177dc07ab646f8UL)]
        public class Params_GetPayloadAssemblyPath : ICapnpSerializable
        {
            public const UInt64 typeId = 0x94177dc07ab646f8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe7b6708f626f7078UL)]
        public class Result_GetPayloadAssemblyPath : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe7b6708f626f7078UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Path = reader.Path;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Path = Path;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Path
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Path => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Path
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd5d43fe1d7cbb930UL)]
        public class Params_SetPayloadServiceProvider : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd5d43fe1d7cbb930UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Provider = reader.Provider;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Provider = Provider;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.ManagedInjectorService.IServiceProvider Provider
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.ManagedInjectorService.IServiceProvider Provider => ctx.ReadCap<CapnpGen.ManagedInjectorService.IServiceProvider>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.ManagedInjectorService.IServiceProvider Provider
                {
                    get => ReadCap<CapnpGen.ManagedInjectorService.IServiceProvider>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa20f3fc15267d918UL)]
        public class Result_SetPayloadServiceProvider : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa20f3fc15267d918UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbe7fda4a70524f96UL)]
    public class Option<TT> : ICapnpSerializable where TT : class
    {
        public const UInt64 typeId = 0xbe7fda4a70524f96UL;
        public enum WHICH : ushort
        {
            Some = 0,
            None = 1,
            undefined = 65535
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.Some:
                    Some = CapnpSerializable.Create<TT>(reader.Some);
                    break;
                case WHICH.None:
                    which = reader.which;
                    break;
            }

            applyDefaults();
        }

        private WHICH _which = WHICH.undefined;
        private object _content;
        public WHICH which
        {
            get => _which;
            set
            {
                if (value == _which)
                    return;
                _which = value;
                switch (value)
                {
                    case WHICH.Some:
                        _content = null;
                        break;
                    case WHICH.None:
                        break;
                }
            }
        }

        public void serialize(WRITER writer)
        {
            writer.which = which;
            switch (which)
            {
                case WHICH.Some:
                    writer.Some.SetObject(Some);
                    break;
                case WHICH.None:
                    break;
            }
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TT Some
        {
            get => _which == WHICH.Some ? (TT)_content : null;
            set
            {
                _which = WHICH.Some;
                _content = value;
            }
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public WHICH which => (WHICH)ctx.ReadDataUShort(0U, (ushort)0);
            public DeserializerState Some => which == WHICH.Some ? ctx.StructReadPointer(0) : default;
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                set => this.WriteData(0U, (ushort)value, (ushort)0);
            }

            public DynamicSerializerState Some
            {
                get => which == WHICH.Some ? BuildPointer<DynamicSerializerState>(0) : default;
                set => Link(0, value);
            }
        }
    }
}