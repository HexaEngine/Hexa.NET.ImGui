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

namespace Hexa.NET.ImGui.Backends.D3D9
{
	public unsafe partial class ImGuiImplD3D9
	{
		/// <summary>
		/// Follow "Getting Started" link and check examples/ folder to learn about using backends!<br/>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte InitNative(IDirect3DDevice9* device)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<IDirect3DDevice9*, byte>)funcTable[16])(device);
			#else
			return (byte)((delegate* unmanaged[Cdecl]<nint, byte>)funcTable[16])((nint)device);
			#endif
		}

		/// <summary>
		/// Follow "Getting Started" link and check examples/ folder to learn about using backends!<br/>
		/// </summary>
		public static bool Init(IDirect3DDevice9Ptr device)
		{
			byte ret = InitNative(device);
			return ret != 0;
		}

		/// <summary>
		/// Follow "Getting Started" link and check examples/ folder to learn about using backends!<br/>
		/// </summary>
		public static bool Init(ref IDirect3DDevice9 device)
		{
			fixed (IDirect3DDevice9* pdevice = &device)
			{
				byte ret = InitNative((IDirect3DDevice9*)pdevice);
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
			((delegate* unmanaged[Cdecl]<void>)funcTable[17])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[17])();
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
			((delegate* unmanaged[Cdecl]<void>)funcTable[18])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[18])();
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
		internal static void RenderDrawDataNative(ImDrawData* drawData)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<ImDrawData*, void>)funcTable[19])(drawData);
			#else
			((delegate* unmanaged[Cdecl]<nint, void>)funcTable[19])((nint)drawData);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RenderDrawData(ImDrawDataPtr drawData)
		{
			RenderDrawDataNative(drawData);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RenderDrawData(ref ImDrawData drawData)
		{
			fixed (ImDrawData* pdrawData = &drawData)
			{
				RenderDrawDataNative((ImDrawData*)pdrawData);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void UpdateTextureNative(ImTextureData* tex)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<ImTextureData*, void>)funcTable[20])(tex);
			#else
			((delegate* unmanaged[Cdecl]<nint, void>)funcTable[20])((nint)tex);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void UpdateTexture(ImTextureDataPtr tex)
		{
			UpdateTextureNative(tex);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void UpdateTexture(ref ImTextureData tex)
		{
			fixed (ImTextureData* ptex = &tex)
			{
				UpdateTextureNative((ImTextureData*)ptex);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte CreateDeviceObjectsNative()
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<byte>)funcTable[21])();
			#else
			return (byte)((delegate* unmanaged[Cdecl]<byte>)funcTable[21])();
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static bool CreateDeviceObjects()
		{
			byte ret = CreateDeviceObjectsNative();
			return ret != 0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void InvalidateDeviceObjectsNative()
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<void>)funcTable[22])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[22])();
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void InvalidateDeviceObjects()
		{
			InvalidateDeviceObjectsNative();
		}

	}
}
