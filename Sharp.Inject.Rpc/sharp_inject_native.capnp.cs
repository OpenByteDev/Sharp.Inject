using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb6519cc7492a033fUL), Proxy(typeof(NativeInjectorService_Proxy)), Skeleton(typeof(NativeInjectorService_Skeleton))]
    public interface INativeInjectorService : IDisposable
    {
        Task SetNativeBootstrapError(CapnpGen.NativeInjectorService.NativeErrorInfo errorInfo, CancellationToken cancellationToken_ = default);
        Task<CapnpGen.NativeInjectorService.ManagedPayloadInfo> GetManagedPayloadInfo(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb6519cc7492a033fUL)]
    public class NativeInjectorService_Proxy : Proxy, INativeInjectorService
    {
        public async Task SetNativeBootstrapError(CapnpGen.NativeInjectorService.NativeErrorInfo errorInfo, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.NativeInjectorService.Params_SetNativeBootstrapError.WRITER>();
            var arg_ = new CapnpGen.NativeInjectorService.Params_SetNativeBootstrapError()
            {ErrorInfo = errorInfo};
            arg_?.serialize(in_);
            using (var d_ = await Call(13137453967756362559UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.NativeInjectorService.Result_SetNativeBootstrapError>(d_);
                return;
            }
        }

        public async Task<CapnpGen.NativeInjectorService.ManagedPayloadInfo> GetManagedPayloadInfo(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.NativeInjectorService.Params_GetManagedPayloadInfo.WRITER>();
            var arg_ = new CapnpGen.NativeInjectorService.Params_GetManagedPayloadInfo()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13137453967756362559UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.NativeInjectorService.Result_GetManagedPayloadInfo>(d_);
                return (r_.Info);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb6519cc7492a033fUL)]
    public class NativeInjectorService_Skeleton : Skeleton<INativeInjectorService>
    {
        public NativeInjectorService_Skeleton()
        {
            SetMethodTable(SetNativeBootstrapError, GetManagedPayloadInfo);
        }

        public override ulong InterfaceId => 13137453967756362559UL;
        async Task<AnswerOrCounterquestion> SetNativeBootstrapError(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.NativeInjectorService.Params_SetNativeBootstrapError>(d_);
                await Impl.SetNativeBootstrapError(in_.ErrorInfo, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.NativeInjectorService.Result_SetNativeBootstrapError.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetManagedPayloadInfo(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetManagedPayloadInfo(cancellationToken_), info =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.NativeInjectorService.Result_GetManagedPayloadInfo.WRITER>();
                    var r_ = new CapnpGen.NativeInjectorService.Result_GetManagedPayloadInfo{Info = info};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class NativeInjectorService
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcbf77134ab330887UL)]
        public class NativeErrorInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcbf77134ab330887UL;
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
                    this.SetStruct(1, 1);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9bd81917cf727f9eUL)]
        public class ManagedPayloadInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9bd81917cf727f9eUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf0986a32dff759dUL)]
        public class Params_SetNativeBootstrapError : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbf0986a32dff759dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                ErrorInfo = CapnpSerializable.Create<CapnpGen.NativeInjectorService.NativeErrorInfo>(reader.ErrorInfo);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                ErrorInfo?.serialize(writer.ErrorInfo);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.NativeInjectorService.NativeErrorInfo ErrorInfo
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
                public CapnpGen.NativeInjectorService.NativeErrorInfo.READER ErrorInfo => ctx.ReadStruct(0, CapnpGen.NativeInjectorService.NativeErrorInfo.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.NativeInjectorService.NativeErrorInfo.WRITER ErrorInfo
                {
                    get => BuildPointer<CapnpGen.NativeInjectorService.NativeErrorInfo.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa3405c12ce312b4bUL)]
        public class Result_SetNativeBootstrapError : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa3405c12ce312b4bUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfaf7ba4e626a239bUL)]
        public class Params_GetManagedPayloadInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfaf7ba4e626a239bUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9250c4fa5810f174UL)]
        public class Result_GetManagedPayloadInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9250c4fa5810f174UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Info = CapnpSerializable.Create<CapnpGen.NativeInjectorService.ManagedPayloadInfo>(reader.Info);
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

            public CapnpGen.NativeInjectorService.ManagedPayloadInfo Info
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
                public CapnpGen.NativeInjectorService.ManagedPayloadInfo.READER Info => ctx.ReadStruct(0, CapnpGen.NativeInjectorService.ManagedPayloadInfo.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.NativeInjectorService.ManagedPayloadInfo.WRITER Info
                {
                    get => BuildPointer<CapnpGen.NativeInjectorService.ManagedPayloadInfo.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}