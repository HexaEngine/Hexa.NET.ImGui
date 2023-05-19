namespace HexaEngine.Core.Graphics
{
    public unsafe interface IImage
    {
        public int Width { get; }

        public int Height { get; }

        public Format Format { get; }

        public int RowPitch { get; }

        public int SlicePitch { get; }

        public byte* Pixels { get; }
    }
}