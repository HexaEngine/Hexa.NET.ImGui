#ifndef CIMGUI_USE_GLFW
#define CIMGUI_USE_GLFW 1
#endif
#ifndef CIMGUI_USE_OPENGL3
#define CIMGUI_USE_OPENGL3 1
#endif
#ifndef CIMGUI_USE_OPENGL2
#define CIMGUI_USE_OPENGL2 1
#endif
#ifndef CIMGUI_USE_SDL2
#define CIMGUI_USE_SDL2 1
#endif
#ifndef CIMGUI_USE_SDL2Renderer
#define CIMGUI_USE_SDL2Renderer 1
#endif
#ifndef CIMGUI_USE_D3D11
#define CIMGUI_USE_D3D11 1
#endif
#ifndef CIMGUI_USE_D3D12
#define CIMGUI_USE_D3D12 1
#endif
#ifndef CIMGUI_USE_METAL
#define CIMGUI_USE_METAL 1
#endif
#ifndef CIMGUI_USE_VULKAN
#define CIMGUI_USE_VULKAN 1
#endif
#ifndef CIMGUI_USE_WIN32
#define CIMGUI_USE_WIN32 1
#endif
#ifndef CIMGUI_USE_OSX
#define CIMGUI_USE_OSX 1
#endif
#ifndef CIMGUI_USE_ANDROID
#define CIMGUI_USE_ANDROID 1
#endif

#include <stdint.h>
#include <string.h> // For memset

CIMGUI_API void igSetCurrentContext(ImGuiContext* ctx);
CIMGUI_API ImGuiContext* igGetCurrentContext(void);

#if CIMGUI_USE_WIN32

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool     CImGui_ImplWin32_Init(void* hwnd);
CIMGUI_API bool     CImGui_ImplWin32_InitForOpenGL(void* hwnd);
CIMGUI_API void     CImGui_ImplWin32_Shutdown();
CIMGUI_API void     CImGui_ImplWin32_NewFrame();

// Win32 message handler your application need to call.
// - Intentionally commented out in a '#if 0' block to avoid dragging dependencies on <windows.h> from this helper.
// - You should COPY the line below into your .cpp code to forward declare the function and then you can call it.
// - Call from your application's message handler. Keep calling your message handler unless this function returns TRUE.

#if 0
extern CIMGUI_API LRESULT CImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
#endif

// DPI-related helpers (optional)
// - Use to enable DPI awareness without having to create an application manifest.
// - Your own app may already do this via a manifest or explicit calls. This is mostly useful for our examples/ apps.
// - In theory we could call simple functions from Windows SDK such as SetProcessDPIAware(), SetProcessDpiAwareness(), etc.
//   but most of the functions provided by Microsoft require Windows 8.1/10+ SDK at compile time and Windows 8/10+ at runtime,
//   neither we want to require the user to have. So we dynamically select and load those functions to avoid dependencies.
CIMGUI_API void     CImGui_ImplWin32_EnableDpiAwareness();
CIMGUI_API float    CImGui_ImplWin32_GetDpiScaleForHwnd(void* hwnd);       // HWND hwnd
CIMGUI_API float    CImGui_ImplWin32_GetDpiScaleForMonitor(void* monitor); // HMONITOR monitor

// Transparency related helpers (optional) [experimental]
// - Use to enable alpha compositing transparency with the desktop.
// - Use together with e.g. clearing your framebuffer with zero-alpha.
CIMGUI_API void     CImGui_ImplWin32_EnableAlphaCompositing(void* hwnd);   // HWND hwnd

#endif

#if CIMGUI_USE_GLFW

