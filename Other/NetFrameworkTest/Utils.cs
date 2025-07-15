namespace ExampleD3D11
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public unsafe struct Pointer<T> where T : unmanaged
    {
        public T* Value;

        public static implicit operator T*(Pointer<T> p) => p.Value;
        public static implicit operator Pointer<T>(T* p) => new() { Value = p };
    }

    public static unsafe class Utils
    {
        public static readonly Guid D3DDebugObjectName = new(0x429b8c22, 0x9188, 0x4b0c, 0x87, 0x42, 0xac, 0xb0, 0xbf, 0x85, 0xc2, 0x00);

        public static T* AllocT<T>() where T : unmanaged
        {
            return (T*)Marshal.AllocHGlobal(sizeof(T));
        }

        public static T* AllocT<T>(int count) where T : unmanaged
        {
            return (T*)Marshal.AllocHGlobal(sizeof(T) * count);
        }

        public static T* AllocT<T>(uint count) where T : unmanaged
        {
            return (T*)Marshal.AllocHGlobal((nint)(sizeof(T) * count));
        }

        public static T* ReAllocT<T>(T* ptr, uint count) where T : unmanaged
        {
            return (T*)Marshal.ReAllocHGlobal((nint)ptr, (nint)(sizeof(T) * count));
        }

        public static void Memcpy(void* src, void* dest, int destInBytes, int srcInBytes)
        {
            Buffer.MemoryCopy(src, dest, destInBytes, srcInBytes);
        }

        public static void MemcpyT<T>(T* src, T* dest, int destCount, int srcCount) where T : unmanaged
        {
            Buffer.MemoryCopy(src, dest, destCount * sizeof(T), srcCount * sizeof(T));
        }

        public static void Memcpy(void* src, void* dest, uint destInBytes, uint srcInBytes)
        {
            Buffer.MemoryCopy(src, dest, destInBytes, srcInBytes);
        }

        public static void MemcpyT<T>(T* src, T* dest, uint destCount, uint srcCount) where T : unmanaged
        {
            Buffer.MemoryCopy(src, dest, destCount * sizeof(T), srcCount * sizeof(T));
        }

        public static void Memcpy(void* src, void* dest, long destInBytes, long srcInBytes)
        {
            Buffer.MemoryCopy(src, dest, destInBytes, srcInBytes);
        }

        public static void MemcpyT<T>(T* src, T* dest, long destCount, long srcCount) where T : unmanaged
        {
            Buffer.MemoryCopy(src, dest, destCount * sizeof(T), srcCount * sizeof(T));
        }

        public static void Memcpy(void* src, void* dest, int sizeInBytes)
        {
            Buffer.MemoryCopy(src, dest, sizeInBytes, sizeInBytes);
        }

        public static void MemcpyT<T>(T* src, T* dest, int count) where T : unmanaged
        {
            Buffer.MemoryCopy(src, dest, count * sizeof(T), count * sizeof(T));
        }

        public static void Memcpy(void* src, void* dest, uint sizeInBytes)
        {
            Buffer.MemoryCopy(src, dest, sizeInBytes, sizeInBytes);
        }

        public static void MemcpyT<T>(T* src, T* dest, uint count) where T : unmanaged
        {
            Buffer.MemoryCopy(src, dest, count * sizeof(T), count * sizeof(T));
        }

        public static void Memcpy(void* src, void* dest, long sizeInBytes)
        {
            Buffer.MemoryCopy(src, dest, sizeInBytes, sizeInBytes);
        }

        public static void MemcpyT<T>(T* src, T* dest, long count) where T : unmanaged
        {
            Buffer.MemoryCopy(src, dest, count * sizeof(T), count * sizeof(T));
        }

        public static void MemsetT<T>(T* ptr, T value, int count) where T : unmanaged
        {
            new Span<T>(ptr, count).Fill(value);
        }

        public static void MemsetT<T>(T* ptr, byte value, int count) where T : unmanaged
        {
            new Span<byte>(ptr, count * sizeof(T)).Fill(value);
        }

        public static void Free(void* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }

        public static byte* ToUTF8Ptr(this string str)
        {
            int byteCount = Encoding.UTF8.GetByteCount(str);
            byte* ptr = AllocT<byte>(byteCount + 1);
            fixed (char* pStr = str)
            {
                Encoding.UTF8.GetBytes(pStr, str.Length, ptr, byteCount);
            }
            ptr[byteCount] = 0;
            return ptr;
        }

        public static void** AllocArray(int count)
        {
            return (void**)AllocT<nint>(count);
        }

        public static void** AllocArray(uint count)
        {
            return (void**)AllocT<nint>(count);
        }

        public static string ToStringFromUTF8(byte* ptr)
        {
            return Marshal.PtrToStringAnsi((IntPtr)ptr);
        }

        public static void ZeroMemoryT<T>(T* ptr) where T : unmanaged
        {
            new Span<byte>(ptr, sizeof(T)).Clear();
        }

        public static void ZeroMemoryT<T>(T* ptr, int count) where T : unmanaged
        {
            new Span<byte>(ptr, sizeof(T) * count).Clear();
        }

        public static void ZeroMemoryT<T>(T* ptr, uint count) where T : unmanaged
        {
            new Span<byte>(ptr, (int)(sizeof(T) * count)).Clear();
        }

        public static Guid* Guid(Guid guid)
        {
            return (Guid*)Unsafe.AsPointer(ref guid);
        }

        public static T2* Cast<T1, T2>(T1* t) where T1 : unmanaged where T2 : unmanaged
        {
            return (T2*)t;
        }

        public static byte* ToBytes(this string str)
        {
            return (byte*)Marshal.StringToHGlobalAnsi(str);
        }

        public static void ThrowHResult(this int code)
        {
            ResultCode resultCode = (ResultCode)code;
            if (resultCode != ResultCode.S_OK)
            {
                throw new D3D11Exception(resultCode);
            }
        }
    }
}