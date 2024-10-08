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
	public partial struct ImGuiSettingsHandler
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe byte* TypeName;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint TypeHash;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* ClearAllFn;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* ReadInitFn;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* ReadOpenFn;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* ReadLineFn;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* ApplyAllFn;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* WriteAllFn;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* UserData;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiSettingsHandler(byte* typeName = default, uint typeHash = default, delegate*<ImGuiContext*, ImGuiSettingsHandler*, void> clearAllFn = default, delegate*<ImGuiContext*, ImGuiSettingsHandler*, void> readInitFn = default, delegate*<ImGuiContext*, ImGuiSettingsHandler*, byte*, void*> readOpenFn = default, delegate*<ImGuiContext*, ImGuiSettingsHandler*, void*, byte*, void> readLineFn = default, delegate*<ImGuiContext*, ImGuiSettingsHandler*, void> applyAllFn = default, delegate*<ImGuiContext*, ImGuiSettingsHandler*, ImGuiTextBuffer*, void> writeAllFn = default, void* userData = default)
		{
			TypeName = typeName;
			TypeHash = typeHash;
			ClearAllFn = (void*)clearAllFn;
			ReadInitFn = (void*)readInitFn;
			ReadOpenFn = (void*)readOpenFn;
			ReadLineFn = (void*)readLineFn;
			ApplyAllFn = (void*)applyAllFn;
			WriteAllFn = (void*)writeAllFn;
			UserData = userData;
		}


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			fixed (ImGuiSettingsHandler* @this = &this)
			{
				ImGui.DestroyNative(@this);
			}
		}

	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiSettingsHandlerPtr : IEquatable<ImGuiSettingsHandlerPtr>
	{
		public ImGuiSettingsHandlerPtr(ImGuiSettingsHandler* handle) { Handle = handle; }

		public ImGuiSettingsHandler* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiSettingsHandlerPtr Null => new ImGuiSettingsHandlerPtr(null);

		public ImGuiSettingsHandler this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiSettingsHandlerPtr(ImGuiSettingsHandler* handle) => new ImGuiSettingsHandlerPtr(handle);

		public static implicit operator ImGuiSettingsHandler*(ImGuiSettingsHandlerPtr handle) => handle.Handle;

		public static bool operator ==(ImGuiSettingsHandlerPtr left, ImGuiSettingsHandlerPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiSettingsHandlerPtr left, ImGuiSettingsHandlerPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiSettingsHandlerPtr left, ImGuiSettingsHandler* right) => left.Handle == right;

		public static bool operator !=(ImGuiSettingsHandlerPtr left, ImGuiSettingsHandler* right) => left.Handle != right;

		public bool Equals(ImGuiSettingsHandlerPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiSettingsHandlerPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiSettingsHandlerPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public byte* TypeName { get => Handle->TypeName; set => Handle->TypeName = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint TypeHash => ref Unsafe.AsRef<uint>(&Handle->TypeHash);
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* ClearAllFn { get => Handle->ClearAllFn; set => Handle->ClearAllFn = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* ReadInitFn { get => Handle->ReadInitFn; set => Handle->ReadInitFn = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* ReadOpenFn { get => Handle->ReadOpenFn; set => Handle->ReadOpenFn = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* ReadLineFn { get => Handle->ReadLineFn; set => Handle->ReadLineFn = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* ApplyAllFn { get => Handle->ApplyAllFn; set => Handle->ApplyAllFn = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* WriteAllFn { get => Handle->WriteAllFn; set => Handle->WriteAllFn = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* UserData { get => Handle->UserData; set => Handle->UserData = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			ImGui.DestroyNative(Handle);
		}

	}

}