typedef struct GLFWwindow GLFWwindow;
typedef struct GLFWmonitor GLFWmonitor;
struct GLFWwindow;
struct GLFWmonitor;
CIMGUI_API bool CImGui_ImplGlfw_InitForOpenGL(GLFWwindow* window, bool install_callbacks);
CIMGUI_API bool CImGui_ImplGlfw_InitForVulkan(GLFWwindow* window, bool install_callbacks);
CIMGUI_API bool CImGui_ImplGlfw_InitForOther(GLFWwindow* window, bool install_callbacks);
CIMGUI_API void CImGui_ImplGlfw_Shutdown(void);
CIMGUI_API void CImGui_ImplGlfw_NewFrame(void);
CIMGUI_API void CImGui_ImplGlfw_InstallCallbacks(GLFWwindow* window);
CIMGUI_API void CImGui_ImplGlfw_RestoreCallbacks(GLFWwindow* window);
CIMGUI_API void CImGui_ImplGlfw_SetCallbacksChainForAllWindows(bool chain_for_all_windows);
CIMGUI_API void CImGui_ImplGlfw_WindowFocusCallback(GLFWwindow* window, int focused);
CIMGUI_API void CImGui_ImplGlfw_CursorEnterCallback(GLFWwindow* window, int entered);
CIMGUI_API void CImGui_ImplGlfw_CursorPosCallback(GLFWwindow* window, double x, double y);
CIMGUI_API void CImGui_ImplGlfw_MouseButtonCallback(GLFWwindow* window, int button, int action, int mods);
CIMGUI_API void CImGui_ImplGlfw_ScrollCallback(GLFWwindow* window, double xoffset, double yoffset);
CIMGUI_API void CImGui_ImplGlfw_KeyCallback(GLFWwindow* window, int key, int scancode, int action, int mods);
CIMGUI_API void CImGui_ImplGlfw_CharCallback(GLFWwindow* window, unsigned int c);
CIMGUI_API void CImGui_ImplGlfw_MonitorCallback(GLFWmonitor* monitor, int event);
CIMGUI_API void CImGui_ImplGlfw_Sleep(int milliseconds);

#endif

#if CIMGUI_USE_SDL2

typedef struct SDL_Window SDL_Window;
typedef struct SDL_Renderer SDL_Renderer;
typedef struct _SDL_GameController _SDL_GameController;
struct SDL_Window;
struct SDL_Renderer;
struct _SDL_GameController;
typedef union SDL_Event SDL_Event;
typedef enum { ImGui_ImplSDL2_GamepadMode_AutoFirst, ImGui_ImplSDL2_GamepadMode_AutoAll, ImGui_ImplSDL2_GamepadMode_Manual }ImGui_ImplSDL2_GamepadMode;
CIMGUI_API bool CImGui_ImplSDL2_InitForOpenGL(SDL_Window* window, void* sdl_gl_context);
CIMGUI_API bool CImGui_ImplSDL2_InitForVulkan(SDL_Window* window);
CIMGUI_API bool CImGui_ImplSDL2_InitForD3D(SDL_Window* window);
CIMGUI_API bool CImGui_ImplSDL2_InitForMetal(SDL_Window* window);
CIMGUI_API bool CImGui_ImplSDL2_InitForSDLRenderer(SDL_Window* window, SDL_Renderer* renderer);
CIMGUI_API bool CImGui_ImplSDL2_InitForOther(SDL_Window* window);
CIMGUI_API void CImGui_ImplSDL2_Shutdown(void);
CIMGUI_API void CImGui_ImplSDL2_NewFrame(void);
CIMGUI_API bool CImGui_ImplSDL2_ProcessEvent(const SDL_Event* event);
CIMGUI_API void CImGui_ImplSDL2_SetGamepadMode(ImGui_ImplSDL2_GamepadMode mode, struct _SDL_GameController** manual_gamepads_array, int manual_gamepads_count);

#ifdef CIMGUI_USE_SDL2Renderer

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool     CImGui_ImplSDLRenderer2_Init(SDL_Renderer* renderer);
CIMGUI_API void     CImGui_ImplSDLRenderer2_Shutdown();
CIMGUI_API void     CImGui_ImplSDLRenderer2_NewFrame();
CIMGUI_API void     CImGui_ImplSDLRenderer2_RenderDrawData(ImDrawData* draw_data, SDL_Renderer* renderer);

// Called by Init/NewFrame/Shutdown
CIMGUI_API bool     CImGui_ImplSDLRenderer2_CreateFontsTexture();
CIMGUI_API void     CImGui_ImplSDLRenderer2_DestroyFontsTexture();
CIMGUI_API bool     CImGui_ImplSDLRenderer2_CreateDeviceObjects();
CIMGUI_API void     CImGui_ImplSDLRenderer2_DestroyDeviceObjects();

