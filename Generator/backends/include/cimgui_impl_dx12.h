#ifndef CIMGUI_IMPL_DX12_H
#define CIMGUI_IMPL_DX12_H

#include "cimgui_config.h"
#if CIMGUI_USE_D3D12
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
typedef struct ID3D12Device ID3D12Device;
typedef struct ID3D12CommandQueue ID3D12CommandQueue;
typedef struct ID3D12DescriptorHeap ID3D12DescriptorHeap;
typedef struct ID3D12GraphicsCommandList ID3D12GraphicsCommandList;
typedef struct D3D12_CPU_DESCRIPTOR_HANDLE {
	size_t ptr;
} D3D12_CPU_DESCRIPTOR_HANDLE;
typedef struct D3D12_GPU_DESCRIPTOR_HANDLE {
	uint64_t ptr;
} D3D12_GPU_DESCRIPTOR_HANDLE;
typedef int DXGI_FORMAT;

struct ImGui_ImplDX12_InitInfo
{
    ID3D12Device*               Device;
    ID3D12CommandQueue*         CommandQueue;       // Command queue used for queuing texture uploads.
    int                         NumFramesInFlight;
    DXGI_FORMAT                 RTVFormat;          // RenderTarget format.
    DXGI_FORMAT                 DSVFormat;          // DepthStencilView format.
    void*                       UserData;

    // Allocating SRV descriptors for textures is up to the application, so we provide callbacks.
    // (current version of the backend will only allocate one descriptor, from 1.92 the backend will need to allocate more)
    ID3D12DescriptorHeap*       SrvDescriptorHeap;
    void                        (*SrvDescriptorAllocFn)(ImGui_ImplDX12_InitInfo* info, D3D12_CPU_DESCRIPTOR_HANDLE* out_cpu_desc_handle, D3D12_GPU_DESCRIPTOR_HANDLE* out_gpu_desc_handle);
    void                        (*SrvDescriptorFreeFn)(ImGui_ImplDX12_InitInfo* info, D3D12_CPU_DESCRIPTOR_HANDLE cpu_desc_handle, D3D12_GPU_DESCRIPTOR_HANDLE gpu_desc_handle);
#ifndef IMGUI_DISABLE_OBSOLETE_FUNCTIONS
    D3D12_CPU_DESCRIPTOR_HANDLE LegacySingleSrvCpuDescriptor; // To facilitate transition from single descriptor to allocator callback, you may use those.
    D3D12_GPU_DESCRIPTOR_HANDLE LegacySingleSrvGpuDescriptor;
#endif
};
#else
#include <d3d12.h>
#include "backends/imgui_impl_dx12.h"
#endif
// Initialization data, for ImGui_ImplDX12_Init()
CIMGUI_API bool CImGui_ImplDX12_Init(ImGui_ImplDX12_InitInfo *info);
CIMGUI_API void CImGui_ImplDX12_Shutdown();
CIMGUI_API void CImGui_ImplDX12_NewFrame();
CIMGUI_API void CImGui_ImplDX12_RenderDrawData(ImDrawData *draw_data, ID3D12GraphicsCommandList *graphics_command_list);
CIMGUI_API void CImGui_ImplDX12_UpdateTexture(ImTextureData *tex);
CIMGUI_API void CImGui_ImplDX12_InvalidateDeviceObjects();
CIMGUI_API bool CImGui_ImplDX12_CreateDeviceObjects();
#endif

#endif // CIMGUI_IMPL_DX12_H
