namespace Generator
{
    using HexaGen;
    using HexaGen.Core.CSharp;
    using HexaGen.CppAst.Model.Metadata;
    using HexaGen.FunctionGeneration;
    using HexaGen.FunctionGeneration.ParameterWriters;
    using HexaGen.GenerationSteps;
    using System.Collections.Generic;

    public class ImGuiCodeGenerator : CsCodeGenerator
    {
        public ImGuiCodeGenerator(CsCodeGeneratorConfig config) : base(config)
        {
        }

        protected override void OnConfigureGenerator()
        {
            FunctionGenerator = new ImGuiFunctionGenerator(config);
            GetGenerationStep<FunctionGenerationStep>().OverwriteParameterWriter<StringParameterWriter>(new ImGuiStringParameterWriter());
        }

        protected override void OnPostConfigure(CsCodeGeneratorConfig config)
        {
            config.LogLevel = HexaGen.Core.Logging.LogSeverity.Error;
            config.Defines.Add("IMGUI_USE_WCHAR32");
            config.Defines.Add("IMGUI_ENABLE_FREETYPE");
            config.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");
            LogLevel = HexaGen.Core.Logging.LogSeverity.Error;

            //Environment.SetEnvironmentVariable("VCINSTALLDIR", @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Tools\MSVC\14.36.32532");
            //Environment.SetEnvironmentVariable("VCToolsInstallDir", @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Tools\MSVC\14.36.32532\");
        }

        public override bool GenerateCore(CppCompilation compilation, List<string> headerFiles, string outputPath, List<string>? allowedHeaders = null)
        {
            return base.GenerateCore(compilation, headerFiles, outputPath, allowedHeaders);
        }
    }

    public class ImGuiFunctionGenerator : FunctionGenerator
    {
        public ImGuiFunctionGenerator(CsCodeGeneratorConfig config) : base(config)
        {
            AddRule(new FunctionGenRuleRef());
            AddRule(new FunctionGenRuleSpan());
            AddRule(new FunctionGenRuleString());
            AddStep(new ImGuiDefaultValueStep());
            AddStep(new ImGuiReturnVariationStep());
            AddStep(new StringReturnGenStep());
        }
    }

    public class ImGuiReturnVariationStep : ReturnVariationGenStep
    {
        // ImGui.GetContentRegionAvail(Vector2* pOut) => return Vector2 ImGui.GetContentRegionAvail()
        public override HashSet<string> AllowedParameterNames { get; } = new(["pOut"]);
    }

    public class ImGuiDefaultValueStep : DefaultValueGenStep
    {
        protected override void TransformDefaultValue(CsFunctionOverload function, CsFunctionVariation variation, CsParameterInfo parameter, ref string defaultValue)
        {
            // fix for enum values that are prefixed with the enum type name eg. ImAxis.-1
            if (defaultValue.StartsWith($"{parameter.Type.Name}."))
            {
                // check if the rest of the string is a number
                var span = defaultValue.AsSpan()[(parameter.Type.Name.Length + 1)..];
                foreach (char c in span)
                {
                    if (!char.IsDigit(c) && c != '-' && c != '.')
                    {
                        return;
                    }
                }
                defaultValue = span.ToString();
            }
        }
    }

    public class ImGuiStringParameterWriter : StringParameterWriter
    {
        protected override string? GetConvertBackCondition(FunctionWriterContext context, CsParameterInfo rootParameter, CsParameterInfo cppParameter, ParameterFlags paramFlags)
        {
            // Special case for input widgets to handle ImGuiInputTextFlags.EnterReturnsTrue.
            if (context.Variation.Parameters.Any(p => p.Name == "flags" && p.Type.Name == "ImGuiInputTextFlags" && p.DefaultValue != "0"))
            {
                return context.Variation.ReturnType.IsBool ? "ret != 0 || ((flags & ImGuiInputTextFlags.EnterReturnsTrue) != 0 && IsItemDeactivatedAfterEdit())" : null;
            }

            // Only inject the condition if the return type is bool to optimize decoding behavior
            return context.Variation.ReturnType.IsBool ? "ret != 0" : null;
        }
    }
}