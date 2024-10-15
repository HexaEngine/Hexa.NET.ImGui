// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HexaGen.Runtime;
using System.Numerics;
using Hexa.NET.ImGui;

namespace Hexa.NET.ImGui.Backends.GLFW
{
	public unsafe partial class ImGuiImplGLFW
	{
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
			ImGuiContextPtr ret = GetCurrentContextNative();
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte InitForOpenGLNative(GLFWwindow* window, byte installCallbacks)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<GLFWwindow*, byte, byte>)funcTable[2])(window, installCallbacks);
			#else
			return (byte)((delegate* unmanaged[Cdecl]<nint, byte, byte>)funcTable[2])((nint)window, installCallbacks);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool InitForOpenGL(GLFWwindowPtr window, bool installCallbacks)
		{
			byte ret = InitForOpenGLNative(window, installCallbacks ? (byte)1 : (byte)0);
			return ret != 0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool InitForOpenGL(ref GLFWwindow window, bool installCallbacks)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				byte ret = InitForOpenGLNative((GLFWwindow*)pwindow, installCallbacks ? (byte)1 : (byte)0);
				return ret != 0;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte InitForVulkanNative(GLFWwindow* window, byte installCallbacks)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<GLFWwindow*, byte, byte>)funcTable[3])(window, installCallbacks);
			#else
			return (byte)((delegate* unmanaged[Cdecl]<nint, byte, byte>)funcTable[3])((nint)window, installCallbacks);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool InitForVulkan(GLFWwindowPtr window, bool installCallbacks)
		{
			byte ret = InitForVulkanNative(window, installCallbacks ? (byte)1 : (byte)0);
			return ret != 0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool InitForVulkan(ref GLFWwindow window, bool installCallbacks)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				byte ret = InitForVulkanNative((GLFWwindow*)pwindow, installCallbacks ? (byte)1 : (byte)0);
				return ret != 0;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte InitForOtherNative(GLFWwindow* window, byte installCallbacks)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<GLFWwindow*, byte, byte>)funcTable[4])(window, installCallbacks);
			#else
			return (byte)((delegate* unmanaged[Cdecl]<nint, byte, byte>)funcTable[4])((nint)window, installCallbacks);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool InitForOther(GLFWwindowPtr window, bool installCallbacks)
		{
			byte ret = InitForOtherNative(window, installCallbacks ? (byte)1 : (byte)0);
			return ret != 0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool InitForOther(ref GLFWwindow window, bool installCallbacks)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				byte ret = InitForOtherNative((GLFWwindow*)pwindow, installCallbacks ? (byte)1 : (byte)0);
				return ret != 0;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ShutdownNative()
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<void>)funcTable[5])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[5])();
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void Shutdown()
		{
			ShutdownNative();
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void NewFrameNative()
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<void>)funcTable[6])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[6])();
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void NewFrame()
		{
			NewFrameNative();
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void InstallCallbacksNative(GLFWwindow* window)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, void>)funcTable[7])(window);
			#else
			((delegate* unmanaged[Cdecl]<nint, void>)funcTable[7])((nint)window);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void InstallCallbacks(GLFWwindowPtr window)
		{
			InstallCallbacksNative(window);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void InstallCallbacks(ref GLFWwindow window)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				InstallCallbacksNative((GLFWwindow*)pwindow);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RestoreCallbacksNative(GLFWwindow* window)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, void>)funcTable[8])(window);
			#else
			((delegate* unmanaged[Cdecl]<nint, void>)funcTable[8])((nint)window);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RestoreCallbacks(GLFWwindowPtr window)
		{
			RestoreCallbacksNative(window);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RestoreCallbacks(ref GLFWwindow window)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				RestoreCallbacksNative((GLFWwindow*)pwindow);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void SetCallbacksChainForAllWindowsNative(byte chainForAllWindows)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<byte, void>)funcTable[9])(chainForAllWindows);
			#else
			((delegate* unmanaged[Cdecl]<byte, void>)funcTable[9])(chainForAllWindows);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void SetCallbacksChainForAllWindows(bool chainForAllWindows)
		{
			SetCallbacksChainForAllWindowsNative(chainForAllWindows ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void WindowFocusCallbackNative(GLFWwindow* window, int focused)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, int, void>)funcTable[10])(window, focused);
			#else
			((delegate* unmanaged[Cdecl]<nint, int, void>)funcTable[10])((nint)window, focused);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void WindowFocusCallback(GLFWwindowPtr window, int focused)
		{
			WindowFocusCallbackNative(window, focused);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void WindowFocusCallback(ref GLFWwindow window, int focused)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				WindowFocusCallbackNative((GLFWwindow*)pwindow, focused);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CursorEnterCallbackNative(GLFWwindow* window, int entered)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, int, void>)funcTable[11])(window, entered);
			#else
			((delegate* unmanaged[Cdecl]<nint, int, void>)funcTable[11])((nint)window, entered);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void CursorEnterCallback(GLFWwindowPtr window, int entered)
		{
			CursorEnterCallbackNative(window, entered);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void CursorEnterCallback(ref GLFWwindow window, int entered)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				CursorEnterCallbackNative((GLFWwindow*)pwindow, entered);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CursorPosCallbackNative(GLFWwindow* window, double x, double y)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, double, double, void>)funcTable[12])(window, x, y);
			#else
			((delegate* unmanaged[Cdecl]<nint, double, double, void>)funcTable[12])((nint)window, x, y);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void CursorPosCallback(GLFWwindowPtr window, double x, double y)
		{
			CursorPosCallbackNative(window, x, y);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void CursorPosCallback(ref GLFWwindow window, double x, double y)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				CursorPosCallbackNative((GLFWwindow*)pwindow, x, y);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void MouseButtonCallbackNative(GLFWwindow* window, int button, int action, int mods)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, int, int, int, void>)funcTable[13])(window, button, action, mods);
			#else
			((delegate* unmanaged[Cdecl]<nint, int, int, int, void>)funcTable[13])((nint)window, button, action, mods);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void MouseButtonCallback(GLFWwindowPtr window, int button, int action, int mods)
		{
			MouseButtonCallbackNative(window, button, action, mods);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void MouseButtonCallback(ref GLFWwindow window, int button, int action, int mods)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				MouseButtonCallbackNative((GLFWwindow*)pwindow, button, action, mods);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ScrollCallbackNative(GLFWwindow* window, double xoffset, double yoffset)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, double, double, void>)funcTable[14])(window, xoffset, yoffset);
			#else
			((delegate* unmanaged[Cdecl]<nint, double, double, void>)funcTable[14])((nint)window, xoffset, yoffset);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ScrollCallback(GLFWwindowPtr window, double xoffset, double yoffset)
		{
			ScrollCallbackNative(window, xoffset, yoffset);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ScrollCallback(ref GLFWwindow window, double xoffset, double yoffset)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				ScrollCallbackNative((GLFWwindow*)pwindow, xoffset, yoffset);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void KeyCallbackNative(GLFWwindow* window, int key, int scancode, int action, int mods)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, int, int, int, int, void>)funcTable[15])(window, key, scancode, action, mods);
			#else
			((delegate* unmanaged[Cdecl]<nint, int, int, int, int, void>)funcTable[15])((nint)window, key, scancode, action, mods);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void KeyCallback(GLFWwindowPtr window, int key, int scancode, int action, int mods)
		{
			KeyCallbackNative(window, key, scancode, action, mods);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void KeyCallback(ref GLFWwindow window, int key, int scancode, int action, int mods)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				KeyCallbackNative((GLFWwindow*)pwindow, key, scancode, action, mods);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CharCallbackNative(GLFWwindow* window, uint c)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWwindow*, uint, void>)funcTable[16])(window, c);
			#else
			((delegate* unmanaged[Cdecl]<nint, uint, void>)funcTable[16])((nint)window, c);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void CharCallback(GLFWwindowPtr window, uint c)
		{
			CharCallbackNative(window, c);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void CharCallback(ref GLFWwindow window, uint c)
		{
			fixed (GLFWwindow* pwindow = &window)
			{
				CharCallbackNative((GLFWwindow*)pwindow, c);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void MonitorCallbackNative(GLFWmonitor* monitor, int evnt)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<GLFWmonitor*, int, void>)funcTable[17])(monitor, evnt);
			#else
			((delegate* unmanaged[Cdecl]<nint, int, void>)funcTable[17])((nint)monitor, evnt);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void MonitorCallback(GLFWmonitorPtr monitor, int evnt)
		{
			MonitorCallbackNative(monitor, evnt);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void MonitorCallback(ref GLFWmonitor monitor, int evnt)
		{
			fixed (GLFWmonitor* pmonitor = &monitor)
			{
				MonitorCallbackNative((GLFWmonitor*)pmonitor, evnt);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void SleepNative(int milliseconds)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<int, void>)funcTable[18])(milliseconds);
			#else
			((delegate* unmanaged[Cdecl]<int, void>)funcTable[18])(milliseconds);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void Sleep(int milliseconds)
		{
			SleepNative(milliseconds);
		}

	}
}