using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;

namespace Sharp.Inject {
    public class InjectedModule {
        private readonly Process process;
        private readonly string modulePath;
        private readonly Reloaded.Injector.Injector underlyingInjector;
        private IntPtr foreignProcessMemBasePtr;
        private int foreignByteCount;

        internal InjectedModule(Process process, string modulePath, Reloaded.Injector.Injector underlyingInjector) {
            this.process = process;
            this.modulePath = modulePath;
            this.underlyingInjector = underlyingInjector;
        }

        public void InvokeEntryPoint<T>(ref T argument, ReadOnlySpan<char> runtimeConfigPath) where T: unmanaged {
            var span = MemoryMarshal.CreateSpan(ref argument, 1);
            var bytes = MemoryMarshal.AsBytes(span);
            InvokeEntryPoint(bytes, runtimeConfigPath);
        }
        public void InvokeEntryPoint(string argument, ReadOnlySpan<char> runtimeConfigPath) {
            InvokeEntryPoint(argument.AsSpan(), runtimeConfigPath);
        }
        public void InvokeEntryPoint(ReadOnlySpan<char> argument, ReadOnlySpan<char> runtimeConfigPath) {
            InvokeEntryPoint(MemoryMarshal.AsBytes(argument), runtimeConfigPath);
        }
        public void InvokeEntryPoint(ReadOnlySpan<byte> argument, ReadOnlySpan<char> runtimeConfigPath) {
            var assemblyNameGuess = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(runtimeConfigPath));
            InvokeEntryPoint(argument, runtimeConfigPath, assemblyNameGuess);
        }
        public void InvokeEntryPoint(ReadOnlySpan<byte> argument, ReadOnlySpan<char> runtimeConfigPath, ReadOnlySpan<char> assemblyName) {
            unsafe {
                var modulePath = this.modulePath;
                var processHandle = (HANDLE)this.process.Handle;


                // Layout of the memory reserved in the target process.
                var containerOffset = 0;
                var containerByteCount = Unsafe.SizeOf<BootstrapArgs>();

                var modulePathOffset = containerOffset + containerByteCount;
                var modulePathByteCount = sizeof(char) * (modulePath.Length + 1);

                var runtimeConfigPathOffset = modulePathOffset + modulePathByteCount;
                var runtimeConfigPathByteCount = sizeof(char) * (runtimeConfigPath.Length + 1);

                var assemblyNameOffset = runtimeConfigPathOffset + runtimeConfigPathByteCount;
                var assemblyNameByteCount = sizeof(char) * (assemblyName.Length + 1);

                var userArgumentOffset = assemblyNameOffset + assemblyNameByteCount;
                var userArgumentByteCount = sizeof(byte) * argument.Length;

                var foreignByteCount = userArgumentOffset + userArgumentByteCount;


                // The arguments are copied into a local buffer before copying them into the target process.
                // This is done to reduce the number of calls to WriteProcessMemory.
                // If the user argument is large we avoid the copy and make a seperate call to WriteProcessMemory.
                var copyArgumentToLocalBuffer = foreignByteCount <= 1024;
                var bufferSize = copyArgumentToLocalBuffer ? foreignByteCount : foreignByteCount - userArgumentByteCount;


                var foreignProcessMemBasePtr = PInvoke.VirtualAllocEx(
                    processHandle,
                    null,
                    (nuint)foreignByteCount,
                    VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT | VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE,
                    PAGE_PROTECTION_FLAGS.PAGE_READWRITE
                );
                if (foreignProcessMemBasePtr == null) {
                    throw new Win32Exception();
                }
                this.foreignProcessMemBasePtr = (IntPtr)foreignProcessMemBasePtr;
                this.foreignByteCount = foreignByteCount;


                // Setup local copy of remote process memory.
                Span<byte> buffer = stackalloc byte[foreignByteCount];

                var argsBuffer = buffer.Slice(containerOffset, containerByteCount);
                ref BootstrapArgs args = ref MemoryMarshal.GetReference(MemoryMarshal.Cast<byte, BootstrapArgs>(argsBuffer));

                args.ReservedBytes = (nuint) foreignByteCount;
                args.ModulePath = WriteStringArg(buffer, modulePathOffset, modulePath, foreignProcessMemBasePtr);
                args.RuntimeConfigPath = WriteStringArg(buffer, runtimeConfigPathOffset, runtimeConfigPath, foreignProcessMemBasePtr);
                args.AssemblyName = WriteStringArg(buffer, assemblyNameOffset, assemblyName, foreignProcessMemBasePtr);

                if (copyArgumentToLocalBuffer) {
                    Debug.Assert(buffer.Length == userArgumentOffset);
                    argument.CopyTo(buffer[userArgumentOffset..]);
                }
                args.UserArgumentSize = (nuint) userArgumentByteCount;
                args.UserArgument = (byte*)((nuint)foreignProcessMemBasePtr + (nuint)userArgumentOffset);

                static LengthPrefixedString WriteStringArg(Span<byte> buffer, int byteOffset, ReadOnlySpan<char> stringArg, void* foreignProcessMemBasePtr) {
                    // write string to buffer
                    var argBuf = buffer.Slice(byteOffset, sizeof(char) * (stringArg.Length + 1));
                    MemoryMarshal.AsBytes(stringArg).CopyTo(argBuf);

                    // write null terminator (hostfxr wants 'em)
                    MemoryMarshal.Cast<byte, char>(argBuf)[^1] = '\0';

                    // create bootstrap argument struct
                    var foreignProcessPointer = (char*)((nuint)foreignProcessMemBasePtr + (nuint)byteOffset);
                    return new LengthPrefixedString((nuint)stringArg.Length, foreignProcessPointer);
                }


                // Copy argument and configuration into target process.
                nuint bytesWritten = 0;
                bool success;
                fixed (byte* bufferPtr = buffer) {
                    success = PInvoke.WriteProcessMemory(
                        processHandle,
                        foreignProcessMemBasePtr,
                        bufferPtr,
                        (nuint)bufferSize,
                        &bytesWritten
                    );
                }
                if (!success) {
                    throw new Win32Exception();
                }

                if (!copyArgumentToLocalBuffer) {
                    nuint additionalBytesWritten = 0;
                    fixed (byte* userArgumentBufferPtr = argument) {
                        success = PInvoke.WriteProcessMemory(
                            processHandle,
                            args.UserArgument,
                            userArgumentBufferPtr,
                            (nuint)userArgumentByteCount,
                            &additionalBytesWritten
                        );
                    }
                    if (!success) {
                        throw new Win32Exception();
                    }
                    Debug.Assert(additionalBytesWritten == (nuint)userArgumentByteCount);
                    bytesWritten += additionalBytesWritten;
                }

                Debug.Assert(bytesWritten == (nuint)foreignByteCount);
                var result = (BootstrapResult)(int)this.underlyingInjector.CallFunction(Injector.BootstrapperModulePath, "bootstrap", (long)foreignProcessMemBasePtr);
                if (result != BootstrapResult.Success) {
                    nuint bytesRead = 0;
                    nuint errorMessageLength = 0;
                    fixed (byte* bufferPtr = buffer) {
                        success = PInvoke.ReadProcessMemory(
                            processHandle,
                            foreignProcessMemBasePtr,
                            &errorMessageLength,
                            (nuint)sizeof(nuint),
                            &bytesRead
                        );
                    }
                    Debug.Assert((int) bytesRead == sizeof(nuint));
                    if (!success) {
                        throw new Win32Exception();
                    }

                    nuint errorMessageByteCount = (errorMessageLength * sizeof(char)) / sizeof(byte);
                    fixed (byte* bufferPtr = buffer) {
                        success = PInvoke.ReadProcessMemory(
                            processHandle,
                            (byte*)foreignProcessMemBasePtr + bytesRead,
                            bufferPtr,
                            errorMessageByteCount,
                            &bytesRead
                        );
                    }
                    Debug.Assert(bytesRead == errorMessageByteCount);
                    if (!success) {
                        throw new Win32Exception();
                    }

                    var errorBuffer = buffer[..(int)errorMessageByteCount];
                    var errorMessage = MemoryMarshal.Cast<byte, char>(errorBuffer);

                    throw result switch {
                        BootstrapResult.BootstrapError => new HostingBootstrapException(errorMessage.ToString()),
                        BootstrapResult.FatalError => new FatalBootstrapException(errorMessage.ToString()),
                        BootstrapResult.ManagedEntryPointException => new PayloadEntryPointBootstrapException(errorMessage.ToString()),
                        _ => new BootstrapException(errorMessage.ToString()),
                    };
                }

                Console.WriteLine(result);
            }
        }

