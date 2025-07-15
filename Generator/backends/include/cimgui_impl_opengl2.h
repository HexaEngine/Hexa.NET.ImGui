#ifndef CIMGUI_IMPL_OPENGL2_H
#define CIMGUI_IMPL_OPENGL2_H

#include "cimgui_config.h"
#if CIMGUI_USE_OPENGL2
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// No non-Im* types or enums to typedef for OpenGL2 backend
#else
#include "backends/imgui_impl_opengl2.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplOpenGL2_Init(void);
CIMGUI_API void CImGui_ImplOpenGL2_Shutdown(void);
CIMGUI_API void CImGui_ImplOpenGL2_NewFrame(void);
CIMGUI_API void CImGui_ImplOpenGL2_RenderDrawData(ImDrawData *draw_data);
CIMGUI_API void CImGui_ImplOpenGL2_UpdateTexture(ImTextureData* tex);
CIMGUI_API bool CImGui_ImplOpenGL2_CreateDeviceObjects(void);
CIMGUI_API void CImGui_ImplOpenGL2_DestroyDeviceObjects(void);
#endif

#endif // CIMGUI_IMPL_OPENGL2_H
