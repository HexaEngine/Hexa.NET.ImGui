namespace Generator
{
    using ClangSharp;
    using CppAst;
    using System.IO;
    using System.Linq;

    public partial class CsCodeGenerator
    {
        public readonly HashSet<string> DefinedDelegates = new();
        public readonly HashSet<string> LibDefinedDelegates = new();

        private bool FilterDelegate(ICppMember member)
        {
            if (settings.AllowedDelegates.Count != 0 && !settings.AllowedDelegates.Contains(member.Name))
                return true;
            if (settings.IgnoredDelegates.Contains(member.Name))
                return true;

            if (LibDefinedDelegates.Contains(member.Name))
            {
                return true;
            }

            if (DefinedDelegates.Contains(member.Name))
            {
                return true;
            }

            DefinedDelegates.Add(member.Name);

            return false;
        }

        private void GenerateDelegates(CppCompilation compilation, string outputPath)
        {
            string[] usings = ["System", "System.Diagnostics", "System.Runtime.CompilerServices", "System.Runtime.InteropServices", "HexaGen.Runtime"];

            if (!settings.GenerateDelegates)
            {
                return;
            }

            string outDir = Path.Combine(outputPath, "Delegates");
            string fileName = Path.Combine(outDir, "Delegates.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            // Generate Delegates
            using var writer = new SplitCodeWriter(fileName, settings.Namespace, 2, usings.Concat(settings.Usings).ToArray());

            // Print All classes, structs
            for (int i = 0; i < compilation.Classes.Count; i++)
            {
                CppClass? cppClass = compilation.Classes[i];
                if (settings.AllowedTypes.Count != 0 && !settings.AllowedTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                if (settings.IgnoredTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                string csName = settings.GetCsCleanName(cppClass.Name);
                WriteClassDelegates(writer, cppClass, csName);
            }

            for (int i = 0; i < compilation.Typedefs.Count; i++)
            {
                CppTypedef typedef = compilation.Typedefs[i];
                if (settings.AllowedDelegates.Count != 0 && !settings.AllowedDelegates.Contains(typedef.Name))
                    continue;
                if (settings.IgnoredDelegates.Contains(typedef.Name))
                    continue;

                if (typedef.ElementType is CppPointerType pointerType && pointerType.ElementType is CppFunctionType functionType)
                {
                    WriteDelegate(writer, typedef, functionType);
                }
            }
        }

        public void WriteClassDelegates(ICodeWriter writer, CppClass cppClass, string csName)
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
                    csSubName = settings.GetCsCleanName(subClass.Name);
                }

                WriteClassDelegates(writer, subClass, csSubName);
            }

            for (int j = 0; j < cppClass.Fields.Count; j++)
            {
                CppField cppField = cppClass.Fields[j];

                if (cppField.Type is CppPointerType cppPointer && cppPointer.IsDelegate(out var functionType))
                {
                    WriteDelegate(writer, cppField, functionType);
                }
                else if (cppField.Type is CppTypedef typedef && typedef.ElementType is CppPointerType pointerType && pointerType.ElementType is CppFunctionType cppFunctionType)
                {
                    WriteDelegate(writer, cppField, cppFunctionType, false);
                }
            }
        }

        private void WriteDelegate<T>(ICodeWriter writer, T field, CppFunctionType functionType, bool isReadOnly = false) where T : class, ICppDeclaration, ICppMember
        {
            if (FilterDelegate(field))
            {
                return;
            }

            string csFieldName = settings.NormalizeFieldName(field.Name);
            string fieldPrefix = isReadOnly ? "readonly " : string.Empty;

            writer.WriteLine("#if NET5_0_OR_GREATER");
            WriteFinal(writer, field, functionType, csFieldName, fieldPrefix);
            writer.WriteLine("#else");
            WriteFinal(writer, field, functionType, csFieldName, fieldPrefix, compatibility: true);
            writer.WriteLine("#endif");
            writer.WriteLine();
        }

        private void WriteFinal<T>(ICodeWriter writer, T field, CppFunctionType functionType, string csFieldName, string fieldPrefix, bool compatibility = false) where T : class, ICppDeclaration, ICppMember
        {
            string signature = settings.GetParameterSignature(functionType.Parameters, canUseOut: false, delegateType: true, compatibility: compatibility);
            string returnCsName = settings.GetCsTypeName(functionType.ReturnType, false);
            returnCsName = returnCsName.Replace("bool", settings.GetBoolType());

            if (functionType.ReturnType is CppTypedef typedef && typedef.ElementType.IsDelegate(out var cppFunction) && !returnCsName.Contains('*'))
            {
                if (cppFunction.Parameters.Count == 0)
                {
                    returnCsName = $"delegate*<{settings.GetCsTypeName(cppFunction.ReturnType)}>";
                }
                else
                {
                    returnCsName = $"delegate*<{settings.GetNamelessParameterSignature(cppFunction.Parameters, canUseOut: false, delegateType: true, compatibility)}, {settings.GetCsTypeName(cppFunction.ReturnType)}>";
                }
            }

            if (compatibility && returnCsName.Contains('*'))
            {
                returnCsName = "nint";
            }

            if (settings.TryGetDelegateMapping(csFieldName, out var mapping))
            {
                returnCsName = mapping.ReturnType;
                signature = mapping.Signature;
            }

            string header = $"{returnCsName} {csFieldName}({signature})";

            WriteCsSummary(field.Comment, writer);
            writer.WriteLine($"[UnmanagedFunctionPointer(CallingConvention.{functionType.CallingConvention.GetCallingConvention()})]");
            writer.WriteLine($"public unsafe {fieldPrefix}delegate {header};");
            writer.WriteLine();
        }
    }
}