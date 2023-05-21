namespace Generator
{
    using ClangSharp;
    using CppAst;
    using System.IO;

    public static partial class CsCodeGenerator
    {
        private static readonly HashSet<string> s_definedTypedefs = new();

        private static void GenerateHandles(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Diagnostics" };
            // Generate Functions
            using var writer = new CodeWriter(Path.Combine(outputPath, "Handles.cs"), usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());

            foreach (CppTypedef typedef in compilation.Typedefs)
            {
                if (CsCodeGeneratorSettings.Default.AllowedTypedefs.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedTypedefs.Contains(typedef.Name))
                    continue;
                if (CsCodeGeneratorSettings.Default.IgnoredTypedefs.Contains(typedef.Name))
                    continue;
                if (s_definedTypedefs.Contains(typedef.Name))
                    continue;
                s_definedTypedefs.Add(typedef.Name);

                if (typedef.ElementType is CppPointerType pointerType)
                {
                    var isDispatchable = true;
                    var csName = GetCsCleanName(typedef.Name);

                    if (IsDelegate(pointerType, out var delegateType))
                    {
                        WriteDelegate(writer, typedef, delegateType, csName);
                        continue;
                    }

                    WriteHandle(writer, typedef, csName, isDispatchable);
                }
            }
        }

        public static void WriteDelegate(CodeWriter writer, CppTypedef typedef, CppFunctionType type, string csName)
        {
            string returnCsName = GetCsTypeName(type.ReturnType, false);
            string signature = GetParameterSignature(null, type.Parameters, false);
            WriteCsSummary(typedef.Comment, writer);
            writer.WriteLine($"public unsafe delegate {returnCsName} {csName}({signature});");
            writer.WriteLine();
        }

        private static void WriteHandle(CodeWriter writer, CppTypedef typedef, string csName, bool isDispatchable)
        {
            WriteCsSummary(typedef.Comment, writer);
            writer.WriteLine($"[DebuggerDisplay(\"{{DebuggerDisplay,nq}}\")]");
            using (writer.PushBlock($"public readonly partial struct {csName} : IEquatable<{csName}>"))
            {
                string handleType = isDispatchable ? "nint" : "ulong";
                string nullValue = "0";

                writer.WriteLine($"public {csName}({handleType} handle) {{ Handle = handle; }}");
                writer.WriteLine($"public {handleType} Handle {{ get; }}");
                writer.WriteLine($"public bool IsNull => Handle == 0;");

                writer.WriteLine($"public static {csName} Null => new {csName}({nullValue});");
                writer.WriteLine($"public static implicit operator {csName}({handleType} handle) => new {csName}(handle);");
                writer.WriteLine($"public static bool operator ==({csName} left, {csName} right) => left.Handle == right.Handle;");
                writer.WriteLine($"public static bool operator !=({csName} left, {csName} right) => left.Handle != right.Handle;");
                writer.WriteLine($"public static bool operator ==({csName} left, {handleType} right) => left.Handle == right;");
                writer.WriteLine($"public static bool operator !=({csName} left, {handleType} right) => left.Handle != right;");
                writer.WriteLine($"public bool Equals({csName} other) => Handle == other.Handle;");
                writer.WriteLine("/// <inheritdoc/>");
                writer.WriteLine($"public override bool Equals(object obj) => obj is {csName} handle && Equals(handle);");
                writer.WriteLine("/// <inheritdoc/>");
                writer.WriteLine($"public override int GetHashCode() => Handle.GetHashCode();");
                writer.WriteLine($"private string DebuggerDisplay => string.Format(\"{csName} [0x{{0}}]\", Handle.ToString(\"X\"));");
            }
            writer.WriteLine();
        }
    }
}