#endif

#endif

#if CIMGUI_USE_OPENGL3
CIMGUI_API bool CImGui_ImplOpenGL3_Init(const char* glsl_version);
CIMGUI_API void CImGui_ImplOpenGL3_Shutdown(void);
CIMGUI_API void CImGui_ImplOpenGL3_NewFrame(void);
CIMGUI_API void CImGui_ImplOpenGL3_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API bool CImGui_ImplOpenGL3_CreateFontsTexture(void);
CIMGUI_API void CImGui_ImplOpenGL3_DestroyFontsTexture(void);
CIMGUI_API bool CImGui_ImplOpenGL3_CreateDeviceObjects(void);
CIMGUI_API void CImGui_ImplOpenGL3_DestroyDeviceObjects(void);

#endif

#if CIMGUI_USE_OPENGL2
CIMGUI_API bool CImGui_ImplOpenGL2_Init(void);
CIMGUI_API void CImGui_ImplOpenGL2_Shutdown(void);
CIMGUI_API void CImGui_ImplOpenGL2_NewFrame(void);
CIMGUI_API void CImGui_ImplOpenGL2_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API bool CImGui_ImplOpenGL2_CreateFontsTexture(void);
CIMGUI_API void CImGui_ImplOpenGL2_DestroyFontsTexture(void);
CIMGUI_API bool CImGui_ImplOpenGL2_CreateDeviceObjects(void);
CIMGUI_API void CImGui_ImplOpenGL2_DestroyDeviceObjects(void);

#endif

#if CIMGUI_USE_D3D11
typedef struct ID3D11Device ID3D11Device;
typedef struct ID3D11DeviceContext ID3D11DeviceContext;

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool     CImGui_ImplDX11_Init(ID3D11Device* device, ID3D11DeviceContext* device_context);
CIMGUI_API void     CImGui_ImplDX11_Shutdown();
CIMGUI_API void     CImGui_ImplDX11_NewFrame();
CIMGUI_API void     CImGui_ImplDX11_RenderDrawData(ImDrawData* draw_data);

// Use if you want to reset your rendering device without losing Dear ImGui state.
CIMGUI_API void     CImGui_ImplDX11_InvalidateDeviceObjects();
CIMGUI_API bool     CImGui_ImplDX11_CreateDeviceObjects();

#endif

#if CIMGUI_USE_D3D12
typedef struct ID3D12Device;
typedef struct ID3D12DescriptorHeap;
typedef struct ID3D12GraphicsCommandList;
typedef struct D3D12_CPU_DESCRIPTOR_HANDLE;
typedef struct D3D12_GPU_DESCRIPTOR_HANDLE;
typedef unsigned int DXGI_FORMAT;

// Follow "Getting Started" link and check examples/ folder to learn about using backends!

// cmd_list is the command list that the implementation will use to render imgui draw lists.
// Before calling the render function, caller must prepare cmd_list by resetting it and setting the appropriate
// render target and descriptor heap that contains font_srv_cpu_desc_handle/font_srv_gpu_desc_handle.
// font_srv_cpu_desc_handle and font_srv_gpu_desc_handle are handles to a single SRV descriptor to use for the internal font texture.
CIMGUI_API bool     CImGui_ImplDX12_Init(ID3D12Device* device, int num_frames_in_flight, DXGI_FORMAT rtv_format, ID3D12DescriptorHeap* cbv_srv_heap, D3D12_CPU_DESCRIPTOR_HANDLE font_srv_cpu_desc_handle, D3D12_GPU_DESCRIPTOR_HANDLE font_srv_gpu_desc_handle);
CIMGUI_API void     CImGui_ImplDX12_Shutdown();
CIMGUI_API void     CImGui_ImplDX12_NewFrame();
CIMGUI_API void     CImGui_ImplDX12_RenderDrawData(ImDrawData* draw_data, ID3D12GraphicsCommandList* graphics_command_list);

