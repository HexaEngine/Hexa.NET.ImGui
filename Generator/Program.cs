namespace Generator
{
    using CppAst;

    public class ArrayMapping
    {
        private CppPrimitiveKind primitive;
        private int size;
        private string name;

        public ArrayMapping(CppPrimitiveKind primitive, int size, string name)
        {
            this.primitive = primitive;
            this.size = size;
            this.name = name;
        }

        public CppPrimitiveKind Primitive { get => primitive; set => primitive = value; }

        public int Size { get => size; set => size = value; }

        public string Name { get => name; set => name = value; }
    }

    internal unsafe class Program
    {
        private static int Main(string[] args)
        {
            string headerFile = "cimgui/cimgui.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            var compilation = CppParser.ParseFile(headerFile, options);

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../HexaEngine.ImGui/Generated");

            return 0;
        }
    }
}