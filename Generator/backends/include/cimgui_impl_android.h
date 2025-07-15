#ifndef CIMGUI_IMPL_ANDROID_H
#define CIMGUI_IMPL_ANDROID_H

#include "cimgui_config.h"
#if CIMGUI_USE_ANDROID
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// typedefs for Android types used in this header
typedef struct ANativeWindow ANativeWindow;
typedef struct AInputEvent AInputEvent;
#else
#include "backends/imgui_impl_android.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplAndroid_Init(ANativeWindow *window);
CIMGUI_API int32_t CImGui_ImplAndroid_HandleInputEvent(const AInputEvent *input_event);
CIMGUI_API void CImGui_ImplAndroid_Shutdown();
CIMGUI_API void CImGui_ImplAndroid_NewFrame();
#endif

#endif // CIMGUI_IMPL_ANDROID_H
