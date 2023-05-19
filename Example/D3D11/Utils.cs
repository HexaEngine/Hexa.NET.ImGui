namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Direct3D11;
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static unsafe class Utils
    {
        public static readonly Guid D3DDebugObjectName = new(0x429b8c22, 0x9188, 0x4b0c, 0x87, 0x42, 0xac, 0xb0, 0xbf, 0x85, 0xc2, 0x00);

        internal static string? GetDebugName(void* target)
        {
            ID3D11DeviceChild* child = (ID3D11DeviceChild*)target;
            if (child == null)
            {
                return null;
            }

            uint len;
            Guid guid = D3DDebugObjectName;
            child->GetPrivateData(&guid, &len, null);
            if (len == 0)
            {
                return string.Empty;
            }

            byte* pName = Alloc<byte>(len);
            child->GetPrivateData(&guid, &len, pName);
            string str = ToStr(pName, len);
            Free(pName);
            return str;
        }

        internal static void SetDebugName(void* target, string name)
        {
            ID3D11DeviceChild* child = (ID3D11DeviceChild*)target;
            if (child == null)
            {
                return;
            }

            Guid guid = D3DDebugObjectName;
            if (name != null)
            {
                byte* pName = name.ToUTF8();
                child->SetPrivateData(&guid, (uint)name.Length, pName);
                Free(pName);
            }
            else
            {
                child->SetPrivateData(&guid, 0, null);
            }
        }

        public static Guid* Guid(Guid guid)
        {
            return (Guid*)Unsafe.AsPointer(ref guid);
        }

        [Obsolete]
        public static T2** ToPointerArray<T1, T2>(T1[]? values) where T1 : IDeviceChild where T2 : unmanaged
        {
            if (values == null)
            {
                return null;
            }

            T2*[] ptrs = new T2*[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
#nullable disable
                ptrs[i] = (T2*)values[i]?.NativePointer;
#nullable enable
            }
            return AsPointer(ptrs);
        }

        [Obsolete]
        public static void** ToPointerArray<T1>(T1[] values) where T1 : IDeviceChild
        {
            void*[] ptrs = new void*[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                ptrs[i] = (void*)values[i].NativePointer;
            }
            fixed (void** ptr = ptrs)
            {
                return ptr;
            }
        }

        [Obsolete]
        public static T** AsPointer<T>(T* value) where T : unmanaged
        {
            fixed (T** ptr = new T*[] { value })
            {
                return ptr;
            }
        }

        [Obsolete]
        public static T** AsPointer<T>(T*[] value) where T : unmanaged
        {
            fixed (T** ptr = value)
            {
                return ptr;
            }
        }

        [Obsolete]
        public static void** AsPointerArray<T>(T*[] value) where T : unmanaged
        {
            fixed (T** ptr = value)
            {
                return (void**)ptr;
            }
        }

        [Obsolete]
        public static T* AsPointer<T>(T[] value) where T : unmanaged
        {
            fixed (T* ptr = value)
            {
                return ptr;
            }
        }

        [Obsolete]
        public static T* AsPointer<T>(T value) where T : unmanaged
        {
            fixed (T* ptr = new T[] { value })
            {
                return ptr;
            }
        }

        public static T2* Cast<T1, T2>(T1* t) where T1 : unmanaged where T2 : unmanaged
        {
            return (T2*)t;
        }

        public static byte* ToBytes(this string str)
        {
            return (byte*)Marshal.StringToHGlobalAnsi(str);
        }

        [Obsolete]
        public static float* ToFloatPtr(this Vector4 vector)
        {
            return (float*)&vector;
        }

        public static void ThrowHResult(this int code)
        {
            ResultCode resultCode = (ResultCode)code;
            if (resultCode != ResultCode.S_OK)
            {
                throw new D3D11Exception(resultCode);
            }
        }

        internal static string ToStr(byte* name)
        {
            var bytes = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(name);
            return Encoding.UTF8.GetString(bytes);
        }

        internal static string ToStr(byte* name, int length)
        {
            return Encoding.UTF8.GetString(new Span<byte>(name, length));
        }

        internal static string ToStr(byte* name, uint length)
        {
            return Encoding.UTF8.GetString(new Span<byte>(name, (int)length));
        }
    }
}