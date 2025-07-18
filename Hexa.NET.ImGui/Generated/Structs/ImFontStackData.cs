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
	public partial struct ImFontStackData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImFont* Font;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float FontSizeBeforeScaling;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float FontSizeAfterScaling;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImFontStackData(ImFontPtr font = default, float fontSizeBeforeScaling = default, float fontSizeAfterScaling = default)
		{
			Font = font;
			FontSizeBeforeScaling = fontSizeBeforeScaling;
			FontSizeAfterScaling = fontSizeAfterScaling;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImFontStackDataPtr : IEquatable<ImFontStackDataPtr>
	{
		public ImFontStackDataPtr(ImFontStackData* handle) { Handle = handle; }

		public ImFontStackData* Handle;

		public bool IsNull => Handle == null;

		public static ImFontStackDataPtr Null => new ImFontStackDataPtr(null);

		public ImFontStackData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImFontStackDataPtr(ImFontStackData* handle) => new ImFontStackDataPtr(handle);

		public static implicit operator ImFontStackData*(ImFontStackDataPtr handle) => handle.Handle;

		public static bool operator ==(ImFontStackDataPtr left, ImFontStackDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImFontStackDataPtr left, ImFontStackDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImFontStackDataPtr left, ImFontStackData* right) => left.Handle == right;

		public static bool operator !=(ImFontStackDataPtr left, ImFontStackData* right) => left.Handle != right;

		public bool Equals(ImFontStackDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImFontStackDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImFontStackDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImFontPtr Font => ref Unsafe.AsRef<ImFontPtr>(&Handle->Font);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float FontSizeBeforeScaling => ref Unsafe.AsRef<float>(&Handle->FontSizeBeforeScaling);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float FontSizeAfterScaling => ref Unsafe.AsRef<float>(&Handle->FontSizeAfterScaling);
	}

}
