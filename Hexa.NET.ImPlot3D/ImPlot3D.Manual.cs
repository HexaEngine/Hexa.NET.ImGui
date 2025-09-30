namespace Hexa.NET.ImPlot3D
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Hexa.NET.ImGui;
    using HexaGen.Runtime;

    public static unsafe partial class ImPlot3D
    {
        /// <summary>
        /// To be documented.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetImGuiContextNative(ImGuiContext* ctx)
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
        public static void SetImGuiContext(ImGuiContextPtr ctx)
        {
            SetImGuiContextNative(ctx);
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static void SetImGuiContext(ref ImGuiContext ctx)
        {
            fixed (ImGuiContext* pctx = &ctx)
            {
                SetImGuiContextNative((ImGuiContext*)pctx);
            }
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ImGuiContext* GetImGuiContextNative()
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
        public static ImGuiContextPtr GetImGuiContext()
        {
            ImGuiContext* ret = GetImGuiContextNative();
            return ret;
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetAllocatorFunctionsNative(ImGuiMemAllocFunc allocFunc, ImGuiMemFreeFunc freeFunc, void* userData)
        {
#if NET5_0_OR_GREATER
            ((delegate* unmanaged[Cdecl]<delegate*<nuint, void*, void*>, delegate*<void*, void*, void>, void*, void>)funcTable[2])((delegate*<nuint, void*, void*>)Utils.GetFunctionPointerForDelegate(allocFunc), (delegate*<void*, void*, void>)Utils.GetFunctionPointerForDelegate(freeFunc), userData);
#else
			((delegate* unmanaged[Cdecl]<nint, nint, nint, void>)funcTable[409])((nint)Utils.GetFunctionPointerForDelegate(allocFunc), (nint)Utils.GetFunctionPointerForDelegate(freeFunc), (nint)userData);
#endif
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static void SetAllocatorFunctions(ImGuiMemAllocFunc allocFunc, ImGuiMemFreeFunc freeFunc, void* userData)
        {
            SetAllocatorFunctionsNative(allocFunc, freeFunc, userData);
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static void SetAllocatorFunctions(ImGuiMemAllocFunc allocFunc, ImGuiMemFreeFunc freeFunc)
        {
            SetAllocatorFunctionsNative(allocFunc, freeFunc, (void*)(default));
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetAllocatorFunctionsNative(delegate*<nuint, void*, void*>* pAllocFunc, delegate*<void*, void*, void>* pFreeFunc, void** pUserData)
        {
#if NET5_0_OR_GREATER
            ((delegate* unmanaged[Cdecl]<delegate*<nuint, void*, void*>*, delegate*<void*, void*, void>*, void**, void>)funcTable[3])(pAllocFunc, pFreeFunc, pUserData);
#else
			((delegate* unmanaged[Cdecl]<nint, nint, nint, void>)funcTable[410])((nint)pAllocFunc, (nint)pFreeFunc, (nint)pUserData);
#endif
        }

        /// <summary>
        /// To be documented.
        /// </summary>
        public static void GetAllocatorFunctions(delegate*<nuint, void*, void*>* pAllocFunc, delegate*<void*, void*, void>* pFreeFunc, void** pUserData)
        {
            GetAllocatorFunctionsNative(pAllocFunc, pFreeFunc, pUserData);
        }
    }
}