#ifndef CIMGUI_IMPL_SDL3_RENDERER_H
#define CIMGUI_IMPL_SDL3_RENDERER_H

#include "cimgui_config.h"
#if CIMGUI_USE_SDL3Renderer
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// typedefs for SDL3Renderer types used in this header
typedef struct SDL_Renderer SDL_Renderer;
// Full struct layout for ImGui_ImplSDLRenderer3_RenderState
struct ImGui_ImplSDLRenderer3_RenderState {
    SDL_Renderer* Renderer;
};
#else
#include "backends/imgui_impl_sdlrenderer3.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplSDLRenderer3_Init(SDL_Renderer *renderer);
CIMGUI_API void CImGui_ImplSDLRenderer3_Shutdown();
CIMGUI_API void CImGui_ImplSDLRenderer3_NewFrame();
CIMGUI_API void CImGui_ImplSDLRenderer3_RenderDrawData(ImDrawData *draw_data, SDL_Renderer *renderer);
// Called by Init/NewFrame/Shutdown
CIMGUI_API void CImGui_ImplSDLRenderer3_CreateDeviceObjects();
CIMGUI_API void CImGui_ImplSDLRenderer3_DestroyDeviceObjects();
// (Advanced) Use e.g. if you need to precisely control the timing of texture updates (e.g. for staged rendering), by setting ImDrawData::Textures = NULL to handle this manually.
CIMGUI_API void CImGui_ImplSDLRenderer3_UpdateTexture(ImTextureData* tex);
#endif

#endif // CIMGUI_IMPL_SDL3_RENDERER_H
