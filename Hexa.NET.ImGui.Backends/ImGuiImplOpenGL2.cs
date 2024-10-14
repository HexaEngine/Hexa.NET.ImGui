namespace Hexa.NET.ImGui.Backends.OpenGL2
{
    using HexaGen.Runtime;
    using System.Runtime.CompilerServices;

    public static unsafe partial class ImGuiImplOpenGL2
    {
        internal static FunctionTable funcTable;

        static ImGuiImplOpenGL2()
        {
            if (ImGuiImpl.funcTable == null)
            {
                ImGuiImpl.InitApi();
            }

            funcTable = ImGuiImpl.funcTable!;
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
        public static void SetCurrentContext(ImGuiContext* ctx)
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
        public static ImGuiContext* GetCurrentContext()
        {
            ImGuiContext* ret = GetCurrentContextNative();
            return ret;
        }
    }
}