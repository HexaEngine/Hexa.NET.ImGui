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
	public partial struct ImGuiPtrOrIndex
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* Ptr;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Index;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiPtrOrIndex(void* ptr = default, int index = default)
		{
			Ptr = ptr;
			Index = index;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiPtrOrIndexPtr : IEquatable<ImGuiPtrOrIndexPtr>
	{
		public ImGuiPtrOrIndexPtr(ImGuiPtrOrIndex* handle) { Handle = handle; }

		public ImGuiPtrOrIndex* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiPtrOrIndexPtr Null => new ImGuiPtrOrIndexPtr(null);

		public ImGuiPtrOrIndex this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiPtrOrIndexPtr(ImGuiPtrOrIndex* handle) => new ImGuiPtrOrIndexPtr(handle);

		public static implicit operator ImGuiPtrOrIndex*(ImGuiPtrOrIndexPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiPtrOrIndexPtr left, ImGuiPtrOrIndexPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiPtrOrIndexPtr left, ImGuiPtrOrIndexPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiPtrOrIndexPtr left, ImGuiPtrOrIndex* right) => left.Handle == right;

		public static bool operator !=(ImGuiPtrOrIndexPtr left, ImGuiPtrOrIndex* right) => left.Handle != right;

		public bool Equals(ImGuiPtrOrIndexPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiPtrOrIndexPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiPtrOrIndexPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* Ptr { get => Handle->Ptr; set => Handle->Ptr = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Index => ref Unsafe.AsRef<int>(&Handle->Index);
	}

}