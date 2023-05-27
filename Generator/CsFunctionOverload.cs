namespace Generator
{
    using System.Collections.Generic;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class CsFunctionOverload : ICsFunction
    {
        public CsFunctionOverload(string exportedName, string name, Dictionary<string, string> defaultValues, string structName, bool isMember, bool isConstructor, bool isDestructor, CsType returnType, List<CsParameterInfo> parameters, List<CsFunctionVariation> variations)
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
            Variations = variations;
        }

        public CsFunctionOverload(string exportedName, string name, string structName, bool isMember, bool isConstructor, bool isDestructor, CsType returnType)
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
            Variations = new();
        }

        public string ExportedName { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> DefaultValues { get; }

        public string StructName { get; set; }

        public bool IsMember { get; set; }

        public bool IsConstructor { get; set; }

        public bool IsDestructor { get; set; }

        public CsType ReturnType { get; set; }

        public List<CsParameterInfo> Parameters { get; set; }

        public List<CsFunctionVariation> Variations { get; set; }

        public bool HasVariation(CsFunctionVariation variation)
        {
            for (int i = 0; i < Variations.Count; i++)
            {
                var iation = Variations[i];
                if (variation.Parameters.Count != iation.Parameters.Count)
                    continue;
                if (variation.Name != iation.Name)
                    continue;

                bool skip = false;
                for (int j = 0; j < iation.Parameters.Count; j++)
                {
                    if (variation.Parameters[j].Type.Name != iation.Parameters[j].Type.Name)
                    {
                        skip = true;
                        break;
                    }
                }

                if (skip)
                    continue;

                return true;
            }

            return false;
        }

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

        public CsFunctionVariation CreateVariationWith()
        {
            return new(ExportedName, Name, StructName, IsMember, IsConstructor, IsDestructor, ReturnType);
        }
    }
}