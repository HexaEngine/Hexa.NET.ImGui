namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Core.CSharp;
    using HexaGen.FunctionGeneration;
    using HexaGen.FunctionGeneration.ParameterWriters;
    using HexaGen.GenerationSteps;
    using System.Collections.Generic;

    public class ImGuiCodeGenerator : CsCodeGenerator
    {
        public ImGuiCodeGenerator(CsCodeGeneratorConfig settings) : base(settings, new ImGuiFunctionGenerator(settings))
        {
            GetGenerationStep<FunctionGenerationStep>().OverwriteParameterWriter<StringParameterWriter>(new ImGuiStringParameterWriter());
        }

        protected override CppParserOptions PrepareSettings()
        {
            var settings = base.PrepareSettings();

            Environment.SetEnvironmentVariable("VCINSTALLDIR", @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Tools\MSVC\14.36.32532");
            Environment.SetEnvironmentVariable("VCToolsInstallDir", @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Tools\MSVC\14.36.32532\");

            return settings;
        }
    }

    public class ImGuiFunctionGenerator : FunctionGenerator
    {
        public ImGuiFunctionGenerator(CsCodeGeneratorConfig settings) : base(settings)
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
            // Only inject the condition if the return type is bool to optimize decoding behavior
            return context.Variation.ReturnType.IsBool ? "ret != 0" : null;
        }
    }
}