namespace HexaEngine.Core.Graphics
{
    public struct MappedSubresource
    {
        public unsafe void* PData;

        public uint RowPitch;

        public uint DepthPitch;

        public unsafe MappedSubresource(void* pData = null, uint? rowPitch = null, uint? depthPitch = null)
        {
            this = default(MappedSubresource);
            if (pData != null)
            {
                PData = pData;
            }

            if (rowPitch.HasValue)
            {
                RowPitch = rowPitch.Value;
            }

            if (depthPitch.HasValue)
            {
                DepthPitch = depthPitch.Value;
            }
        }

        public unsafe Span<T> AsSpan<T>(int length) where T : unmanaged
        {
            return new Span<T>(PData, length);
        }
    }
}