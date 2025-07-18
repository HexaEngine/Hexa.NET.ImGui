// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HexaGen.Runtime;
using System.Numerics;

namespace Hexa.NET.ImGui
{
	/// <summary>
	/// To be documented.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ImDrawData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Valid;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int CmdListsCount;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int TotalIdxCount;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int TotalVtxCount;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<ImDrawListPtr> CmdLists;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 DisplayPos;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 DisplaySize;

		/// <summary>
		/// To be documented.
		/// </summary>
		public Vector2 FramebufferScale;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiViewport* OwnerViewport;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImVector<ImTextureDataPtr>* Textures;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImDrawData(bool valid = default, int cmdListsCount = default, int totalIdxCount = default, int totalVtxCount = default, ImVector<ImDrawListPtr> cmdLists = default, Vector2 displayPos = default, Vector2 displaySize = default, Vector2 framebufferScale = default, ImGuiViewportPtr ownerViewport = default, ImVector<ImTextureDataPtr>* textures = default)
		{
			Valid = valid ? (byte)1 : (byte)0;
			CmdListsCount = cmdListsCount;
			TotalIdxCount = totalIdxCount;
			TotalVtxCount = totalVtxCount;
			CmdLists = cmdLists;
			DisplayPos = displayPos;
			DisplaySize = displaySize;
			FramebufferScale = framebufferScale;
			OwnerViewport = ownerViewport;
			Textures = textures;
		}


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void AddDrawList(ImDrawListPtr drawList)
		{
			fixed (ImDrawData* @this = &this)
			{
				ImGui.AddDrawListNative(@this, drawList);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void AddDrawList(ref ImDrawList drawList)
		{
			fixed (ImDrawData* @this = &this)
			{
				fixed (ImDrawList* pdrawList = &drawList)
				{
					ImGui.AddDrawListNative(@this, (ImDrawList*)pdrawList);
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Clear()
		{
			fixed (ImDrawData* @this = &this)
			{
				ImGui.ClearNative(@this);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void DeIndexAllBuffers()
		{
			fixed (ImDrawData* @this = &this)
			{
				ImGui.DeIndexAllBuffersNative(@this);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			fixed (ImDrawData* @this = &this)
			{
				ImGui.DestroyNative(@this);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void ScaleClipRects(Vector2 fbScale)
		{
			fixed (ImDrawData* @this = &this)
			{
				ImGui.ScaleClipRectsNative(@this, fbScale);
			}
		}

	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImDrawDataPtr : IEquatable<ImDrawDataPtr>
	{
		public ImDrawDataPtr(ImDrawData* handle) { Handle = handle; }

		public ImDrawData* Handle;

		public bool IsNull => Handle == null;

		public static ImDrawDataPtr Null => new ImDrawDataPtr(null);

		public ImDrawData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImDrawDataPtr(ImDrawData* handle) => new ImDrawDataPtr(handle);

		public static implicit operator ImDrawData*(ImDrawDataPtr handle) => handle.Handle;

		public static bool operator ==(ImDrawDataPtr left, ImDrawDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImDrawDataPtr left, ImDrawDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImDrawDataPtr left, ImDrawData* right) => left.Handle == right;

		public static bool operator !=(ImDrawDataPtr left, ImDrawData* right) => left.Handle != right;

		public bool Equals(ImDrawDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImDrawDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImDrawDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Valid => ref Unsafe.AsRef<bool>(&Handle->Valid);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int CmdListsCount => ref Unsafe.AsRef<int>(&Handle->CmdListsCount);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int TotalIdxCount => ref Unsafe.AsRef<int>(&Handle->TotalIdxCount);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int TotalVtxCount => ref Unsafe.AsRef<int>(&Handle->TotalVtxCount);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<ImDrawListPtr> CmdLists => ref Unsafe.AsRef<ImVector<ImDrawListPtr>>(&Handle->CmdLists);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 DisplayPos => ref Unsafe.AsRef<Vector2>(&Handle->DisplayPos);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 DisplaySize => ref Unsafe.AsRef<Vector2>(&Handle->DisplaySize);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref Vector2 FramebufferScale => ref Unsafe.AsRef<Vector2>(&Handle->FramebufferScale);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiViewportPtr OwnerViewport => ref Unsafe.AsRef<ImGuiViewportPtr>(&Handle->OwnerViewport);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<ImTextureDataPtr> Textures => ref Unsafe.AsRef<ImVector<ImTextureDataPtr>>(Handle->Textures);
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void AddDrawList(ImDrawListPtr drawList)
		{
			ImGui.AddDrawListNative(Handle, drawList);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void AddDrawList(ref ImDrawList drawList)
		{
			fixed (ImDrawList* pdrawList = &drawList)
			{
				ImGui.AddDrawListNative(Handle, (ImDrawList*)pdrawList);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Clear()
		{
			ImGui.ClearNative(Handle);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void DeIndexAllBuffers()
		{
			ImGui.DeIndexAllBuffersNative(Handle);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			ImGui.DestroyNative(Handle);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void ScaleClipRects(Vector2 fbScale)
		{
			ImGui.ScaleClipRectsNative(Handle, fbScale);
		}

	}

}
