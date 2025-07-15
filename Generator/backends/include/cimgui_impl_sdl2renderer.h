#ifndef CIMGUI_IMPL_SDL2_RENDERER_H
#define CIMGUI_IMPL_SDL2_RENDERER_H

#include "cimgui_config.h"
#if CIMGUI_USE_SDL2Renderer
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// typedefs for SDL2Renderer types used in this header
typedef struct SDL_Renderer SDL_Renderer;
// Full struct layout for ImGui_ImplSDLRenderer2_RenderState
struct ImGui_ImplSDLRenderer2_RenderState {
    SDL_Renderer* Renderer;
};
#else
#include "backends/imgui_impl_sdlrenderer2.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplSDLRenderer2_Init(SDL_Renderer *renderer);
CIMGUI_API void CImGui_ImplSDLRenderer2_Shutdown();
CIMGUI_API void CImGui_ImplSDLRenderer2_NewFrame();
CIMGUI_API void CImGui_ImplSDLRenderer2_RenderDrawData(ImDrawData *draw_data, SDL_Renderer *renderer);
// Called by Init/NewFrame/Shutdown
CIMGUI_API void CImGui_ImplSDLRenderer2_CreateDeviceObjects();
CIMGUI_API void CImGui_ImplSDLRenderer2_DestroyDeviceObjects();
// (Advanced) Use e.g. if you need to precisely control the timing of texture updates (e.g. for staged rendering), by setting ImDrawData::Textures = NULL to handle this manually.
CIMGUI_API void CImGui_ImplSDLRenderer2_UpdateTexture(ImTextureData* tex);
#endif

#endif // CIMGUI_IMPL_SDL2_RENDERER_H
