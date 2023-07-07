namespace Generator
{
    using ClangSharp;
    using CppAst;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public static partial class CsCodeGenerator
    {
        public static readonly HashSet<string> DefinedDelegates = new();

        private static void GenerateDelegates(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Diagnostics", "System.Runtime.CompilerServices", "System.Runtime.InteropServices" };

            if (!CsCodeGeneratorSettings.Default.GenerateDelegates)
            {
                return;
            }

            // Generate Delegates
            using var writer = new CodeWriter(Path.Combine(outputPath, "Delegates.cs"), usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());

            // Print All classes, structs
            for (int i = 0; i < compilation.Classes.Count; i++)
            {
                CppClass? cppClass = compilation.Classes[i];
                if (CsCodeGeneratorSettings.Default.AllowedTypes.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                if (CsCodeGeneratorSettings.Default.IgnoredTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                string csName = GetCsCleanName(cppClass.Name);
                WriteClassDelegates(writer, compilation, cppClass, csName);
            }

            for (int i = 0; i < compilation.Typedefs.Count; i++)
            {
                CppTypedef typedef = compilation.Typedefs[i];
                if (CsCodeGeneratorSettings.Default.AllowedDelegates.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedDelegates.Contains(typedef.Name))
                    continue;
                if (CsCodeGeneratorSettings.Default.IgnoredDelegates.Contains(typedef.Name))
                    continue;
                if (DefinedDelegates.Contains(typedef.Name))
                    continue;

                if (typedef.ElementType is CppPointerType pointerType && pointerType.ElementType is CppFunctionType functionType)
                {
                    var csName = GetCsCleanName(typedef.Name);
                    DefinedDelegates.Add(csName);
                    WriteDelegate(writer, typedef, functionType, csName);
                }
            }
        }

        public static void WriteDelegate(CodeWriter writer, CppTypedef typedef, CppFunctionType type, string csName)
        {
            string returnCsName = GetCsTypeName(type.ReturnType, false);
            string signature = GetParameterSignature(type.Parameters, false);

            if (CsCodeGeneratorSettings.Default.TryGetDelegateMapping(csName, out var mapping))
            {
                returnCsName = mapping.ReturnType;
                signature = mapping.Signature;
            }

            WriteCsSummary(typedef.Comment, writer);
            writer.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{GetCallingConvention(type.CallingConvention)})]");
            writer.WriteLine($"public unsafe delegate {returnCsName} {csName}({signature});");
            writer.WriteLine();
        }

        public static void WriteClassDelegates(CodeWriter writer, CppCompilation compilation, CppClass cppClass, string csName)
        {
            if (cppClass.ClassKind == CppClassKind.Class || cppClass.Name.EndsWith("_T") || csName == "void")
            {
                return;
            }

            for (int j = 0; j < cppClass.Classes.Count; j++)
            {
                var subClass = cppClass.Classes[j];
                string csSubName;
                if (string.IsNullOrEmpty(subClass.Name))
                {
                    string label = cppClass.Classes.Count == 1 ? "" : j.ToString();
                    csSubName = csName + "Union" + label;
                }
                else
                {
                    csSubName = GetCsCleanName(subClass.Name);
                }

                WriteClassDelegates(writer, compilation, subClass, csSubName);
            }

            for (int j = 0; j < cppClass.Fields.Count; j++)
            {
                CppField cppField = cppClass.Fields[j];

                if (cppField.Type is CppPointerType cppPointer && IsDelegate(cppPointer, out var functionType))
                {
                    string csFieldName = NormalizeFieldName(cppField.Name);

                    if (DefinedDelegates.Contains(csFieldName))
                    {
                        continue;
                    }

                    DefinedDelegates.Add(csFieldName);

                    string returnCsName = GetCsTypeName(functionType.ReturnType, false);
                    string signature = GetParameterSignature(functionType.Parameters, false);
                    returnCsName = returnCsName.Replace("bool", "byte");

                    if (CsCodeGeneratorSettings.Default.TryGetDelegateMapping(csFieldName, out var mapping))
                    {
                        returnCsName = mapping.ReturnType;
                        signature = mapping.Signature;
                    }

                    WriteCsSummary(cppField.Comment, writer);
                    writer.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{GetCallingConvention(functionType.CallingConvention)})]");
                    writer.WriteLine($"public unsafe delegate {returnCsName} {csFieldName}({signature});");
                    writer.WriteLine();
                }
                else
                {
                    WriteDelegate(writer, cppField, false);
                }
            }
        }

        private static void WriteDelegate(CodeWriter writer, CppField field, bool isReadOnly = false)
        {
            string csFieldName = NormalizeFieldName(field.Name);

            if (DefinedDelegates.Contains(csFieldName))
            {
                return;
            }

            DefinedDelegates.Add(csFieldName);

            string fieldPrefix = isReadOnly ? "readonly " : string.Empty;

            if (field.Type is CppTypedef typedef &&
                typedef.ElementType is CppPointerType pointerType &&
                pointerType.ElementType is CppFunctionType functionType)
            {
                string signature = GetParameterSignature(functionType.Parameters, false);
                string returnCsName = GetCsTypeName(functionType.ReturnType, false);
                returnCsName = returnCsName.Replace("bool", "byte");

                if (CsCodeGeneratorSettings.Default.TryGetDelegateMapping(csFieldName, out var mapping))
                {
                    returnCsName = mapping.ReturnType;
                    signature = mapping.Signature;
                }

                WriteCsSummary(field.Comment, writer);
                writer.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{GetCallingConvention(functionType.CallingConvention)})]");
                writer.WriteLine($"public unsafe {fieldPrefix}delegate {returnCsName} {csFieldName}({signature});");
                writer.WriteLine();
            }
        }
    }
}