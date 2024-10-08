// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using HexaGen.Runtime;
using System.Numerics;

namespace Hexa.NET.ImGui
{
	/// <summary>
	/// To be documented.
	/// </summary>
	[Flags]
	public enum ImGuiActivateFlags : int
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		None = unchecked(0),

		/// <summary>
		/// Favor activation that requires keyboard text input (e.g. for SliderDrag). Default for Enter key.<br/>
		/// </summary>
		PreferInput = unchecked(1),

		/// <summary>
		/// Favor activation for tweaking with arrows or gamepad (e.g. for SliderDrag). Default for Space key and if keyboard is not used.<br/>
		/// </summary>
		PreferTweak = unchecked(2),

		/// <summary>
		/// Request widget to preserve state if it can (e.g. InputText will try to preserve cursorselection)<br/>
		/// </summary>
		TryToPreserveState = unchecked(4),

		/// <summary>
		/// Activation requested by a tabbing request<br/>
		/// </summary>
		FromTabbing = unchecked(8),

		/// <summary>
		/// Activation requested by an item shortcut via SetNextItemShortcut() function.<br/>
		/// </summary>
		FromShortcut = unchecked(16),
	}
}
