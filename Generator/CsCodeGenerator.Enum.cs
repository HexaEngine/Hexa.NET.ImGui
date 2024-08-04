namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public partial class CsCodeGenerator
    {
        public readonly HashSet<string> DefinedEnums = new();
        public readonly HashSet<string> LibDefinedEnums = new();

        private bool FilterEnum(CppEnum cppEnum)
        {
            if (settings.AllowedEnums.Count != 0 && !settings.AllowedEnums.Contains(cppEnum.Name))
                return true;
            if (settings.IgnoredEnums.Contains(cppEnum.Name))
                return true;

            if (LibDefinedEnums.Contains(cppEnum.Name))
                return true;

            if (DefinedEnums.Contains(cppEnum.Name))
                return true;

            DefinedEnums.Add(cppEnum.Name);
            return false;
        }

        public void GenerateEnums(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "HexaGen.Runtime" };

            string outDir = Path.Combine(outputPath, "Enums");
            string fileName = Path.Combine(outDir, "Enums.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            using var writer = new SplitCodeWriter(fileName, settings.Namespace, 2, usings.Concat(settings.Usings).ToArray());
            var createdEnums = new Dictionary<string, string>();

            foreach (CppEnum cppEnum in compilation.Enums)
            {
                if (FilterEnum(cppEnum))
                {
                    continue;
                }

                string csName = settings.GetCsCleanName(cppEnum.Name);
                string enumNamePrefix = settings.GetEnumNamePrefix(cppEnum.Name);
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
                        var enumItemName = settings.GetEnumItemName(cppEnum, enumItem.Name, enumNamePrefix);

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
                            string enumValueName = settings.GetEnumItemName(cppEnum, rawExpression.Text, enumNamePrefix);

                            if (settings.KnownEnumValueNames.TryGetValue(rawExpression.Text, out string? knownName))
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
    }
}