﻿namespace Hexa.NET.ImGui.Backends
{
    using HexaGen.Runtime;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class ImGuiImplConfig
    {
        public static bool AotStaticLink;
    }

    public static unsafe partial class ImGuiImpl
    {
        static ImGuiImpl()
        {
            if (ImGuiImplConfig.AotStaticLink)
            {
                InitApi(new NativeLibraryContext(Process.GetCurrentProcess().MainModule!.BaseAddress));
            }
            else
            {
                InitApi(new NativeLibraryContext(LibraryLoader.LoadLibrary(GetLibraryName, null)));
            }
        }

        public static string GetLibraryName()
        {
            return "ImGuiImpl";
        }

        /// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetCurrentContextNative(ImGuiContext* ctx)
        {
#if NET5_0_OR_GREATER
            ((delegate* unmanaged[Cdecl]<ImGuiContext*, void>)funcTable[0])(ctx);
#else
			((delegate* unmanaged[Cdecl]<nint, void>)funcTable[0])((nint)ctx);
#endif
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static void SetCurrentContext(ImGuiContextPtr ctx)
        {
            SetCurrentContextNative(ctx);
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static void SetCurrentContext(ref ImGuiContext ctx)
        {
            fixed (ImGuiContext* pctx = &ctx)
            {
                SetCurrentContextNative((ImGuiContext*)pctx);
            }
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ImGuiContext* GetCurrentContextNative()
        {
#if NET5_0_OR_GREATER
            return ((delegate* unmanaged[Cdecl]<ImGuiContext*>)funcTable[1])();
#else
			return (ImGuiContext*)((delegate* unmanaged[Cdecl]<nint>)funcTable[1])();
#endif
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static ImGuiContextPtr GetCurrentContext()
        {
            ImGuiContext* ret = GetCurrentContextNative();
            return ret;
        }
    }
}