namespace HexaEngine.Core.Graphics
{
    public interface ITexture2D : IResource
    {
        public Texture2DDescription Description { get; }
    }
}