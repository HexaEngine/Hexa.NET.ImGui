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
	public partial struct ImSpanImGuiTableColumn
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiTableColumn* Data;

		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiTableColumn* DataEnd;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImSpanImGuiTableColumn(ImGuiTableColumn* data = default, ImGuiTableColumn* dataEnd = default)
		{
			Data = data;
			DataEnd = dataEnd;
		}


	}

}
