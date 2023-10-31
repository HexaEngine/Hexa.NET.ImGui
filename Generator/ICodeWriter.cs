namespace Generator
{
    using System;

    public interface ICodeWriter
    {
        int IndentLevel { get; }

        void BeginBlock(string content);
        void Dedent(int count = 1);
        void Dispose();
        void EndBlock();
        void Indent(int count = 1);
        IDisposable PushBlock(string marker = "{");
        void Write(char chr);
        void Write(string @string);
        void WriteLine();
        void WriteLine(string @string);
    }
}