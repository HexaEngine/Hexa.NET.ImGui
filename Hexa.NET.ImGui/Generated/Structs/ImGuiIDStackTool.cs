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
	public partial struct ImGuiIDStackTool
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public int LastActiveFrame;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int StackLevel;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint QueryId;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<ImGuiStackLevelInfo> Results;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte CopyToClipboardOnCtrlC;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float CopyToClipboardLastTime;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiTextBuffer ResultPathBuf;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiIDStackTool(int lastActiveFrame = default, int stackLevel = default, uint queryId = default, ImVector<ImGuiStackLevelInfo> results = default, bool copyToClipboardOnCtrlC = default, float copyToClipboardLastTime = default, ImGuiTextBuffer resultPathBuf = default)
		{
			LastActiveFrame = lastActiveFrame;
			StackLevel = stackLevel;
			QueryId = queryId;
			Results = results;
			CopyToClipboardOnCtrlC = copyToClipboardOnCtrlC ? (byte)1 : (byte)0;
			CopyToClipboardLastTime = copyToClipboardLastTime;
			ResultPathBuf = resultPathBuf;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiIDStackToolPtr : IEquatable<ImGuiIDStackToolPtr>
	{
		public ImGuiIDStackToolPtr(ImGuiIDStackTool* handle) { Handle = handle; }

		public ImGuiIDStackTool* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiIDStackToolPtr Null => new ImGuiIDStackToolPtr(null);

		public ImGuiIDStackTool this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiIDStackToolPtr(ImGuiIDStackTool* handle) => new ImGuiIDStackToolPtr(handle);

		public static implicit operator ImGuiIDStackTool*(ImGuiIDStackToolPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiIDStackToolPtr left, ImGuiIDStackToolPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiIDStackToolPtr left, ImGuiIDStackToolPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiIDStackToolPtr left, ImGuiIDStackTool* right) => left.Handle == right;

		public static bool operator !=(ImGuiIDStackToolPtr left, ImGuiIDStackTool* right) => left.Handle != right;

		public bool Equals(ImGuiIDStackToolPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiIDStackToolPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiIDStackToolPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int LastActiveFrame => ref Unsafe.AsRef<int>(&Handle->LastActiveFrame);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int StackLevel => ref Unsafe.AsRef<int>(&Handle->StackLevel);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint QueryId => ref Unsafe.AsRef<uint>(&Handle->QueryId);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<ImGuiStackLevelInfo> Results => ref Unsafe.AsRef<ImVector<ImGuiStackLevelInfo>>(&Handle->Results);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool CopyToClipboardOnCtrlC => ref Unsafe.AsRef<bool>(&Handle->CopyToClipboardOnCtrlC);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float CopyToClipboardLastTime => ref Unsafe.AsRef<float>(&Handle->CopyToClipboardLastTime);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiTextBuffer ResultPathBuf => ref Unsafe.AsRef<ImGuiTextBuffer>(&Handle->ResultPathBuf);
	}

}
