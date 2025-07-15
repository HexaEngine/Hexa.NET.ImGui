#ifndef CIMGUI_IMPL_WIN32_H
#define CIMGUI_IMPL_WIN32_H

#include "cimgui_config.h"
#if CIMGUI_USE_WIN32
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// typedefs for Win32 types used in this header
typedef struct HWND HWND;
typedef unsigned int UINT;
typedef unsigned long WPARAM;
typedef long LPARAM;
typedef long LRESULT;
#else
#include <windows.h>
#include "backends/imgui_impl_win32.h"
#endif
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplWin32_Init(void* hwnd);
CIMGUI_API bool CImGui_ImplWin32_InitForOpenGL(void* hwnd);
CIMGUI_API void CImGui_ImplWin32_Shutdown();
CIMGUI_API void CImGui_ImplWin32_NewFrame();
// Win32 message handler your application need to call.
// - Intentionally commented out in a '#if 0' block to avoid dragging dependencies on <windows.h> from this helper.
// - You should COPY the line below into your .cpp code to forward declare the function and then you can call it.
// - Call from your application's message handler. Keep calling your message handler unless this function returns TRUE.
CIMGUI_API LRESULT CImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
// DPI-related helpers (optional)
// - Use to enable DPI awareness without having to create an application manifest.
// - Your own app may already do this via a manifest or explicit calls. This is mostly useful for our examples/ apps.
// - In theory we could call simple functions from Windows SDK such as SetProcessDPIAware(), SetProcessDpiAwareness(), etc.
//   but most of the functions provided by Microsoft require Windows 8.1/10+ SDK at compile time and Windows 8/10+ at runtime,
//   neither we want to require the user to have. So we dynamically select and load those functions to avoid dependencies.
CIMGUI_API void CImGui_ImplWin32_EnableDpiAwareness();
CIMGUI_API float CImGui_ImplWin32_GetDpiScaleForHwnd(void* hwnd);
CIMGUI_API float CImGui_ImplWin32_GetDpiScaleForMonitor(void* monitor);
// Transparency related helpers (optional) [experimental]
// - Use to enable alpha compositing transparency with the desktop.
// - Use together with e.g. clearing your framebuffer with zero-alpha.
CIMGUI_API void CImGui_ImplWin32_EnableAlphaCompositing(void* hwnd);
#endif

#endif // CIMGUI_IMPL_WIN32_H
