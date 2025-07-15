#ifndef CIMGUI_IMPL_DX11_H
#define CIMGUI_IMPL_DX11_H

#include "cimgui_config.h"
#if CIMGUI_USE_D3D11
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
typedef struct ID3D11Device ID3D11Device;
typedef struct ID3D11DeviceContext ID3D11DeviceContext;
typedef struct ID3D11SamplerState ID3D11SamplerState;
typedef struct ID3D11Buffer ID3D11Buffer;
// Full struct layout for ImGui_ImplDX11_RenderState
struct ImGui_ImplDX11_RenderState {
    ID3D11Device*           Device;
    ID3D11DeviceContext*    DeviceContext;
    ID3D11SamplerState*     SamplerDefault;
    ID3D11Buffer*           VertexConstantBuffer;
};
#else
#include "backends/imgui_impl_dx11.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplDX11_Init(ID3D11Device *device, ID3D11DeviceContext *device_context);
CIMGUI_API void CImGui_ImplDX11_Shutdown();
CIMGUI_API void CImGui_ImplDX11_NewFrame();
CIMGUI_API void CImGui_ImplDX11_RenderDrawData(ImDrawData *draw_data);
CIMGUI_API void CImGui_ImplDX11_UpdateTexture(ImTextureData* tex);
CIMGUI_API void CImGui_ImplDX11_InvalidateDeviceObjects();
CIMGUI_API bool CImGui_ImplDX11_CreateDeviceObjects();
#endif

#endif // CIMGUI_IMPL_DX11_H
