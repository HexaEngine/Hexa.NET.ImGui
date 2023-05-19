namespace HexaEngine.D3D11
{
    using HexaEngine.Core;
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;

    public unsafe class D3D11ClassLinkage : DeviceChildBase, IClassLinkage
    {
        internal readonly ComPtr<ID3D11ClassLinkage> classLinkage;

        public D3D11ClassLinkage(ComPtr<ID3D11ClassLinkage> classLinkage)
        {
            this.classLinkage = classLinkage;
            nativePointer = new(classLinkage);
        }

        public IClassInstance CreateClassInstance(string name, uint cbOffset, uint cvOffset, uint texOffset, uint samplerOffset)
        {
            ID3D11ClassInstance* instance;
            byte* pName = name.ToUTF8();
            classLinkage.CreateClassInstance(pName, cbOffset, cvOffset, texOffset, samplerOffset, &instance);
            Free(pName);
            return new D3D11ClassInstance(instance);
        }

        public IClassInstance GetClassInstance(string name, uint index)
        {
            ID3D11ClassInstance* instance;
            byte* pName = name.ToUTF8();
            classLinkage.GetClassInstance(pName, index, &instance);
            Free(pName);
            return new D3D11ClassInstance(instance);
        }

        protected override void DisposeCore()
        {
            classLinkage.Release();
        }
    }
}