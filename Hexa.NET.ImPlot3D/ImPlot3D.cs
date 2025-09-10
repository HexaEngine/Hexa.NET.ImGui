namespace Hexa.NET.ImPlot3D
{
    using Hexa.NET.ImGui;
    using HexaGen.Runtime;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class ImPlot3DConfig
    {
        public static bool AotStaticLink;
    }

    public static unsafe partial class ImPlot3D
    {
        static ImPlot3D()
        {
            if (ImPlot3DConfig.AotStaticLink)
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
            return "cimplot3d";
        }

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
    }
}