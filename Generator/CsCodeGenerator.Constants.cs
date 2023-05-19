namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public partial class CsCodeGenerator
    {
        private static void GenerateConstants(CppCompilation compilation, string outputPath)
        {
            /*
            CodeWriter writer = new CodeWriter(Path.Combine(outputPath, "Constants.cs"), "System");
            for (int i = 0; i < compilation.Macros.Count; i++)
            {
                var macro = compilation.Macros[i];
                var name = GetPrettyConstantName(macro.Name);
                var value = NormalizeConstantValue(macro.Value);

                if (value == string.Empty)
                    continue;
                writer.WriteLine();
            }
            */
        }

        private static string NormalizeConstantValue(string value)
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