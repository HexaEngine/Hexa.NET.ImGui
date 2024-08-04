namespace Generator
{
    using CppAst;
    using System.IO;

    public partial class CsCodeGenerator
    {
        public readonly HashSet<string> DefinedTypedefs = new();
        public readonly HashSet<string> LibDefinedTypedefs = new();

        private bool FilterTypedef(CppTypedef typedef)
        {
            if (settings.AllowedTypedefs.Count != 0 && !settings.AllowedTypedefs.Contains(typedef.Name))
                return true;
            if (settings.IgnoredTypedefs.Contains(typedef.Name))
                return true;
            if (LibDefinedTypedefs.Contains(typedef.Name))
                return true;
            if (DefinedTypedefs.Contains(typedef.Name))
                return true;
            DefinedTypedefs.Add(typedef.Name);
            return false;
        }

        private void GenerateHandles(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Diagnostics", "System.Runtime.InteropServices", "HexaGen.Runtime" };

            string outDir = Path.Combine(outputPath, "Handles");
            string fileName = Path.Combine(outDir, "Handles.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            // Generate Functions
            using var writer = new CodeWriter(fileName, settings.Namespace, usings.Concat(settings.Usings).ToArray());

            for (int i = 0; i < compilation.Typedefs.Count; i++)
            {
                CppTypedef typedef = compilation.Typedefs[i];
                if (FilterTypedef(typedef))
                {
                    continue;
                }

                if (typedef.ElementType is CppPointerType pointerType && pointerType.ElementType is not CppFunctionType)
                {
                    var isDispatchable = true;
                    var csName = settings.GetCsCleanName(typedef.Name);
                    WriteHandle(writer, typedef, csName, isDispatchable);
                }
            }
        }

        private static void WriteHandle(ICodeWriter writer, CppTypedef typedef, string csName, bool isDispatchable)
        {
            WriteCsSummary(typedef.Comment, writer);
            writer.WriteLine("#if NET5_0_OR_GREATER");
            writer.WriteLine($"[DebuggerDisplay(\"{{DebuggerDisplay,nq}}\")]");
            writer.WriteLine("#endif");
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
                writer.WriteLine("#if NET5_0_OR_GREATER");
                writer.WriteLine($"private string DebuggerDisplay => string.Format(\"{csName} [0x{{0}}]\", Handle.ToString(\"X\"));");
                writer.WriteLine("#endif");
            }
            writer.WriteLine();
        }
    }
}