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
	public partial struct ImGuiDockNodeSettings
	{


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiDockNodeSettingsPtr : IEquatable<ImGuiDockNodeSettingsPtr>
	{
		public ImGuiDockNodeSettingsPtr(ImGuiDockNodeSettings* handle) { Handle = handle; }

		public ImGuiDockNodeSettings* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiDockNodeSettingsPtr Null => new ImGuiDockNodeSettingsPtr(null);

		public ImGuiDockNodeSettings this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiDockNodeSettingsPtr(ImGuiDockNodeSettings* handle) => new ImGuiDockNodeSettingsPtr(handle);

		public static implicit operator ImGuiDockNodeSettings*(ImGuiDockNodeSettingsPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiDockNodeSettingsPtr left, ImGuiDockNodeSettingsPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiDockNodeSettingsPtr left, ImGuiDockNodeSettingsPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiDockNodeSettingsPtr left, ImGuiDockNodeSettings* right) => left.Handle == right;

		public static bool operator !=(ImGuiDockNodeSettingsPtr left, ImGuiDockNodeSettings* right) => left.Handle != right;

		public bool Equals(ImGuiDockNodeSettingsPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiDockNodeSettingsPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiDockNodeSettingsPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
	}

}
