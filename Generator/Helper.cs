namespace Generator
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Helper
    {
        public static void MergeVTable(string target, string source, int mergedVtableLength)
        {
            string vtTargetFile = Path.Combine(target, "Functions.VT.cs");
            string vtPatchFile = Path.Combine(source, "Functions.VT.cs");

            var vtContent = File.ReadAllText(vtTargetFile);
            var rootMatch = Regex.Match(vtContent, "vt = new VTable\\(LibraryLoader\\.LoadLibrary\\(\\), (.*)\\);");

            Regex loadPattern = new("vt.Load\\((.*), \"(.*)\"\\);", RegexOptions.Singleline);
            var matches = loadPattern.Matches(vtContent);
            var lastMatch = matches[^1];
            int start = lastMatch.Index + lastMatch.Length;

            var content = File.ReadAllText(vtPatchFile);
            File.Delete(vtPatchFile);

            StringBuilder builder = new(vtContent[..(start + 1)]);

            foreach (Match match in loadPattern.Matches(content))
            {
                builder.AppendLine($"\t\t\t" + match.Value);
            }

            builder.Remove(rootMatch.Index, rootMatch.Length);
            builder.Insert(rootMatch.Index, $"vt = new VTable(LibraryLoader.LoadLibrary(), {mergedVtableLength});");

            builder.Append(vtContent.AsSpan(start));

            var newContent = builder.ToString();

            File.WriteAllText(vtTargetFile, newContent);
        }
    }
}