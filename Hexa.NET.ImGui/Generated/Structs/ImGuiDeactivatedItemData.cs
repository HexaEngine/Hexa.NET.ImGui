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
	public partial struct ImGuiDeactivatedItemData
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public uint ID;

		/// <summary>
		/// To be documented.
		/// </summary>
		public int ElapseFrame;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte HasBeenEditedBefore;

		/// <summary>
		/// To be documented.
		/// </summary>
		public byte IsAlive;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiDeactivatedItemData(uint id = default, int elapseFrame = default, bool hasBeenEditedBefore = default, bool isAlive = default)
		{
			ID = id;
			ElapseFrame = elapseFrame;
			HasBeenEditedBefore = hasBeenEditedBefore ? (byte)1 : (byte)0;
			IsAlive = isAlive ? (byte)1 : (byte)0;
		}


	}

}