// Use if you want to reset your rendering device without losing Dear ImGui state.
CIMGUI_API void     CImGui_ImplDX12_InvalidateDeviceObjects();
CIMGUI_API bool     CImGui_ImplDX12_CreateDeviceObjects();
#endif

#if CIMGUI_USE_VULKAN

// Forward declarations for Vulkan types
typedef struct VkInstance_T* VkInstance;
typedef struct VkPhysicalDevice_T* VkPhysicalDevice;
typedef struct VkDevice_T* VkDevice;
typedef struct VkQueue_T* VkQueue;
typedef struct VkDescriptorPool_T* VkDescriptorPool;
typedef struct VkRenderPass_T* VkRenderPass;
typedef struct VkPipelineCache_T* VkPipelineCache;
typedef struct VkAllocationCallbacks_T* VkAllocationCallbacks;
typedef struct VkCommandBuffer_T* VkCommandBuffer;
typedef struct VkSemaphore_T* VkSemaphore;
typedef struct VkImage_T* VkImage;
typedef struct VkImageView_T* VkImageView;
typedef struct VkFramebuffer_T* VkFramebuffer;
typedef struct VkSwapchainKHR_T* VkSwapchainKHR;
typedef struct VkSurfaceKHR_T* VkSurfaceKHR;
typedef struct VkSurfaceFormatKHR_T* VkSurfaceFormatKHR;
typedef struct VkPresentModeKHR_T* VkPresentModeKHR;
typedef struct VkDescriptorSet_T* VkDescriptorSet;
typedef struct VkSampler_T* VkSampler;
typedef struct VkCommandPool_T* VkCommandPool;
typedef struct VkFence_T* VkFence;

// Simplified Vulkan types
typedef int VkResult;
typedef unsigned int VkSampleCountFlagBits;
typedef unsigned long long VkDeviceSize;

// Vulkan constants
#define VK_NULL_HANDLE nullptr

// Format and color space types
typedef unsigned int VkFormat;
typedef unsigned int VkColorSpaceKHR;

// Pipeline and layout types
typedef struct VkPipeline_T* VkPipeline;
typedef struct VkImageLayout_T* VkImageLayout;

// Clear value structure
typedef struct VkClearValue {
	float color[4]; // For color attachments
	struct {
		float depth; // Depth value
		unsigned int stencil; // Stencil value
	} depthStencil; // For depth/stencil attachments
} VkClearValue;

// Initialization data, for ImGui_ImplVulkan_Init()
// - VkDescriptorPool should be created with VK_DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT,
//   and must contain a pool size large enough to hold an ImGui VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER descriptor.
// - When using dynamic rendering, set UseDynamicRendering=true and fill PipelineRenderingCreateInfo structure.
// [Please zero-clear before use!]
struct ImGui_ImplVulkan_InitInfo
{
	VkInstance                      Instance;
	VkPhysicalDevice                PhysicalDevice;
	VkDevice                        Device;
	uint32_t                        QueueFamily;
	VkQueue                         Queue;
	VkDescriptorPool                DescriptorPool;               // See requirements in note above
	VkRenderPass                    RenderPass;                   // Ignored if using dynamic rendering
	uint32_t                        MinImageCount;                // >= 2
	uint32_t                        ImageCount;                   // >= MinImageCount
	VkSampleCountFlagBits           MSAASamples;                  // 0 defaults to VK_SAMPLE_COUNT_1_BIT

	// (Optional)
	VkPipelineCache                 PipelineCache;
	uint32_t                        Subpass;

	// (Optional) Dynamic Rendering
	// Need to explicitly enable VK_KHR_dynamic_rendering extension to use this, even for Vulkan 1.3.
	bool                            UseDynamicRendering;
#ifdef IMGUI_IMPL_VULKAN_HAS_DYNAMIC_RENDERING
	VkPipelineRenderingCreateInfoKHR PipelineRenderingCreateInfo;
#endif

