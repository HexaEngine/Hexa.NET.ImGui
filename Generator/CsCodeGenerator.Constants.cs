namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public partial class CsCodeGenerator
    {
        public static readonly HashSet<string> DefinedConstants = new();

        private static void GenerateConstants(CppCompilation compilation, string outputPath)
        {
            string outDir = Path.Combine(outputPath, "Constants");
            string fileName = Path.Combine(outDir, "Constants.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            using SplitCodeWriter writer = new(fileName, CsCodeGeneratorSettings.Default.Namespace, 2, "System");

            using (writer.PushBlock($"public unsafe partial class {CsCodeGeneratorSettings.Default.ApiName}"))
            {
                for (int i = 0; i < compilation.Macros.Count; i++)
                {
                    WriteConstant(writer, compilation.Macros[i]);
                }
            }
        }

        private static void WriteConstant(ICodeWriter writer, CppMacro macro)
        {
            if (DefinedConstants.Contains(macro.Name))
            {
                return;
            }
            DefinedConstants.Add(macro.Name);

            var name = GetPrettyConstantName(macro.Name);
            var value = NormalizeConstantValue(macro.Value);

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (value.IsNumeric(out var type))
            {
                writer.WriteLine($"public const {type.GetNumberType()} {name} = {value};");
                writer.WriteLine();
            }
            else if (value.IsString())
            {
                writer.WriteLine($"public const string {name} = {value};");
                writer.WriteLine();
            }
        }

        public static string NormalizeConstantValue(this string value)
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

            if (value.StartsWith("L\"") && value.StartsWith("R\"") && value.StartsWith("LR\"") && value.EndsWith("\"") && value.Count(c => c == '"') > 2)
            {
                string[] parts = value.Split('"', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                StringBuilder sb = new();
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (part == "L" || part == "R" || part == "LR")
                        continue;
                    sb.Append(part);
                }
                return $"@\"{sb}\"";
            }
            else
            {
                if (value.StartsWith("L\"") && value.EndsWith("\""))
                {
                    return value[1..];
                }

                if (value.StartsWith("R\"") && value.EndsWith("\""))
                {
                    return $"@{value[1..]}";
                }

                if (value.StartsWith("LR\"") && value.EndsWith("\""))
                {
                    var lines = value[3..^1].Split("\n");
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i] = lines[i].TrimEnd('\r');
                    }
                    return $"@\"{string.Join("\n", lines)}\"";
                }
            }

            return value.Replace("ULL", "UL");
        }

        private static string GetPrettyConstantName(string value)
        {
            if (CsCodeGeneratorSettings.Default.KnownConstantNames.TryGetValue(value, out string? knownName))
            {
                return knownName;
            }

            string[] parts = value.Split('_', StringSplitOptions.RemoveEmptyEntries).SelectMany(x => x.SplitByCase()).ToArray();

            bool capture = false;
            var sb = new StringBuilder();
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (CsCodeGeneratorSettings.Default.IgnoredParts.Contains(part))
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

            return sb.ToString();
        }
    }
}