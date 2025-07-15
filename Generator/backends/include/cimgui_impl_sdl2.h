#ifndef CIMGUI_IMPL_SDL2_H
#define CIMGUI_IMPL_SDL2_H

#include "cimgui_config.h"
#if CIMGUI_USE_SDL2
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// typedefs for SDL2 types used in this header
typedef struct SDL_Window SDL_Window;
typedef struct SDL_Renderer SDL_Renderer;
typedef struct _SDL_GameController _SDL_GameController;
typedef union SDL_Event SDL_Event;
// enums from backend
enum ImGui_ImplSDL2_GamepadMode { ImGui_ImplSDL2_GamepadMode_AutoFirst, ImGui_ImplSDL2_GamepadMode_AutoAll, ImGui_ImplSDL2_GamepadMode_Manual };
#else
#include "backends/imgui_impl_sdl2.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplSDL2_InitForOpenGL(SDL_Window *window, void *sdl_gl_context);
CIMGUI_API bool CImGui_ImplSDL2_InitForVulkan(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL2_InitForD3D(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL2_InitForMetal(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL2_InitForSDLRenderer(SDL_Window *window, SDL_Renderer *renderer);
CIMGUI_API bool CImGui_ImplSDL2_InitForOther(SDL_Window *window);
CIMGUI_API void CImGui_ImplSDL2_Shutdown(void);
CIMGUI_API void CImGui_ImplSDL2_NewFrame(void);
CIMGUI_API bool CImGui_ImplSDL2_ProcessEvent(const SDL_Event *event);
// DPI-related helpers (optional)
CIMGUI_API void CImGui_ImplSDL2_SetGamepadMode(ImGui_ImplSDL2_GamepadMode mode, struct _SDL_GameController **manual_gamepads_array, int manual_gamepads_count);
#endif

#endif // CIMGUI_IMPL_SDL2_H
