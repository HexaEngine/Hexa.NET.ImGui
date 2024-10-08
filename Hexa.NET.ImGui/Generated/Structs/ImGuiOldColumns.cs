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
	public partial struct ImGuiOldColumns
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiOldColumnFlags Flags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte IsFirstFrame;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte IsBeingResized;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Current;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Count;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float OffMinX;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float OffMaxX;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float LineMinY;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float LineMaxY;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float HostCursorPosY;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float HostCursorMaxPosX;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect HostInitialClipRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect HostBackupClipRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect HostBackupParentWorkRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<ImGuiOldColumnData> Columns;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImDrawListSplitter Splitter;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiOldColumns(uint id = default, ImGuiOldColumnFlags flags = default, bool isFirstFrame = default, bool isBeingResized = default, int current = default, int count = default, float offMinX = default, float offMaxX = default, float lineMinY = default, float lineMaxY = default, float hostCursorPosY = default, float hostCursorMaxPosX = default, ImRect hostInitialClipRect = default, ImRect hostBackupClipRect = default, ImRect hostBackupParentWorkRect = default, ImVector<ImGuiOldColumnData> columns = default, ImDrawListSplitter splitter = default)
		{
			ID = id;
			Flags = flags;
			IsFirstFrame = isFirstFrame ? (byte)1 : (byte)0;
			IsBeingResized = isBeingResized ? (byte)1 : (byte)0;
			Current = current;
			Count = count;
			OffMinX = offMinX;
			OffMaxX = offMaxX;
			LineMinY = lineMinY;
			LineMaxY = lineMaxY;
			HostCursorPosY = hostCursorPosY;
			HostCursorMaxPosX = hostCursorMaxPosX;
			HostInitialClipRect = hostInitialClipRect;
			HostBackupClipRect = hostBackupClipRect;
			HostBackupParentWorkRect = hostBackupParentWorkRect;
			Columns = columns;
			Splitter = splitter;
		}


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			fixed (ImGuiOldColumns* @this = &this)
			{
				ImGui.DestroyNative(@this);
			}
		}

	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiOldColumnsPtr : IEquatable<ImGuiOldColumnsPtr>
	{
		public ImGuiOldColumnsPtr(ImGuiOldColumns* handle) { Handle = handle; }

		public ImGuiOldColumns* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiOldColumnsPtr Null => new ImGuiOldColumnsPtr(null);

		public ImGuiOldColumns this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiOldColumnsPtr(ImGuiOldColumns* handle) => new ImGuiOldColumnsPtr(handle);

		public static implicit operator ImGuiOldColumns*(ImGuiOldColumnsPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiOldColumnsPtr left, ImGuiOldColumnsPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiOldColumnsPtr left, ImGuiOldColumnsPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiOldColumnsPtr left, ImGuiOldColumns* right) => left.Handle == right;

		public static bool operator !=(ImGuiOldColumnsPtr left, ImGuiOldColumns* right) => left.Handle != right;

		public bool Equals(ImGuiOldColumnsPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiOldColumnsPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiOldColumnsPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ID => ref Unsafe.AsRef<uint>(&Handle->ID);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiOldColumnFlags Flags => ref Unsafe.AsRef<ImGuiOldColumnFlags>(&Handle->Flags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool IsFirstFrame => ref Unsafe.AsRef<bool>(&Handle->IsFirstFrame);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool IsBeingResized => ref Unsafe.AsRef<bool>(&Handle->IsBeingResized);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Current => ref Unsafe.AsRef<int>(&Handle->Current);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Count => ref Unsafe.AsRef<int>(&Handle->Count);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float OffMinX => ref Unsafe.AsRef<float>(&Handle->OffMinX);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float OffMaxX => ref Unsafe.AsRef<float>(&Handle->OffMaxX);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float LineMinY => ref Unsafe.AsRef<float>(&Handle->LineMinY);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float LineMaxY => ref Unsafe.AsRef<float>(&Handle->LineMaxY);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float HostCursorPosY => ref Unsafe.AsRef<float>(&Handle->HostCursorPosY);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float HostCursorMaxPosX => ref Unsafe.AsRef<float>(&Handle->HostCursorMaxPosX);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect HostInitialClipRect => ref Unsafe.AsRef<ImRect>(&Handle->HostInitialClipRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect HostBackupClipRect => ref Unsafe.AsRef<ImRect>(&Handle->HostBackupClipRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect HostBackupParentWorkRect => ref Unsafe.AsRef<ImRect>(&Handle->HostBackupParentWorkRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<ImGuiOldColumnData> Columns => ref Unsafe.AsRef<ImVector<ImGuiOldColumnData>>(&Handle->Columns);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImDrawListSplitter Splitter => ref Unsafe.AsRef<ImDrawListSplitter>(&Handle->Splitter);
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			ImGui.DestroyNative(Handle);
		}

	}

}