	// (Optional) Allocation, Debugging
	const VkAllocationCallbacks* Allocator;
	void                            (*CheckVkResultFn)(VkResult err);
	VkDeviceSize                    MinAllocationSize;      // Minimum allocation size. Set to 1024*1024 to satisfy zealous best practices validation layer and waste a little memory.
};

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool         CImGui_ImplVulkan_Init(ImGui_ImplVulkan_InitInfo* info);
CIMGUI_API void         CImGui_ImplVulkan_Shutdown();
CIMGUI_API void         CImGui_ImplVulkan_NewFrame();
CIMGUI_API void         CImGui_ImplVulkan_RenderDrawData(ImDrawData* draw_data, VkCommandBuffer command_buffer, VkPipeline pipeline = VK_NULL_HANDLE);
CIMGUI_API bool         CImGui_ImplVulkan_CreateFontsTexture();
CIMGUI_API void         CImGui_ImplVulkan_DestroyFontsTexture();
CIMGUI_API void         CImGui_ImplVulkan_SetMinImageCount(uint32_t min_image_count); // To override MinImageCount after initialization (e.g. if swap chain is recreated)

// Register a texture (VkDescriptorSet == ImTextureID)
// FIXME: This is experimental in the sense that we are unsure how to best design/tackle this problem
// Please post to https://github.com/ocornut/imgui/pull/914 if you have suggestions.
CIMGUI_API VkDescriptorSet CImGui_ImplVulkan_AddTexture(VkSampler sampler, VkImageView image_view, VkImageLayout image_layout);
CIMGUI_API void            CImGui_ImplVulkan_RemoveTexture(VkDescriptorSet descriptor_set);

typedef void* (*PFN_vkVoidFunction)(const char* function_name, void* user_data);

// Optional: load Vulkan functions with a custom function loader
// This is only useful with IMGUI_IMPL_VULKAN_NO_PROTOTYPES / VK_NO_PROTOTYPES
CIMGUI_API bool         CImGui_ImplVulkan_LoadFunctions(PFN_vkVoidFunction loader_func, void* user_data);

//-------------------------------------------------------------------------
// Internal / Miscellaneous Vulkan Helpers
// (Used by example's main.cpp. Used by multi-viewport features. PROBABLY NOT used by your own engine/app.)
//-------------------------------------------------------------------------
// You probably do NOT need to use or care about those functions.
// Those functions only exist because:
//   1) they facilitate the readability and maintenance of the multiple main.cpp examples files.
//   2) the multi-viewport / platform window implementation needs them internally.
// Generally we avoid exposing any kind of superfluous high-level helpers in the bindings,
// but it is too much code to duplicate everywhere so we exceptionally expose them.
//
// Your engine/app will likely _already_ have code to setup all that stuff (swap chain, render pass, frame buffers, etc.).
// You may read this code to learn about Vulkan, but it is recommended you use you own custom tailored code to do equivalent work.
// (The ImGui_ImplVulkanH_XXX functions do not interact with any of the state used by the regular ImGui_ImplVulkan_XXX functions)
//-------------------------------------------------------------------------

struct ImGui_ImplVulkanH_Frame;
struct ImGui_ImplVulkanH_Window;

// Helpers
CIMGUI_API void                 CImGui_ImplVulkanH_CreateOrResizeWindow(VkInstance instance, VkPhysicalDevice physical_device, VkDevice device, ImGui_ImplVulkanH_Window* wd, uint32_t queue_family, const VkAllocationCallbacks* allocator, int w, int h, uint32_t min_image_count);
CIMGUI_API void                 CImGui_ImplVulkanH_DestroyWindow(VkInstance instance, VkDevice device, ImGui_ImplVulkanH_Window* wd, const VkAllocationCallbacks* allocator);
CIMGUI_API VkSurfaceFormatKHR   CImGui_ImplVulkanH_SelectSurfaceFormat(VkPhysicalDevice physical_device, VkSurfaceKHR surface, const VkFormat* request_formats, int request_formats_count, VkColorSpaceKHR request_color_space);
CIMGUI_API VkPresentModeKHR     CImGui_ImplVulkanH_SelectPresentMode(VkPhysicalDevice physical_device, VkSurfaceKHR surface, const VkPresentModeKHR* request_modes, int request_modes_count);
CIMGUI_API int                  CImGui_ImplVulkanH_GetMinImageCountFromPresentMode(VkPresentModeKHR present_mode);

