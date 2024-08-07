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
	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int Formatter([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "buff")] [NativeName(NativeNameType.Type, "char*")] byte* buff, [NativeName(NativeNameType.Param, "size")] [NativeName(NativeNameType.Type, "int")] int size, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int Formatter([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "buff")] [NativeName(NativeNameType.Type, "char*")] nint buff, [NativeName(NativeNameType.Param, "size")] [NativeName(NativeNameType.Type, "int")] int size, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void Locator([NativeName(NativeNameType.Param, "ticker")] [NativeName(NativeNameType.Type, "ImPlotTicker*")] ImPlotTicker* ticker, [NativeName(NativeNameType.Param, "range")] [NativeName(NativeNameType.Type, "const ImPlotRange")] ImPlotRange range, [NativeName(NativeNameType.Param, "pixels")] [NativeName(NativeNameType.Type, "float")] float pixels, [NativeName(NativeNameType.Param, "vertical")] [NativeName(NativeNameType.Type, "bool")] byte vertical, [NativeName(NativeNameType.Param, "formatter")] [NativeName(NativeNameType.Type, "ImPlotFormatter")] delegate*<double, byte*, int, void*, int> formatter, [NativeName(NativeNameType.Param, "formatter_data")] [NativeName(NativeNameType.Type, "void*")] void* formatterData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void Locator([NativeName(NativeNameType.Param, "ticker")] [NativeName(NativeNameType.Type, "ImPlotTicker*")] nint ticker, [NativeName(NativeNameType.Param, "range")] [NativeName(NativeNameType.Type, "const ImPlotRange")] ImPlotRange range, [NativeName(NativeNameType.Param, "pixels")] [NativeName(NativeNameType.Type, "float")] float pixels, [NativeName(NativeNameType.Param, "vertical")] [NativeName(NativeNameType.Type, "bool")] byte vertical, [NativeName(NativeNameType.Param, "formatter")] [NativeName(NativeNameType.Type, "ImPlotFormatter")] nint formatter, [NativeName(NativeNameType.Param, "formatter_data")] [NativeName(NativeNameType.Type, "void*")] nint formatterData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate double TransformForward([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate double TransformForward([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate double TransformInverse([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate double TransformInverse([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int UserFormatter([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "buff")] [NativeName(NativeNameType.Type, "char*")] byte* buff, [NativeName(NativeNameType.Param, "size")] [NativeName(NativeNameType.Type, "int")] int size, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int UserFormatter([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "buff")] [NativeName(NativeNameType.Type, "char*")] nint buff, [NativeName(NativeNameType.Param, "size")] [NativeName(NativeNameType.Type, "int")] int size, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int ImPlotFormatter([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "buff")] [NativeName(NativeNameType.Type, "char*")] byte* buff, [NativeName(NativeNameType.Param, "size")] [NativeName(NativeNameType.Type, "int")] int size, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate int ImPlotFormatter([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "buff")] [NativeName(NativeNameType.Type, "char*")] nint buff, [NativeName(NativeNameType.Param, "size")] [NativeName(NativeNameType.Type, "int")] int size, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void ImPlotLocator([NativeName(NativeNameType.Param, "ticker")] [NativeName(NativeNameType.Type, "ImPlotTicker*")] ImPlotTicker* ticker, [NativeName(NativeNameType.Param, "range")] [NativeName(NativeNameType.Type, "const ImPlotRange")] ImPlotRange range, [NativeName(NativeNameType.Param, "pixels")] [NativeName(NativeNameType.Type, "float")] float pixels, [NativeName(NativeNameType.Param, "vertical")] [NativeName(NativeNameType.Type, "bool")] byte vertical, [NativeName(NativeNameType.Param, "formatter")] [NativeName(NativeNameType.Type, "ImPlotFormatter")] delegate*<double, byte*, int, void*, int> formatter, [NativeName(NativeNameType.Param, "formatter_data")] [NativeName(NativeNameType.Type, "void*")] void* formatterData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void ImPlotLocator([NativeName(NativeNameType.Param, "ticker")] [NativeName(NativeNameType.Type, "ImPlotTicker*")] nint ticker, [NativeName(NativeNameType.Param, "range")] [NativeName(NativeNameType.Type, "const ImPlotRange")] ImPlotRange range, [NativeName(NativeNameType.Param, "pixels")] [NativeName(NativeNameType.Type, "float")] float pixels, [NativeName(NativeNameType.Param, "vertical")] [NativeName(NativeNameType.Type, "bool")] byte vertical, [NativeName(NativeNameType.Param, "formatter")] [NativeName(NativeNameType.Type, "ImPlotFormatter")] nint formatter, [NativeName(NativeNameType.Param, "formatter_data")] [NativeName(NativeNameType.Type, "void*")] nint formatterData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate double ImPlotTransform([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate double ImPlotTransform([NativeName(NativeNameType.Param, "value")] [NativeName(NativeNameType.Type, "double")] double value, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate ImPlotPoint ImPlotGetter([NativeName(NativeNameType.Param, "idx")] [NativeName(NativeNameType.Type, "int")] int idx, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] void* userData);

	#else
	/// <summary>
	/// To be documented.
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate ImPlotPoint ImPlotGetter([NativeName(NativeNameType.Param, "idx")] [NativeName(NativeNameType.Type, "int")] int idx, [NativeName(NativeNameType.Param, "user_data")] [NativeName(NativeNameType.Type, "void*")] nint userData);

	#endif

	#if NET5_0_OR_GREATER
	/// <summary>
	/// ImPlotPoint getters manually wrapped use this<br/>
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate void* ImPlotPointGetter([NativeName(NativeNameType.Param, "data")] [NativeName(NativeNameType.Type, "void*")] void* data, [NativeName(NativeNameType.Param, "idx")] [NativeName(NativeNameType.Type, "int")] int idx, [NativeName(NativeNameType.Param, "point")] [NativeName(NativeNameType.Type, "ImPlotPoint*")] ImPlotPoint* point);

	#else
	/// <summary>
	/// ImPlotPoint getters manually wrapped use this<br/>
	/// </summary>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public unsafe delegate nint ImPlotPointGetter([NativeName(NativeNameType.Param, "data")] [NativeName(NativeNameType.Type, "void*")] nint data, [NativeName(NativeNameType.Param, "idx")] [NativeName(NativeNameType.Type, "int")] int idx, [NativeName(NativeNameType.Param, "point")] [NativeName(NativeNameType.Type, "ImPlotPoint*")] nint point);

	#endif

}
