namespace HexaEngine.Core.Graphics
{
    public interface IEffect : IDisposable
    {
        void Draw(IGraphicsContext context);

        void BeginResize();

        void EndResize(int width, int height);

        Task Initialize(IGraphicsDevice device, int width, int height);
    }
}