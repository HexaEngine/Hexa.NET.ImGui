#ifndef CIMGUI_IMPL_OPENGL3_H
#define CIMGUI_IMPL_OPENGL3_H

#include "cimgui_config.h"
#if CIMGUI_USE_OPENGL3
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// No non-Im* types or enums to typedef for OpenGL3 backend
#else
#include "backends/imgui_impl_opengl3.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplOpenGL3_Init(const char *glsl_version);
CIMGUI_API void CImGui_ImplOpenGL3_Shutdown(void);
CIMGUI_API void CImGui_ImplOpenGL3_NewFrame(void);
CIMGUI_API void CImGui_ImplOpenGL3_RenderDrawData(ImDrawData *draw_data);
CIMGUI_API void CImGui_ImplOpenGL3_UpdateTexture(ImTextureData* tex);
CIMGUI_API bool CImGui_ImplOpenGL3_CreateDeviceObjects(void);
CIMGUI_API void CImGui_ImplOpenGL3_DestroyDeviceObjects(void);
#endif

#endif // CIMGUI_IMPL_OPENGL3_H
