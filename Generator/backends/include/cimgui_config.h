#ifndef CIMGUI_CONFIG_H
#define CIMGUI_CONFIG_H

#include <stdio.h>
#include <stdint.h>
#if defined _WIN32 || defined __CYGWIN__
#ifdef CIMGUI_NO_EXPORT
#define API
#else
#define API __declspec(dllexport)
#endif
#else
#ifdef __GNUC__
#define API __attribute__((__visibility__("default")))
#else
#define API
#endif
#endif

#if defined __cplusplus
#define EXTERN extern "C"
#else
#include <stdarg.h>
#include <stdbool.h>
#define EXTERN extern
#endif

#define CIMGUI_API EXTERN API
#define CONST const

#ifdef _MSC_VER
typedef unsigned __int64 ImU64;
#else
// typedef unsigned long long ImU64;
#endif

#ifdef __APPLE__
#define TARGET_OS_MAC 1
#endif // __APPLE__

#ifndef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
#include "imgui.h"
#ifdef CIMGUI_FREETYPE
#include "misc/freetype/imgui_freetype.h"
#endif
#include "imgui_internal.h"
#endif

#endif