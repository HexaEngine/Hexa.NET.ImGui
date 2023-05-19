namespace HexaEngine.Core.Graphics
{
    public struct BufferDescription
    {
        public int ByteWidth;
        public Usage Usage;
        public BindFlags BindFlags;
        public CpuAccessFlags CPUAccessFlags;
        public ResourceMiscFlag MiscFlags;
        public int StructureByteStride;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferDescription"/> struct.
        /// </summary>
        /// <param name="byteWidth">The size in bytes.</param>
        /// <param name="bindFlags">The bind flags.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="cpuAccessFlags">The CPU access flags.</param>
        /// <param name="miscFlags">The option flags.</param>
        /// <param name="structureByteStride">The structure byte stride.</param>
        public BufferDescription(int byteWidth,
            BindFlags bindFlags,
            Usage usage = Usage.Default,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.None,
            ResourceMiscFlag miscFlags = ResourceMiscFlag.None,
            int structureByteStride = 0)
        {
            ByteWidth = byteWidth;
            BindFlags = bindFlags;
            Usage = usage;
            CPUAccessFlags = cpuAccessFlags;
            MiscFlags = miscFlags;
            StructureByteStride = structureByteStride;
        }
    }
}