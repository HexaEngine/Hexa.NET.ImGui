namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public partial class CsCodeGenerator
    {
        public readonly HashSet<string> DefinedExtensions = new();
        public readonly HashSet<string> LibDefinedExtensions = new();

        private bool FilterExtension(CppTypedef typedef)
        {
            if (settings.IgnoredTypedefs.Contains(typedef.Name))
                return true;

            if (LibDefinedExtensions.Contains(typedef.Name))
                return true;

            if (DefinedExtensions.Contains(typedef.Name))
                return true;
            DefinedExtensions.Add(typedef.Name);
            if (typedef.ElementType is not CppPointerType)
            {
                return true;
            }

            return false;
        }

        private void GenerateExtensions(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Runtime.CompilerServices", "System.Runtime.InteropServices", "HexaGen.Runtime" };

            string outDir = Path.Combine(outputPath, "Extensions");
            string fileName = Path.Combine(outDir, "Extensions.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            // Generate Functions
            using var writer = new SplitCodeWriter(fileName, settings.Namespace, 2, usings.Concat(settings.Usings).ToArray());
            using (writer.PushBlock($"public static unsafe class Extensions"))
            {
                for (int i = 0; i < compilation.Typedefs.Count; i++)
                {
                    CppTypedef typedef = compilation.Typedefs[i];
                    FilterExtension(typedef);

                    if (typedef.IsDelegate(out _))
                    {
                        continue;
                    }

                    for (int j = 0; j < compilation.Functions.Count; j++)
                    {
                        var cppFunction = compilation.Functions[j];
                        if (settings.AllowedFunctions.Count != 0 && !settings.AllowedFunctions.Contains(cppFunction.Name))
                            continue;
                        if (settings.IgnoredFunctions.Contains(cppFunction.Name))
                            continue;
                        if (cppFunction.Parameters.Count == 0 || cppFunction.Parameters[0].Type.TypeKind == CppTypeKind.Pointer)
                            continue;

                        if (cppFunction.Parameters[0].Type.GetDisplayName().Contains(typedef.GetDisplayName()))
                        {
                            /*
                            var extensionPrefix = GetExtensionNamePrefix(typedef.Name);
                            var csFunctionName = GetPrettyCommandName(cppFunction.Name);
                            var csExtensionName = GetPrettyExtensionName(csFunctionName, extensionPrefix);
                            bool canUseOut = s_outReturnFunctions.Contains(cppFunction.Name);
                            var argumentsString = GetParameterSignature(cppFunction, canUseOut);
                            var sigs = GetVariantParameterSignatures(cppFunction.Name, cppFunction.Parameters, argumentsString, canUseOut);
                            sigs.Add(argumentsString);

                            WriteExtensions(writer, cppFunction, csFunctionName, csExtensionName, sigs);
                            */
                        }
                    }
                }
            }
        }

        private void WriteExtensions(ICodeWriter writer, CppFunction cppFunction, string command, string extension, List<string> signatures)
        {
            bool voidReturn = cppFunction.ReturnType.IsVoid();
            bool stringReturn = cppFunction.ReturnType.IsString();
            string returnCsName = settings.GetCsTypeName(cppFunction.ReturnType, false);

            for (int i = 0; i < signatures.Count; i++)
            {
                string signature = "this " + signatures[i];

                if (stringReturn)
                    WriteExtensionMethod(writer, cppFunction, command, extension, voidReturn, true, "string", signature);

                WriteExtensionMethod(writer, cppFunction, command, extension, voidReturn, false, returnCsName, signature);
            }
        }

        private void WriteExtensionMethod(ICodeWriter writer, CppFunction cppFunction, string command, string extension, bool voidReturn, bool stringReturn, string returnCsName, string signature)
        {
            string[] paramList = signature.Split(',', StringSplitOptions.RemoveEmptyEntries);

            WriteCsSummary(cppFunction.Comment, writer);
            string header;

            if (stringReturn)
            {
                header = $"public static string {extension}S({signature})";
            }
            else
            {
                header = $"public static {returnCsName} {extension}({signature})";
            }

            using (writer.PushBlock(header))
            {
                StringBuilder sb = new();
                if (!voidReturn)
                {
                    sb.Append($"{returnCsName} ret = ");
                }

                if (stringReturn)
                {
                    WriteStringConvertToManaged(sb, cppFunction.ReturnType);
                }

                sb.Append($"{settings.ApiName}.{command}Native(");
                int strings = 0;
                int stacks = 0;
                for (int j = 0; j < cppFunction.Parameters.Count; j++)
                {
                    var isRef = paramList[j].Contains("ref");
                    var isStr = paramList[j].Contains("string");
                    var cppParameter = cppFunction.Parameters[j];
                    var paramCsTypeName = settings.GetCsTypeName(cppParameter.Type, false);
                    var paramCsName = settings.GetParameterName(cppParameter.Type, cppParameter.Name);
                    if (isRef)
                    {
                        writer.BeginBlock($"fixed ({paramCsTypeName} p{paramCsName} = &{paramCsName})");
                        sb.Append($"p{paramCsName}");
                        stacks++;
                    }
                    else if (isStr)
                    {
                        WriteStringConvertToUnmanaged(writer, cppParameter.Type, paramCsName, strings);
                        sb.Append($"pStr{strings}");
                        strings++;
                    }
                    else
                    {
                        sb.Append(paramCsName);
                    }
                    if (j != cppFunction.Parameters.Count - 1)
                        sb.Append(", ");
                }

                if (stringReturn)
                    sb.Append("));");
                else
                    sb.Append(");");

                writer.WriteLine(sb.ToString());

                while (strings > 0)
                {
                    strings--;
                    writer.WriteLine($"Marshal.FreeHGlobal((nint)pStr{strings});");
                }

                if (!voidReturn)
                    writer.WriteLine("return ret;");

                while (stacks > 0)
                {
                    stacks--;
                    writer.EndBlock();
                }
            }

            writer.WriteLine();
        }
    }
}