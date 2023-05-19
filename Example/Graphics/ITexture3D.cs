namespace HexaEngine.Core.Graphics
{
    public interface ITexture3D : IResource
    {
        public Texture3DDescription Description { get; }
    }

    public interface IClassLinkage : IDeviceChild
    {
        IClassInstance CreateClassInstance(string name, uint cbOffset, uint cvOffset, uint texOffset, uint samplerOffset);

        IClassInstance GetClassInstance(string name, uint index);
    }

    public interface IClassInstance : IDeviceChild
    {
    }
}