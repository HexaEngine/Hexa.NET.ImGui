namespace HexaEngine.Core.Graphics
{
    public interface IComputePipeline : IDisposable
    {
        string Name { get; }

        void BeginDispatch(IGraphicsContext context);

        void Dispatch(IGraphicsContext context, int x, int y, int z);

        void EndDispatch(IGraphicsContext context);

        void Recompile();
    }
}