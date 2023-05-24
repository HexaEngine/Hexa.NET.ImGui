namespace Generator
{
    using System;
    using System.Collections.Generic;

    public class CsFunctionVariation
    {
        public CsFunctionVariation(string exportedName, string name, string structName, bool isMember, bool isConstructor, bool isDestructor, CsType returnType, List<CsParameterInfo> parameters)
        {
            ExportedName = exportedName;
            Name = name;

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
            StructName = structName;
            IsMember = isMember;
            IsConstructor = isConstructor;
            IsDestructor = isDestructor;
            ReturnType = returnType;
            Parameters = new();
        }

        public string ExportedName { get; set; }

        public string Name { get; set; }

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

        public bool HasParameter(CsParameterInfo cppParameter)
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i].Name == cppParameter.Name)
                    return true;
            }
            return false;
        }
    }
}