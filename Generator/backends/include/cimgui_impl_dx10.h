#ifndef CIMGUI_IMPL_DX10_H
#define CIMGUI_IMPL_DX10_H

#include "cimgui_config.h"
#if CIMGUI_USE_D3D10
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
typedef struct ID3D10Device ID3D10Device;
typedef struct ID3D10SamplerState ID3D10SamplerState;
typedef struct ID3D10Buffer ID3D10Buffer;
// Full struct layout for ImGui_ImplDX10_RenderState
struct ImGui_ImplDX10_RenderState {
    ID3D10Device*           Device;
    ID3D10SamplerState*     SamplerDefault;
    ID3D10Buffer*           VertexConstantBuffer;
};
#else
#include "backends/imgui_impl_dx10.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplDX10_Init(ID3D10Device *device);
CIMGUI_API void CImGui_ImplDX10_Shutdown();
CIMGUI_API void CImGui_ImplDX10_NewFrame();
CIMGUI_API void CImGui_ImplDX10_RenderDrawData(ImDrawData *draw_data);
CIMGUI_API void CImGui_ImplDX10_UpdateTexture(ImTextureData* tex);
CIMGUI_API bool CImGui_ImplDX10_CreateDeviceObjects();
CIMGUI_API void CImGui_ImplDX10_InvalidateDeviceObjects();
#endif

#endif // CIMGUI_IMPL_DX10_H
