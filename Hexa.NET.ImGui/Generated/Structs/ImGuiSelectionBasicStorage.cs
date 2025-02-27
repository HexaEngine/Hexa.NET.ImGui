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
	/// Optional helper to store multi-selection state + apply multi-selection requests.<br/>
	/// - Used by our demos and provided as a convenience to easily implement basic multi-selection.<br/>
	/// - Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id))  ... '<br/>
	/// Or you can check 'if (Contains(id))  ... ' for each possible object if their number is not too high to iterate.<br/>
	/// - USING THIS IS NOT MANDATORY. This is only a helper and not a required API.<br/>
	/// To store a multi-selection, in your application you could:<br/>
	/// - Use this helper as a convenience. We use our simple key-&gt;value ImGuiStorage as a std::set&lt;ImGuiID&gt; replacement.<br/>
	/// - Use your own external storage: e.g. std::set&lt;MyObjectId&gt;, std::vector&lt;MyObjectId&gt;, interval trees, intrusively stored selection etc.<br/>
	/// In ImGuiSelectionBasicStorage we:<br/>
	/// - always use indices in the multi-selection API (passed to SetNextItemSelectionUserData(), retrieved in ImGuiMultiSelectIO)<br/>
	/// - use the AdapterIndexToStorageId() indirection layer to abstract how persistent selection data is derived from an index.<br/>
	/// - use decently optimized logic to allow queries and insertion of very large selection sets.<br/>
	/// - do not preserve selection order.<br/>
	/// Many combinations are possible depending on how you prefer to store your items and how you prefer to store your selection.<br/>
	/// Large applications are likely to eventually want to get rid of this indirection layer and do their own thing.<br/>
	/// See https:github.comocornutimguiwikiMulti-Select for details and pseudo-code using this helper.<br/>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ImGuiSelectionBasicStorage
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public int Size;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte PreserveOrder;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* UserData;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* AdapterIndexToStorageId;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int SelectionOrder;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiStorage Storage;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiSelectionBasicStorage(int size = default, bool preserveOrder = default, void* userData = default, delegate*<ImGuiSelectionBasicStorage*, int, uint> adapterIndexToStorageId = default, int selectionOrder = default, ImGuiStorage storage = default)
		{
			Size = size;
			PreserveOrder = preserveOrder ? (byte)1 : (byte)0;
			UserData = userData;
			AdapterIndexToStorageId = (void*)adapterIndexToStorageId;
			SelectionOrder = selectionOrder;
			Storage = storage;
		}


		/// <summary>
		/// Apply selection requests coming from BeginMultiSelect() and EndMultiSelect() functions. It uses 'items_count' passed to BeginMultiSelect()<br/>
		/// </summary>
		public unsafe void ApplyRequests(ImGuiMultiSelectIOPtr msIo)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				ImGui.ApplyRequestsNative(@this, msIo);
			}
		}

		/// <summary>
		/// Apply selection requests coming from BeginMultiSelect() and EndMultiSelect() functions. It uses 'items_count' passed to BeginMultiSelect()<br/>
		/// </summary>
		public unsafe void ApplyRequests(ref ImGuiMultiSelectIO msIo)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				fixed (ImGuiMultiSelectIO* pmsIo = &msIo)
				{
					ImGui.ApplyRequestsNative(@this, (ImGuiMultiSelectIO*)pmsIo);
				}
			}
		}

		/// <summary>
		/// Clear selection<br/>
		/// </summary>
		public unsafe void Clear()
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				ImGui.ClearNative(@this);
			}
		}

		/// <summary>
		/// Query if an item id is in selection.<br/>
		/// </summary>
		public unsafe bool Contains(uint id)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				byte ret = ImGui.ContainsNative(@this, id);
				return ret != 0;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				ImGui.DestroyNative(@this);
			}
		}

		/// <summary>
		/// Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id))  ... '<br/>
		/// </summary>
		public unsafe bool GetNextSelectedItem(void** opaqueIt, uint* outId)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				byte ret = ImGui.GetNextSelectedItemNative(@this, opaqueIt, outId);
				return ret != 0;
			}
		}

		/// <summary>
		/// Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id))  ... '<br/>
		/// </summary>
		public unsafe bool GetNextSelectedItem(void** opaqueIt, ref uint outId)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				fixed (uint* poutId = &outId)
				{
					byte ret = ImGui.GetNextSelectedItemNative(@this, opaqueIt, (uint*)poutId);
					return ret != 0;
				}
			}
		}

		/// <summary>
		/// Convert index to item id based on provided adapter.<br/>
		/// </summary>
		public unsafe uint GetStorageIdFromIndex(int idx)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				uint ret = ImGui.GetStorageIdFromIndexNative(@this, idx);
				return ret;
			}
		}

		/// <summary>
		/// Addremove an item from selection (generally done by ApplyRequests() function)<br/>
		/// </summary>
		public unsafe void SetItemSelected(uint id, bool selected)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				ImGui.SetItemSelectedNative(@this, id, selected ? (byte)1 : (byte)0);
			}
		}

		/// <summary>
		/// Swap two selections<br/>
		/// </summary>
		public unsafe void Swap(ImGuiSelectionBasicStorage* r)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				ImGui.SwapNative(@this, r);
			}
		}

		/// <summary>
		/// Swap two selections<br/>
		/// </summary>
		public unsafe void Swap(ref ImGuiSelectionBasicStorage r)
		{
			fixed (ImGuiSelectionBasicStorage* @this = &this)
			{
				fixed (ImGuiSelectionBasicStorage* pr = &r)
				{
					ImGui.SwapNative(@this, (ImGuiSelectionBasicStorage*)pr);
				}
			}
		}

	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImGuiSelectionBasicStoragePtr : IEquatable<ImGuiSelectionBasicStoragePtr>
	{
		public ImGuiSelectionBasicStoragePtr(ImGuiSelectionBasicStorage* handle) { Handle = handle; }

		public ImGuiSelectionBasicStorage* Handle;

		public bool IsNull => Handle == null;

		public static ImGuiSelectionBasicStoragePtr Null => new ImGuiSelectionBasicStoragePtr(null);

		public ImGuiSelectionBasicStorage this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImGuiSelectionBasicStoragePtr(ImGuiSelectionBasicStorage* handle) => new ImGuiSelectionBasicStoragePtr(handle);

		public static implicit operator ImGuiSelectionBasicStorage*(ImGuiSelectionBasicStoragePtr handle) => handle.Handle;

		public static bool operator ==(ImGuiSelectionBasicStoragePtr left, ImGuiSelectionBasicStoragePtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImGuiSelectionBasicStoragePtr left, ImGuiSelectionBasicStoragePtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImGuiSelectionBasicStoragePtr left, ImGuiSelectionBasicStorage* right) => left.Handle == right;

		public static bool operator !=(ImGuiSelectionBasicStoragePtr left, ImGuiSelectionBasicStorage* right) => left.Handle != right;

		public bool Equals(ImGuiSelectionBasicStoragePtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImGuiSelectionBasicStoragePtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImGuiSelectionBasicStoragePtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Size => ref Unsafe.AsRef<int>(&Handle->Size);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool PreserveOrder => ref Unsafe.AsRef<bool>(&Handle->PreserveOrder);
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* UserData { get => Handle->UserData; set => Handle->UserData = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* AdapterIndexToStorageId { get => Handle->AdapterIndexToStorageId; set => Handle->AdapterIndexToStorageId = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int SelectionOrder => ref Unsafe.AsRef<int>(&Handle->SelectionOrder);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiStorage Storage => ref Unsafe.AsRef<ImGuiStorage>(&Handle->Storage);
		/// <summary>
		/// Apply selection requests coming from BeginMultiSelect() and EndMultiSelect() functions. It uses 'items_count' passed to BeginMultiSelect()<br/>
		/// </summary>
		public unsafe void ApplyRequests(ImGuiMultiSelectIOPtr msIo)
		{
			ImGui.ApplyRequestsNative(Handle, msIo);
		}

		/// <summary>
		/// Apply selection requests coming from BeginMultiSelect() and EndMultiSelect() functions. It uses 'items_count' passed to BeginMultiSelect()<br/>
		/// </summary>
		public unsafe void ApplyRequests(ref ImGuiMultiSelectIO msIo)
		{
			fixed (ImGuiMultiSelectIO* pmsIo = &msIo)
			{
				ImGui.ApplyRequestsNative(Handle, (ImGuiMultiSelectIO*)pmsIo);
			}
		}

		/// <summary>
		/// Clear selection<br/>
		/// </summary>
		public unsafe void Clear()
		{
			ImGui.ClearNative(Handle);
		}

		/// <summary>
		/// Query if an item id is in selection.<br/>
		/// </summary>
		public unsafe bool Contains(uint id)
		{
			byte ret = ImGui.ContainsNative(Handle, id);
			return ret != 0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			ImGui.DestroyNative(Handle);
		}

		/// <summary>
		/// Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id))  ... '<br/>
		/// </summary>
		public unsafe bool GetNextSelectedItem(void** opaqueIt, uint* outId)
		{
			byte ret = ImGui.GetNextSelectedItemNative(Handle, opaqueIt, outId);
			return ret != 0;
		}

		/// <summary>
		/// Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id))  ... '<br/>
		/// </summary>
		public unsafe bool GetNextSelectedItem(void** opaqueIt, ref uint outId)
		{
			fixed (uint* poutId = &outId)
			{
				byte ret = ImGui.GetNextSelectedItemNative(Handle, opaqueIt, (uint*)poutId);
				return ret != 0;
			}
		}

		/// <summary>
		/// Convert index to item id based on provided adapter.<br/>
		/// </summary>
		public unsafe uint GetStorageIdFromIndex(int idx)
		{
			uint ret = ImGui.GetStorageIdFromIndexNative(Handle, idx);
			return ret;
		}

		/// <summary>
		/// Addremove an item from selection (generally done by ApplyRequests() function)<br/>
		/// </summary>
		public unsafe void SetItemSelected(uint id, bool selected)
		{
			ImGui.SetItemSelectedNative(Handle, id, selected ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Swap two selections<br/>
		/// </summary>
		public unsafe void Swap(ImGuiSelectionBasicStorage* r)
		{
			ImGui.SwapNative(Handle, r);
		}

		/// <summary>
		/// Swap two selections<br/>
		/// </summary>
		public unsafe void Swap(ref ImGuiSelectionBasicStorage r)
		{
			fixed (ImGuiSelectionBasicStorage* pr = &r)
			{
				ImGui.SwapNative(Handle, (ImGuiSelectionBasicStorage*)pr);
			}
		}

	}

}
