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
	public partial struct ImGuiWindowStackData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiWindow* Window;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiLastItemData ParentLastItemDataBackup;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiErrorRecoveryState StackSizesInBegin;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte DisabledOverrideReenable;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float DisabledOverrideReenableAlphaBackup;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiWindowStackData(ImGuiWindowPtr window = default, ImGuiLastItemData parentLastItemDataBackup = default, ImGuiErrorRecoveryState stackSizesInBegin = default, bool disabledOverrideReenable = default, float disabledOverrideReenableAlphaBackup = default)
		{
			Window = window;
			ParentLastItemDataBackup = parentLastItemDataBackup;
			StackSizesInBegin = stackSizesInBegin;
			DisabledOverrideReenable = disabledOverrideReenable ? (byte)1 : (byte)0;
			DisabledOverrideReenableAlphaBackup = disabledOverrideReenableAlphaBackup;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiWindowStackDataPtr : IEquatable<ImGuiWindowStackDataPtr>
	{
		public ImGuiWindowStackDataPtr(ImGuiWindowStackData* handle) { Handle = handle; }

		public ImGuiWindowStackData* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiWindowStackDataPtr Null => new ImGuiWindowStackDataPtr(null);

		public ImGuiWindowStackData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiWindowStackDataPtr(ImGuiWindowStackData* handle) => new ImGuiWindowStackDataPtr(handle);

		public static implicit operator ImGuiWindowStackData*(ImGuiWindowStackDataPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiWindowStackDataPtr left, ImGuiWindowStackDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiWindowStackDataPtr left, ImGuiWindowStackDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiWindowStackDataPtr left, ImGuiWindowStackData* right) => left.Handle == right;

		public static bool operator !=(ImGuiWindowStackDataPtr left, ImGuiWindowStackData* right) => left.Handle != right;

		public bool Equals(ImGuiWindowStackDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiWindowStackDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiWindowStackDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiWindowPtr Window => ref Unsafe.AsRef<ImGuiWindowPtr>(&Handle->Window);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiLastItemData ParentLastItemDataBackup => ref Unsafe.AsRef<ImGuiLastItemData>(&Handle->ParentLastItemDataBackup);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiErrorRecoveryState StackSizesInBegin => ref Unsafe.AsRef<ImGuiErrorRecoveryState>(&Handle->StackSizesInBegin);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool DisabledOverrideReenable => ref Unsafe.AsRef<bool>(&Handle->DisabledOverrideReenable);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float DisabledOverrideReenableAlphaBackup => ref Unsafe.AsRef<float>(&Handle->DisabledOverrideReenableAlphaBackup);
	}

}
