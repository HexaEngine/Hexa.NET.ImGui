namespace HexaEngine.ImGui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public static unsafe class Utils
    {
        public static T* Alloc<T>(int size) where T : unmanaged => (T*)Marshal.AllocHGlobal(size * sizeof(T));

        public static T** AllocArray<T>(nint size) where T : unmanaged => (T**)Marshal.AllocHGlobal(size * sizeof(T));

        public static void Free(void* ptr) => Marshal.FreeHGlobal((nint)ptr);

        public static void Free(void** ptr) => Marshal.FreeHGlobal((nint)ptr);

        public const int MaxStackallocSize = 2048;

        public static int GetByteCountUTF8(string str)
        {
            return Encoding.UTF8.GetByteCount(str);
        }

        public static int EncodeStringUTF8(string str, byte* data, int size)
        {
            return Encoding.UTF8.GetBytes(str, new Span<byte>(data, size));
        }

        public static string DecodeStringUTF8(byte* data)
        {
            return Encoding.UTF8.GetString(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(data));
        }

        public static int GetByteCountArray<T>(T[] array) => array.Length * sizeof(nuint);
    }
}