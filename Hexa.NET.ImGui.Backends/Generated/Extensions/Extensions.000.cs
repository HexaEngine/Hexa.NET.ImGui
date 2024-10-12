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
	public static unsafe partial class Extensions
	{
		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void ImplVulkanHCreateOrResizeWindow(this VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ImGuiImplVulkanHWindowPtr wd, uint queueFamily, VkAllocationCallbacks* allocator, int w, int h, uint minImageCount)
		{
			ImGuiBackends.ImplVulkanHCreateOrResizeWindowNative(instance, physicalDevice, device, wd, queueFamily, allocator, w, h, minImageCount);
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void ImplVulkanHCreateOrResizeWindow(this VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ref ImGuiImplVulkanHWindow wd, uint queueFamily, VkAllocationCallbacks* allocator, int w, int h, uint minImageCount)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				ImGuiBackends.ImplVulkanHCreateOrResizeWindowNative(instance, physicalDevice, device, (ImGuiImplVulkanHWindow*)pwd, queueFamily, allocator, w, h, minImageCount);
			}
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void ImplVulkanHCreateOrResizeWindow(this VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ImGuiImplVulkanHWindowPtr wd, uint queueFamily, ref VkAllocationCallbacks allocator, int w, int h, uint minImageCount)
		{
			fixed (VkAllocationCallbacks* pallocator = &allocator)
			{
				ImGuiBackends.ImplVulkanHCreateOrResizeWindowNative(instance, physicalDevice, device, wd, queueFamily, (VkAllocationCallbacks*)pallocator, w, h, minImageCount);
			}
		}

		/// <summary>
		/// Helpers<br/>
		/// </summary>
		public static void ImplVulkanHCreateOrResizeWindow(this VkInstance instance, VkPhysicalDevice physicalDevice, VkDevice device, ref ImGuiImplVulkanHWindow wd, uint queueFamily, ref VkAllocationCallbacks allocator, int w, int h, uint minImageCount)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				fixed (VkAllocationCallbacks* pallocator = &allocator)
				{
					ImGuiBackends.ImplVulkanHCreateOrResizeWindowNative(instance, physicalDevice, device, (ImGuiImplVulkanHWindow*)pwd, queueFamily, (VkAllocationCallbacks*)pallocator, w, h, minImageCount);
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ImplVulkanHDestroyWindow(this VkInstance instance, VkDevice device, ImGuiImplVulkanHWindowPtr wd, VkAllocationCallbacks* allocator)
		{
			ImGuiBackends.ImplVulkanHDestroyWindowNative(instance, device, wd, allocator);
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ImplVulkanHDestroyWindow(this VkInstance instance, VkDevice device, ref ImGuiImplVulkanHWindow wd, VkAllocationCallbacks* allocator)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				ImGuiBackends.ImplVulkanHDestroyWindowNative(instance, device, (ImGuiImplVulkanHWindow*)pwd, allocator);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ImplVulkanHDestroyWindow(this VkInstance instance, VkDevice device, ImGuiImplVulkanHWindowPtr wd, ref VkAllocationCallbacks allocator)
		{
			fixed (VkAllocationCallbacks* pallocator = &allocator)
			{
				ImGuiBackends.ImplVulkanHDestroyWindowNative(instance, device, wd, (VkAllocationCallbacks*)pallocator);
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ImplVulkanHDestroyWindow(this VkInstance instance, VkDevice device, ref ImGuiImplVulkanHWindow wd, ref VkAllocationCallbacks allocator)
		{
			fixed (ImGuiImplVulkanHWindow* pwd = &wd)
			{
				fixed (VkAllocationCallbacks* pallocator = &allocator)
				{
					ImGuiBackends.ImplVulkanHDestroyWindowNative(instance, device, (ImGuiImplVulkanHWindow*)pwd, (VkAllocationCallbacks*)pallocator);
				}
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkSurfaceFormatKHR ImplVulkanHSelectSurfaceFormat(this VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint* requestFormats, int requestFormatsCount, uint requestColorSpace)
		{
			VkSurfaceFormatKHR ret = ImGuiBackends.ImplVulkanHSelectSurfaceFormatNative(physicalDevice, surface, requestFormats, requestFormatsCount, requestColorSpace);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkSurfaceFormatKHR ImplVulkanHSelectSurfaceFormat(this VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref uint requestFormats, int requestFormatsCount, uint requestColorSpace)
		{
			fixed (uint* prequestFormats = &requestFormats)
			{
				VkSurfaceFormatKHR ret = ImGuiBackends.ImplVulkanHSelectSurfaceFormatNative(physicalDevice, surface, (uint*)prequestFormats, requestFormatsCount, requestColorSpace);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkPresentModeKHR ImplVulkanHSelectPresentMode(this VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* requestModes, int requestModesCount)
		{
			VkPresentModeKHR ret = ImGuiBackends.ImplVulkanHSelectPresentModeNative(physicalDevice, surface, requestModes, requestModesCount);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static VkPresentModeKHR ImplVulkanHSelectPresentMode(this VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref VkPresentModeKHR requestModes, int requestModesCount)
		{
			fixed (VkPresentModeKHR* prequestModes = &requestModes)
			{
				VkPresentModeKHR ret = ImGuiBackends.ImplVulkanHSelectPresentModeNative(physicalDevice, surface, (VkPresentModeKHR*)prequestModes, requestModesCount);
				return ret;
			}
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static int ImplVulkanHGetMinImageCountFromPresentMode(this VkPresentModeKHR presentMode)
		{
			int ret = ImGuiBackends.ImplVulkanHGetMinImageCountFromPresentModeNative(presentMode);
			return ret;
		}

		/// <summary>
		/// To be documented.
		/// </summary>
		public static void ImplVulkanRemoveTexture(this VkDescriptorSet descriptorSet)
		{
			ImGuiBackends.ImplVulkanRemoveTextureNative(descriptorSet);
		}

		/// <summary>
		/// Register a texture (VkDescriptorSet == ImTextureID)<br/>
		/// FIXME: This is experimental in the sense that we are unsure how to best design/tackle this problem<br/>
		/// Please post to https://github.com/ocornut/imgui/pull/914 if you have suggestions.<br/>
		/// </summary>
		public static VkDescriptorSet ImplVulkanAddTexture(this VkSampler sampler, VkImageView imageView, VkImageLayout imageLayout)
		{
			VkDescriptorSet ret = ImGuiBackends.ImplVulkanAddTextureNative(sampler, imageView, imageLayout);
			return ret;
		}

	}
}