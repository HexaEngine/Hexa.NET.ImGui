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
	public partial struct ImGuiShrinkWidthItem
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public int Index;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float Width;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float InitialWidth;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiShrinkWidthItem(int index = default, float width = default, float initialWidth = default)
		{
			Index = index;
			Width = width;
			InitialWidth = initialWidth;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiShrinkWidthItemPtr : IEquatable<ImGuiShrinkWidthItemPtr>
	{
		public ImGuiShrinkWidthItemPtr(ImGuiShrinkWidthItem* handle) { Handle = handle; }

		public ImGuiShrinkWidthItem* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiShrinkWidthItemPtr Null => new ImGuiShrinkWidthItemPtr(null);

		public ImGuiShrinkWidthItem this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiShrinkWidthItemPtr(ImGuiShrinkWidthItem* handle) => new ImGuiShrinkWidthItemPtr(handle);

		public static implicit operator ImGuiShrinkWidthItem*(ImGuiShrinkWidthItemPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiShrinkWidthItemPtr left, ImGuiShrinkWidthItemPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiShrinkWidthItemPtr left, ImGuiShrinkWidthItemPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiShrinkWidthItemPtr left, ImGuiShrinkWidthItem* right) => left.Handle == right;

		public static bool operator !=(ImGuiShrinkWidthItemPtr left, ImGuiShrinkWidthItem* right) => left.Handle != right;

		public bool Equals(ImGuiShrinkWidthItemPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiShrinkWidthItemPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiShrinkWidthItemPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Index => ref Unsafe.AsRef<int>(&Handle->Index);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float Width => ref Unsafe.AsRef<float>(&Handle->Width);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float InitialWidth => ref Unsafe.AsRef<float>(&Handle->InitialWidth);
	}

}
