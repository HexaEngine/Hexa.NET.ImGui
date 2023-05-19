namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.DirectXTex;

    public class D3DImage : IImage
    {
        public readonly Image Image;

        public D3DImage(Image image)
        {
            Image = image;
        }

        public int Width => (int)Image.Width;

        public int Height => (int)Image.Height;

        public Format Format => Helper.ConvertBack((Silk.NET.DXGI.Format)Image.Format);

        public int RowPitch => (int)Image.RowPitch;

        public int SlicePitch => (int)Image.SlicePitch;

        public unsafe byte* Pixels => Image.Pixels;
    }
}