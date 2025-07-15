#ifndef CIMGUI_IMPL_DX9_H
#define CIMGUI_IMPL_DX9_H

#include "cimgui_config.h"
#if CIMGUI_USE_D3D9
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
typedef struct IDirect3DDevice9 IDirect3DDevice9;
#else
#include "backends/imgui_impl_dx9.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplDX9_Init(IDirect3DDevice9 *device);
CIMGUI_API void CImGui_ImplDX9_Shutdown();
CIMGUI_API void CImGui_ImplDX9_NewFrame();
CIMGUI_API void CImGui_ImplDX9_RenderDrawData(ImDrawData *draw_data);
CIMGUI_API void CImGui_ImplDX9_UpdateTexture(ImTextureData* tex);
CIMGUI_API bool CImGui_ImplDX9_CreateDeviceObjects();
CIMGUI_API void CImGui_ImplDX9_InvalidateDeviceObjects();
#endif

#endif // CIMGUI_IMPL_DX9_H
