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
	public enum ImGuiInputFlagsPrivate : int
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatRateDefault = unchecked(2),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatRateNavMove = unchecked(4),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatRateNavTweak = unchecked(8),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatUntilRelease = unchecked(16),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatUntilKeyModsChange = unchecked(32),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatUntilKeyModsChangeFromNone = unchecked(64),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatUntilOtherKeyPress = unchecked(128),

		/// <summary>
		/// To be documented.
		/// </summary>
		LockThisFrame = unchecked(1048576),

		/// <summary>
		/// To be documented.
		/// </summary>
		LockUntilRelease = unchecked(2097152),

		/// <summary>
		/// To be documented.
		/// </summary>
		CondHovered = unchecked(4194304),

		/// <summary>
		/// To be documented.
		/// </summary>
		CondActive = unchecked(8388608),

		/// <summary>
		/// To be documented.
		/// </summary>
		CondDefault = unchecked(12582912),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatRateMask = unchecked(14),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatUntilMask = unchecked(240),

		/// <summary>
		/// To be documented.
		/// </summary>
		RepeatMask = unchecked(255),

		/// <summary>
		/// To be documented.
		/// </summary>
		CondMask = unchecked(12582912),

		/// <summary>
		/// To be documented.
		/// </summary>
		RouteTypeMask = unchecked(15360),

		/// <summary>
		/// To be documented.
		/// </summary>
		RouteOptionsMask = unchecked(245760),

		/// <summary>
		/// To be documented.
		/// </summary>
		SupportedByIsKeyPressed = RepeatMask,

		/// <summary>
		/// To be documented.
		/// </summary>
		SupportedByIsMouseClicked = unchecked((int)ImGuiInputFlags.Repeat),

		/// <summary>
		/// To be documented.
		/// </summary>
		SupportedByShortcut = unchecked(261375),

		/// <summary>
		/// To be documented.
		/// </summary>
		SupportedBySetNextItemShortcut = unchecked(523519),

		/// <summary>
		/// To be documented.
		/// </summary>
		SupportedBySetKeyOwner = unchecked(3145728),

		/// <summary>
		/// To be documented.
		/// </summary>
		SupportedBySetItemKeyOwner = unchecked(15728640),
	}
}
