#ifndef CIMGUI_IMPL_GLFW_H
#define CIMGUI_IMPL_GLFW_H

#include "cimgui_config.h"
#if CIMGUI_USE_GLFW
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
typedef struct GLFWwindow GLFWwindow;
typedef struct GLFWmonitor GLFWmonitor;
#else
#include "backends/imgui_impl_glfw.h"
#endif

// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplGlfw_InitForOpenGL(GLFWwindow *window, bool install_callbacks);
CIMGUI_API bool CImGui_ImplGlfw_InitForVulkan(GLFWwindow *window, bool install_callbacks);
CIMGUI_API bool CImGui_ImplGlfw_InitForOther(GLFWwindow *window, bool install_callbacks);
CIMGUI_API void CImGui_ImplGlfw_Shutdown();
CIMGUI_API void CImGui_ImplGlfw_NewFrame();

// Emscripten related initialization phase methods (call after ImGui_ImplGlfw_InitForOpenGL)
#ifdef __EMSCRIPTEN__
CIMGUI_API void CImGui_ImplGlfw_InstallEmscriptenCallbacks(GLFWwindow *window, const char *canvas_selector);
// static inline void    ImGui_ImplGlfw_InstallEmscriptenCanvasResizeCallback(const char* canvas_selector) { ImGui_ImplGlfw_InstallEmscriptenCallbacks(nullptr, canvas_selector); } } // Renamed in 1.91.0
#endif

// GLFW callbacks install
// - When calling Init with 'install_callbacks=true': ImGui_ImplGlfw_InstallCallbacks() is called. GLFW callbacks will be installed for you. They will chain-call user's previously installed callbacks, if any.
// - When calling Init with 'install_callbacks=false': GLFW callbacks won't be installed. You will need to call individual function yourself from your own GLFW callbacks.
CIMGUI_API void CImGui_ImplGlfw_InstallCallbacks(GLFWwindow *window);
CIMGUI_API void CImGui_ImplGlfw_RestoreCallbacks(GLFWwindow *window);

// GFLW callbacks options:
// - Set 'chain_for_all_windows=true' to enable chaining callbacks for all windows (including secondary viewports created by backends or by user)
CIMGUI_API void CImGui_ImplGlfw_SetCallbacksChainForAllWindows(bool chain_for_all_windows);

// GLFW callbacks (individual callbacks to call yourself if you didn't install callbacks)
CIMGUI_API void CImGui_ImplGlfw_WindowFocusCallback(GLFWwindow *window, int focused);      // Since 1.84
CIMGUI_API void CImGui_ImplGlfw_CursorEnterCallback(GLFWwindow *window, int entered);      // Since 1.84
CIMGUI_API void CImGui_ImplGlfw_CursorPosCallback(GLFWwindow *window, double x, double y); // Since 1.87
CIMGUI_API void CImGui_ImplGlfw_MouseButtonCallback(GLFWwindow *window, int button, int action, int mods);
CIMGUI_API void CImGui_ImplGlfw_ScrollCallback(GLFWwindow *window, double xoffset, double yoffset);
CIMGUI_API void CImGui_ImplGlfw_KeyCallback(GLFWwindow *window, int key, int scancode, int action, int mods);
CIMGUI_API void CImGui_ImplGlfw_CharCallback(GLFWwindow *window, unsigned int c);
CIMGUI_API void CImGui_ImplGlfw_MonitorCallback(GLFWmonitor *monitor, int event);

// GLFW helpers
CIMGUI_API void CImGui_ImplGlfw_Sleep(int milliseconds);
CIMGUI_API float CImGui_ImplGlfw_GetContentScaleForWindow(GLFWwindow *window);
CIMGUI_API float CImGui_ImplGlfw_GetContentScaleForMonitor(GLFWmonitor *monitor);

#endif

#endif // CIMGUI_IMPL_GLFW_H
