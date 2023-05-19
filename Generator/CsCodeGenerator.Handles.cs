namespace Generator
{
    using CppAst;
    using System.IO;

    public static partial class CsCodeGenerator
    {
        private static void GenerateHandles(CppCompilation compilation, string outputPath)
        {
            // Generate Functions
            using var writer = new CodeWriter(Path.Combine(outputPath, "Handles.cs"),
                "System",
                "System.Diagnostics"
                );

            foreach (CppTypedef typedef in compilation.Typedefs)
            {
                if (!(typedef.ElementType is CppPointerType))
                {
                    continue;
                }

                var isDispatchable = true;

                var csName = GetCsCleanName(typedef.Name);

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
}