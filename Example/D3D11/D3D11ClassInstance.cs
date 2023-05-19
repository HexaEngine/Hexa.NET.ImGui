namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;

    public unsafe class D3D11ClassInstance : DeviceChildBase, IClassInstance
    {
        internal readonly ComPtr<ID3D11ClassInstance> classInstance;

        public D3D11ClassInstance(ComPtr<ID3D11ClassInstance> classInstance)
        {
            this.classInstance = classInstance;
            nativePointer = new(classInstance);
        }

        protected override void DisposeCore()
        {
            classInstance.Release();
        }
    }
}