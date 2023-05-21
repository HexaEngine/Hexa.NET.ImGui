namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public partial class CsCodeGenerator
    {
        private static readonly HashSet<string> s_definedExtensions = new();

        private static void GenerateExtensions(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Runtime.CompilerServices", "System.Runtime.InteropServices" };
            // Generate Functions
            using var writer = new CodeWriter(Path.Combine(outputPath, "Extensions.cs"), usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());
            using (writer.PushBlock($"public static unsafe class Extensions"))
            {
                for (int i = 0; i < compilation.Typedefs.Count; i++)
                {
                    CppTypedef typedef = compilation.Typedefs[i];
                    if (CsCodeGeneratorSettings.Default.IgnoredTypedefs.Contains(typedef.Name))
                        continue;
                    if (s_definedExtensions.Contains(typedef.Name))
                        continue;
                    s_definedExtensions.Add(typedef.Name);
                    if (typedef.ElementType is not CppPointerType)
                    {
                        continue;
                    }

                    if (IsDelegate(typedef, out _))
                    {
                        continue;
                    }

                    for (int j = 0; j < compilation.Functions.Count; j++)
                    {
                        var cppFunction = compilation.Functions[j];
                        if (CsCodeGeneratorSettings.Default.AllowedFunctions.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedFunctions.Contains(cppFunction.Name))
                            continue;
                        if (CsCodeGeneratorSettings.Default.IgnoredFunctions.Contains(cppFunction.Name))
                            continue;
                        if (cppFunction.Parameters.Count == 0 || cppFunction.Parameters[0].Type.TypeKind == CppTypeKind.Pointer)
                            continue;

                        if (cppFunction.Parameters[0].Type.GetDisplayName().Contains(typedef.GetDisplayName()))
                        {
                            var extensionPrefix = GetExtensionNamePrefix(typedef.Name);
                            var csFunctionName = GetPrettyCommandName(cppFunction.Name);
                            var csExtensionName = GetPrettyExtensionName(csFunctionName, extensionPrefix);
                            bool canUseOut = s_outReturnFunctions.Contains(cppFunction.Name);
                            var argumentsString = GetParameterSignature(cppFunction, canUseOut);
                            var sigs = GetVariantParameterSignatures(cppFunction.Name, cppFunction.Parameters, argumentsString, canUseOut);
                            sigs.Add(argumentsString);

                            WriteExtensions(writer, cppFunction, csFunctionName, csExtensionName, sigs);
                        }
                    }
                }
            }
        }

        private static void WriteExtensions(CodeWriter writer, CppFunction cppFunction, string command, string extension, List<string> signatures)
        {
            bool voidReturn = IsVoid(cppFunction.ReturnType);
            bool stringReturn = IsString(cppFunction.ReturnType);
            string returnCsName = GetCsTypeName(cppFunction.ReturnType, false);

            for (int i = 0; i < signatures.Count; i++)
            {
                string signature = "this " + signatures[i];

                if (stringReturn)
                    WriteExtensionMethod(writer, cppFunction, command, extension, voidReturn, true, "string", signature);

                WriteExtensionMethod(writer, cppFunction, command, extension, voidReturn, false, returnCsName, signature);
            }
        }

        private static void WriteExtensionMethod(CodeWriter writer, CppFunction cppFunction, string command, string extension, bool voidReturn, bool stringReturn, string returnCsName, string signature)
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

                sb.Append($"{CsCodeGeneratorSettings.Default.ApiName}.{command}Native(");
                int strings = 0;
                int stacks = 0;
                for (int j = 0; j < cppFunction.Parameters.Count; j++)
                {
                    var isRef = paramList[j].Contains("ref");
                    var isStr = paramList[j].Contains("string");
                    var cppParameter = cppFunction.Parameters[j];
                    var paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                    var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);
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

        public static string GetExtensionNamePrefix(string typeName)
        {
            if (CsCodeGeneratorSettings.Default.KnownExtensionPrefixes.TryGetValue(typeName, out string? knownValue))
            {
                return knownValue;
            }

            string[] parts = typeName.Split('_', StringSplitOptions.RemoveEmptyEntries).SelectMany(x => x.SplitByCase()).ToArray();

            return string.Join("_", parts.Select(s => s.ToUpper()));
        }

        public static string GetPrettyExtensionName(string value, string extensionPrefix)
        {
            if (CsCodeGeneratorSettings.Default.KnownExtensionNames.TryGetValue(value, out string? knownName))
            {
                return knownName;
            }

            string[] parts = value.Split('_', StringSplitOptions.RemoveEmptyEntries).SelectMany(x => x.SplitByCase()).ToArray();
            string[] prefixParts = extensionPrefix.Split('_', StringSplitOptions.RemoveEmptyEntries);

            bool capture = false;
            var sb = new StringBuilder();
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (prefixParts.Contains(part, StringComparer.InvariantCultureIgnoreCase) && !capture)
                {
                    continue;
                }

                part = part.ToLower();

                sb.Append(char.ToUpper(part[0]));
                sb.Append(part[1..]);
                capture = true;
            }

            if (sb.Length == 0)
                sb.Append(value);

            string prettyName = sb.ToString();
            return (char.IsNumber(prettyName[0])) ? prefixParts[^1].ToCamelCase() + prettyName : prettyName;
        }
    }
}