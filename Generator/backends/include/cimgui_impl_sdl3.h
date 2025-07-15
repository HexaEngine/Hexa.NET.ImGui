#ifndef CIMGUI_IMPL_SDL3_H
#define CIMGUI_IMPL_SDL3_H

#include "cimgui_config.h"
#if CIMGUI_USE_SDL3
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// typedefs for SDL3 types used in this header
typedef struct SDL_Window SDL_Window;
typedef struct SDL_Renderer SDL_Renderer;
typedef struct SDL_Gamepad SDL_Gamepad;
typedef union SDL_Event SDL_Event;
// enums from backend
enum ImGui_ImplSDL3_GamepadMode { ImGui_ImplSDL3_GamepadMode_AutoFirst, ImGui_ImplSDL3_GamepadMode_AutoAll, ImGui_ImplSDL3_GamepadMode_Manual };
#else
#include "backends/imgui_impl_sdl3.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplSDL3_InitForOpenGL(SDL_Window *window, void *sdl_gl_context);
CIMGUI_API bool CImGui_ImplSDL3_InitForVulkan(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL3_InitForD3D(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL3_InitForMetal(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL3_InitForSDLRenderer(SDL_Window *window, SDL_Renderer *renderer);
CIMGUI_API bool CImGui_ImplSDL3_InitForSDLGPU(SDL_Window *window);
CIMGUI_API bool CImGui_ImplSDL3_InitForOther(SDL_Window *window);
CIMGUI_API void CImGui_ImplSDL3_Shutdown();
CIMGUI_API void CImGui_ImplSDL3_NewFrame();
CIMGUI_API bool CImGui_ImplSDL3_ProcessEvent(const SDL_Event *event);
// Gamepad selection automatically starts in AutoFirst mode, picking first available SDL_Gamepad. You may override this.
// When using manual mode, caller is responsible for opening/closing gamepad.
CIMGUI_API void CImGui_ImplSDL3_SetGamepadMode(ImGui_ImplSDL3_GamepadMode mode, SDL_Gamepad **manual_gamepads_array, int manual_gamepads_count);
#endif

#endif // CIMGUI_IMPL_SDL3_H
