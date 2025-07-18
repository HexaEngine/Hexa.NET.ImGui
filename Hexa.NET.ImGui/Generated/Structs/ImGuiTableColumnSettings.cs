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
	public partial struct ImGuiTableColumnSettings
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public float WidthOrWeight;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint UserID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public sbyte Index;

		/// <summary>
		/// To be documented.
		/// </summary>
		public sbyte DisplayOrder;

		/// <summary>
		/// To be documented.
		/// </summary>
		public sbyte SortOrder;

		public byte RawBits0;
		public sbyte RawBits1;
		public byte RawBits2;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiTableColumnSettings(float widthOrWeight = default, uint userId = default, sbyte index = default, sbyte displayOrder = default, sbyte sortOrder = default, byte sortDirection = default, sbyte isEnabled = default, byte isStretch = default)
		{
			WidthOrWeight = widthOrWeight;
			UserID = userId;
			Index = index;
			DisplayOrder = displayOrder;
			SortOrder = sortOrder;
			SortDirection = sortDirection;
			IsEnabled = isEnabled;
			IsStretch = isStretch;
		}


		public byte SortDirection { get => Bitfield.Get(RawBits0, 0, 2); set => Bitfield.Set(ref RawBits0, value, 0, 2); }

		public sbyte IsEnabled { get => Bitfield.Get(RawBits1, 0, 2); set => Bitfield.Set(ref RawBits1, value, 0, 2); }

		public byte IsStretch { get => Bitfield.Get(RawBits2, 0, 1); set => Bitfield.Set(ref RawBits2, value, 0, 1); }

	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiTableColumnSettingsPtr : IEquatable<ImGuiTableColumnSettingsPtr>
	{
		public ImGuiTableColumnSettingsPtr(ImGuiTableColumnSettings* handle) { Handle = handle; }

		public ImGuiTableColumnSettings* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiTableColumnSettingsPtr Null => new ImGuiTableColumnSettingsPtr(null);

		public ImGuiTableColumnSettings this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiTableColumnSettingsPtr(ImGuiTableColumnSettings* handle) => new ImGuiTableColumnSettingsPtr(handle);

		public static implicit operator ImGuiTableColumnSettings*(ImGuiTableColumnSettingsPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiTableColumnSettingsPtr left, ImGuiTableColumnSettingsPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiTableColumnSettingsPtr left, ImGuiTableColumnSettingsPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiTableColumnSettingsPtr left, ImGuiTableColumnSettings* right) => left.Handle == right;

		public static bool operator !=(ImGuiTableColumnSettingsPtr left, ImGuiTableColumnSettings* right) => left.Handle != right;

		public bool Equals(ImGuiTableColumnSettingsPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiTableColumnSettingsPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiTableColumnSettingsPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float WidthOrWeight => ref Unsafe.AsRef<float>(&Handle->WidthOrWeight);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint UserID => ref Unsafe.AsRef<uint>(&Handle->UserID);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref sbyte Index => ref Unsafe.AsRef<sbyte>(&Handle->Index);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref sbyte DisplayOrder => ref Unsafe.AsRef<sbyte>(&Handle->DisplayOrder);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref sbyte SortOrder => ref Unsafe.AsRef<sbyte>(&Handle->SortOrder);
		/// <summary>
		/// To be documented.
		/// </summary>
		public byte SortDirection { get => Handle->SortDirection; set => Handle->SortDirection = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public sbyte IsEnabled { get => Handle->IsEnabled; set => Handle->IsEnabled = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public byte IsStretch { get => Handle->IsStretch; set => Handle->IsStretch = value; }
	}

}
