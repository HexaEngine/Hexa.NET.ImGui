namespace Generator
{
    using ClangSharp;
    using CppAst;
    using System.IO;
    using System.Runtime.InteropServices;

    public static partial class CsCodeGenerator
    {
        public static readonly HashSet<string> DefinedTypedefs = new();

        private static void GenerateHandles(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Diagnostics", "System.Runtime.InteropServices" };
            // Generate Functions
            using var writer = new CodeWriter(Path.Combine(outputPath, "Handles.cs"), usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());

            foreach (CppTypedef typedef in compilation.Typedefs)
            {
                if (CsCodeGeneratorSettings.Default.AllowedTypedefs.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedTypedefs.Contains(typedef.Name))
                    continue;
                if (CsCodeGeneratorSettings.Default.IgnoredTypedefs.Contains(typedef.Name))
                    continue;
                if (DefinedTypedefs.Contains(typedef.Name))
                    continue;
                DefinedTypedefs.Add(typedef.Name);

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
            string signature = GetParameterSignature(type.Parameters, false);
            WriteCsSummary(typedef.Comment, writer);

            writer.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{Map(type.CallingConvention)})]");
            writer.WriteLine($"public unsafe delegate {returnCsName} {csName}({signature});");
            writer.WriteLine();
        }

        public static CallingConvention Map(CppCallingConvention convention)
        {
            return convention switch
            {
                CppCallingConvention.Default => throw new NotImplementedException(),
                CppCallingConvention.C => CallingConvention.Cdecl,
                CppCallingConvention.X86StdCall => CallingConvention.StdCall,
                CppCallingConvention.X86FastCall => CallingConvention.FastCall,
                CppCallingConvention.X86ThisCall => CallingConvention.ThisCall,
                CppCallingConvention.X86Pascal => throw new NotImplementedException(),
                CppCallingConvention.AAPCS => throw new NotImplementedException(),
                CppCallingConvention.AAPCS_VFP => throw new NotImplementedException(),
                CppCallingConvention.X86RegCall => throw new NotImplementedException(),
                CppCallingConvention.IntelOclBicc => throw new NotImplementedException(),
                CppCallingConvention.Win64 => CallingConvention.Winapi,
                CppCallingConvention.X86_64SysV => throw new NotImplementedException(),
                CppCallingConvention.X86VectorCall => throw new NotImplementedException(),
                CppCallingConvention.Swift => throw new NotImplementedException(),
                CppCallingConvention.PreserveMost => throw new NotImplementedException(),
                CppCallingConvention.PreserveAll => throw new NotImplementedException(),
                CppCallingConvention.AArch64VectorCall => throw new NotImplementedException(),
                CppCallingConvention.Invalid => throw new NotImplementedException(),
                CppCallingConvention.Unexposed => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
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