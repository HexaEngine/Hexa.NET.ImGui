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

namespace Hexa.NET.ImPlot
{
	/// <summary>
	/// To be documented.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ImPlotColormapData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<uint> Keys;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> KeyCounts;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> KeyOffsets;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<uint> Tables;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> TableSizes;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> TableOffsets;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiTextBuffer Text;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> TextOffsets;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImVector<int> Quals;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImGuiStorage Map;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int Count;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormapData(ImVector<uint> keys = default, ImVector<int> keyCounts = default, ImVector<int> keyOffsets = default, ImVector<uint> tables = default, ImVector<int> tableSizes = default, ImVector<int> tableOffsets = default, ImGuiTextBuffer text = default, ImVector<int> textOffsets = default, ImVector<int> quals = default, ImGuiStorage map = default, int count = default)
		{
			Keys = keys;
			KeyCounts = keyCounts;
			KeyOffsets = keyOffsets;
			Tables = tables;
			TableSizes = tableSizes;
			TableOffsets = tableOffsets;
			Text = text;
			TextOffsets = textOffsets;
			Quals = quals;
			Map = map;
			Count = count;
		}


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void _AppendTable(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				ImPlot._AppendTableNative(@this, cmap);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(byte* name, uint* keys, int count, bool qual)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				int ret = ImPlot.AppendNative(@this, name, keys, count, qual ? (byte)1 : (byte)0);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(ref byte name, uint* keys, int count, bool qual)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				fixed (byte* pname = &name)
				{
					int ret = ImPlot.AppendNative(@this, (byte*)pname, keys, count, qual ? (byte)1 : (byte)0);
					return ret;
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(ReadOnlySpan<byte> name, uint* keys, int count, bool qual)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				fixed (byte* pname = name)
				{
					int ret = ImPlot.AppendNative(@this, (byte*)pname, keys, count, qual ? (byte)1 : (byte)0);
					return ret;
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(string name, uint* keys, int count, bool qual)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				byte* pStr0 = null;
				int pStrSize0 = 0;
				if (name != null)
				{
					pStrSize0 = Utils.GetByteCountUTF8(name);
					if (pStrSize0 >= Utils.MaxStackallocSize)
					{
						pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
					}
					else
					{
						byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
						pStr0 = pStrStack0;
					}
					int pStrOffset0 = Utils.EncodeStringUTF8(name, pStr0, pStrSize0);
					pStr0[pStrOffset0] = 0;
				}
				int ret = ImPlot.AppendNative(@this, pStr0, keys, count, qual ? (byte)1 : (byte)0);
				if (pStrSize0 >= Utils.MaxStackallocSize)
				{
					Utils.Free(pStr0);
				}
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				ImPlot.DestroyNative(@this);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(byte* name)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				ImPlotColormap ret = ImPlot.GetIndexNative(@this, name);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(ref byte name)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				fixed (byte* pname = &name)
				{
					ImPlotColormap ret = ImPlot.GetIndexNative(@this, (byte*)pname);
					return ret;
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(ReadOnlySpan<byte> name)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				fixed (byte* pname = name)
				{
					ImPlotColormap ret = ImPlot.GetIndexNative(@this, (byte*)pname);
					return ret;
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(string name)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				byte* pStr0 = null;
				int pStrSize0 = 0;
				if (name != null)
				{
					pStrSize0 = Utils.GetByteCountUTF8(name);
					if (pStrSize0 >= Utils.MaxStackallocSize)
					{
						pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
					}
					else
					{
						byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
						pStr0 = pStrStack0;
					}
					int pStrOffset0 = Utils.EncodeStringUTF8(name, pStr0, pStrSize0);
					pStr0[pStrOffset0] = 0;
				}
				ImPlotColormap ret = ImPlot.GetIndexNative(@this, pStr0);
				if (pStrSize0 >= Utils.MaxStackallocSize)
				{
					Utils.Free(pStr0);
				}
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint GetKeyColor(ImPlotColormap cmap, int idx)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				uint ret = ImPlot.GetKeyColorNative(@this, cmap, idx);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int GetKeyCount(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				int ret = ImPlot.GetKeyCountNative(@this, cmap);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint* GetKeys(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				uint* ret = ImPlot.GetKeysNative(@this, cmap);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe byte* GetName(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				byte* ret = ImPlot.GetNameNative(@this, cmap);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe string GetNameS(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				string ret = Utils.DecodeStringUTF8(ImPlot.GetNameNative(@this, cmap));
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint* GetTable(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				uint* ret = ImPlot.GetTableNative(@this, cmap);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint GetTableColor(ImPlotColormap cmap, int idx)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				uint ret = ImPlot.GetTableColorNative(@this, cmap, idx);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int GetTableSize(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				int ret = ImPlot.GetTableSizeNative(@this, cmap);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe bool IsQual(ImPlotColormap cmap)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				byte ret = ImPlot.IsQualNative(@this, cmap);
				return ret != 0;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint LerpTable(ImPlotColormap cmap, float t)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				uint ret = ImPlot.LerpTableNative(@this, cmap, t);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void RebuildTables()
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				ImPlot.RebuildTablesNative(@this);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void SetKeyColor(ImPlotColormap cmap, int idx, uint value)
		{
			fixed (ImPlotColormapData* @this = &this)
			{
				ImPlot.SetKeyColorNative(@this, cmap, idx, value);
			}
		}

	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImPlotColormapDataPtr : IEquatable<ImPlotColormapDataPtr>
	{
		public ImPlotColormapDataPtr(ImPlotColormapData* handle) { Handle = handle; }

		public ImPlotColormapData* Handle;

		public bool IsNull => Handle == null;

		public static ImPlotColormapDataPtr Null => new ImPlotColormapDataPtr(null);

		public ImPlotColormapData this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImPlotColormapDataPtr(ImPlotColormapData* handle) => new ImPlotColormapDataPtr(handle);

		public static implicit operator ImPlotColormapData*(ImPlotColormapDataPtr handle) => handle.Handle;

		public static bool operator ==(ImPlotColormapDataPtr left, ImPlotColormapDataPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImPlotColormapDataPtr left, ImPlotColormapDataPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImPlotColormapDataPtr left, ImPlotColormapData* right) => left.Handle == right;

		public static bool operator !=(ImPlotColormapDataPtr left, ImPlotColormapData* right) => left.Handle != right;

		public bool Equals(ImPlotColormapDataPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImPlotColormapDataPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImPlotColormapDataPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<uint> Keys => ref Unsafe.AsRef<ImVector<uint>>(&Handle->Keys);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> KeyCounts => ref Unsafe.AsRef<ImVector<int>>(&Handle->KeyCounts);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> KeyOffsets => ref Unsafe.AsRef<ImVector<int>>(&Handle->KeyOffsets);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<uint> Tables => ref Unsafe.AsRef<ImVector<uint>>(&Handle->Tables);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> TableSizes => ref Unsafe.AsRef<ImVector<int>>(&Handle->TableSizes);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> TableOffsets => ref Unsafe.AsRef<ImVector<int>>(&Handle->TableOffsets);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiTextBuffer Text => ref Unsafe.AsRef<ImGuiTextBuffer>(&Handle->Text);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> TextOffsets => ref Unsafe.AsRef<ImVector<int>>(&Handle->TextOffsets);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImVector<int> Quals => ref Unsafe.AsRef<ImVector<int>>(&Handle->Quals);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImGuiStorage Map => ref Unsafe.AsRef<ImGuiStorage>(&Handle->Map);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int Count => ref Unsafe.AsRef<int>(&Handle->Count);
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void _AppendTable(ImPlotColormap cmap)
		{
			ImPlot._AppendTableNative(Handle, cmap);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(byte* name, uint* keys, int count, bool qual)
		{
			int ret = ImPlot.AppendNative(Handle, name, keys, count, qual ? (byte)1 : (byte)0);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(ref byte name, uint* keys, int count, bool qual)
		{
			fixed (byte* pname = &name)
			{
				int ret = ImPlot.AppendNative(Handle, (byte*)pname, keys, count, qual ? (byte)1 : (byte)0);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(ReadOnlySpan<byte> name, uint* keys, int count, bool qual)
		{
			fixed (byte* pname = name)
			{
				int ret = ImPlot.AppendNative(Handle, (byte*)pname, keys, count, qual ? (byte)1 : (byte)0);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int Append(string name, uint* keys, int count, bool qual)
		{
			byte* pStr0 = null;
			int pStrSize0 = 0;
			if (name != null)
			{
				pStrSize0 = Utils.GetByteCountUTF8(name);
				if (pStrSize0 >= Utils.MaxStackallocSize)
				{
					pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
				}
				else
				{
					byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
					pStr0 = pStrStack0;
				}
				int pStrOffset0 = Utils.EncodeStringUTF8(name, pStr0, pStrSize0);
				pStr0[pStrOffset0] = 0;
			}
			int ret = ImPlot.AppendNative(Handle, pStr0, keys, count, qual ? (byte)1 : (byte)0);
			if (pStrSize0 >= Utils.MaxStackallocSize)
			{
				Utils.Free(pStr0);
			}
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void Destroy()
		{
			ImPlot.DestroyNative(Handle);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(byte* name)
		{
			ImPlotColormap ret = ImPlot.GetIndexNative(Handle, name);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(ref byte name)
		{
			fixed (byte* pname = &name)
			{
				ImPlotColormap ret = ImPlot.GetIndexNative(Handle, (byte*)pname);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(ReadOnlySpan<byte> name)
		{
			fixed (byte* pname = name)
			{
				ImPlotColormap ret = ImPlot.GetIndexNative(Handle, (byte*)pname);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotColormap GetIndex(string name)
		{
			byte* pStr0 = null;
			int pStrSize0 = 0;
			if (name != null)
			{
				pStrSize0 = Utils.GetByteCountUTF8(name);
				if (pStrSize0 >= Utils.MaxStackallocSize)
				{
					pStr0 = Utils.Alloc<byte>(pStrSize0 + 1);
				}
				else
				{
					byte* pStrStack0 = stackalloc byte[pStrSize0 + 1];
					pStr0 = pStrStack0;
				}
				int pStrOffset0 = Utils.EncodeStringUTF8(name, pStr0, pStrSize0);
				pStr0[pStrOffset0] = 0;
			}
			ImPlotColormap ret = ImPlot.GetIndexNative(Handle, pStr0);
			if (pStrSize0 >= Utils.MaxStackallocSize)
			{
				Utils.Free(pStr0);
			}
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint GetKeyColor(ImPlotColormap cmap, int idx)
		{
			uint ret = ImPlot.GetKeyColorNative(Handle, cmap, idx);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int GetKeyCount(ImPlotColormap cmap)
		{
			int ret = ImPlot.GetKeyCountNative(Handle, cmap);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint* GetKeys(ImPlotColormap cmap)
		{
			uint* ret = ImPlot.GetKeysNative(Handle, cmap);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe byte* GetName(ImPlotColormap cmap)
		{
			byte* ret = ImPlot.GetNameNative(Handle, cmap);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe string GetNameS(ImPlotColormap cmap)
		{
			string ret = Utils.DecodeStringUTF8(ImPlot.GetNameNative(Handle, cmap));
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint* GetTable(ImPlotColormap cmap)
		{
			uint* ret = ImPlot.GetTableNative(Handle, cmap);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint GetTableColor(ImPlotColormap cmap, int idx)
		{
			uint ret = ImPlot.GetTableColorNative(Handle, cmap, idx);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe int GetTableSize(ImPlotColormap cmap)
		{
			int ret = ImPlot.GetTableSizeNative(Handle, cmap);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe bool IsQual(ImPlotColormap cmap)
		{
			byte ret = ImPlot.IsQualNative(Handle, cmap);
			return ret != 0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe uint LerpTable(ImPlotColormap cmap, float t)
		{
			uint ret = ImPlot.LerpTableNative(Handle, cmap, t);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void RebuildTables()
		{
			ImPlot.RebuildTablesNative(Handle);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void SetKeyColor(ImPlotColormap cmap, int idx, uint value)
		{
			ImPlot.SetKeyColorNative(Handle, cmap, idx, value);
		}

	}

}
