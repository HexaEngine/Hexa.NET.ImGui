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

namespace Hexa.NET.ImGui.Backends.SDL2
{
	/// <summary>
	/// Full struct layout for ImGui_ImplSDLRenderer2_RenderState<br/>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct ImGuiImplSDLRenderer2RenderState
	{
		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe SDLRenderer* Renderer;


		/// <summary>
		/// To be documented.
		/// </summary>
		public unsafe ImGuiImplSDLRenderer2RenderState(SDLRendererPtr renderer = default)
		{
			Renderer = renderer;
		}


	}

}
