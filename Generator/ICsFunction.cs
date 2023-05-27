namespace Generator
{
    using System.Collections.Generic;

    public interface ICsFunction
    {
        string ExportedName { get; set; }
        bool IsConstructor { get; set; }
        bool IsDestructor { get; set; }
        bool IsMember { get; set; }
        string Name { get; set; }
        List<CsParameterInfo> Parameters { get; set; }
        CsType ReturnType { get; set; }
        string StructName { get; set; }

        string BuildSignature();
        bool HasParameter(CsParameterInfo cppParameter);
        string ToString();
    }
}