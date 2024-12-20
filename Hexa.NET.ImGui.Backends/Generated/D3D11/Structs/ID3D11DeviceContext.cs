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
using Hexa.NET.ImGui;

namespace Hexa.NET.ImGui.Backends.D3D11
{
	/// <summary>
	/// To be documented.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ID3D11DeviceContext
	{


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ID3D11DeviceContextPtr : IEquatable<ID3D11DeviceContextPtr>
	{
		public ID3D11DeviceContextPtr(ID3D11DeviceContext* handle) { Handle = handle; }

		public ID3D11DeviceContext* Handle;

		public bool IsNull => Handle == null;

		public static ID3D11DeviceContextPtr Null => new ID3D11DeviceContextPtr(null);

		public ID3D11DeviceContext this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ID3D11DeviceContextPtr(ID3D11DeviceContext* handle) => new ID3D11DeviceContextPtr(handle);

		public static implicit operator ID3D11DeviceContext*(ID3D11DeviceContextPtr handle) => handle.Handle;

		public static bool operator ==(ID3D11DeviceContextPtr left, ID3D11DeviceContextPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ID3D11DeviceContextPtr left, ID3D11DeviceContextPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ID3D11DeviceContextPtr left, ID3D11DeviceContext* right) => left.Handle == right;

		public static bool operator !=(ID3D11DeviceContextPtr left, ID3D11DeviceContext* right) => left.Handle != right;

		public bool Equals(ID3D11DeviceContextPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ID3D11DeviceContextPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ID3D11DeviceContextPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
	}

}
