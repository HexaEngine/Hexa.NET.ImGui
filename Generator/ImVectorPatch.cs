namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Patching;
    using System.Collections.Generic;

    public class ImVectorPatch : PrePatch
    {
        public override void Apply(PatchContext context, CsCodeGeneratorSettings settings, List<string> files, CppCompilation compilation)
        {
            base.Apply(context, settings, files, compilation);
        }

        protected override void PatchClass(CsCodeGeneratorSettings settings, CppClass cppClass)
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

        protected override void PatchTypedef(CsCodeGeneratorSettings settings, CppTypedef cppTypedef)
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