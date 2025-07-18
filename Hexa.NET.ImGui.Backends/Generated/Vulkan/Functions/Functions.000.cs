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

namespace Hexa.NET.ImGui.Backends.Vulkan
{
	public unsafe partial class ImGuiImplVulkan
	{
		/// <summary>
		/// Follow "Getting Started" link and check examples/ folder to learn about using backends!<br/>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte InitNative(ImGuiImplVulkanInitInfo* info)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<ImGuiImplVulkanInitInfo*, byte>)funcTable[44])(info);
			#else
			return (byte)((delegate* unmanaged[Cdecl]<nint, byte>)funcTable[44])((nint)info);
			#endif
		}

		/// <summary>
		/// Follow "Getting Started" link and check examples/ folder to learn about using backends!<br/>
		/// </summary>
		public static bool Init(ImGuiImplVulkanInitInfoPtr info)
		{
			byte ret = InitNative(info);
			return ret != 0;
		}

		/// <summary>
		/// Follow "Getting Started" link and check examples/ folder to learn about using backends!<br/>
		/// </summary>
		public static bool Init(ref ImGuiImplVulkanInitInfo info)
		{
			fixed (ImGuiImplVulkanInitInfo* pinfo = &info)
			{
				byte ret = InitNative((ImGuiImplVulkanInitInfo*)pinfo);
				return ret != 0;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ShutdownNative()
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<void>)funcTable[45])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[45])();
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void Shutdown()
		{
			ShutdownNative();
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void NewFrameNative()
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<void>)funcTable[46])();
			#else
			((delegate* unmanaged[Cdecl]<void>)funcTable[46])();
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void NewFrame()
		{
			NewFrameNative();
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RenderDrawDataNative(ImDrawData* drawData, VkCommandBuffer commandBuffer, VkPipeline pipeline)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<ImDrawData*, VkCommandBuffer, VkPipeline, void>)funcTable[47])(drawData, commandBuffer, pipeline);
			#else
			((delegate* unmanaged[Cdecl]<nint, VkCommandBuffer, VkPipeline, void>)funcTable[47])((nint)drawData, commandBuffer, pipeline);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RenderDrawData(ImDrawDataPtr drawData, VkCommandBuffer commandBuffer, VkPipeline pipeline)
		{
			RenderDrawDataNative(drawData, commandBuffer, pipeline);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RenderDrawData(ref ImDrawData drawData, VkCommandBuffer commandBuffer, VkPipeline pipeline)
		{
			fixed (ImDrawData* pdrawData = &drawData)
			{
				RenderDrawDataNative((ImDrawData*)pdrawData, commandBuffer, pipeline);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void SetMinImageCountNative(uint minImageCount)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<uint, void>)funcTable[48])(minImageCount);
			#else
			((delegate* unmanaged[Cdecl]<uint, void>)funcTable[48])(minImageCount);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void SetMinImageCount(uint minImageCount)
		{
			SetMinImageCountNative(minImageCount);
		}

		/// <summary>
		/// (Advanced) Use e.g. if you need to precisely control the timing of texture updates (e.g. for staged rendering), by setting ImDrawData::Textures = NULL to handle this manually.<br/>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void UpdateTextureNative(ImTextureData* tex)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<ImTextureData*, void>)funcTable[49])(tex);
			#else
			((delegate* unmanaged[Cdecl]<nint, void>)funcTable[49])((nint)tex);
			#endif
		}

		/// <summary>
		/// (Advanced) Use e.g. if you need to precisely control the timing of texture updates (e.g. for staged rendering), by setting ImDrawData::Textures = NULL to handle this manually.<br/>
		/// </summary>
		public static void UpdateTexture(ImTextureDataPtr tex)
		{
			UpdateTextureNative(tex);
		}

		/// <summary>
		/// (Advanced) Use e.g. if you need to precisely control the timing of texture updates (e.g. for staged rendering), by setting ImDrawData::Textures = NULL to handle this manually.<br/>
		/// </summary>
		public static void UpdateTexture(ref ImTextureData tex)
		{
			fixed (ImTextureData* ptex = &tex)
			{
				UpdateTextureNative((ImTextureData*)ptex);
			}
		}

		/// <summary>
		/// Register a texture (VkDescriptorSet == ImTextureID)<br/>
		/// FIXME: This is experimental in the sense that we are unsure how to best design/tackle this problem<br/>
		/// Please post to https://github.com/ocornut/imgui/pull/914 if you have suggestions.<br/>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static VkDescriptorSet AddTextureNative(VkSampler sampler, VkImageView imageView, VkImageLayout imageLayout)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<VkSampler, VkImageView, VkImageLayout, VkDescriptorSet>)funcTable[50])(sampler, imageView, imageLayout);
			#else
			return (VkDescriptorSet)((delegate* unmanaged[Cdecl]<VkSampler, VkImageView, VkImageLayout, VkDescriptorSet>)funcTable[50])(sampler, imageView, imageLayout);
			#endif
		}

		/// <summary>
		/// Register a texture (VkDescriptorSet == ImTextureID)<br/>
		/// FIXME: This is experimental in the sense that we are unsure how to best design/tackle this problem<br/>
		/// Please post to https://github.com/ocornut/imgui/pull/914 if you have suggestions.<br/>
		/// </summary>
		public static VkDescriptorSet AddTexture(VkSampler sampler, VkImageView imageView, VkImageLayout imageLayout)
		{
			VkDescriptorSet ret = AddTextureNative(sampler, imageView, imageLayout);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RemoveTextureNative(VkDescriptorSet descriptorSet)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<VkDescriptorSet, void>)funcTable[51])(descriptorSet);
			#else
			((delegate* unmanaged[Cdecl]<VkDescriptorSet, void>)funcTable[51])(descriptorSet);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void RemoveTexture(VkDescriptorSet descriptorSet)
		{
			RemoveTextureNative(descriptorSet);
		}

		/// <summary>
		/// Optional: load Vulkan functions with a custom function loader<br/>
		/// This is only useful with IMGUI_IMPL_VULKAN_NO_PROTOTYPES / VK_NO_PROTOTYPES<br/>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static byte LoadFunctionsNative(uint apiVersion, delegate*<uint, delegate*<byte*, void*, PFNVkVoidFunction>, void*, PFNVkVoidFunction> loaderFunc, void* userData)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<uint, delegate*<uint, delegate*<byte*, void*, PFNVkVoidFunction>, void*, PFNVkVoidFunction>, void*, byte>)funcTable[52])(apiVersion, loaderFunc, userData);
			#else
			return (byte)((delegate* unmanaged[Cdecl]<uint, nint, nint, byte>)funcTable[52])(apiVersion, (nint)loaderFunc, (nint)userData);
			#endif
		}

		/// <summary>
		/// Optional: load Vulkan functions with a custom function loader<br/>
		/// This is only useful with IMGUI_IMPL_VULKAN_NO_PROTOTYPES / VK_NO_PROTOTYPES<br/>
		/// </summary>
		public static bool LoadFunctions(uint apiVersion, delegate*<uint, delegate*<byte*, void*, PFNVkVoidFunction>, void*, PFNVkVoidFunction> loaderFunc, void* userData)
		{
			byte ret = LoadFunctionsNative(apiVersion, loaderFunc, userData);
			return ret != 0;
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void HCreateOrResizeWindowNative(VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ImGuiImplVulkanHWindow* wd, uint queueFamily, VkAllocationCallbacks* allocator, int w, int h, uint minImageCount)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<VkInstance, VkPhysicalDevice, VkDevice, ImGuiImplVulkanHWindow*, uint, VkAllocationCallbacks*, int, int, uint, void>)funcTable[53])(instance, physicalDevice, device, wd, queueFamily, allocator, w, h, minImageCount);
			#else
			((delegate* unmanaged[Cdecl]<VkInstance, VkPhysicalDevice, VkDevice, nint, uint, nint, int, int, uint, void>)funcTable[53])(instance, physicalDevice, device, (nint)wd, queueFamily, (nint)allocator, w, h, minImageCount);
			#endif
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void HCreateOrResizeWindow(VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ImGuiImplVulkanHWindowPtr wd, uint queueFamily, VkAllocationCallbacks* allocator, int w, int h, uint minImageCount)
		{
			HCreateOrResizeWindowNative(instance, physicalDevice, device, wd, queueFamily, allocator, w, h, minImageCount);
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void HCreateOrResizeWindow(VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ref ImGuiImplVulkanHWindow wd, uint queueFamily, VkAllocationCallbacks* allocator, int w, int h, uint minImageCount)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				HCreateOrResizeWindowNative(instance, physicalDevice, device, (ImGuiImplVulkanHWindow*)pwd, queueFamily, allocator, w, h, minImageCount);
			}
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void HCreateOrResizeWindow(VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ImGuiImplVulkanHWindowPtr wd, uint queueFamily, ref VkAllocationCallbacks allocator, int w, int h, uint minImageCount)
		{
			fixed (VkAllocationCallbacks* pallocator = &allocator)
			{
				HCreateOrResizeWindowNative(instance, physicalDevice, device, wd, queueFamily, (VkAllocationCallbacks*)pallocator, w, h, minImageCount);
			}
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void HCreateOrResizeWindow(VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ref ImGuiImplVulkanHWindow wd, uint queueFamily, ref VkAllocationCallbacks allocator, int w, int h, uint minImageCount)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				fixed (VkAllocationCallbacks* pallocator = &allocator)
				{
					HCreateOrResizeWindowNative(instance, physicalDevice, device, (ImGuiImplVulkanHWindow*)pwd, queueFamily, (VkAllocationCallbacks*)pallocator, w, h, minImageCount);
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void HDestroyWindowNative(VkInstance instance, VkDevice device, ImGuiImplVulkanHWindow* wd, VkAllocationCallbacks* allocator)
		{
			#if NET5_0_OR_GREATER
			((delegate* unmanaged[Cdecl]<VkInstance, VkDevice, ImGuiImplVulkanHWindow*, VkAllocationCallbacks*, void>)funcTable[54])(instance, device, wd, allocator);
			#else
			((delegate* unmanaged[Cdecl]<VkInstance, VkDevice, nint, nint, void>)funcTable[54])(instance, device, (nint)wd, (nint)allocator);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void HDestroyWindow(VkInstance instance, VkDevice device, ImGuiImplVulkanHWindowPtr wd, VkAllocationCallbacks* allocator)
		{
			HDestroyWindowNative(instance, device, wd, allocator);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void HDestroyWindow(VkInstance instance, VkDevice device, ref ImGuiImplVulkanHWindow wd, VkAllocationCallbacks* allocator)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				HDestroyWindowNative(instance, device, (ImGuiImplVulkanHWindow*)pwd, allocator);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void HDestroyWindow(VkInstance instance, VkDevice device, ImGuiImplVulkanHWindowPtr wd, ref VkAllocationCallbacks allocator)
		{
			fixed (VkAllocationCallbacks* pallocator = &allocator)
			{
				HDestroyWindowNative(instance, device, wd, (VkAllocationCallbacks*)pallocator);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void HDestroyWindow(VkInstance instance, VkDevice device, ref ImGuiImplVulkanHWindow wd, ref VkAllocationCallbacks allocator)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				fixed (VkAllocationCallbacks* pallocator = &allocator)
				{
					HDestroyWindowNative(instance, device, (ImGuiImplVulkanHWindow*)pwd, (VkAllocationCallbacks*)pallocator);
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static VkSurfaceFormatKHR HSelectSurfaceFormatNative(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint* requestFormats, int requestFormatsCount, uint requestColorSpace)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<VkPhysicalDevice, VkSurfaceKHR, uint*, int, uint, VkSurfaceFormatKHR>)funcTable[55])(physicalDevice, surface, requestFormats, requestFormatsCount, requestColorSpace);
			#else
			return (VkSurfaceFormatKHR)((delegate* unmanaged[Cdecl]<VkPhysicalDevice, VkSurfaceKHR, nint, int, uint, VkSurfaceFormatKHR>)funcTable[55])(physicalDevice, surface, (nint)requestFormats, requestFormatsCount, requestColorSpace);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkSurfaceFormatKHR HSelectSurfaceFormat(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint* requestFormats, int requestFormatsCount, uint requestColorSpace)
		{
			VkSurfaceFormatKHR ret = HSelectSurfaceFormatNative(physicalDevice, surface, requestFormats, requestFormatsCount, requestColorSpace);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkSurfaceFormatKHR HSelectSurfaceFormat(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint requestFormats, int requestFormatsCount, uint requestColorSpace)
		{
			fixed (uint* prequestFormats = &requestFormats)
			{
				VkSurfaceFormatKHR ret = HSelectSurfaceFormatNative(physicalDevice, surface, (uint*)prequestFormats, requestFormatsCount, requestColorSpace);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static VkPresentModeKHR HSelectPresentModeNative(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* requestModes, int requestModesCount)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<VkPhysicalDevice, VkSurfaceKHR, VkPresentModeKHR*, int, VkPresentModeKHR>)funcTable[56])(physicalDevice, surface, requestModes, requestModesCount);
			#else
			return (VkPresentModeKHR)((delegate* unmanaged[Cdecl]<VkPhysicalDevice, VkSurfaceKHR, nint, int, VkPresentModeKHR>)funcTable[56])(physicalDevice, surface, (nint)requestModes, requestModesCount);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkPresentModeKHR HSelectPresentMode(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* requestModes, int requestModesCount)
		{
			VkPresentModeKHR ret = HSelectPresentModeNative(physicalDevice, surface, requestModes, requestModesCount);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkPresentModeKHR HSelectPresentMode(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref VkPresentModeKHR requestModes, int requestModesCount)
		{
			fixed (VkPresentModeKHR* prequestModes = &requestModes)
			{
				VkPresentModeKHR ret = HSelectPresentModeNative(physicalDevice, surface, (VkPresentModeKHR*)prequestModes, requestModesCount);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static VkPhysicalDevice HSelectPhysicalDeviceNative(VkInstance instance)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<VkInstance, VkPhysicalDevice>)funcTable[57])(instance);
			#else
			return (VkPhysicalDevice)((delegate* unmanaged[Cdecl]<VkInstance, VkPhysicalDevice>)funcTable[57])(instance);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkPhysicalDevice HSelectPhysicalDevice(VkInstance instance)
		{
			VkPhysicalDevice ret = HSelectPhysicalDeviceNative(instance);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint HSelectQueueFamilyIndexNative(VkPhysicalDevice physicalDevice)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<VkPhysicalDevice, uint>)funcTable[58])(physicalDevice);
			#else
			return (uint)((delegate* unmanaged[Cdecl]<VkPhysicalDevice, uint>)funcTable[58])(physicalDevice);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static uint HSelectQueueFamilyIndex(VkPhysicalDevice physicalDevice)
		{
			uint ret = HSelectQueueFamilyIndexNative(physicalDevice);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int HGetMinImageCountFromPresentModeNative(VkPresentModeKHR presentMode)
		{
			#if NET5_0_OR_GREATER
			return ((delegate* unmanaged[Cdecl]<VkPresentModeKHR, int>)funcTable[59])(presentMode);
			#else
			return (int)((delegate* unmanaged[Cdecl]<VkPresentModeKHR, int>)funcTable[59])(presentMode);
			#endif
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static int HGetMinImageCountFromPresentMode(VkPresentModeKHR presentMode)
		{
			int ret = HGetMinImageCountFromPresentModeNative(presentMode);
			return ret;
		}

	}
}
