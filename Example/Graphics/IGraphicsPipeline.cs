namespace HexaEngine.Core.Graphics
{
    public interface IGraphicsPipeline : IDisposable
    {
        GraphicsPipelineDesc Description { get; }

        string DebugName { get; }

        GraphicsPipelineState State { get; set; }

        void BeginDraw(IGraphicsContext context);

        void EndDraw(IGraphicsContext context);

        void Recompile();

        /// <summary>
        /// Indicates that the pipeline is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Indicates if one or more shaders have errors.
        /// </summary>
        bool IsValid { get; }
    }
}