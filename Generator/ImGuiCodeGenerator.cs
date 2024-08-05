namespace Generator
{
    using HexaGen;
    using HexaGen.Core.CSharp;
    using System.Collections.Generic;

    public class ImGuiCodeGenerator : CsCodeGenerator
    {
        public ImGuiCodeGenerator(CsCodeGeneratorSettings settings) : base(settings, new ImGuiFunctionGenerator(settings))
        {
        }

        public class ImGuiFunctionGenerator : FunctionGenerator
        {
            public ImGuiFunctionGenerator(CsCodeGeneratorSettings settings) : base(settings)
            {
                OverwriteStep<ReturnVariationGenStep>(new ImGuiReturnVariationStep());
                OverwriteStep<DefaultValueGenStep>(new ImGuiDefaultValueStep());
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
    }
}