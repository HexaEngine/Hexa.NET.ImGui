namespace Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class TextExtensions
    {
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