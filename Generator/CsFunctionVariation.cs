namespace Generator
{
    using System.Collections.Generic;

    public class CsFunctionVariation
    {
        public CsFunctionVariation(string exportedName, string name, Dictionary<string, string> defaultValues, string structName, bool isMember, bool isConstructor, bool isDestructor, CsType returnType, List<CsParameterInfo> parameters)
        {
            ExportedName = exportedName;
            Name = name;
            DefaultValues = defaultValues;
            StructName = structName;
            IsMember = isMember;
            IsConstructor = isConstructor;
            IsDestructor = isDestructor;
            ReturnType = returnType;
            Parameters = parameters;
        }

        public CsFunctionVariation(string exportedName, string name, string structName, bool isMember, bool isConstructor, bool isDestructor, CsType returnType)
        {
            ExportedName = exportedName;
            Name = name;
            DefaultValues = new();
            StructName = structName;
            IsMember = isMember;
            IsConstructor = isConstructor;
            IsDestructor = isDestructor;
            ReturnType = returnType;
            Parameters = new();
        }

        public string ExportedName { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> DefaultValues { get; set; }

        public string StructName { get; set; }

        public bool IsMember { get; set; }

        public bool IsConstructor { get; set; }

        public bool IsDestructor { get; set; }

        public CsType ReturnType { get; set; }

        public List<CsParameterInfo> Parameters { get; set; }

        public string BuildSignature()
        {
            return string.Join(", ", Parameters.Select(p => $"{p.Type.Name} {p.Name}"));
        }

        public override string ToString()
        {
            return BuildSignature();
        }
    }
}