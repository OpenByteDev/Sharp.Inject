using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Sharp.Inject.Bootstrap {
    [Generator]
    public class EntryPointGenerator : ISourceGenerator {

        private const string attributeText = @"
using System;

namespace Sharp.Inject.Bootstrap {
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class EntryPointAttribute : Attribute {
        public EntryPointAttribute() {
        }
    }
}
";

        public void Initialize(GeneratorInitializationContext context) {
            // Register the attribute source
            context.RegisterForPostInitialization(i => i.AddSource("EntryPointAttribute", attributeText));

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context) {
            try {
                // retrieve the populated receiver 
                if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
                    return;

                if (receiver.MethodSymbol is null || receiver.MethodSyntaxNode is null)
                    return;

                var userEntryPoint = receiver.MethodSymbol;
                var userEntryPointSyntax = receiver.MethodSyntaxNode;
                var parameterTypes = userEntryPoint.Parameters.Select(p => p.Type).ToArray();
                var returnType = userEntryPoint.ReturnType;

                var semanticModel = context.Compilation.GetSemanticModel(context.Compilation.SyntaxTrees.First());

                /*
                TODO: support int returns
                if (!userEntryPoint.ReturnsVoid  && !TypeSymbolMatchesType(returnType, typeof(int), semanticModel)) {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SI0002",
                            "The managed entry point has to return void or int.",
                            "The managed entry point has to return void or int.",
                            "Usage",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        userEntryPointSyntax.ReturnType.GetLocation()
                    ));
                    return;
                }
                */
                if (!userEntryPoint.ReturnsVoid) {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SI0002",
                            "The managed entry point has to return void.",
                            "The managed entry point has to return void.",
                            "Usage",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        userEntryPointSyntax.ReturnType.GetLocation()
                    ));
                    return;
                }
                if (!userEntryPoint.IsStatic) {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SI0003",
                            "The managed entry point has to be static.",
                            "The managed entry point has to be static.",
                            "Usage",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        userEntryPointSyntax.Modifiers.First(mod => mod.IsKind(SyntaxKind.StaticKeyword)).GetLocation()
                    ));
                    return;
                }


                var callParameterCode = GenerateBootstrapCode(parameterTypes, semanticModel, out string bootstrapCode);

                if (bootstrapCode == null || callParameterCode == null) {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "SI0004",
                            "The managed entry point has an incompatible signature.",
                            "The managed entry point has an incompatible signature.",
                            "Usage",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        userEntryPointSyntax.ParameterList.GetLocation()
                    ));
                }

                if (userEntryPoint.ReturnsVoid) {
                    bootstrapCode += $@"{GetFullName(userEntryPoint)}({callParameterCode}); return 0;";
                } else {
                    bootstrapCode += $@"return {GetFullName(userEntryPoint)}({callParameterCode});";
                }

               var source = $@"
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Sharp.Inject.Bootstrap {{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class Bootstrapper {{
        [UnmanagedCallersOnly]
        public static unsafe int ManagedEntryPoint(ManagedBootstrapArgs* args) {{
            var userArgPtr = args->UserArgument;
            var userArgSize = args->UserArgumentSize;
            try {{
                {bootstrapCode}
            }} catch (Exception e) {{
                var message = e.ToString().AsSpan();
                var errorByteBuffer = new Span<byte>(args->ErrorMessageBuffer, (int) args->ErrorMessageBufferSize);
                var errorBuffer = MemoryMarshal.Cast<byte, char>(errorByteBuffer);
                message.CopyTo(errorBuffer);
                *args->ErrorMessageLen = (nuint) message.Length;
            }}
            return 42;
        }}

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct ManagedBootstrapArgs {{
            public void* UserArgument;
            public nuint UserArgumentSize;
            public char* ErrorMessageBuffer;
            public nuint* ErrorMessageLen;
            public nuint ErrorMessageBufferSize;
        }}
    }}
}}";

                context.AddSource("Bootstrapper", source);

            } catch (Exception e) {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "SI0001",
                        "Unhandled exception during entry point generation.",
                        "Unhandled exception during entry point generation: {0}",
                        "Usage",
                        DiagnosticSeverity.Error,
                        true
                    ), null, e));
            }
        }

        private string? GenerateBootstrapCode(ITypeSymbol[] parameterTypes, SemanticModel semanticModel, out string bootstrapCode) {
            bootstrapCode = "";

            // void
            if (parameterTypes.Length == 0) {
                return "";
            }

            // (unmanaged*|IntPtr) [int|uint|nuint|long|ulong]
            if (parameterTypes[0] is IPointerTypeSymbol pointerType && pointerType.PointedAtType.IsUnmanagedType ||
                TypeSymbolMatchesType(parameterTypes[0], typeof(IntPtr), semanticModel)) {

                switch (parameterTypes.Length) {
                    case 0: throw new UnreachableException();
                    case 1:
                        return $@"({GetFullName(parameterTypes[0])}) userArgPtr";
                    case 2:
                        if (parameterTypes[1] is ITypeSymbol sizeParameter && (
                            TypeSymbolMatchesType(sizeParameter, typeof(int), semanticModel) ||
                            TypeSymbolMatchesType(sizeParameter, typeof(uint), semanticModel) ||
                            TypeSymbolMatchesType(sizeParameter, typeof(long), semanticModel) ||
                            TypeSymbolMatchesType(sizeParameter, typeof(ulong), semanticModel) ||
                            sizeParameter.IsNativeIntegerType)) {
                            return $@"({GetFullName(parameterTypes[0])}) userArgPtr, ({GetFullName(sizeParameter)}) userArgSize";
                        }
                        break;
                }
            }

            // (Span|ReadOnlySpan)<unmanaged>
            if (parameterTypes.Length == 1 && (
                TypeSymbolMatchesType(parameterTypes[0], typeof(Span<>), semanticModel) ||
                TypeSymbolMatchesType(parameterTypes[0], typeof(ReadOnlySpan<>), semanticModel)) &&
                parameterTypes[0] is INamedTypeSymbol namedTypeSymbol &&
                namedTypeSymbol.IsGenericType &&
                namedTypeSymbol.TypeArguments.Length >= 1 &&
                namedTypeSymbol.TypeArguments[0] is var itemType &&
                namedTypeSymbol.TypeArguments[0].IsUnmanagedType) {

                bootstrapCode = $@"
                    var bytes = new {GetFullName(typeof(Span<byte>))}((userArgPtr, ({GetFullName(typeof(int))}) userArgSize);
                    var casted = {GetFullName(typeof(MemoryMarshal))}.Cast<{GetFullName(typeof(byte))}, {GetFullName(itemType)}>(bytes);
                ";
                return $@"casted";
            }

            // string
            if (parameterTypes.Length == 1 && TypeSymbolMatchesType(parameterTypes[0], typeof(string), semanticModel)) {
                bootstrapCode = $@"
                    var bytes = new {GetFullName(typeof(Span<byte>))}(userArgPtr, ({GetFullName(typeof(int))}) userArgSize);
                    var casted = {GetFullName(typeof(MemoryMarshal))}.Cast<{GetFullName(typeof(byte))}, {GetFullName(typeof(char))}>(bytes);
                ";
                return $@"casted.ToString()";
            }


            return null;
        }

        private static readonly SymbolDisplayFormat fullyQualifiedSymbolDisplayFormat =
            new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces, genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

        private static string GetFullName(ITypeSymbol typeSymbol) {
            return typeSymbol.ToDisplayString(fullyQualifiedSymbolDisplayFormat);
        }

        private static string GetFullName(Type type) {
            var name = type.FullName.Replace("+", ".");
            if (!type.IsGenericType) {
                return name;
            }

            var builder = new StringBuilder();
            var index = name.IndexOf("`");
            builder.Append(name.Substring(0, index));
            builder.Append('<');
            var first = true;
            foreach (var arg in type.GetGenericArguments()) {
                if (!first) {
                    builder.Append(',');
                }
                builder.Append(GetFullName(arg));
                first = false;
            }
            builder.Append('>');
            return builder.ToString();
        }

        private static string GetFullName(IMethodSymbol methodSymbol) {
            return $"{GetFullName(methodSymbol.ContainingType)}.{methodSymbol.Name}";
        }

        static bool TypeSymbolMatchesType(ITypeSymbol typeSymbol, Type type, SemanticModel semanticModel) {
            if (type.IsGenericTypeDefinition && !type.IsConstructedGenericType &&
                typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType && !namedTypeSymbol.IsUnboundGenericType) {
                typeSymbol = namedTypeSymbol.ConstructedFrom;
            }
            return GetTypeSymbolForType(type, semanticModel).Equals(typeSymbol, SymbolEqualityComparer.Default);
        }

        static INamedTypeSymbol GetTypeSymbolForType(Type type, SemanticModel semanticModel) {
            if (!type.IsConstructedGenericType) {
                return semanticModel.Compilation.GetTypeByMetadataName(type.FullName)!;
            }

            // get all typeInfo's for the Type arguments 
            var typeArgumentsTypeInfos = type.GenericTypeArguments.Select(a => GetTypeSymbolForType(a, semanticModel));

            var openType = type.GetGenericTypeDefinition();
            var typeSymbol = semanticModel.Compilation.GetTypeByMetadataName(openType.FullName)!;
            return typeSymbol.Construct(typeArgumentsTypeInfos.ToArray<ITypeSymbol>());
        }
    }

    internal class SyntaxReceiver : ISyntaxContextReceiver {
        public IMethodSymbol? MethodSymbol { get; private set; }
        public MethodDeclarationSyntax? MethodSyntaxNode { get; private set; }

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
            if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax)
                return;

            if (methodDeclarationSyntax.AttributeLists.Count == 0)
                return;

            if (context.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax) is not IMethodSymbol methodSymbol)
                return;

            if (!methodSymbol.GetAttributes().Any(attr => attr.AttributeClass?.ToDisplayString() == "Sharp.Inject.Bootstrap.EntryPointAttribute"))
                return;

            MethodSymbol = methodSymbol;
            MethodSyntaxNode = methodDeclarationSyntax;
        }
    }
}
