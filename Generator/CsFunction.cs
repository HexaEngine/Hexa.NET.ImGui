namespace Generator
{
    using System.Collections.Generic;

    public class CsFunction
    {
        public CsFunction(string name, string? comment, List<CsFunctionOverload> overloads)
        {
            Name = name;
            Comment = comment;
            Overloads = overloads;
        }

        public CsFunction(string name, string? comment)
        {
            Name = name;
            Comment = comment;
            Overloads = new();
        }

        public string Name { get; set; }

        public string? Comment { get; set; }

        public List<CsFunctionOverload> Overloads { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}