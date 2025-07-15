#ifndef CIMGUI_IMPL_METAL_H
#define CIMGUI_IMPL_METAL_H

#include "cimgui_config.h"
#if CIMGUI_USE_METAL
#ifdef CIMGUI_DEFINE_ENUMS_AND_STRUCTS

#else
#include "backends/imgui_impl_metal.h"
#import <AppKit/AppKit.h>
#import <Metal/Metal.h>
#import <QuartzCore/QuartzCore.h>
#endif

#ifdef __OBJC__
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplMetal_Init(void* device);
CIMGUI_API void CImGui_ImplMetal_Shutdown();
CIMGUI_API void CImGui_ImplMetal_NewFrame(void* renderPassDescriptor);
CIMGUI_API void CImGui_ImplMetal_RenderDrawData(ImDrawData* draw_data, void* commandBuffer, void* commandEncoder);
CIMGUI_API void CImGui_ImplMetal_UpdateTexture(ImTextureData* tex);
CIMGUI_API bool CImGui_ImplMetal_CreateDeviceObjects(void* device);
CIMGUI_API void CImGui_ImplMetal_DestroyDeviceObjects();
#endif
#ifdef CIMGUI_IMPL_METAL_CPP
#ifndef __OBJC__
typedef struct MTLDevice MTLDevice;
typedef struct MTLRenderPassDescriptor MTLRenderPassDescriptor;
typedef struct MTLCommandBuffer MTLCommandBuffer;
typedef struct MTLRenderCommandEncoder MTLRenderCommandEncoder;
// Follow "Getting Started" link and check examples/ folder to learn about using backends!
CIMGUI_API bool CImGui_ImplMetal_Init(MTLDevice* device);
CIMGUI_API void CImGui_ImplMetal_Shutdown();
CIMGUI_API void CImGui_ImplMetal_NewFrame(MTLRenderPassDescriptor* renderPassDescriptor);
CIMGUI_API void CImGui_ImplMetal_RenderDrawData(ImDrawData* draw_data, MTLCommandBuffer* commandBuffer, MTLRenderCommandEncoder* commandEncoder);
CIMGUI_API void CImGui_ImplMetal_UpdateTexture(ImTextureData* tex);
CIMGUI_API bool CImGui_ImplMetal_CreateDeviceObjects(MTLDevice* device);
CIMGUI_API void CImGui_ImplMetal_DestroyDeviceObjects();
#endif
#endif
#endif

#endif // CIMGUI_IMPL_METAL_H