// Helper structure to hold the data needed by one rendering frame
// (Used by example's main.cpp. Used by multi-viewport features. Probably NOT used by your own engine/app.)
// [Please zero-clear before use!]
struct ImGui_ImplVulkanH_Frame
{
	VkCommandPool       CommandPool;
	VkCommandBuffer     CommandBuffer;
	VkFence             Fence;
	VkImage             Backbuffer;
	VkImageView         BackbufferView;
	VkFramebuffer       Framebuffer;
};

struct ImGui_ImplVulkanH_FrameSemaphores
{
	VkSemaphore         ImageAcquiredSemaphore;
	VkSemaphore         RenderCompleteSemaphore;
};

// Helper structure to hold the data needed by one rendering context into one OS window
// (Used by example's main.cpp. Used by multi-viewport features. Probably NOT used by your own engine/app.)
struct ImGui_ImplVulkanH_Window
{
	int                 Width;
	int                 Height;
	VkSwapchainKHR      Swapchain;
	VkSurfaceKHR        Surface;
	VkSurfaceFormatKHR  SurfaceFormat;
	VkPresentModeKHR    PresentMode;
	VkRenderPass        RenderPass;
	bool                UseDynamicRendering;
	bool                ClearEnable;
	VkClearValue        ClearValue;
	uint32_t            FrameIndex;             // Current frame being rendered to (0 <= FrameIndex < FrameInFlightCount)
	uint32_t            ImageCount;             // Number of simultaneous in-flight frames (returned by vkGetSwapchainImagesKHR, usually derived from min_image_count)
	uint32_t            SemaphoreCount;         // Number of simultaneous in-flight frames + 1, to be able to use it in vkAcquireNextImageKHR
	uint32_t            SemaphoreIndex;         // Current set of swapchain wait semaphores we're using (needs to be distinct from per frame data)
	ImGui_ImplVulkanH_Frame* Frames;
	ImGui_ImplVulkanH_FrameSemaphores* FrameSemaphores;

	ImGui_ImplVulkanH_Window()
	{
		memset((void*)this, 0, sizeof(*this));
		PresentMode = (VkPresentModeKHR)~0;     // Ensure we get an error if user doesn't set this.
		ClearEnable = true;
	}
};

#endif

#if CIMGUI_USE_OSX

CIMGUI_API bool CImGui_ImplOSX_Init(void* view);
CIMGUI_API void CImGui_ImplOSX_Shutdown();
CIMGUI_API void CImGui_ImplOSX_NewFrame(void* view);

#endif

#if CIMGUI_USE_METAL

typedef struct MTLDevice MTLDevice;
typedef struct MTLRenderPassDescriptor MTLRenderPassDescriptor;
typedef struct MTLCommandBuffer MTLCommandBuffer;
typedef struct MTLRenderCommandEncoder MTLRenderCommandEncoder;

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplMetal_Init(MTLDevice* device);
CIMGUI_API void CImGui_ImplMetal_Shutdown();
CIMGUI_API void CImGui_ImplMetal_NewFrame(MTLRenderPassDescriptor* renderPassDescriptor);
CIMGUI_API void CImGui_ImplMetal_RenderDrawData(ImDrawData* draw_data, MTLCommandBuffer* commandBuffer, MTLRenderCommandEncoder* commandEncoder);

// Called by Init/NewFrame/Shutdown
CIMGUI_API bool CImGui_ImplMetal_CreateFontsTexture(MTLDevice* device);
CIMGUI_API void CImGui_ImplMetal_DestroyFontsTexture();
CIMGUI_API bool CImGui_ImplMetal_CreateDeviceObjects(MTLDevice* device);
CIMGUI_API void CImGui_ImplMetal_DestroyDeviceObjects();

#endif

#if CIMGUI_USE_ANDROID

typedef struct ANativeWindow ANativeWindow;
typedef struct AInputEvent AInputEvent;

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplAndroid_Init(ANativeWindow* window);
CIMGUI_API int32_t CImGui_ImplAndroid_HandleInputEvent(const AInputEvent* input_event);
CIMGUI_API void CImGui_ImplAndroid_Shutdown();
CIMGUI_API void CImGui_ImplAndroid_NewFrame();

#endif