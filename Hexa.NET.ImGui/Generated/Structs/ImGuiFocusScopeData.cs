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
	public partial struct ImGuiFocusScopeData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint WindowID;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiFocusScopeData(uint id = default, uint windowId = default)
		{
			ID = id;
			WindowID = windowId;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiFocusScopeDataPtr : IEquatable<ImGuiFocusScopeDataPtr>
	{
		public ImGuiFocusScopeDataPtr(ImGuiFocusScopeData* handle) { Handle = handle; }

		public ImGuiFocusScopeData* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiFocusScopeDataPtr Null => new ImGuiFocusScopeDataPtr(null);

		public ImGuiFocusScopeData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiFocusScopeDataPtr(ImGuiFocusScopeData* handle) => new ImGuiFocusScopeDataPtr(handle);

		public static implicit operator ImGuiFocusScopeData*(ImGuiFocusScopeDataPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiFocusScopeDataPtr left, ImGuiFocusScopeDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiFocusScopeDataPtr left, ImGuiFocusScopeDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiFocusScopeDataPtr left, ImGuiFocusScopeData* right) => left.Handle == right;

		public static bool operator !=(ImGuiFocusScopeDataPtr left, ImGuiFocusScopeData* right) => left.Handle != right;

		public bool Equals(ImGuiFocusScopeDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiFocusScopeDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiFocusScopeDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ID => ref Unsafe.AsRef<uint>(&Handle->ID);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint WindowID => ref Unsafe.AsRef<uint>(&Handle->WindowID);
	}

}