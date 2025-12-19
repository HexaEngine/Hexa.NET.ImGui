namespace Generator
{
    using HexaGen;
    using HexaGen.CppAst.Model.Declarations;
    using HexaGen.Patching;
    using System.Collections.Generic;

    public class ImVectorPatch : PrePatch
    {
        public override void Apply(PatchContext context, CsCodeGeneratorConfig settings, List<string> files, ParseResult result)
        {
            base.Apply(context, settings, files, result);
        }

        protected override void PatchClass(CsCodeGeneratorConfig settings, CppClass cppClass)
        {
            if (cppClass.Name.StartsWith("ImVector_"))
            {
                var name = cppClass.Name;
                var nameValue = name[9..];
                switch (nameValue)
                {
                    case "const_charPtr":
                        nameValue = "ConstPointer<byte>";
                        break;

                    case "unsigned_char":
                        nameValue = "byte";
                        break;
                }
                if (settings.TypeMappings.TryGetValue(nameValue, out var mappedName))
                {
                    nameValue = mappedName;
                }

                settings.IgnoredTypes.Add(name);
                settings.IgnoredTypedefs.Add(name);
                settings.TypeMappings.TryAdd(name, $"ImVector<{nameValue}>");
            }
        }

        protected override void PatchTypedef(CsCodeGeneratorConfig settings, CppTypedef cppTypedef)
        {
            if (cppTypedef.Name.StartsWith("ImVector_"))
            {
                var name = cppTypedef.Name;
                var nameValue = name[9..];
                switch (nameValue)
                {
                    case "const_charPtr":
                        nameValue = "ConstPointer<byte>";
                        break;

                    case "unsigned_char":
                        nameValue = "byte";
                        break;
                }
                if (settings.TypeMappings.TryGetValue(nameValue, out var mappedName))
                {
                    nameValue = mappedName;
                }

                settings.IgnoredTypes.Add(name);
                settings.IgnoredTypedefs.Add(name);
                settings.TypeMappings.TryAdd(name, $"ImVector<{nameValue}>");
            }
        }
    }
}