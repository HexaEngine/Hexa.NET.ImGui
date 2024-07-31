namespace Generator
{
    using CppAst;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static partial class CsCodeGenerator
    {
        public static readonly HashSet<string> DefinedEnums = new();

        public static void GenerateEnums(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System" };

            string outDir = Path.Combine(outputPath, "Enums");
            string fileName = Path.Combine(outDir, "Enums.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            using var writer = new SplitCodeWriter(fileName, CsCodeGeneratorSettings.Default.Namespace, 2, usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());
            var createdEnums = new Dictionary<string, string>();

            foreach (CppEnum cppEnum in compilation.Enums)
            {
                if (CsCodeGeneratorSettings.Default.AllowedEnums.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedEnums.Contains(cppEnum.Name))
                    continue;
                if (CsCodeGeneratorSettings.Default.IgnoredEnums.Contains(cppEnum.Name))
                    continue;

                if (DefinedEnums.Contains(cppEnum.Name))
                    continue;
                DefinedEnums.Add(cppEnum.Name);

                string csName = GetCsCleanName(cppEnum.Name);
                string enumNamePrefix = GetEnumNamePrefix(cppEnum.Name);
                if (csName.EndsWith("_"))
                {
                    csName = csName.Remove(csName.Length - 1);
                }

                // Remove extension suffix from enum item values
                string extensionPrefix = "";

                createdEnums.Add(csName, cppEnum.Name);

                bool noneAdded = false;
                WriteCsSummary(cppEnum.Comment, writer);
                using (writer.PushBlock($"public enum {csName}"))
                {
                    for (int i = 0; i < cppEnum.Items.Count; i++)
                    {
                        CppEnumItem? enumItem = cppEnum.Items[i];
                        var enumItemName = GetEnumItemName(cppEnum, enumItem.Name, enumNamePrefix);

                        if (!string.IsNullOrEmpty(extensionPrefix) && enumItemName.EndsWith(extensionPrefix))
                        {
                            enumItemName = enumItemName.Remove(enumItemName.Length - extensionPrefix.Length);
                        }

                        if (enumItemName == "None" && noneAdded)
                        {
                            continue;
                        }

                        var commentWritten = WriteCsSummary(enumItem.Comment, writer);
                        if (enumItem.ValueExpression is CppRawExpression rawExpression)
                        {
                            string enumValueName = GetEnumItemName(cppEnum, rawExpression.Text, enumNamePrefix);

                            if (CsCodeGeneratorSettings.Default.KnownEnumValueNames.TryGetValue(rawExpression.Text, out string? knownName))
                            {
                                enumValueName = knownName;
                            }

                            if (enumItem.Name == rawExpression.Text)
                            {
                                writer.WriteLine($"{enumItemName} = {i},");
                                continue;
                            }

                            if (!string.IsNullOrEmpty(extensionPrefix) && enumValueName.EndsWith(extensionPrefix))
                            {
                                enumValueName = enumValueName.Remove(enumValueName.Length - extensionPrefix.Length);

                                if (enumItemName == enumValueName)
                                    continue;
                            }

                            if (rawExpression.Kind == CppExpressionKind.Unexposed)
                            {
                                writer.WriteLine($"{enumItemName} = unchecked((int){enumValueName.Replace("_", "")}),");
                            }
                            else
                            {
                                writer.WriteLine($"{enumItemName} = {enumValueName},");
                            }
                        }
                        else
                        {
                            writer.WriteLine($"{enumItemName} = unchecked({enumItem.Value}),");
                        }

                        if (commentWritten)
                            writer.WriteLine();
                    }
                }

                writer.WriteLine();
            }
        }

        private static string GetEnumItemName(CppEnum @enum, string cppEnumItemName, string enumNamePrefix)
        {
            string enumItemName = GetPrettyEnumName(cppEnumItemName, enumNamePrefix);

            return enumItemName;
        }

        private static string NormalizeEnumValue(string value)
        {
            if (value == "(~0U)")
            {
                return "~0u";
            }

            if (value == "(~0ULL)")
            {
                return "~0ul";
            }

            if (value == "(~0U-1)")
            {
                return "~0u - 1";
            }

            if (value == "(~0U-2)")
            {
                return "~0u - 2";
            }

            if (value == "(~0U-3)")
            {
                return "~0u - 3";
            }

            return value.Replace("ULL", "UL");
        }

        public static string GetEnumNamePrefix(string typeName)
        {
            if (CsCodeGeneratorSettings.Default.KnownEnumPrefixes.TryGetValue(typeName, out string? knownValue))
            {
                return knownValue;
            }

            string[] parts = typeName.Split('_', StringSplitOptions.RemoveEmptyEntries).SelectMany(x => x.SplitByCase()).ToArray();

            return string.Join("_", parts.Select(s => s.ToUpper()));
        }

        private static string GetPrettyEnumName(string value, string enumPrefix)
        {
            if (value.StartsWith("0x"))
                return value;

            string[] parts = value.Split('_', StringSplitOptions.RemoveEmptyEntries).SelectMany(x => x.SplitByCase()).ToArray();
            string[] prefixParts = enumPrefix.Split('_', StringSplitOptions.RemoveEmptyEntries);

            bool capture = false;
            var sb = new StringBuilder();
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (CsCodeGeneratorSettings.Default.IgnoredParts.Contains(part, StringComparer.InvariantCultureIgnoreCase) || (prefixParts.Contains(part, StringComparer.InvariantCultureIgnoreCase) && !capture))
                {
                    continue;
                }

                part = part.ToLower();

                sb.Append(char.ToUpper(part[0]));
                sb.Append(part[1..]);
                capture = true;
            }

            if (sb.Length == 0)
                sb.Append(prefixParts[^1].ToCamelCase());

            string prettyName = sb.ToString();
            return (char.IsNumber(prettyName[0])) ? prefixParts[^1].ToCamelCase() + prettyName : prettyName;
        }

        public static unsafe string ToCamelCase(this string str)
        {
            string output = new('\0', str.Length);
            fixed (char* p = output)
            {
                p[0] = char.ToUpper(str[0]);
                for (int i = 1; i < str.Length; i++)
                {
                    p[i] = char.ToLower(str[i]);
                }
            }
            return output;
        }

        public static string[] SplitByCase(this string s)
        {
            var ʀ = new List<string>();
            var ᴛ = new StringBuilder();
            var previous = SplitByCaseModes.None;
            foreach (var ɪ in s)
            {
                SplitByCaseModes mode_ɪ;
                if (string.IsNullOrWhiteSpace(ɪ.ToString()))
                {
                    mode_ɪ = SplitByCaseModes.WhiteSpace;
                }
                else if ("0123456789".Contains(ɪ))
                {
                    mode_ɪ = SplitByCaseModes.Digit;
                }
                else if (ɪ == ɪ.ToString().ToUpper()[0])
                {
                    mode_ɪ = SplitByCaseModes.UpperCase;
                }
                else
                {
                    mode_ɪ = SplitByCaseModes.LowerCase;
                }
                if ((previous == SplitByCaseModes.None) || (previous == mode_ɪ))
                {
                    ᴛ.Append(ɪ);
                }
                else if ((previous == SplitByCaseModes.UpperCase) && (mode_ɪ == SplitByCaseModes.LowerCase))
                {
                    if (ᴛ.Length > 1)
                    {
                        ʀ.Add(ᴛ.ToString().Substring(0, ᴛ.Length - 1));
                        ᴛ.Remove(0, ᴛ.Length - 1);
                    }
                    ᴛ.Append(ɪ);
                }
                else
                {
                    ʀ.Add(ᴛ.ToString());
                    ᴛ.Clear();
                    ᴛ.Append(ɪ);
                }
                previous = mode_ɪ;
            }
            if (ᴛ.Length != 0) ʀ.Add(ᴛ.ToString());
            return ʀ.ToArray();
        }

        private enum SplitByCaseModes
        { None, WhiteSpace, Digit, UpperCase, LowerCase }
    }
}