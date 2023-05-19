namespace HexaEngine.Core.Unsafes
{
    using System;

    public unsafe struct Pointer : IEquatable<Pointer>
    {
        public void* Data;

        public Pointer(void* pointer)
        {
            Data = pointer;
        }

        public static Pointer CreateFrom<T>(ref T t) where T : unmanaged
        {
            fixed (T* ptr = &t)
            {
                return new(ptr);
            }
        }

        public static Pointer CreateFrom<T>(T[] t) where T : unmanaged
        {
            fixed (T* ptr = &t[0])
            {
                return new(ptr);
            }
        }

        public Pointer<T> Cast<T>() where T : unmanaged
        {
            return new Pointer<T>((T*)Data);
        }

        public override bool Equals(object? obj)
        {
            return obj is Pointer pointer && Equals(pointer);
        }

        public bool Equals(Pointer other)
        {
            return Data == other.Data;
        }

        public static bool operator ==(Pointer left, Pointer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Pointer left, Pointer right)
        {
            return !(left == right);
        }

        public static implicit operator void*(Pointer pointer) => pointer.Data;

        public static implicit operator Pointer(void* pointer) => new(pointer);

        public static implicit operator IntPtr(Pointer pointer) => (IntPtr)pointer.Data;

        public static implicit operator Pointer(IntPtr pointer) => new((void*)pointer);

        public override int GetHashCode()
        {
            return ((IntPtr)Data).GetHashCode();
        }
    }

    public unsafe struct Pointer<T> : IEquatable<Pointer<T>> where T : unmanaged
    {
        public T* Data;

        public Pointer(T* pointer)
        {
            Data = pointer;
        }

        public bool IsNull => Data == null;

        public void Free()
        {
            Utils.Free(Data);
            Data = null;
        }

        public Pointer Cast()
        {
            return new Pointer(Data);
        }

        public Pointer<CastTo> Cast<CastTo>() where CastTo : unmanaged
        {
            return new Pointer<CastTo>((CastTo*)Data);
        }

        public override bool Equals(object? obj)
        {
            return obj is Pointer<T> pointer && Equals(pointer);
        }

        public bool Equals(Pointer<T> other)
        {
            return Data == other.Data;
        }

        public static bool operator ==(Pointer<T> left, Pointer<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Pointer<T> left, Pointer<T> right)
        {
            return !(left == right);
        }

        public static implicit operator T*(Pointer<T> pointer) => pointer.Data;

        public static implicit operator Pointer<T>(T* pointer) => new(pointer);

        public static implicit operator IntPtr(Pointer<T> pointer) => (IntPtr)pointer.Data;

        public static implicit operator Pointer<T>(IntPtr pointer) => new((T*)pointer);

        public static implicit operator Pointer<T>(Pointer pointer) => new((T*)pointer.Data);

        public static implicit operator Pointer(Pointer<T> pointer) => pointer.Data;

        public override int GetHashCode()
        {
            return ((IntPtr)Data).GetHashCode();
        }
    }

    public unsafe struct PointerPointer<T> where T : unmanaged
    {
        public T** Data;

        public PointerPointer(T** pointer)
        {
            Data = pointer;
        }

        public static implicit operator T**(PointerPointer<T> pointer) => pointer.Data;

        public static implicit operator PointerPointer<T>(T** pointer) => new(pointer);

        public static implicit operator IntPtr(PointerPointer<T> pointer) => (IntPtr)pointer.Data;

        public static implicit operator PointerPointer<T>(IntPtr pointer) => new((T**)pointer);
    }
}