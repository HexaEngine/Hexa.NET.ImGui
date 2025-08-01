// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HexaGen.Runtime;
using System.Numerics;
using Hexa.NET.ImGui;

namespace Hexa.NET.ImGui.Backends
{
	public unsafe partial class ImGuiImpl
	{
		internal static FunctionTable funcTable;

		/// <summary>
		/// Initializes the function table, automatically called. Do not call manually, only after <see cref="FreeApi"/>.
		/// </summary>
		public static void InitApi(INativeContext context)
		{
			funcTable = new FunctionTable(context, 83);
			funcTable.Load(0, "igSetCurrentContext");
			funcTable.Load(1, "igGetCurrentContext");
			funcTable.Load(2, "CImGui_ImplOpenGL3_Init");
			funcTable.Load(3, "CImGui_ImplOpenGL3_Shutdown");
			funcTable.Load(4, "CImGui_ImplOpenGL3_NewFrame");
			funcTable.Load(5, "CImGui_ImplOpenGL3_RenderDrawData");
			funcTable.Load(6, "CImGui_ImplOpenGL3_UpdateTexture");
			funcTable.Load(7, "CImGui_ImplOpenGL3_CreateDeviceObjects");
			funcTable.Load(8, "CImGui_ImplOpenGL3_DestroyDeviceObjects");
			funcTable.Load(9, "CImGui_ImplOpenGL2_Init");
			funcTable.Load(10, "CImGui_ImplOpenGL2_Shutdown");
			funcTable.Load(11, "CImGui_ImplOpenGL2_NewFrame");
			funcTable.Load(12, "CImGui_ImplOpenGL2_RenderDrawData");
			funcTable.Load(13, "CImGui_ImplOpenGL2_UpdateTexture");
			funcTable.Load(14, "CImGui_ImplOpenGL2_CreateDeviceObjects");
			funcTable.Load(15, "CImGui_ImplOpenGL2_DestroyDeviceObjects");
			funcTable.Load(16, "CImGui_ImplDX9_Init");
			funcTable.Load(17, "CImGui_ImplDX9_Shutdown");
			funcTable.Load(18, "CImGui_ImplDX9_NewFrame");
			funcTable.Load(19, "CImGui_ImplDX9_RenderDrawData");
			funcTable.Load(20, "CImGui_ImplDX9_UpdateTexture");
			funcTable.Load(21, "CImGui_ImplDX9_CreateDeviceObjects");
			funcTable.Load(22, "CImGui_ImplDX9_InvalidateDeviceObjects");
			funcTable.Load(23, "CImGui_ImplDX10_Init");
			funcTable.Load(24, "CImGui_ImplDX10_Shutdown");
			funcTable.Load(25, "CImGui_ImplDX10_NewFrame");
			funcTable.Load(26, "CImGui_ImplDX10_RenderDrawData");
			funcTable.Load(27, "CImGui_ImplDX10_UpdateTexture");
			funcTable.Load(28, "CImGui_ImplDX10_CreateDeviceObjects");
			funcTable.Load(29, "CImGui_ImplDX10_InvalidateDeviceObjects");
			funcTable.Load(30, "CImGui_ImplDX11_Init");
			funcTable.Load(31, "CImGui_ImplDX11_Shutdown");
			funcTable.Load(32, "CImGui_ImplDX11_NewFrame");
			funcTable.Load(33, "CImGui_ImplDX11_RenderDrawData");
			funcTable.Load(34, "CImGui_ImplDX11_UpdateTexture");
			funcTable.Load(35, "CImGui_ImplDX11_InvalidateDeviceObjects");
			funcTable.Load(36, "CImGui_ImplDX11_CreateDeviceObjects");
			funcTable.Load(37, "CImGui_ImplDX12_Init");
			funcTable.Load(38, "CImGui_ImplDX12_Shutdown");
			funcTable.Load(39, "CImGui_ImplDX12_NewFrame");
			funcTable.Load(40, "CImGui_ImplDX12_RenderDrawData");
			funcTable.Load(41, "CImGui_ImplDX12_UpdateTexture");
			funcTable.Load(42, "CImGui_ImplDX12_InvalidateDeviceObjects");
			funcTable.Load(43, "CImGui_ImplDX12_CreateDeviceObjects");
			funcTable.Load(44, "CImGui_ImplVulkan_Init");
			funcTable.Load(45, "CImGui_ImplVulkan_Shutdown");
			funcTable.Load(46, "CImGui_ImplVulkan_NewFrame");
			funcTable.Load(47, "CImGui_ImplVulkan_RenderDrawData");
			funcTable.Load(48, "CImGui_ImplVulkan_SetMinImageCount");
			funcTable.Load(49, "CImGui_ImplVulkan_UpdateTexture");
			funcTable.Load(50, "CImGui_ImplVulkan_AddTexture");
			funcTable.Load(51, "CImGui_ImplVulkan_RemoveTexture");
			funcTable.Load(52, "CImGui_ImplVulkan_LoadFunctions");
			funcTable.Load(53, "CImGui_ImplVulkanH_CreateOrResizeWindow");
			funcTable.Load(54, "CImGui_ImplVulkanH_DestroyWindow");
			funcTable.Load(55, "CImGui_ImplVulkanH_SelectSurfaceFormat");
			funcTable.Load(56, "CImGui_ImplVulkanH_SelectPresentMode");
			funcTable.Load(57, "CImGui_ImplVulkanH_SelectPhysicalDevice");
			funcTable.Load(58, "CImGui_ImplVulkanH_SelectQueueFamilyIndex");
			funcTable.Load(59, "CImGui_ImplVulkanH_GetMinImageCountFromPresentMode");
			funcTable.Load(60, "CImGui_ImplWin32_Init");
			funcTable.Load(61, "CImGui_ImplWin32_InitForOpenGL");
			funcTable.Load(62, "CImGui_ImplWin32_Shutdown");
			funcTable.Load(63, "CImGui_ImplWin32_NewFrame");
			funcTable.Load(64, "CImGui_ImplWin32_WndProcHandler");
			funcTable.Load(65, "CImGui_ImplWin32_EnableDpiAwareness");
			funcTable.Load(66, "CImGui_ImplWin32_GetDpiScaleForHwnd");
			funcTable.Load(67, "CImGui_ImplWin32_GetDpiScaleForMonitor");
			funcTable.Load(68, "CImGui_ImplWin32_EnableAlphaCompositing");
			funcTable.Load(69, "CImGui_ImplOSX_Init");
			funcTable.Load(70, "CImGui_ImplOSX_Shutdown");
			funcTable.Load(71, "CImGui_ImplOSX_NewFrame");
			funcTable.Load(72, "CImGui_ImplMetal_Init");
			funcTable.Load(73, "CImGui_ImplMetal_Shutdown");
			funcTable.Load(74, "CImGui_ImplMetal_NewFrame");
			funcTable.Load(75, "CImGui_ImplMetal_RenderDrawData");
			funcTable.Load(76, "CImGui_ImplMetal_UpdateTexture");
			funcTable.Load(77, "CImGui_ImplMetal_CreateDeviceObjects");
			funcTable.Load(78, "CImGui_ImplMetal_DestroyDeviceObjects");
			funcTable.Load(79, "CImGui_ImplAndroid_Init");
			funcTable.Load(80, "CImGui_ImplAndroid_HandleInputEvent");
			funcTable.Load(81, "CImGui_ImplAndroid_Shutdown");
			funcTable.Load(82, "CImGui_ImplAndroid_NewFrame");
		}

		public static void FreeApi()
		{
			funcTable.Free();
		}
	}
}
