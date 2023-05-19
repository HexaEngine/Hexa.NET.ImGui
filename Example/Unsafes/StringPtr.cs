namespace HexaEngine.Core.Unsafes
{
    using HexaEngine.Mathematics;
    using System;
    using System.Buffers.Binary;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public unsafe struct UnsafeString : IFreeable
    {
        public UnsafeString(string str)
        {
            fixed (char* strPtr = str)
            {
                Ptr = strPtr;
            }
            Length = str.Length;
        }

        public UnsafeString(char* ptr, int length)
        {
            Ptr = ptr;
            Length = length;
        }

        public UnsafeString(int length)
        {
            Ptr = (char*)Marshal.AllocHGlobal(length * sizeof(char));
            Length = length;
        }

        public char* Ptr;
        public int Length;

        public bool Compare(UnsafeString* other)
        {
            if (Length != other->Length)
            {
                return false;
            }

            for (uint i = 0; i < Length; i++)
            {
                if (Ptr[i] != other->Ptr[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static int Write(UnsafeString* str, Endianness endianness, Span<byte> dest)
        {
            Span<char> srcChars = new(str->Ptr, str->Length + 1);
            Span<byte> src = MemoryMarshal.AsBytes(srcChars);
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dest, src.Length);
            }

            if (endianness == Endianness.BigEndian)
            {
                BinaryPrimitives.WriteInt32BigEndian(dest, src.Length);
            }

            src.CopyTo(dest[4..]);
            return src.Length + 4;
        }

        public static int Read(UnsafeString** ppStr, Endianness endianness, Span<byte> src)
        {
            int length = endianness == Endianness.LittleEndian ? BinaryPrimitives.ReadInt32LittleEndian(src) : BinaryPrimitives.ReadInt32BigEndian(src);
            UnsafeString* pStr = Alloc<UnsafeString>();

            *ppStr = pStr;
            pStr->Ptr = Alloc<char>(length);
            fixed (byte* srcPtr = src.Slice(4, length))
            {
                Unsafe.CopyBlock(pStr->Ptr, srcPtr, (uint)length);
            }
            pStr->Length = length;
            return length + 4;
        }

        public void Free()
        {
            Utils.Free(Ptr);
        }

        public int Sizeof()
        {
            return (Length + 1) * sizeof(char) + 4;
        }

        public static implicit operator string(UnsafeString ptr)
        {
            return new(ptr.Ptr);
        }

        public static implicit operator UnsafeString(string str)
        {
            return new(str);
        }

        public override string ToString()
        {
            return this;
        }
    }

    public unsafe struct UnsafeArray<T> where T : unmanaged
    {
        public UnsafeArray(T[] array)
        {
            fixed (T* ptr = array)
            {
                Ptr = ptr;
            }
            Length = array.Length;
        }

        public UnsafeArray(T* ptr, int length)
        {
            Ptr = ptr;
            Length = length;
        }

        public T* Ptr;
        public int Length;

        public static int Write(UnsafeArray<T>* str, Endianness endianness, Span<byte> dest)
        {
            Span<T> srcChars = new(str->Ptr, str->Length);
            Span<byte> src = MemoryMarshal.AsBytes(srcChars);
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dest, src.Length);
            }

            if (endianness == Endianness.BigEndian)
            {
                BinaryPrimitives.WriteInt32BigEndian(dest, src.Length);
            }

            src.CopyTo(dest[4..]);
            return src.Length + 4;
        }

        public static int Read(UnsafeArray<T>* str, Endianness endianness, Span<byte> src)
        {
            int length = endianness == Endianness.LittleEndian ? BinaryPrimitives.ReadInt32LittleEndian(src) : BinaryPrimitives.ReadInt32BigEndian(src);
            fixed (byte* srcPtr = src.Slice(4, length))
            {
                str->Ptr = (T*)srcPtr;
            }
            str->Length = length;
            return length + 4;
        }

        public int Sizeof()
        {
            return Length * sizeof(T) + 4;
        }

        public static implicit operator Span<T>(UnsafeArray<T> ptr)
        {
            return new Span<T>(ptr.Ptr, ptr.Length);
        }

        public static implicit operator UnsafeArray<T>(T[] str)
        {
            return new(str);
        }
    }

    public unsafe struct U16String : IFreeable
    {
        public U16String(string str)
        {
            int sizeInBytes = (str.Length + 1) * sizeof(char);
            Ptr = (char*)Marshal.AllocHGlobal(sizeInBytes);
            fixed (char* strPtr = str)
            {
                MemoryCopy(strPtr, Ptr, sizeInBytes, sizeInBytes);
            }
            Length = str.Length;
        }

        public U16String(char* ptr, nint length)
        {
            Ptr = ptr;
            Length = length;
        }

        public U16String(nint length)
        {
            nint sizeInBytes = length * sizeof(char);
            Ptr = (char*)Marshal.AllocHGlobal(sizeInBytes);
            Length = length;
            Zero(Ptr, sizeInBytes);
        }

        public char* Ptr;
        public nint Length;

        public char this[int index] { get => Ptr[index]; set => Ptr[index] = value; }

        public bool Compare(U16String* other)
        {
            if (Length != other->Length)
            {
                return false;
            }

            for (uint i = 0; i < Length; i++)
            {
                if (Ptr[i] != other->Ptr[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void Resize(int newSize)
        {
            int sizeInBytes = newSize * sizeof(char);
            var newPtr = (char*)Marshal.AllocHGlobal(sizeInBytes);
            MemoryCopy(Ptr, newPtr, sizeInBytes, sizeInBytes);
            Marshal.FreeHGlobal((nint)Ptr);
            Ptr = newPtr;
        }

        public void Free()
        {
            Utils.Free(Ptr);
        }

        public static implicit operator string(U16String ptr)
        {
            return new(ptr.Ptr);
        }

        public static implicit operator U16String(string str)
        {
            return new(str);
        }

        public override string ToString()
        {
            return this;
        }
    }

    public unsafe struct U8String : IFreeable
    {
        public U8String(string str)
        {
            int sizeInBytes = (str.Length + 1) * sizeof(byte);
            Ptr = (byte*)Marshal.AllocHGlobal(sizeInBytes);
            fixed (char* strPtr = str)
            {
                MemoryCopy(strPtr, Ptr, sizeInBytes, sizeInBytes);
            }
            Length = str.Length;
        }

        public U8String(byte* ptr, nint length)
        {
            Ptr = ptr;
            Length = length;
        }

        public U8String(nint length)
        {
            nint sizeInBytes = length * sizeof(byte);
            Ptr = (byte*)Marshal.AllocHGlobal(sizeInBytes);
            Length = length;
            Zero(Ptr, sizeInBytes);
        }

        public byte* Ptr;
        public nint Length;

        public byte this[int index] { get => Ptr[index]; set => Ptr[index] = value; }

        public bool Compare(U8String* other)
        {
            if (Length != other->Length)
            {
                return false;
            }

            for (uint i = 0; i < Length; i++)
            {
                if (Ptr[i] != other->Ptr[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void Resize(int newSize)
        {
            int sizeInBytes = newSize * sizeof(byte);
            var newPtr = (byte*)Marshal.AllocHGlobal(sizeInBytes);
            MemoryCopy(Ptr, newPtr, sizeInBytes, sizeInBytes);
            Marshal.FreeHGlobal((nint)Ptr);
            Ptr = newPtr;
        }

        public void Free()
        {
            Utils.Free(Ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(U8String ptr)
        {
            return Encoding.UTF8.GetString(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr.Ptr));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator U8String(string str)
        {
            return new(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte*(U8String str) => str.Ptr;

        public override string ToString()
        {
            return this;
        }
    }
}