        public void Eject() {
            if (!process.HasExited) {
                unsafe {
                    var failure = PInvoke.VirtualFreeEx(
                        (HANDLE)this.process.Handle,
                        (void*)this.foreignProcessMemBasePtr,
                        (nuint)this.foreignByteCount,
                        VIRTUAL_FREE_TYPE.MEM_RELEASE
                    );
                    if (failure) {
                        throw new Win32Exception();
                    }
                }
            }

            this.underlyingInjector.Eject(this.modulePath);
        }

        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct BootstrapArgs {
            public nuint ReservedBytes;
            public LengthPrefixedString ModulePath;
            public LengthPrefixedString RuntimeConfigPath;
            public LengthPrefixedString AssemblyName;
            public nuint UserArgumentSize;
            public byte* UserArgument;
        }

        [StructLayout(LayoutKind.Sequential)]
        private unsafe readonly struct LengthPrefixedString {
            public readonly nuint Length;
            public readonly char* Chars;

            public LengthPrefixedString(nuint length, char* chars) {
                Length = length;
                Chars = chars;
            }
        }

        private enum BootstrapResult: int {
            Success = 0,
            BootstrapError = 1,
            ManagedEntryPointException = 2,
            FatalError = 3,
        }


        public class BootstrapException: Exception {
            public BootstrapException() {
            }

            public BootstrapException(string message)
                : base(message) {
            }

            public BootstrapException(string message, Exception inner)
                : base(message, inner) {
            }
        }

        public class FatalBootstrapException : BootstrapException {
            public FatalBootstrapException() {
            }

            public FatalBootstrapException(string message)
                : base(message) {
            }

            public FatalBootstrapException(string message, Exception inner)
                : base(message, inner) {
            }
        }

        public class HostingBootstrapException : BootstrapException {
            public HostingBootstrapException() {
            }

            public HostingBootstrapException(string message)
                : base(message) {
            }

            public HostingBootstrapException(string message, Exception inner)
                : base(message, inner) {
            }
        }

        public class PayloadEntryPointBootstrapException : BootstrapException {
            public PayloadEntryPointBootstrapException() {
            }

            public PayloadEntryPointBootstrapException(string message)
                : base(message) {
            }

            public PayloadEntryPointBootstrapException(string message, Exception inner)
                : base(message, inner) {
            }
        }
    }
}
