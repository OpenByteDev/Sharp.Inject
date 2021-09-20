using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc88ddc0b1778b27cUL), Proxy(typeof(TestPayloadService_Proxy)), Skeleton(typeof(TestPayloadService_Skeleton))]
    public interface ITestPayloadService : IDisposable
    {
        Task Log(string message, CancellationToken cancellationToken_ = default);
        Task<string> Echo(string message, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc88ddc0b1778b27cUL)]
    public class TestPayloadService_Proxy : Proxy, ITestPayloadService
    {
        public async Task Log(string message, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.TestPayloadService.Params_Log.WRITER>();
            var arg_ = new CapnpGen.TestPayloadService.Params_Log()
            {Message = message};
            arg_?.serialize(in_);
            using (var d_ = await Call(14451448719498326652UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.TestPayloadService.Result_Log>(d_);
                return;
            }
        }

        public async Task<string> Echo(string message, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.TestPayloadService.Params_Echo.WRITER>();
            var arg_ = new CapnpGen.TestPayloadService.Params_Echo()
            {Message = message};
            arg_?.serialize(in_);
            using (var d_ = await Call(14451448719498326652UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.TestPayloadService.Result_Echo>(d_);
                return (r_.Message);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc88ddc0b1778b27cUL)]
    public class TestPayloadService_Skeleton : Skeleton<ITestPayloadService>
    {
        public TestPayloadService_Skeleton()
        {
            SetMethodTable(Log, Echo);
        }

        public override ulong InterfaceId => 14451448719498326652UL;
        async Task<AnswerOrCounterquestion> Log(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.TestPayloadService.Params_Log>(d_);
                await Impl.Log(in_.Message, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.TestPayloadService.Result_Log.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> Echo(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.TestPayloadService.Params_Echo>(d_);
                return Impatient.MaybeTailCall(Impl.Echo(in_.Message, cancellationToken_), message =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.TestPayloadService.Result_Echo.WRITER>();
                    var r_ = new CapnpGen.TestPayloadService.Result_Echo{Message = message};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class TestPayloadService
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8d48e43234818d68UL)]
        public class Params_Log : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8d48e43234818d68UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Message = reader.Message;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Message = Message;
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Message
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb3fd25211121f125UL)]
        public class Result_Log : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb3fd25211121f125UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9938a48d7ca59e33UL)]
        public class Params_Echo : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9938a48d7ca59e33UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Message = reader.Message;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Message = Message;
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Message
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c5d77e502c5e3c5UL)]
        public class Result_Echo : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9c5d77e502c5e3c5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Message = reader.Message;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Message = Message;
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Message
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }
    }
}