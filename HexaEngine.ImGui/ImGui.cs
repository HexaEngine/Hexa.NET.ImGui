namespace HexaEngine.ImGuiNET
{
    using System.Numerics;
    using System.Runtime.InteropServices;

    public static unsafe partial class ImGui
    {
        static ImGui()
        {
            LibraryLoader.SetImportResolver();
        }

        [LibraryImport(LibName, EntryPoint = "igInputText")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial byte InputTextNative(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData);

        public static bool InputText(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextNative(label, buf, bufSize, flags, callback, userData);
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextNative(label, buf, bufSize, flags, callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte ret = InputTextNative(label, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize)
        {
            byte ret = InputTextNative(label, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextNative(label, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte ret = InputTextNative(label, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize, void* userData)
        {
            byte ret = InputTextNative(label, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputText(byte* label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextNative(label, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            return ret != 0;
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputText(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextNative((byte*)plabel, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, flags, callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextNative(label, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(byte* label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextNative(label, pStr0, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, flags, callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputText(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextNative((byte*)plabel, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputText(string label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextNative(pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        [LibraryImport(LibName, EntryPoint = "igInputTextMultiline")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial byte InputTextMultilineNative(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData);

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, flags, callback, userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, flags, callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextMultilineNative(label, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
            return ret != 0;
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(ref byte label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextMultilineNative((byte*)plabel, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, flags, callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, buf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextMultilineNative(label, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(byte* label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextMultilineNative(label, pStr0, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, flags, callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(ref byte label, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextMultilineNative((byte*)plabel, (byte*)pbuf, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, Vector2 size, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, size, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextMultiline(string label, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextMultilineNative(pStr0, pStr1, bufSize, (Vector2)(new Vector2(0, 0)), flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        [LibraryImport(LibName, EntryPoint = "igInputTextWithHint")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial byte InputTextWithHintNative(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData);

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, flags, callback, userData);
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, flags, callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, void* userData)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextWithHintNative(label, hint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            return ret != 0;
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextWithHintNative((byte*)plabel, hint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, flags, callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextWithHintNative(label, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, flags, callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, flags, callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, flags, callback, userData);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, flags, callback, (void*)(default));
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, byte* buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, buf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, flags, callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextWithHintNative(label, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextWithHintNative(label, hint, pStr0, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, flags, callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, byte* hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative((byte*)plabel, hint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, byte* hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, hint, pStr1, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, flags, callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextWithHintNative(label, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(byte* label, string hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextWithHintNative(label, pStr0, pStr1, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, flags, callback, userData);
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, flags, callback, (void*)(default));
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(ref byte label, ref byte hint, ref byte buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextWithHintNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, flags, callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, flags, (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, (ImGuiInputTextFlags)(0), callback, (void*)(default));
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, ImGuiInputTextFlags flags, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, flags, (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, (ImGuiInputTextFlags)(0), (ImGuiInputTextCallback)(default), userData);
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextWithHint(string label, string hint, ref string buf, nuint bufSize, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextWithHintNative(pStr0, pStr1, pStr2, bufSize, (ImGuiInputTextFlags)(0), callback, userData);
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igInputTextEx")]
        internal static extern byte InputTextExNative(byte* label, byte* hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData);

        public static bool InputTextEx(byte* label, byte* hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte ret = InputTextExNative(label, hint, buf, bufSize, sizeArg, flags, callback, userData);
            return ret != 0;
        }

        public static bool InputTextEx(ref byte label, byte* hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = InputTextExNative((byte*)plabel, hint, buf, bufSize, sizeArg, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextEx(string label, byte* hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextExNative(pStr0, hint, buf, bufSize, sizeArg, flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextEx(byte* label, ref byte hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                byte ret = InputTextExNative(label, (byte*)phint, buf, bufSize, sizeArg, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextEx(byte* label, string hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextExNative(label, pStr0, buf, bufSize, sizeArg, flags, callback, userData);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextEx(ref byte label, ref byte hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    byte ret = InputTextExNative((byte*)plabel, (byte*)phint, buf, bufSize, sizeArg, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextEx(string label, string hint, byte* buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextExNative(pStr0, pStr1, buf, bufSize, sizeArg, flags, callback, userData);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextEx(byte* label, byte* hint, ref byte buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = InputTextExNative(label, hint, (byte*)pbuf, bufSize, sizeArg, flags, callback, userData);
                return ret != 0;
            }
        }

        public static bool InputTextEx(byte* label, byte* hint, ref string buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = InputTextExNative(label, hint, pStr0, bufSize, sizeArg, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextEx(ref byte label, byte* hint, ref byte buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextExNative((byte*)plabel, hint, (byte*)pbuf, bufSize, sizeArg, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextEx(string label, byte* hint, ref string buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextExNative(pStr0, hint, pStr1, bufSize, sizeArg, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextEx(byte* label, ref byte hint, ref byte buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* phint = &hint)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = InputTextExNative(label, (byte*)phint, (byte*)pbuf, bufSize, sizeArg, flags, callback, userData);
                    return ret != 0;
                }
            }
        }

        public static bool InputTextEx(byte* label, string hint, ref string buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (hint != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(hint);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(hint, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = InputTextExNative(label, pStr0, pStr1, bufSize, sizeArg, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool InputTextEx(ref byte label, ref byte hint, ref byte buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* phint = &hint)
                {
                    fixed (byte* pbuf = &buf)
                    {
                        byte ret = InputTextExNative((byte*)plabel, (byte*)phint, (byte*)pbuf, bufSize, sizeArg, flags, callback, userData);
                        return ret != 0;
                    }
                }
            }
        }

        public static bool InputTextEx(string label, string hint, ref string buf, int bufSize, Vector2 sizeArg, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* userData)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (hint != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(hint);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(hint, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* pStr2 = null;
            int pStrSize2 = 0;
            if (buf != null)
            {
                pStrSize2 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize2 >= Utils.MaxStackallocSize)
                {
                    pStr2 = Utils.Alloc<byte>(pStrSize2 + 1);
                }
                else
                {
                    byte* pStrStack2 = stackalloc byte[pStrSize2 + 1];
                    pStr2 = pStrStack2;
                }
                int pStrOffset2 = Utils.EncodeStringUTF8(buf, pStr2, pStrSize2);
                pStr2[pStrOffset2] = 0;
            }
            byte ret = InputTextExNative(pStr0, pStr1, pStr2, bufSize, sizeArg, flags, callback, userData);
            buf = Utils.DecodeStringUTF8(pStr2);
            if (pStrSize2 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr2);
            }
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igTempInputText")]
        internal static extern byte TempInputTextNative(ImRect bb, int id, byte* label, byte* buf, int bufSize, ImGuiInputTextFlags flags);

        public static bool TempInputText(ImRect bb, int id, byte* label, byte* buf, int bufSize, ImGuiInputTextFlags flags)
        {
            byte ret = TempInputTextNative(bb, id, label, buf, bufSize, flags);
            return ret != 0;
        }

        public static bool TempInputText(ImRect bb, int id, ref byte label, byte* buf, int bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                byte ret = TempInputTextNative(bb, id, (byte*)plabel, buf, bufSize, flags);
                return ret != 0;
            }
        }

        public static bool TempInputText(ImRect bb, int id, string label, byte* buf, int bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = TempInputTextNative(bb, id, pStr0, buf, bufSize, flags);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool TempInputText(ImRect bb, int id, byte* label, ref byte buf, int bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* pbuf = &buf)
            {
                byte ret = TempInputTextNative(bb, id, label, (byte*)pbuf, bufSize, flags);
                return ret != 0;
            }
        }

        public static bool TempInputText(ImRect bb, int id, byte* label, ref string buf, int bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte ret = TempInputTextNative(bb, id, label, pStr0, bufSize, flags);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        public static bool TempInputText(ImRect bb, int id, ref byte label, ref byte buf, int bufSize, ImGuiInputTextFlags flags)
        {
            fixed (byte* plabel = &label)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte ret = TempInputTextNative(bb, id, (byte*)plabel, (byte*)pbuf, bufSize, flags);
                    return ret != 0;
                }
            }
        }

        public static bool TempInputText(ImRect bb, int id, string label, ref string buf, int bufSize, ImGuiInputTextFlags flags)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (label != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(label);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(label, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte ret = TempInputTextNative(bb, id, pStr0, pStr1, bufSize, flags);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret != 0;
        }

        [LibraryImport(LibName, EntryPoint = "igImFormatString")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial int ImFormatStringNative(byte* buf, nuint bufSize, byte* fmt);

        public static int ImFormatString(byte* buf, nuint bufSize, byte* fmt)
        {
            int ret = ImFormatStringNative(buf, bufSize, fmt);
            return ret;
        }

        public static int ImFormatString(ref byte buf, nuint bufSize, byte* fmt)
        {
            fixed (byte* pbuf = &buf)
            {
                int ret = ImFormatStringNative((byte*)pbuf, bufSize, fmt);
                return ret;
            }
        }

        public static int ImFormatString(ref string buf, nuint bufSize, byte* fmt)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImFormatStringNative(pStr0, bufSize, fmt);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImFormatString(byte* buf, nuint bufSize, ref byte fmt)
        {
            fixed (byte* pfmt = &fmt)
            {
                int ret = ImFormatStringNative(buf, bufSize, (byte*)pfmt);
                return ret;
            }
        }

        public static int ImFormatString(byte* buf, nuint bufSize, string fmt)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (fmt != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(fmt);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(fmt, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImFormatStringNative(buf, bufSize, pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImFormatString(ref byte buf, nuint bufSize, ref byte fmt)
        {
            fixed (byte* pbuf = &buf)
            {
                fixed (byte* pfmt = &fmt)
                {
                    int ret = ImFormatStringNative((byte*)pbuf, bufSize, (byte*)pfmt);
                    return ret;
                }
            }
        }

        public static int ImFormatString(ref string buf, nuint bufSize, string fmt)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (fmt != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(fmt);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(fmt, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            int ret = ImFormatStringNative(pStr0, bufSize, pStr1);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        [LibraryImport(LibName, EntryPoint = "igImFormatStringV")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial int ImFormatStringVNative(byte* buf, nuint bufSize, byte* fmt, nuint args);

        public static int ImFormatStringV(byte* buf, nuint bufSize, byte* fmt, nuint args)
        {
            int ret = ImFormatStringVNative(buf, bufSize, fmt, args);
            return ret;
        }

        public static int ImFormatStringV(ref byte buf, nuint bufSize, byte* fmt, nuint args)
        {
            fixed (byte* pbuf = &buf)
            {
                int ret = ImFormatStringVNative((byte*)pbuf, bufSize, fmt, args);
                return ret;
            }
        }

        public static int ImFormatStringV(ref string buf, nuint bufSize, byte* fmt, nuint args)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImFormatStringVNative(pStr0, bufSize, fmt, args);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImFormatStringV(byte* buf, nuint bufSize, ref byte fmt, nuint args)
        {
            fixed (byte* pfmt = &fmt)
            {
                int ret = ImFormatStringVNative(buf, bufSize, (byte*)pfmt, args);
                return ret;
            }
        }

        public static int ImFormatStringV(byte* buf, nuint bufSize, string fmt, nuint args)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (fmt != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(fmt);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(fmt, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImFormatStringVNative(buf, bufSize, pStr0, args);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImFormatStringV(ref byte buf, nuint bufSize, ref byte fmt, nuint args)
        {
            fixed (byte* pbuf = &buf)
            {
                fixed (byte* pfmt = &fmt)
                {
                    int ret = ImFormatStringVNative((byte*)pbuf, bufSize, (byte*)pfmt, args);
                    return ret;
                }
            }
        }

        public static int ImFormatStringV(ref string buf, nuint bufSize, string fmt, nuint args)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (fmt != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(fmt);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(fmt, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            int ret = ImFormatStringVNative(pStr0, bufSize, pStr1, args);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        [LibraryImport(LibName, EntryPoint = "igImParseFormatTrimDecorations")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial byte* ImParseFormatTrimDecorationsNative(byte* format, byte* buf, nuint bufSize);

        public static byte* ImParseFormatTrimDecorations(byte* format, byte* buf, nuint bufSize)
        {
            byte* ret = ImParseFormatTrimDecorationsNative(format, buf, bufSize);
            return ret;
        }

        public static string ImParseFormatTrimDecorationsS(byte* format, byte* buf, nuint bufSize)
        {
            string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative(format, buf, bufSize));
            return ret;
        }

        public static byte* ImParseFormatTrimDecorations(ref byte format, byte* buf, nuint bufSize)
        {
            fixed (byte* pformat = &format)
            {
                byte* ret = ImParseFormatTrimDecorationsNative((byte*)pformat, buf, bufSize);
                return ret;
            }
        }

        public static string ImParseFormatTrimDecorationsS(ref byte format, byte* buf, nuint bufSize)
        {
            fixed (byte* pformat = &format)
            {
                string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative((byte*)pformat, buf, bufSize));
                return ret;
            }
        }

        public static byte* ImParseFormatTrimDecorations(string format, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (format != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(format);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(format, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* ret = ImParseFormatTrimDecorationsNative(pStr0, buf, bufSize);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static string ImParseFormatTrimDecorationsS(string format, byte* buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (format != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(format);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(format, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative(pStr0, buf, bufSize));
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static byte* ImParseFormatTrimDecorations(byte* format, ref byte buf, nuint bufSize)
        {
            fixed (byte* pbuf = &buf)
            {
                byte* ret = ImParseFormatTrimDecorationsNative(format, (byte*)pbuf, bufSize);
                return ret;
            }
        }

        public static string ImParseFormatTrimDecorationsS(byte* format, ref byte buf, nuint bufSize)
        {
            fixed (byte* pbuf = &buf)
            {
                string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative(format, (byte*)pbuf, bufSize));
                return ret;
            }
        }

        public static byte* ImParseFormatTrimDecorations(byte* format, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* ret = ImParseFormatTrimDecorationsNative(format, pStr0, bufSize);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static string ImParseFormatTrimDecorationsS(byte* format, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative(format, pStr0, bufSize));
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static byte* ImParseFormatTrimDecorations(ref byte format, ref byte buf, nuint bufSize)
        {
            fixed (byte* pformat = &format)
            {
                fixed (byte* pbuf = &buf)
                {
                    byte* ret = ImParseFormatTrimDecorationsNative((byte*)pformat, (byte*)pbuf, bufSize);
                    return ret;
                }
            }
        }

        public static string ImParseFormatTrimDecorationsS(ref byte format, ref byte buf, nuint bufSize)
        {
            fixed (byte* pformat = &format)
            {
                fixed (byte* pbuf = &buf)
                {
                    string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative((byte*)pformat, (byte*)pbuf, bufSize));
                    return ret;
                }
            }
        }

        public static byte* ImParseFormatTrimDecorations(string format, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (format != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(format);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(format, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            byte* ret = ImParseFormatTrimDecorationsNative(pStr0, pStr1, bufSize);
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static string ImParseFormatTrimDecorationsS(string format, ref string buf, nuint bufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (format != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(format);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(format, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (buf != null)
            {
                pStrSize1 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(buf, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            string ret = Utils.DecodeStringUTF8(ImParseFormatTrimDecorationsNative(pStr0, pStr1, bufSize));
            buf = Utils.DecodeStringUTF8(pStr1);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        [LibraryImport(LibName, EntryPoint = "igImTextStrToUtf8")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial int ImTextToUtf8Native(byte* outBuf, int outBufSize, char* inText, char* inTextEnd);

        public static int ImTextToUtf8(byte* outBuf, int outBufSize, char* inText, char* inTextEnd)
        {
            int ret = ImTextToUtf8Native(outBuf, outBufSize, inText, inTextEnd);
            return ret;
        }

        public static int ImTextToUtf8(ref byte outBuf, int outBufSize, char* inText, char* inTextEnd)
        {
            fixed (byte* poutBuf = &outBuf)
            {
                int ret = ImTextToUtf8Native((byte*)poutBuf, outBufSize, inText, inTextEnd);
                return ret;
            }
        }

        public static int ImTextToUtf8(ref string outBuf, int outBufSize, char* inText, char* inTextEnd)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (outBuf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(outBuf), (int)outBufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(outBuf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImTextToUtf8Native(pStr0, outBufSize, inText, inTextEnd);
            outBuf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImTextToUtf8(byte* outBuf, int outBufSize, ref char inText, char* inTextEnd)
        {
            fixed (char* pinText = &inText)
            {
                int ret = ImTextToUtf8Native(outBuf, outBufSize, (char*)pinText, inTextEnd);
                return ret;
            }
        }

        public static int ImTextToUtf8(ref byte outBuf, int outBufSize, ref char inText, char* inTextEnd)
        {
            fixed (byte* poutBuf = &outBuf)
            {
                fixed (char* pinText = &inText)
                {
                    int ret = ImTextToUtf8Native((byte*)poutBuf, outBufSize, (char*)pinText, inTextEnd);
                    return ret;
                }
            }
        }

        public static int ImTextToUtf8(ref string outBuf, int outBufSize, ref char inText, char* inTextEnd)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (outBuf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(outBuf), (int)outBufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(outBuf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            fixed (char* pinText = &inText)
            {
                int ret = ImTextToUtf8Native(pStr0, outBufSize, (char*)pinText, inTextEnd);
                outBuf = Utils.DecodeStringUTF8(pStr0);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextToUtf8(byte* outBuf, int outBufSize, char* inText, ref char inTextEnd)
        {
            fixed (char* pinTextEnd = &inTextEnd)
            {
                int ret = ImTextToUtf8Native(outBuf, outBufSize, inText, (char*)pinTextEnd);
                return ret;
            }
        }

        public static int ImTextToUtf8(ref byte outBuf, int outBufSize, char* inText, ref char inTextEnd)
        {
            fixed (byte* poutBuf = &outBuf)
            {
                fixed (char* pinTextEnd = &inTextEnd)
                {
                    int ret = ImTextToUtf8Native((byte*)poutBuf, outBufSize, inText, (char*)pinTextEnd);
                    return ret;
                }
            }
        }

        public static int ImTextToUtf8(ref string outBuf, int outBufSize, char* inText, ref char inTextEnd)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (outBuf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(outBuf), (int)outBufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(outBuf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            fixed (char* pinTextEnd = &inTextEnd)
            {
                int ret = ImTextToUtf8Native(pStr0, outBufSize, inText, (char*)pinTextEnd);
                outBuf = Utils.DecodeStringUTF8(pStr0);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextToUtf8(byte* outBuf, int outBufSize, ref char inText, ref char inTextEnd)
        {
            fixed (char* pinText = &inText)
            {
                fixed (char* pinTextEnd = &inTextEnd)
                {
                    int ret = ImTextToUtf8Native(outBuf, outBufSize, (char*)pinText, (char*)pinTextEnd);
                    return ret;
                }
            }
        }

        public static int ImTextToUtf8(ref byte outBuf, int outBufSize, ref char inText, ref char inTextEnd)
        {
            fixed (byte* poutBuf = &outBuf)
            {
                fixed (char* pinText = &inText)
                {
                    fixed (char* pinTextEnd = &inTextEnd)
                    {
                        int ret = ImTextToUtf8Native((byte*)poutBuf, outBufSize, (char*)pinText, (char*)pinTextEnd);
                        return ret;
                    }
                }
            }
        }

        public static int ImTextToUtf8(ref string outBuf, int outBufSize, ref char inText, ref char inTextEnd)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (outBuf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(outBuf), (int)outBufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(outBuf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            fixed (char* pinText = &inText)
            {
                fixed (char* pinTextEnd = &inTextEnd)
                {
                    int ret = ImTextToUtf8Native(pStr0, outBufSize, (char*)pinText, (char*)pinTextEnd);
                    outBuf = Utils.DecodeStringUTF8(pStr0);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        Utils.Free(pStr0);
                    }
                    return ret;
                }
            }
        }

        [LibraryImport(LibName, EntryPoint = "igImTextStrFromUtf8")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial int ImTextFromUtf8Native(char* outBuf, int outBufSize, byte* inText, byte* inTextEnd, byte** inRemaining);

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, byte* inText, byte* inTextEnd, byte** inRemaining)
        {
            int ret = ImTextFromUtf8Native(outBuf, outBufSize, inText, inTextEnd, inRemaining);
            return ret;
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, byte* inText, byte* inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, inText, inTextEnd, inRemaining);
                return ret;
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, ref byte inText, byte* inTextEnd, byte** inRemaining)
        {
            fixed (byte* pinText = &inText)
            {
                int ret = ImTextFromUtf8Native(outBuf, outBufSize, (byte*)pinText, inTextEnd, inRemaining);
                return ret;
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, string inText, byte* inTextEnd, byte** inRemaining)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (inText != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(inText);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImTextFromUtf8Native(outBuf, outBufSize, pStr0, inTextEnd, inRemaining);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, ref byte inText, byte* inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte* pinText = &inText)
                {
                    int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, (byte*)pinText, inTextEnd, inRemaining);
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, string inText, byte* inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                byte* pStr0 = null;
                int pStrSize0 = 0;
                if (inText != null)
                {
                    pStrSize0 = Utils.GetByteCountUTF8(inText);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                    }
                    else
                    {
                        byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                        pStr0 = pStrStack0;
                    }
                    int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                    pStr0[pStrOffset0] = 0;
                }
                int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, pStr0, inTextEnd, inRemaining);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, byte* inText, ref byte inTextEnd, byte** inRemaining)
        {
            fixed (byte* pinTextEnd = &inTextEnd)
            {
                int ret = ImTextFromUtf8Native(outBuf, outBufSize, inText, (byte*)pinTextEnd, inRemaining);
                return ret;
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, byte* inText, string inTextEnd, byte** inRemaining)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (inTextEnd != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(inTextEnd);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(inTextEnd, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = ImTextFromUtf8Native(outBuf, outBufSize, inText, pStr0, inRemaining);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, byte* inText, ref byte inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte* pinTextEnd = &inTextEnd)
                {
                    int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, inText, (byte*)pinTextEnd, inRemaining);
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, byte* inText, string inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                byte* pStr0 = null;
                int pStrSize0 = 0;
                if (inTextEnd != null)
                {
                    pStrSize0 = Utils.GetByteCountUTF8(inTextEnd);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                    }
                    else
                    {
                        byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                        pStr0 = pStrStack0;
                    }
                    int pStrOffset0 = Utils.EncodeStringUTF8(inTextEnd, pStr0, pStrSize0);
                    pStr0[pStrOffset0] = 0;
                }
                int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, inText, pStr0, inRemaining);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, ref byte inText, ref byte inTextEnd, byte** inRemaining)
        {
            fixed (byte* pinText = &inText)
            {
                fixed (byte* pinTextEnd = &inTextEnd)
                {
                    int ret = ImTextFromUtf8Native(outBuf, outBufSize, (byte*)pinText, (byte*)pinTextEnd, inRemaining);
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, string inText, string inTextEnd, byte** inRemaining)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (inText != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(inText);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (inTextEnd != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(inTextEnd);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(inTextEnd, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            int ret = ImTextFromUtf8Native(outBuf, outBufSize, pStr0, pStr1, inRemaining);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, ref byte inText, ref byte inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte* pinText = &inText)
                {
                    fixed (byte* pinTextEnd = &inTextEnd)
                    {
                        int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, (byte*)pinText, (byte*)pinTextEnd, inRemaining);
                        return ret;
                    }
                }
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, string inText, string inTextEnd, byte** inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                byte* pStr0 = null;
                int pStrSize0 = 0;
                if (inText != null)
                {
                    pStrSize0 = Utils.GetByteCountUTF8(inText);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                    }
                    else
                    {
                        byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                        pStr0 = pStrStack0;
                    }
                    int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                    pStr0[pStrOffset0] = 0;
                }
                byte* pStr1 = null;
                int pStrSize1 = 0;
                if (inTextEnd != null)
                {
                    pStrSize1 = Utils.GetByteCountUTF8(inTextEnd);
                    if (pStrSize1 >= Utils.MaxStackallocSize)
                    {
                        pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                    }
                    else
                    {
                        byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                        pStr1 = pStrStack1;
                    }
                    int pStrOffset1 = Utils.EncodeStringUTF8(inTextEnd, pStr1, pStrSize1);
                    pStr1[pStrOffset1] = 0;
                }
                int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, pStr0, pStr1, inRemaining);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr1);
                }
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, byte* inText, byte* inTextEnd, ref byte* inRemaining)
        {
            fixed (byte** pinRemaining = &inRemaining)
            {
                int ret = ImTextFromUtf8Native(outBuf, outBufSize, inText, inTextEnd, (byte**)pinRemaining);
                return ret;
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, byte* inText, byte* inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte** pinRemaining = &inRemaining)
                {
                    int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, inText, inTextEnd, (byte**)pinRemaining);
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, ref byte inText, byte* inTextEnd, ref byte* inRemaining)
        {
            fixed (byte* pinText = &inText)
            {
                fixed (byte** pinRemaining = &inRemaining)
                {
                    int ret = ImTextFromUtf8Native(outBuf, outBufSize, (byte*)pinText, inTextEnd, (byte**)pinRemaining);
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, string inText, byte* inTextEnd, ref byte* inRemaining)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (inText != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(inText);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            fixed (byte** pinRemaining = &inRemaining)
            {
                int ret = ImTextFromUtf8Native(outBuf, outBufSize, pStr0, inTextEnd, (byte**)pinRemaining);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, ref byte inText, byte* inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte* pinText = &inText)
                {
                    fixed (byte** pinRemaining = &inRemaining)
                    {
                        int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, (byte*)pinText, inTextEnd, (byte**)pinRemaining);
                        return ret;
                    }
                }
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, string inText, byte* inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                byte* pStr0 = null;
                int pStrSize0 = 0;
                if (inText != null)
                {
                    pStrSize0 = Utils.GetByteCountUTF8(inText);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                    }
                    else
                    {
                        byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                        pStr0 = pStrStack0;
                    }
                    int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                    pStr0[pStrOffset0] = 0;
                }
                fixed (byte** pinRemaining = &inRemaining)
                {
                    int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, pStr0, inTextEnd, (byte**)pinRemaining);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        Utils.Free(pStr0);
                    }
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, byte* inText, ref byte inTextEnd, ref byte* inRemaining)
        {
            fixed (byte* pinTextEnd = &inTextEnd)
            {
                fixed (byte** pinRemaining = &inRemaining)
                {
                    int ret = ImTextFromUtf8Native(outBuf, outBufSize, inText, (byte*)pinTextEnd, (byte**)pinRemaining);
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, byte* inText, string inTextEnd, ref byte* inRemaining)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (inTextEnd != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(inTextEnd);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(inTextEnd, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            fixed (byte** pinRemaining = &inRemaining)
            {
                int ret = ImTextFromUtf8Native(outBuf, outBufSize, inText, pStr0, (byte**)pinRemaining);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, byte* inText, ref byte inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte* pinTextEnd = &inTextEnd)
                {
                    fixed (byte** pinRemaining = &inRemaining)
                    {
                        int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, inText, (byte*)pinTextEnd, (byte**)pinRemaining);
                        return ret;
                    }
                }
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, byte* inText, string inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                byte* pStr0 = null;
                int pStrSize0 = 0;
                if (inTextEnd != null)
                {
                    pStrSize0 = Utils.GetByteCountUTF8(inTextEnd);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                    }
                    else
                    {
                        byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                        pStr0 = pStrStack0;
                    }
                    int pStrOffset0 = Utils.EncodeStringUTF8(inTextEnd, pStr0, pStrSize0);
                    pStr0[pStrOffset0] = 0;
                }
                fixed (byte** pinRemaining = &inRemaining)
                {
                    int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, inText, pStr0, (byte**)pinRemaining);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        Utils.Free(pStr0);
                    }
                    return ret;
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, ref byte inText, ref byte inTextEnd, ref byte* inRemaining)
        {
            fixed (byte* pinText = &inText)
            {
                fixed (byte* pinTextEnd = &inTextEnd)
                {
                    fixed (byte** pinRemaining = &inRemaining)
                    {
                        int ret = ImTextFromUtf8Native(outBuf, outBufSize, (byte*)pinText, (byte*)pinTextEnd, (byte**)pinRemaining);
                        return ret;
                    }
                }
            }
        }

        public static int ImTextFromUtf8(char* outBuf, int outBufSize, string inText, string inTextEnd, ref byte* inRemaining)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (inText != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(inText);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (inTextEnd != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(inTextEnd);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(inTextEnd, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            fixed (byte** pinRemaining = &inRemaining)
            {
                int ret = ImTextFromUtf8Native(outBuf, outBufSize, pStr0, pStr1, (byte**)pinRemaining);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr1);
                }
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    Utils.Free(pStr0);
                }
                return ret;
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, ref byte inText, ref byte inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                fixed (byte* pinText = &inText)
                {
                    fixed (byte* pinTextEnd = &inTextEnd)
                    {
                        fixed (byte** pinRemaining = &inRemaining)
                        {
                            int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, (byte*)pinText, (byte*)pinTextEnd, (byte**)pinRemaining);
                            return ret;
                        }
                    }
                }
            }
        }

        public static int ImTextFromUtf8(ref char outBuf, int outBufSize, string inText, string inTextEnd, ref byte* inRemaining)
        {
            fixed (char* poutBuf = &outBuf)
            {
                byte* pStr0 = null;
                int pStrSize0 = 0;
                if (inText != null)
                {
                    pStrSize0 = Utils.GetByteCountUTF8(inText);
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                    }
                    else
                    {
                        byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                        pStr0 = pStrStack0;
                    }
                    int pStrOffset0 = Utils.EncodeStringUTF8(inText, pStr0, pStrSize0);
                    pStr0[pStrOffset0] = 0;
                }
                byte* pStr1 = null;
                int pStrSize1 = 0;
                if (inTextEnd != null)
                {
                    pStrSize1 = Utils.GetByteCountUTF8(inTextEnd);
                    if (pStrSize1 >= Utils.MaxStackallocSize)
                    {
                        pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                    }
                    else
                    {
                        byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                        pStr1 = pStrStack1;
                    }
                    int pStrOffset1 = Utils.EncodeStringUTF8(inTextEnd, pStr1, pStrSize1);
                    pStr1[pStrOffset1] = 0;
                }
                fixed (byte** pinRemaining = &inRemaining)
                {
                    int ret = ImTextFromUtf8Native((char*)poutBuf, outBufSize, pStr0, pStr1, (byte**)pinRemaining);
                    if (pStrSize1 >= Utils.MaxStackallocSize)
                    {
                        Utils.Free(pStr1);
                    }
                    if (pStrSize0 >= Utils.MaxStackallocSize)
                    {
                        Utils.Free(pStr0);
                    }
                    return ret;
                }
            }
        }

        [LibraryImport(LibName, EntryPoint = "igGetKeyChordName")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial void GetKeyChordNameNative(int keyChord, byte* outBuf, int outBufSize);

        public static void GetKeyChordName(int keyChord, byte* outBuf, int outBufSize)
        {
            GetKeyChordNameNative(keyChord, outBuf, outBufSize);
        }

        public static void GetKeyChordName(int keyChord, ref byte outBuf, int outBufSize)
        {
            fixed (byte* poutBuf = &outBuf)
            {
                GetKeyChordNameNative(keyChord, (byte*)poutBuf, outBufSize);
            }
        }

        public static void GetKeyChordName(int keyChord, ref string outBuf, int outBufSize)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (outBuf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(outBuf), (int)outBufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(outBuf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            GetKeyChordNameNative(keyChord, pStr0, outBufSize);
            outBuf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
        }

        [LibraryImport(LibName, EntryPoint = "igDataTypeFormatString")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        internal static partial int DataTypeFormatStringNative(byte* buf, int bufSize, ImGuiDataType dataType, void* pData, byte* format);

        public static int DataTypeFormatString(byte* buf, int bufSize, ImGuiDataType dataType, void* pData, byte* format)
        {
            int ret = DataTypeFormatStringNative(buf, bufSize, dataType, pData, format);
            return ret;
        }

        public static int DataTypeFormatString(ref byte buf, int bufSize, ImGuiDataType dataType, void* pData, byte* format)
        {
            fixed (byte* pbuf = &buf)
            {
                int ret = DataTypeFormatStringNative((byte*)pbuf, bufSize, dataType, pData, format);
                return ret;
            }
        }

        public static int DataTypeFormatString(ref string buf, int bufSize, ImGuiDataType dataType, void* pData, byte* format)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = DataTypeFormatStringNative(pStr0, bufSize, dataType, pData, format);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int DataTypeFormatString(byte* buf, int bufSize, ImGuiDataType dataType, void* pData, ref byte format)
        {
            fixed (byte* pformat = &format)
            {
                int ret = DataTypeFormatStringNative(buf, bufSize, dataType, pData, (byte*)pformat);
                return ret;
            }
        }

        public static int DataTypeFormatString(byte* buf, int bufSize, ImGuiDataType dataType, void* pData, string format)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (format != null)
            {
                pStrSize0 = Utils.GetByteCountUTF8(format);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(format, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            int ret = DataTypeFormatStringNative(buf, bufSize, dataType, pData, pStr0);
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }

        public static int DataTypeFormatString(ref byte buf, int bufSize, ImGuiDataType dataType, void* pData, ref byte format)
        {
            fixed (byte* pbuf = &buf)
            {
                fixed (byte* pformat = &format)
                {
                    int ret = DataTypeFormatStringNative((byte*)pbuf, bufSize, dataType, pData, (byte*)pformat);
                    return ret;
                }
            }
        }

        public static int DataTypeFormatString(ref string buf, int bufSize, ImGuiDataType dataType, void* pData, string format)
        {
            byte* pStr0 = null;
            int pStrSize0 = 0;
            if (buf != null)
            {
                pStrSize0 = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);
                if (pStrSize0 >= Utils.MaxStackallocSize)
                {
                    pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
                }
                else
                {
                    byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
                    pStr0 = pStrStack0;
                }
                int pStrOffset0 = Utils.EncodeStringUTF8(buf, pStr0, pStrSize0);
                pStr0[pStrOffset0] = 0;
            }
            byte* pStr1 = null;
            int pStrSize1 = 0;
            if (format != null)
            {
                pStrSize1 = Utils.GetByteCountUTF8(format);
                if (pStrSize1 >= Utils.MaxStackallocSize)
                {
                    pStr1 = Utils.Alloc<byte>(pStrSize1 + 1);
                }
                else
                {
                    byte* pStrStack1 = stackalloc byte[pStrSize1 + 1];
                    pStr1 = pStrStack1;
                }
                int pStrOffset1 = Utils.EncodeStringUTF8(format, pStr1, pStrSize1);
                pStr1[pStrOffset1] = 0;
            }
            int ret = DataTypeFormatStringNative(pStr0, bufSize, dataType, pData, pStr1);
            buf = Utils.DecodeStringUTF8(pStr0);
            if (pStrSize1 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr1);
            }
            if (pStrSize0 >= Utils.MaxStackallocSize)
            {
                Utils.Free(pStr0);
            }
            return ret;
        }
    }
}