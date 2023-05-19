namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;

    public unsafe class D3D11CommandList : DeviceChildBase, ICommandList
    {
        internal readonly ComPtr<ID3D11CommandList> commandList;

        public D3D11CommandList(ComPtr<ID3D11CommandList> commandList)
        {
            this.commandList = commandList;
            nativePointer = new(commandList);
        }

        protected override void DisposeCore()
        {
            commandList.Release();
        }
    }
}