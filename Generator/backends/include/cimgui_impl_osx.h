#ifndef CIMGUI_IMPL_OSX_H
#define CIMGUI_IMPL_OSX_H

#include "cimgui_config.h"
#if CIMGUI_USE_OSX
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// No non-Im* types or enums to typedef for OSX backend
#else
#include "backends/imgui_impl_osx.h"
#import <AppKit/AppKit.h>
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplOSX_Init(void *view);
CIMGUI_API void CImGui_ImplOSX_Shutdown();
CIMGUI_API void CImGui_ImplOSX_NewFrame(void *view);

#endif

#endif // CIMGUI_IMPL_OSX_H
