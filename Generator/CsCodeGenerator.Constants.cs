namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public partial class CsCodeGenerator
    {
        public readonly HashSet<string> DefinedConstants = new();
        public readonly HashSet<string> LibDefinedConstants = new();

        private bool FilterConstant(CppMacro macro)
        {
            if (LibDefinedConstants.Contains(macro.Name))
            {
                return true;
            }

            if (DefinedConstants.Contains(macro.Name))
            {
                return true;
            }
            DefinedConstants.Add(macro.Name);
            return false;
        }

        private void GenerateConstants(CppCompilation compilation, string outputPath)
        {
            string[] usings = ["System", "HexaGen.Runtime"];

            string outDir = Path.Combine(outputPath, "Constants");
            string fileName = Path.Combine(outDir, "Constants.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            using SplitCodeWriter writer = new(fileName, settings.Namespace, 2, usings.Concat(settings.Usings).ToArray());

            using (writer.PushBlock($"public unsafe partial class {settings.ApiName}"))
            {
                for (int i = 0; i < compilation.Macros.Count; i++)
                {
                    WriteConstant(writer, compilation.Macros[i]);
                }
            }
        }

        private void WriteConstant(ICodeWriter writer, CppMacro macro)
        {
            if (FilterConstant(macro))
            {
                return;
            }

            var name = settings.GetPrettyConstantName(macro.Name);
            var value = macro.Value.NormalizeConstantValue();

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
    }
}