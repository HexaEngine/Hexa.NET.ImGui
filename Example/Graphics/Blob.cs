namespace HexaEngine.Core.Graphics
{
    using System.Runtime.InteropServices;

    public unsafe class Blob : IDisposable
    {
        public Blob(IntPtr bufferPointer, PointerSize pointerSize)
        {
            BufferPointer = bufferPointer;
            PointerSize = pointerSize;
        }

        public Blob(void* bufferPointer, nuint pointerSize)
        {
            BufferPointer = new(bufferPointer);
            PointerSize = (int)pointerSize;
        }

        public Blob(byte[] data)
        {
            BufferPointer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, BufferPointer, data.Length);
            PointerSize = data.Length;
        }

        public Blob(Span<byte> data)
        {
            BufferPointer = Marshal.AllocHGlobal(data.Length);
            PointerSize = data.Length;
            fixed (byte* ptr = data)
            {
                Buffer.MemoryCopy(ptr, (void*)BufferPointer, PointerSize, PointerSize);
            }
        }

        public IntPtr BufferPointer { get; }

        public PointerSize PointerSize { get; }

        public string AsString()
        {
            return Marshal.PtrToStringAnsi(BufferPointer) ?? string.Empty;
        }

        public byte[] AsBytes()
        {
            return new Span<byte>((void*)BufferPointer, PointerSize).ToArray();
        }

        public Span<byte> AsSpan()
        {
            return new Span<byte>((void*)BufferPointer, PointerSize);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(BufferPointer);
            GC.SuppressFinalize(this);
        }
    }
}