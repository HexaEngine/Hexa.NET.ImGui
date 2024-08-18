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
	public partial struct ImPlotAxis
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotAxisFlags Flags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotAxisFlags PreviousFlags;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotRange Range;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotCond RangeCond;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotScale Scale;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotRange FitExtents;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotAxis* OrthoAxis;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotRange ConstraintRange;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotRange ConstraintZoom;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotTicker Ticker;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* Formatter;
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* FormatterData;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte FormatSpec_0;
		public byte FormatSpec_1;
		public byte FormatSpec_2;
		public byte FormatSpec_3;
		public byte FormatSpec_4;
		public byte FormatSpec_5;
		public byte FormatSpec_6;
		public byte FormatSpec_7;
		public byte FormatSpec_8;
		public byte FormatSpec_9;
		public byte FormatSpec_10;
		public byte FormatSpec_11;
		public byte FormatSpec_12;
		public byte FormatSpec_13;
		public byte FormatSpec_14;
		public byte FormatSpec_15;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* Locator;
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe double* LinkedMin;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe double* LinkedMax;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int PickerLevel;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotTime PickerTimeMin;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImPlotTime PickerTimeMax;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* TransformForward;
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* TransformInverse;
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe void* TransformData;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PixelMin;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float PixelMax;

		/// <summary>
		/// To be documented.
		/// </summary>
		public double ScaleMin;

		/// <summary>
		/// To be documented.
		/// </summary>
		public double ScaleMax;

		/// <summary>
		/// To be documented.
		/// </summary>
		public double ScaleToPixel;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float Datum1;

		/// <summary>
		/// To be documented.
		/// </summary>
		public float Datum2;

		/// <summary>
		/// To be documented.
		/// </summary>
		public ImRect HoverRect;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int LabelOffset;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorMaj;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorMin;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorTick;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorTxt;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorBg;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorHov;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorAct;

		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ColorHiLi;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Enabled;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Vertical;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte FitThisFrame;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte HasRange;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte HasFormatSpec;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte ShowDefaultTicks;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Hovered;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte Held;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotAxis(uint id = default, ImPlotAxisFlags flags = default, ImPlotAxisFlags previousFlags = default, ImPlotRange range = default, ImPlotCond rangeCond = default, ImPlotScale scale = default, ImPlotRange fitExtents = default, ImPlotAxis* orthoAxis = default, ImPlotRange constraintRange = default, ImPlotRange constraintZoom = default, ImPlotTicker ticker = default, ImPlotFormatter formatter = default, void* formatterData = default, byte* formatSpec = default, ImPlotLocator locator = default, double* linkedMin = default, double* linkedMax = default, int pickerLevel = default, ImPlotTime pickerTimeMin = default, ImPlotTime pickerTimeMax = default, ImPlotTransform transformForward = default, ImPlotTransform transformInverse = default, void* transformData = default, float pixelMin = default, float pixelMax = default, double scaleMin = default, double scaleMax = default, double scaleToPixel = default, float datum1 = default, float datum2 = default, ImRect hoverRect = default, int labelOffset = default, uint colorMaj = default, uint colorMin = default, uint colorTick = default, uint colorTxt = default, uint colorBg = default, uint colorHov = default, uint colorAct = default, uint colorHiLi = default, bool enabled = default, bool vertical = default, bool fitThisFrame = default, bool hasRange = default, bool hasFormatSpec = default, bool showDefaultTicks = default, bool hovered = default, bool held = default)
		{
			ID = id;
			Flags = flags;
			PreviousFlags = previousFlags;
			Range = range;
			RangeCond = rangeCond;
			Scale = scale;
			FitExtents = fitExtents;
			OrthoAxis = orthoAxis;
			ConstraintRange = constraintRange;
			ConstraintZoom = constraintZoom;
			Ticker = ticker;
			Formatter = (void*)Marshal.GetFunctionPointerForDelegate(formatter);
			FormatterData = formatterData;
			if (formatSpec != default(byte*))
			{
				FormatSpec_0 = formatSpec[0];
				FormatSpec_1 = formatSpec[1];
				FormatSpec_2 = formatSpec[2];
				FormatSpec_3 = formatSpec[3];
				FormatSpec_4 = formatSpec[4];
				FormatSpec_5 = formatSpec[5];
				FormatSpec_6 = formatSpec[6];
				FormatSpec_7 = formatSpec[7];
				FormatSpec_8 = formatSpec[8];
				FormatSpec_9 = formatSpec[9];
				FormatSpec_10 = formatSpec[10];
				FormatSpec_11 = formatSpec[11];
				FormatSpec_12 = formatSpec[12];
				FormatSpec_13 = formatSpec[13];
				FormatSpec_14 = formatSpec[14];
				FormatSpec_15 = formatSpec[15];
			}
			Locator = (void*)Marshal.GetFunctionPointerForDelegate(locator);
			LinkedMin = linkedMin;
			LinkedMax = linkedMax;
			PickerLevel = pickerLevel;
			PickerTimeMin = pickerTimeMin;
			PickerTimeMax = pickerTimeMax;
			TransformForward = (void*)Marshal.GetFunctionPointerForDelegate(transformForward);
			TransformInverse = (void*)Marshal.GetFunctionPointerForDelegate(transformInverse);
			TransformData = transformData;
			PixelMin = pixelMin;
			PixelMax = pixelMax;
			ScaleMin = scaleMin;
			ScaleMax = scaleMax;
			ScaleToPixel = scaleToPixel;
			Datum1 = datum1;
			Datum2 = datum2;
			HoverRect = hoverRect;
			LabelOffset = labelOffset;
			ColorMaj = colorMaj;
			ColorMin = colorMin;
			ColorTick = colorTick;
			ColorTxt = colorTxt;
			ColorBg = colorBg;
			ColorHov = colorHov;
			ColorAct = colorAct;
			ColorHiLi = colorHiLi;
			Enabled = enabled ? (byte)1 : (byte)0;
			Vertical = vertical ? (byte)1 : (byte)0;
			FitThisFrame = fitThisFrame ? (byte)1 : (byte)0;
			HasRange = hasRange ? (byte)1 : (byte)0;
			HasFormatSpec = hasFormatSpec ? (byte)1 : (byte)0;
			ShowDefaultTicks = showDefaultTicks ? (byte)1 : (byte)0;
			Hovered = hovered ? (byte)1 : (byte)0;
			Held = held ? (byte)1 : (byte)0;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImPlotAxis(uint id = default, ImPlotAxisFlags flags = default, ImPlotAxisFlags previousFlags = default, ImPlotRange range = default, ImPlotCond rangeCond = default, ImPlotScale scale = default, ImPlotRange fitExtents = default, ImPlotAxis* orthoAxis = default, ImPlotRange constraintRange = default, ImPlotRange constraintZoom = default, ImPlotTicker ticker = default, ImPlotFormatter formatter = default, void* formatterData = default, Span<byte> formatSpec = default, ImPlotLocator locator = default, double* linkedMin = default, double* linkedMax = default, int pickerLevel = default, ImPlotTime pickerTimeMin = default, ImPlotTime pickerTimeMax = default, ImPlotTransform transformForward = default, ImPlotTransform transformInverse = default, void* transformData = default, float pixelMin = default, float pixelMax = default, double scaleMin = default, double scaleMax = default, double scaleToPixel = default, float datum1 = default, float datum2 = default, ImRect hoverRect = default, int labelOffset = default, uint colorMaj = default, uint colorMin = default, uint colorTick = default, uint colorTxt = default, uint colorBg = default, uint colorHov = default, uint colorAct = default, uint colorHiLi = default, bool enabled = default, bool vertical = default, bool fitThisFrame = default, bool hasRange = default, bool hasFormatSpec = default, bool showDefaultTicks = default, bool hovered = default, bool held = default)
		{
			ID = id;
			Flags = flags;
			PreviousFlags = previousFlags;
			Range = range;
			RangeCond = rangeCond;
			Scale = scale;
			FitExtents = fitExtents;
			OrthoAxis = orthoAxis;
			ConstraintRange = constraintRange;
			ConstraintZoom = constraintZoom;
			Ticker = ticker;
			Formatter = (void*)Marshal.GetFunctionPointerForDelegate(formatter);
			FormatterData = formatterData;
			if (formatSpec != default(Span<byte>))
			{
				FormatSpec_0 = formatSpec[0];
				FormatSpec_1 = formatSpec[1];
				FormatSpec_2 = formatSpec[2];
				FormatSpec_3 = formatSpec[3];
				FormatSpec_4 = formatSpec[4];
				FormatSpec_5 = formatSpec[5];
				FormatSpec_6 = formatSpec[6];
				FormatSpec_7 = formatSpec[7];
				FormatSpec_8 = formatSpec[8];
				FormatSpec_9 = formatSpec[9];
				FormatSpec_10 = formatSpec[10];
				FormatSpec_11 = formatSpec[11];
				FormatSpec_12 = formatSpec[12];
				FormatSpec_13 = formatSpec[13];
				FormatSpec_14 = formatSpec[14];
				FormatSpec_15 = formatSpec[15];
			}
			Locator = (void*)Marshal.GetFunctionPointerForDelegate(locator);
			LinkedMin = linkedMin;
			LinkedMax = linkedMax;
			PickerLevel = pickerLevel;
			PickerTimeMin = pickerTimeMin;
			PickerTimeMax = pickerTimeMax;
			TransformForward = (void*)Marshal.GetFunctionPointerForDelegate(transformForward);
			TransformInverse = (void*)Marshal.GetFunctionPointerForDelegate(transformInverse);
			TransformData = transformData;
			PixelMin = pixelMin;
			PixelMax = pixelMax;
			ScaleMin = scaleMin;
			ScaleMax = scaleMax;
			ScaleToPixel = scaleToPixel;
			Datum1 = datum1;
			Datum2 = datum2;
			HoverRect = hoverRect;
			LabelOffset = labelOffset;
			ColorMaj = colorMaj;
			ColorMin = colorMin;
			ColorTick = colorTick;
			ColorTxt = colorTxt;
			ColorBg = colorBg;
			ColorHov = colorHov;
			ColorAct = colorAct;
			ColorHiLi = colorHiLi;
			Enabled = enabled ? (byte)1 : (byte)0;
			Vertical = vertical ? (byte)1 : (byte)0;
			FitThisFrame = fitThisFrame ? (byte)1 : (byte)0;
			HasRange = hasRange ? (byte)1 : (byte)0;
			HasFormatSpec = hasFormatSpec ? (byte)1 : (byte)0;
			ShowDefaultTicks = showDefaultTicks ? (byte)1 : (byte)0;
			Hovered = hovered ? (byte)1 : (byte)0;
			Held = held ? (byte)1 : (byte)0;
		}


	}

	/// <summary>
	/// To be documented.
	/// </summary>
	#if NET5_0_OR_GREATER
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	#endif
	public unsafe struct ImPlotAxisPtr : IEquatable<ImPlotAxisPtr>
	{
		public ImPlotAxisPtr(ImPlotAxis* handle) { Handle = handle; }

		public ImPlotAxis* Handle;

		public bool IsNull => Handle == null;

		public static ImPlotAxisPtr Null => new ImPlotAxisPtr(null);

		public ImPlotAxis this[int index] { get => Handle[index]; set => Handle[index] = value; }

		public static implicit operator ImPlotAxisPtr(ImPlotAxis* handle) => new ImPlotAxisPtr(handle);

		public static implicit operator ImPlotAxis*(ImPlotAxisPtr handle) => handle.Handle;

		public static bool operator ==(ImPlotAxisPtr left, ImPlotAxisPtr right) => left.Handle == right.Handle;

		public static bool operator !=(ImPlotAxisPtr left, ImPlotAxisPtr right) => left.Handle != right.Handle;

		public static bool operator ==(ImPlotAxisPtr left, ImPlotAxis* right) => left.Handle == right;

		public static bool operator !=(ImPlotAxisPtr left, ImPlotAxis* right) => left.Handle != right;

		public bool Equals(ImPlotAxisPtr other) => Handle == other.Handle;

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is ImPlotAxisPtr handle && Equals(handle);

		/// <inheritdoc/>
		public override int GetHashCode() => ((nuint)Handle).GetHashCode();

		#if NET5_0_OR_GREATER
		private string DebuggerDisplay => string.Format("ImPlotAxisPtr [0x{0}]", ((nuint)Handle).ToString("X"));
		#endif
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ID => ref Unsafe.AsRef<uint>(&Handle->ID);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotAxisFlags Flags => ref Unsafe.AsRef<ImPlotAxisFlags>(&Handle->Flags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotAxisFlags PreviousFlags => ref Unsafe.AsRef<ImPlotAxisFlags>(&Handle->PreviousFlags);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotRange Range => ref Unsafe.AsRef<ImPlotRange>(&Handle->Range);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotCond RangeCond => ref Unsafe.AsRef<ImPlotCond>(&Handle->RangeCond);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotScale Scale => ref Unsafe.AsRef<ImPlotScale>(&Handle->Scale);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotRange FitExtents => ref Unsafe.AsRef<ImPlotRange>(&Handle->FitExtents);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotAxisPtr OrthoAxis => ref Unsafe.AsRef<ImPlotAxisPtr>(&Handle->OrthoAxis);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotRange ConstraintRange => ref Unsafe.AsRef<ImPlotRange>(&Handle->ConstraintRange);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotRange ConstraintZoom => ref Unsafe.AsRef<ImPlotRange>(&Handle->ConstraintZoom);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotTicker Ticker => ref Unsafe.AsRef<ImPlotTicker>(&Handle->Ticker);
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* Formatter { get => Handle->Formatter; set => Handle->Formatter = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* FormatterData { get => Handle->FormatterData; set => Handle->FormatterData = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe Span<byte> FormatSpec
		
		{
			get
			{
				return new Span<byte>(&Handle->FormatSpec_0, 16);
			}
		}
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* Locator { get => Handle->Locator; set => Handle->Locator = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public double* LinkedMin { get => Handle->LinkedMin; set => Handle->LinkedMin = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public double* LinkedMax { get => Handle->LinkedMax; set => Handle->LinkedMax = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int PickerLevel => ref Unsafe.AsRef<int>(&Handle->PickerLevel);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotTime PickerTimeMin => ref Unsafe.AsRef<ImPlotTime>(&Handle->PickerTimeMin);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImPlotTime PickerTimeMax => ref Unsafe.AsRef<ImPlotTime>(&Handle->PickerTimeMax);
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* TransformForward { get => Handle->TransformForward; set => Handle->TransformForward = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* TransformInverse { get => Handle->TransformInverse; set => Handle->TransformInverse = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public void* TransformData { get => Handle->TransformData; set => Handle->TransformData = value; }
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float PixelMin => ref Unsafe.AsRef<float>(&Handle->PixelMin);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float PixelMax => ref Unsafe.AsRef<float>(&Handle->PixelMax);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref double ScaleMin => ref Unsafe.AsRef<double>(&Handle->ScaleMin);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref double ScaleMax => ref Unsafe.AsRef<double>(&Handle->ScaleMax);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref double ScaleToPixel => ref Unsafe.AsRef<double>(&Handle->ScaleToPixel);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float Datum1 => ref Unsafe.AsRef<float>(&Handle->Datum1);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref float Datum2 => ref Unsafe.AsRef<float>(&Handle->Datum2);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref ImRect HoverRect => ref Unsafe.AsRef<ImRect>(&Handle->HoverRect);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref int LabelOffset => ref Unsafe.AsRef<int>(&Handle->LabelOffset);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorMaj => ref Unsafe.AsRef<uint>(&Handle->ColorMaj);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorMin => ref Unsafe.AsRef<uint>(&Handle->ColorMin);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorTick => ref Unsafe.AsRef<uint>(&Handle->ColorTick);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorTxt => ref Unsafe.AsRef<uint>(&Handle->ColorTxt);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorBg => ref Unsafe.AsRef<uint>(&Handle->ColorBg);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorHov => ref Unsafe.AsRef<uint>(&Handle->ColorHov);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorAct => ref Unsafe.AsRef<uint>(&Handle->ColorAct);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref uint ColorHiLi => ref Unsafe.AsRef<uint>(&Handle->ColorHiLi);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Enabled => ref Unsafe.AsRef<bool>(&Handle->Enabled);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Vertical => ref Unsafe.AsRef<bool>(&Handle->Vertical);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool FitThisFrame => ref Unsafe.AsRef<bool>(&Handle->FitThisFrame);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool HasRange => ref Unsafe.AsRef<bool>(&Handle->HasRange);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool HasFormatSpec => ref Unsafe.AsRef<bool>(&Handle->HasFormatSpec);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool ShowDefaultTicks => ref Unsafe.AsRef<bool>(&Handle->ShowDefaultTicks);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Hovered => ref Unsafe.AsRef<bool>(&Handle->Hovered);
		/// <summary>
		/// To be documented.
		/// </summary>
		public ref bool Held => ref Unsafe.AsRef<bool>(&Handle->Held);
	}

}