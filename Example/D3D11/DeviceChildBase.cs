namespace HexaEngine.D3D11
{
    using HexaEngine.Core;
    using HexaEngine.Core.Graphics;
    using Silk.NET.Direct3D11;
    using System;

    public abstract unsafe class DeviceChildBase : DisposableBase, IDeviceChild
    {
        protected IntPtr nativePointer;

        public static readonly Guid D3DDebugObjectName = new(0x429b8c22, 0x9188, 0x4b0c, 0x87, 0x42, 0xac, 0xb0, 0xbf, 0x85, 0xc2, 0x00);

        public virtual string? DebugName
        {
            get
            {
                ID3D11DeviceChild* child = (ID3D11DeviceChild*)nativePointer;
                if (child == null)
                {
                    return null;
                }

                uint len;
                Guid guid = D3DDebugObjectName;
                child->GetPrivateData(&guid, &len, null);
                byte* pName = Alloc<byte>(len);
                child->GetPrivateData(&guid, &len, pName);
                string str = Utils.ToStr(pName, len);
                Free(pName);
                return str;
            }
            set
            {
                ID3D11DeviceChild* child = (ID3D11DeviceChild*)nativePointer;
                if (child == null)
                {
                    return;
                }

                Guid guid = D3DDebugObjectName;
                if (value != null)
                {
                    byte* pName = value.ToUTF8();
                    child->SetPrivateData(&guid, (uint)value.Length, pName);
                    Free(pName);
                }
                else
                {
                    child->SetPrivateData(&guid, 0, null);
                }
            }
        }

        public IntPtr NativePointer => nativePointer;
    }
}