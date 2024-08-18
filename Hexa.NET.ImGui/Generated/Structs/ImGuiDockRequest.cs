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
	public partial struct ImGuiDockRequest
	{


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiDockRequestPtr : IEquatable<ImGuiDockRequestPtr>
	{
		public ImGuiDockRequestPtr(ImGuiDockRequest* handle) { Handle = handle; }

		public ImGuiDockRequest* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiDockRequestPtr Null => new ImGuiDockRequestPtr(null);

		public ImGuiDockRequest this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiDockRequestPtr(ImGuiDockRequest* handle) => new ImGuiDockRequestPtr(handle);

		public static implicit operator ImGuiDockRequest*(ImGuiDockRequestPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiDockRequestPtr left, ImGuiDockRequestPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiDockRequestPtr left, ImGuiDockRequestPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiDockRequestPtr left, ImGuiDockRequest* right) => left.Handle == right;

		public static bool operator !=(ImGuiDockRequestPtr left, ImGuiDockRequest* right) => left.Handle != right;

		public bool Equals(ImGuiDockRequestPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiDockRequestPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiDockRequestPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
	}

}