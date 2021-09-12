using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf939db62c09c894fUL), Proxy(typeof(InjectorService_Proxy)), Skeleton(typeof(InjectorService_Skeleton))]
    public interface IInjectorService : IDisposable
    {
        Task NotifyError(CapnpGen.InjectorService.ErrorInfo error, CancellationToken cancellationToken_ = default);
        Task<CapnpGen.InjectorService.ManagedPayloadInfo> GetManagedPayloadInfo(CancellationToken cancellationToken_ = default);
        Task PutPayloadService(CapnpGen.IPayloadService service, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf939db62c09c894fUL)]
    public class InjectorService_Proxy : Proxy, IInjectorService
    {
        public async Task NotifyError(CapnpGen.InjectorService.ErrorInfo error, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.InjectorService.Params_NotifyError.WRITER>();
            var arg_ = new CapnpGen.InjectorService.Params_NotifyError()
            {Error = error};
            arg_?.serialize(in_);
            using (var d_ = await Call(17958626206301325647UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.InjectorService.Result_NotifyError>(d_);
                return;
            }
        }

        public async Task<CapnpGen.InjectorService.ManagedPayloadInfo> GetManagedPayloadInfo(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.InjectorService.Params_GetManagedPayloadInfo.WRITER>();
            var arg_ = new CapnpGen.InjectorService.Params_GetManagedPayloadInfo()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17958626206301325647UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.InjectorService.Result_GetManagedPayloadInfo>(d_);
                return (r_.Info);
            }
        }

        public async Task PutPayloadService(CapnpGen.IPayloadService service, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.InjectorService.Params_PutPayloadService.WRITER>();
            var arg_ = new CapnpGen.InjectorService.Params_PutPayloadService()
            {Service = service};
            arg_?.serialize(in_);
            using (var d_ = await Call(17958626206301325647UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.InjectorService.Result_PutPayloadService>(d_);
                return;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf939db62c09c894fUL)]
    public class InjectorService_Skeleton : Skeleton<IInjectorService>
    {
        public InjectorService_Skeleton()
        {
            SetMethodTable(NotifyError, GetManagedPayloadInfo, PutPayloadService);
        }

        public override ulong InterfaceId => 17958626206301325647UL;
        async Task<AnswerOrCounterquestion> NotifyError(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.InjectorService.Params_NotifyError>(d_);
                await Impl.NotifyError(in_.Error, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.InjectorService.Result_NotifyError.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetManagedPayloadInfo(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetManagedPayloadInfo(cancellationToken_), info =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.InjectorService.Result_GetManagedPayloadInfo.WRITER>();
                    var r_ = new CapnpGen.InjectorService.Result_GetManagedPayloadInfo{Info = info};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> PutPayloadService(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.InjectorService.Params_PutPayloadService>(d_);
                await Impl.PutPayloadService(in_.Service, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.InjectorService.Result_PutPayloadService.WRITER>();
                return s_;
            }
        }
    }

    public static class InjectorService
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc9ccce4a0da7a522UL)]
        public class ErrorInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc9ccce4a0da7a522UL;
            public enum WHICH : ushort
            {
                Native = 0,
                Managed = 1,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Native:
                        Native = CapnpSerializable.Create<CapnpGen.InjectorService.ErrorInfo.native>(reader.Native);
                        break;
                    case WHICH.Managed:
                        Managed = CapnpSerializable.Create<CapnpGen.InjectorService.ManagedExceptionInfo>(reader.Managed);
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
                        case WHICH.Native:
                            _content = null;
                            break;
                        case WHICH.Managed:
                            _content = null;
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.Native:
                        Native?.serialize(writer.Native);
                        break;
                    case WHICH.Managed:
                        Managed?.serialize(writer.Managed);
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

            public CapnpGen.InjectorService.ErrorInfo.native Native
            {
                get => _which == WHICH.Native ? (CapnpGen.InjectorService.ErrorInfo.native)_content : null;
                set
                {
                    _which = WHICH.Native;
                    _content = value;
                }
            }

            public CapnpGen.InjectorService.ManagedExceptionInfo Managed
            {
                get => _which == WHICH.Managed ? (CapnpGen.InjectorService.ManagedExceptionInfo)_content : null;
                set
                {
                    _which = WHICH.Managed;
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
                public WHICH which => (WHICH)ctx.ReadDataUShort(16U, (ushort)0);
                public native.READER Native => which == WHICH.Native ? new native.READER(ctx) : default;
                public CapnpGen.InjectorService.ManagedExceptionInfo.READER Managed => which == WHICH.Managed ? ctx.ReadStruct(0, CapnpGen.InjectorService.ManagedExceptionInfo.READER.create) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(16U, (ushort)0);
                    set => this.WriteData(16U, (ushort)value, (ushort)0);
                }

                public native.WRITER Native
                {
                    get => which == WHICH.Native ? Rewrap<native.WRITER>() : default;
                }

                public CapnpGen.InjectorService.ManagedExceptionInfo.WRITER Managed
                {
                    get => which == WHICH.Managed ? BuildPointer<CapnpGen.InjectorService.ManagedExceptionInfo.WRITER>(0) : default;
                    set => Link(0, value);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdcb91e01c2be8918UL)]
            public class native : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdcb91e01c2be8918UL;
                public enum WHICH : ushort
                {
                    Hosting = 0,
                    Panic = 1,
                    Other = 2,
                    undefined = 65535
                }

                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    switch (reader.which)
                    {
                        case WHICH.Hosting:
                            Hosting = reader.Hosting;
                            break;
                        case WHICH.Panic:
                            Panic = reader.Panic;
                            break;
                        case WHICH.Other:
                            Other = reader.Other;
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
                            case WHICH.Hosting:
                                _content = null;
                                break;
                            case WHICH.Panic:
                                _content = null;
                                break;
                            case WHICH.Other:
                                _content = null;
                                break;
                        }
                    }
                }

                public void serialize(WRITER writer)
                {
                    writer.which = which;
                    switch (which)
                    {
                        case WHICH.Hosting:
                            writer.Hosting = Hosting;
                            break;
                        case WHICH.Panic:
                            writer.Panic = Panic;
                            break;
                        case WHICH.Other:
                            writer.Other = Other;
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

                public string Hosting
                {
                    get => _which == WHICH.Hosting ? (string)_content : null;
                    set
                    {
                        _which = WHICH.Hosting;
                        _content = value;
                    }
                }

                public string Panic
                {
                    get => _which == WHICH.Panic ? (string)_content : null;
                    set
                    {
                        _which = WHICH.Panic;
                        _content = value;
                    }
                }

                public string Other
                {
                    get => _which == WHICH.Other ? (string)_content : null;
                    set
                    {
                        _which = WHICH.Other;
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
                    public string Hosting => which == WHICH.Hosting ? ctx.ReadText(0, null) : default;
                    public string Panic => which == WHICH.Panic ? ctx.ReadText(0, null) : default;
                    public string Other => which == WHICH.Other ? ctx.ReadText(0, null) : default;
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                    }

                    public WHICH which
                    {
                        get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                        set => this.WriteData(0U, (ushort)value, (ushort)0);
                    }

                    public string Hosting
                    {
                        get => which == WHICH.Hosting ? this.ReadText(0, null) : default;
                        set => this.WriteText(0, value, null);
                    }

                    public string Panic
                    {
                        get => which == WHICH.Panic ? this.ReadText(0, null) : default;
                        set => this.WriteText(0, value, null);
                    }

                    public string Other
                    {
                        get => which == WHICH.Other ? this.ReadText(0, null) : default;
                        set => this.WriteText(0, value, null);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa0aff649111e364fUL)]
        public class ManagedExceptionInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa0aff649111e364fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Message = reader.Message;
                TypeName = reader.TypeName;
                StackTrace = CapnpSerializable.Create<CapnpGen.Option<string>>(reader.StackTrace);
                Inner = CapnpSerializable.Create<CapnpGen.Option<CapnpGen.InjectorService.ManagedExceptionInfo>>(reader.Inner);
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

            public CapnpGen.Option<CapnpGen.InjectorService.ManagedExceptionInfo> Inner
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
                public CapnpGen.Option<CapnpGen.InjectorService.ManagedExceptionInfo>.READER Inner => ctx.ReadStruct(3, CapnpGen.Option<CapnpGen.InjectorService.ManagedExceptionInfo>.READER.create);
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

                public CapnpGen.Option<CapnpGen.InjectorService.ManagedExceptionInfo>.WRITER Inner
                {
                    get => BuildPointer<CapnpGen.Option<CapnpGen.InjectorService.ManagedExceptionInfo>.WRITER>(3);
                    set => Link(3, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8cb60b0b461f0de7UL)]
        public class ManagedPayloadInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8cb60b0b461f0de7UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                AssemblyPath = reader.AssemblyPath;
                RuntimeConfigPath = reader.RuntimeConfigPath;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.AssemblyPath = AssemblyPath;
                writer.RuntimeConfigPath = RuntimeConfigPath;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string AssemblyPath
            {
                get;
                set;
            }

            public string RuntimeConfigPath
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
                public string AssemblyPath => ctx.ReadText(0, null);
                public string RuntimeConfigPath => ctx.ReadText(1, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string AssemblyPath
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public string RuntimeConfigPath
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa6e856acf167a115UL)]
        public class Params_NotifyError : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa6e856acf167a115UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Error = CapnpSerializable.Create<CapnpGen.InjectorService.ErrorInfo>(reader.Error);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Error?.serialize(writer.Error);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.InjectorService.ErrorInfo Error
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
                public CapnpGen.InjectorService.ErrorInfo.READER Error => ctx.ReadStruct(0, CapnpGen.InjectorService.ErrorInfo.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.InjectorService.ErrorInfo.WRITER Error
                {
                    get => BuildPointer<CapnpGen.InjectorService.ErrorInfo.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc32643a2fa6a162aUL)]
        public class Result_NotifyError : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc32643a2fa6a162aUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdb4aa2ae4438d919UL)]
        public class Params_GetManagedPayloadInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdb4aa2ae4438d919UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa7f94c6063315db5UL)]
        public class Result_GetManagedPayloadInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa7f94c6063315db5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Info = CapnpSerializable.Create<CapnpGen.InjectorService.ManagedPayloadInfo>(reader.Info);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Info?.serialize(writer.Info);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.InjectorService.ManagedPayloadInfo Info
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
                public CapnpGen.InjectorService.ManagedPayloadInfo.READER Info => ctx.ReadStruct(0, CapnpGen.InjectorService.ManagedPayloadInfo.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.InjectorService.ManagedPayloadInfo.WRITER Info
                {
                    get => BuildPointer<CapnpGen.InjectorService.ManagedPayloadInfo.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd88a850bfc5f890cUL)]
        public class Params_PutPayloadService : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd88a850bfc5f890cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Service = reader.Service;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Service = Service;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.IPayloadService Service
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
                public CapnpGen.IPayloadService Service => ctx.ReadCap<CapnpGen.IPayloadService>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.IPayloadService Service
                {
                    get => ReadCap<CapnpGen.IPayloadService>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe95195784501ab61UL)]
        public class Result_PutPayloadService : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe95195784501ab61UL;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaee89bac83b6f260UL), Proxy(typeof(PayloadService_Proxy)), Skeleton(typeof(PayloadService_Skeleton))]
    public interface IPayloadService : IDisposable
    {
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaee89bac83b6f260UL)]
    public class PayloadService_Proxy : Proxy, IPayloadService
    {
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaee89bac83b6f260UL)]
    public class PayloadService_Skeleton : Skeleton<IPayloadService>
    {
        public PayloadService_Skeleton()
        {
            SetMethodTable();
        }

        public override ulong InterfaceId => 12603494722442818144UL;
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf6b144c9bebc3559UL)]
    public class Option<TT> : ICapnpSerializable where TT : class
    {
        public const UInt64 typeId = 0xf6b144c9bebc3559UL;
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