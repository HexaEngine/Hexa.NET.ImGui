namespace HexaEngine.Core.Graphics
{
    using System.ComponentModel;

    public struct GraphicsPipelineDesc
    {
        public GraphicsPipelineDesc()
        {
        }

        [DefaultValue(null)]
        public string? VertexShader = null;

        [DefaultValue("main")]
        public string VertexShaderEntrypoint = "main";

        [DefaultValue(null)]
        public string? HullShader = null;

        [DefaultValue("main")]
        public string HullShaderEntrypoint = "main";

        [DefaultValue(null)]
        public string? DomainShader = null;

        [DefaultValue("main")]
        public string DomainShaderEntrypoint = "main";

        [DefaultValue(null)]
        public string? GeometryShader = null;

        [DefaultValue("main")]
        public string GeometryShaderEntrypoint = "main";

        [DefaultValue(null)]
        public string? PixelShader = null;

        [DefaultValue("main")]
        public string PixelShaderEntrypoint = "main";
    }
}