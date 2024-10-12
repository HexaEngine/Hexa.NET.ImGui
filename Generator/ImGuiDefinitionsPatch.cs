namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Core.Mapping;
    using HexaGen.Patching;

    public class ImGuiDefinitionsIgnorePatch : PrePatch
    {
        public override void Apply(PatchContext context, CsCodeGeneratorConfig settings, List<string> files, CppCompilation compilation)
        {
            if (settings.LibName != "cimgui")
            {
                return;
            }

            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    settings.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }

            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                settings.IgnoredEnums.Add(enumDefinition.Names[0]);
            }

            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                settings.IgnoredTypes.Add(typeDefinition.Name);
            }

            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                settings.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    settings.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }
        }
    }

    public enum InternalsGenerationType
    {
        NoInternals,
        OnlyInternals,
        BothOrDontCare,
        SkipInternalsMetadata
    }

    public class ImGuiDefinitionsPatch : PrePatch
    {
        private readonly InternalsGenerationType generationType;
        private HashSet<string> names = [];

        public ImGuiDefinitionsPatch(InternalsGenerationType generationType)
        {
            this.generationType = generationType;
        }

        protected override void PatchCompilation(CsCodeGeneratorConfig settings, CppCompilation compilation)
        {
            if (settings.LibName == "backends")
            {
                return;
            }
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom(settings.LibName);

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];

                if (generationType != InternalsGenerationType.BothOrDontCare)
                {
                    bool isInternal = functionDefinition.Overloads.Any(x => x.Internal);
                    if (isInternal && generationType == InternalsGenerationType.SkipInternalsMetadata)
                    {
                        continue;
                    }

                    if (generationType == InternalsGenerationType.NoInternals && isInternal || generationType == InternalsGenerationType.OnlyInternals && !isInternal)
                    {
                        // removes internals.
                        for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                        {
                            var overload = functionDefinition.Overloads[j];
                            settings.IgnoredFunctions.Add(overload.ExportedName);
                        }

                        continue;
                    }
                }

                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];

                    var signature = $"{overload.ReturnType} {overload.FriendlyName} {overload.Args}";
                    bool useName = false;
                    if (!names.Contains(signature))
                    {
                        useName = true;
                        names.Add(signature);
                    }

                    settings.FunctionMappings.Add(new(overload.ExportedName, useName ? overload.FriendlyName : settings.GetCsFunctionName(overload.ExportedName), overload.Comment, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (settings.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            settings.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                    else if (overload.IsConstructor)
                    {
                        if (settings.KnownConstructors.TryGetValue(overload.StructName, out var knownConstructors))
                        {
                            if (!knownConstructors.Contains(overload.ExportedName))
                                knownConstructors.Add(overload.ExportedName);
                        }
                        else
                        {
                            settings.KnownConstructors.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                EnumMapping mapping = new(enumDefinition.Name, null, enumDefinition.Comment);
                for (int j = 0; j < enumDefinition.Members.Length; j++)
                {
                    var member = enumDefinition.Members[j];
                    EnumItemMapping valueMapping = new(member.Name, null, member.Comment, null);
                    mapping.ItemMappings.Add(valueMapping);
                }
                settings.EnumMappings.Add(mapping);
            }

            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefiniton = imguiDefinitions.Types[i];
                TypeMapping mapping = new(typeDefiniton.Name, null, typeDefiniton.Comment);
                for (int j = 0; j < typeDefiniton.Fields.Length; j++)
                {
                    var field = typeDefiniton.Fields[j];
                    TypeFieldMapping fieldMapping = new(field.Name, null, field.Comment);
                    mapping.FieldMappings.Add(fieldMapping);
                }
                settings.ClassMappings.Add(mapping);
            }
        }
    }
}