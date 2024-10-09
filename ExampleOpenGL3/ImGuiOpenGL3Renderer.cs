#define IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
#define IMGUI_IMPL_OPENGL_HAS_EXTENSIONS        // has glGetIntegerv(GL_NUM_EXTENSIONS)
#define IMGUI_IMPL_OPENGL_MAY_HAVE_POLYGON_MODE // may have glPolygonMode()
#define IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_BUFFER_PIXEL_UNPACK
#define IMGUI_IMPL_OPENGL_MAY_HAVE_PRIMITIVE_RESTART
#define IMGUI_IMPL_OPENGL_MAY_HAVE_VTX_OFFSET
#define IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_SAMPLER

namespace ExampleOpenGL3
{
    using Hexa.NET.ImGui;
    using Hexa.NET.Utilities;
    using Silk.NET.OpenGL;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using ImDrawIdx = UInt16;

    public static unsafe class ImGuiOpenGL3Renderer
    {
        private static GL GL;

        private struct RenderData
        {
            public uint GlVersion;               // Extracted at runtime using GL_MAJOR_VERSION, GL_MINOR_VERSION queries (e.g. 320 for GL 3.2)
            public byte* GlslVersionString;   // Specified by user or detected based on compile time GL settings.
            public bool GlProfileIsES2;
            public bool GlProfileIsES3;
            public bool GlProfileIsCompat;
            public int GlProfileMask;
            public uint FontTexture;
            public uint ShaderHandle;
            public int AttribLocationTex;       // Uniforms location
            public int AttribLocationProjMtx;
            public uint AttribLocationVtxPos;    // Vertex attributes location
            public uint AttribLocationVtxUV;
            public uint AttribLocationVtxColor;
            public uint VboHandle, ElementsHandle;
            public nuint VertexBufferSize;
            public nuint IndexBufferSize;
            public bool HasPolygonMode;
            public bool HasClipOrigin;
            public bool UseBufferSubData;
        };

        private static RenderData* GetBackendData()
        {
            return !ImGui.GetCurrentContext().IsNull ? (RenderData*)ImGui.GetIO().BackendRendererUserData : null;
        }

        private struct VtxAttribState
        {
            public int Enabled, Size, Type, Normalized, Stride;
            public void* Ptr;

            public void GetState(uint index)
            {
                GL.GetVertexAttrib(index, GLEnum.VertexAttribArrayEnabled, out Enabled);
                GL.GetVertexAttrib(index, GLEnum.VertexAttribArraySize, out Size);
                GL.GetVertexAttrib(index, GLEnum.VertexAttribArrayType, out Type);
                GL.GetVertexAttrib(index, GLEnum.VertexAttribArrayNormalized, out Normalized);
                GL.GetVertexAttrib(index, GLEnum.VertexAttribArrayStride, out Stride);
                GL.GetVertexAttribPointer(index, GLEnum.VertexAttribArrayPointer, out Ptr);
            }

            public void SetState(uint index)
            {
                GL.VertexAttribPointer(index, Size, (GLEnum)Type, Normalized != 0, (uint)Stride, Ptr);
                if (Enabled != 0)
                {
                    GL.EnableVertexAttribArray(index);
                }
                else
                {
                    GL.DisableVertexAttribArray(index);
                }
            }
        };

        public static bool Init(GL gl, string? glsl_version_str)
        {
            GL = gl;
            ImGuiIOPtr io = ImGui.GetIO();
            Debug.Assert(io.BackendRendererUserData == null, "Already initialized a renderer backend!");

            RenderData* bd = AllocT<RenderData>();
            ZeroMemoryT(bd);
            io.BackendRendererUserData = bd;
            io.BackendRendererName = "imgui_impl_opengl3".ToUTF8Ptr();

#if IMGUI_IMPL_OPENGL_ES2
    // GLES 2
    bd->GlVersion = 200;
    bd->GlProfileIsES2 = true;
#else

            // Desktop or GLES 3
            string gl_version_str = GL.GetStringS(GLEnum.Version);
            int major = 0;
            int minor = 0;
            GL.GetInteger(GLEnum.MajorVersion, &major);
            GL.GetInteger(GLEnum.MinorVersion, &minor);
            if (major == 0 && minor == 0)
            {
                // Query GL_VERSION in desktop GL 2.X, the string will start with "<major>.<minor>"
                var parts = gl_version_str.Split('.');
                major = int.Parse(parts[0]);
                minor = int.Parse(parts[1]);
            }

            bd->GlVersion = (uint)(major * 100 + minor * 10);
#if GL_CONTEXT_PROFILE_MASK
    if (bd->GlVersion >= 320)
        GL.GetInteger(GL_CONTEXT_PROFILE_MASK, &bd->GlProfileMask);
    bd->GlProfileIsCompat = (bd->GlProfileMask & GL_CONTEXT_COMPATIBILITY_PROFILE_BIT) != 0;
#endif

#if IMGUI_IMPL_OPENGL_ES3
        bd.GlProfileIsES3 = true;
#else
            if (gl_version_str.StartsWith("OpenGL ES 3"))
            {
                bd->GlProfileIsES3 = true;
            }
#endif

            bd->UseBufferSubData = false;

#endif

#if IMGUI_IMPL_OPENGL_DEBUG
        Debug.WriteLine($"GlVersion = {bd.GlVersion}, \"{glVersionStr}\"\nGlProfileIsCompat = {bd.GlProfileIsCompat}\nGlProfileMask = 0x{bd.GlProfileMask:X}\nGlProfileIsES2 = {bd.GlProfileIsES2}\nGlProfileIsES3 = {bd.GlProfileIsES3}\nGL_VENDOR = '{Marshal.PtrToStringAnsi((IntPtr)_gl.GetString(StringName.Vendor))}'\nGL_RENDERER = '{Marshal.PtrToStringAnsi((IntPtr)_gl.GetString(StringName.Renderer))}'");
#endif

#if IMGUI_IMPL_OPENGL_MAY_HAVE_VTX_OFFSET
            if (bd->GlVersion >= 320)
            {
                io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;  // We can honor the ImDrawCmd::VtxOffset field, allowing for large meshes.
            }
#endif
            io.BackendFlags |= ImGuiBackendFlags.RendererHasViewports;  // We can create multi-viewports on the Renderer side (optional)

            // Store GLSL version string so we can refer to it later in case we recreate shaders.
            // Note: GLSL version is NOT the same as GL version. Leave this to null if unsure.
            if (string.IsNullOrEmpty(glsl_version_str))
            {
#if IMGUI_IMPL_OPENGL_ES2
            glsl_version_str = "#version 100";
#elif IMGUI_IMPL_OPENGL_ES3
            glsl_version_str = "#version 300 es";
#elif __APPLE__
            glsl_version_str = "#version 150";
#else
                glsl_version_str = "#version 130";
#endif
            }
            bd->GlslVersionString = (glsl_version_str + "\n").ToUTF8Ptr();

            // Make an arbitrary GL call (we don't actually need the result)
            // IF YOU GET A CRASH HERE: it probably means the OpenGL function loader didn't do its job. Let us know!
            GL.GetInteger(GLEnum.TextureBinding2D, out int currentTexture);

            // Detect extensions we support
#if IMGUI_IMPL_OPENGL_MAY_HAVE_POLYGON_MODE
            bd->HasPolygonMode = !bd->GlProfileIsES2 && !bd->GlProfileIsES3;
#endif
            bd->HasClipOrigin = bd->GlVersion >= 450;
#if IMGUI_IMPL_OPENGL_HAS_EXTENSIONS
            GL.GetInteger(GLEnum.NumExtensions, out int numExtensions);
            for (int i = 0; i < numExtensions; i++)
            {
                string extension = GL.GetStringS(StringName.Extensions, (uint)i);
                if (extension != null && extension.Equals("GL_ARB_clip_control", StringComparison.OrdinalIgnoreCase))
                {
                    bd->HasClipOrigin = true;
                }
            }
#endif

            CreateDeviceObjects();

            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
            {
                InitPlatformInterface();
            }

            return true;
        }

        public static void Shutdown()
        {
            RenderData* bd = GetBackendData();
            Debug.Assert(bd != null, "No renderer backend to shutdown, or already shutdown?");
            ImGuiIOPtr io = ImGui.GetIO();

            ShutdownPlatformInterface();
            DestroyDeviceObjects();
            Free(io.BackendRendererName);
            io.BackendRendererName = null;
            io.BackendRendererUserData = null;
            io.BackendFlags &= ~(ImGuiBackendFlags.RendererHasVtxOffset | ImGuiBackendFlags.RendererHasViewports);

            Free(bd);
        }

        public static void NewFrame()
        {
            RenderData* bd = GetBackendData();
            Debug.Assert(bd != null, "Context or backend not initialized! Did you call ImGui_ImplOpenGL3_Init()?");

            if (bd->ShaderHandle == 0)
            {
                CreateDeviceObjects();
            }

            if (bd->FontTexture == 0)
            {
                CreateFontsTexture();
            }
        }

        private static void SetupRenderState(ImDrawData* draw_data, int fb_width, int fb_height, uint vertex_array_object)
        {
            RenderData* bd = GetBackendData();

            // Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, polygon fill
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(GLEnum.FuncAdd);
            GL.BlendFuncSeparate(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha, BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.StencilTest);
            GL.Enable(EnableCap.ScissorTest);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_PRIMITIVE_RESTART
            if (bd->GlVersion >= 310)
            {
                GL.Disable(EnableCap.PrimitiveRestart);
            }
#endif
#if IMGUI_IMPL_OPENGL_MAY_HAVE_POLYGON_MODE
            if (bd->HasPolygonMode)
            {
                GL.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);
            }
#endif

            // Support for GL 4.5 rarely used glClipControl(GL_UPPER_LEFT)
#if GL_CLIP_ORIGIN
    bool clip_origin_lower_left = true;
    if (bd->HasClipOrigin)
    {
        GLenum current_clip_origin = 0; GL.GetInteger(GL_CLIP_ORIGIN, (int*)&current_clip_origin);
        if (current_clip_origin == GL_UPPER_LEFT)
            clip_origin_lower_left = false;
    }
#endif

            // Setup viewport, orthographic projection matrix
            // Our visible imgui space lies from draw_data->DisplayPos (top left) to draw_data->DisplayPos+data_data->DisplaySize (bottom right). DisplayPos is (0,0) for single viewport apps.
            /*GL_CALL*/
            GL.Viewport(0, 0, (uint)fb_width, (uint)fb_height);
            float L = draw_data->DisplayPos.X;
            float R = draw_data->DisplayPos.X + draw_data->DisplaySize.X;
            float T = draw_data->DisplayPos.Y;
            float B = draw_data->DisplayPos.Y + draw_data->DisplaySize.Y;
#if GL_CLIP_ORIGIN
    if (!clip_origin_lower_left) { float tmp = T; T = B; B = tmp; } // Swap top and bottom if origin is upper left
#endif
            Matrix4x4 mvp = new
            (
                2.0f / (R - L), 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / (T - B), 0.0f, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f,
                (R + L) / (L - R), (T + B) / (B - T), 0.0f, 1.0f
            );
            GL.UseProgram(bd->ShaderHandle);
            GL.Uniform1(bd->AttribLocationTex, 0);
            GL.UniformMatrix4(bd->AttribLocationProjMtx, 1, false, (float*)&mvp);

#if IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_SAMPLER
            if (bd->GlVersion >= 330 || bd->GlProfileIsES3)
            {
                GL.BindSampler(0, 0); // We use combined texture/sampler state. Applications using GL 3.3 and GL ES 3.0 may set that otherwise.
            }
#endif

#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            GL.BindVertexArray(vertex_array_object);
#endif

            // Bind vertex/index buffers and setup attributes for ImDrawVert
            /*GL_CALL*/
            GL.BindBuffer(GLEnum.ArrayBuffer, bd->VboHandle);
            /*GL_CALL*/
            GL.BindBuffer(GLEnum.ElementArrayBuffer, bd->ElementsHandle);
            /*GL_CALL*/
            GL.EnableVertexAttribArray(bd->AttribLocationVtxPos);
            /*GL_CALL*/
            GL.EnableVertexAttribArray(bd->AttribLocationVtxUV);
            /*GL_CALL*/
            GL.EnableVertexAttribArray(bd->AttribLocationVtxColor);
            /*GL_CALL*/
            GL.VertexAttribPointer(bd->AttribLocationVtxPos, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)0);
            /*GL_CALL*/
            GL.VertexAttribPointer(bd->AttribLocationVtxUV, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)8);
            /*GL_CALL*/
            GL.VertexAttribPointer(bd->AttribLocationVtxColor, 4, GLEnum.UnsignedByte, true, (uint)sizeof(ImDrawVert), (void*)16);
        }

        // OpenGL3 Render function.
        // Note that this implementation is little overcomplicated because we are saving/setting up/restoring every OpenGL state explicitly.
        // This is in order to be able to run within an OpenGL engine that doesn't do so.
        public static void RenderDrawData(ImDrawData* draw_data)
        {
            // Avoid rendering when minimized, scale coordinates for retina displays (screen coordinates != framebuffer coordinates)
            int fb_width = (int)(draw_data->DisplaySize.X * draw_data->FramebufferScale.X);
            int fb_height = (int)(draw_data->DisplaySize.Y * draw_data->FramebufferScale.Y);
            if (fb_width <= 0 || fb_height <= 0)
            {
                return;
            }

            RenderData* bd = GetBackendData();

            // Backup GL state
            GLEnum last_active_texture; GL.GetInteger(GLEnum.ActiveTexture, (int*)&last_active_texture);
            GL.ActiveTexture(GLEnum.Texture0);
            uint last_program; GL.GetInteger(GLEnum.CurrentProgram, (int*)&last_program);
            uint last_texture; GL.GetInteger(GLEnum.TextureBinding2D, (int*)&last_texture);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_SAMPLER
            uint last_sampler; if (bd->GlVersion >= 330 || bd->GlProfileIsES3) { GL.GetInteger(GLEnum.SamplerBinding, (int*)&last_sampler); } else { last_sampler = 0; }
#endif
            uint last_array_buffer; GL.GetInteger(GLEnum.ArrayBufferBinding, (int*)&last_array_buffer);
#if !IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            // This is part of VAO on OpenGL 3.0+ and OpenGL ES 3.0+.
            int last_element_array_buffer; GL.GetInteger(GLEnum.ElementArrayBufferBinding, &last_element_array_buffer);
            //ImGui_ImplOpenGL3_VtxAttribState last_vtx_attrib_state_pos = default; last_vtx_attrib_state_pos.GetState(bd->AttribLocationVtxPos);
            //ImGui_ImplOpenGL3_VtxAttribState last_vtx_attrib_state_uv = default; last_vtx_attrib_state_uv.GetState(bd->AttribLocationVtxUV);
            //ImGui_ImplOpenGL3_VtxAttribState last_vtx_attrib_state_color = default; last_vtx_attrib_state_color.GetState(bd->AttribLocationVtxColor);
#endif
#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            uint last_vertex_array_object; GL.GetInteger(GLEnum.VertexArrayBinding, (int*)&last_vertex_array_object);
#endif
#if IMGUI_IMPL_OPENGL_MAY_HAVE_POLYGON_MODE
            int* last_polygon_mode = stackalloc int[2]; if (bd->HasPolygonMode) { GL.GetInteger(GLEnum.PolygonMode, last_polygon_mode); }
#endif
            int* last_viewport = stackalloc int[4]; GL.GetInteger(GLEnum.Viewport, last_viewport);
            int* last_scissor_box = stackalloc int[4]; GL.GetInteger(GLEnum.ScissorBox, last_scissor_box);
            GLEnum last_blend_src_rgb; GL.GetInteger(GLEnum.BlendSrcRgb, (int*)&last_blend_src_rgb);
            GLEnum last_blend_dst_rgb; GL.GetInteger(GLEnum.BlendDstRgb, (int*)&last_blend_dst_rgb);
            GLEnum last_blend_src_alpha; GL.GetInteger(GLEnum.BlendSrcAlpha, (int*)&last_blend_src_alpha);
            GLEnum last_blend_dst_alpha; GL.GetInteger(GLEnum.BlendDstAlpha, (int*)&last_blend_dst_alpha);
            GLEnum last_blend_equation_rgb; GL.GetInteger(GLEnum.BlendEquationRgb, (int*)&last_blend_equation_rgb);
            GLEnum last_blend_equation_alpha; GL.GetInteger(GLEnum.BlendEquationAlpha, (int*)&last_blend_equation_alpha);
            bool last_enable_blend = GL.IsEnabled(EnableCap.Blend);
            bool last_enable_cull_face = GL.IsEnabled(GLEnum.CullFace);
            bool last_enable_depth_test = GL.IsEnabled(GLEnum.DepthTest);
            bool last_enable_stencil_test = GL.IsEnabled(GLEnum.StencilTest);
            bool last_enable_scissor_test = GL.IsEnabled(GLEnum.ScissorTest);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_PRIMITIVE_RESTART
            bool last_enable_primitive_restart = bd->GlVersion >= 310 ? GL.IsEnabled(GLEnum.PrimitiveRestart) : false;
#endif

            // Setup desired GL state
            // Recreate the VAO every time (this is to easily allow multiple GL contexts to be rendered to. VAO are not shared among GL contexts)
            // The renderer would actually work without any VAO bound, but then our VertexAttrib calls would overwrite the default one currently bound.
            uint vertex_array_object = 0;
#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            /*GL_CALL*/
            GL.GenVertexArrays(1, &vertex_array_object);
#endif
            SetupRenderState(draw_data, fb_width, fb_height, vertex_array_object);

            // Will project scissor/clipping rectangles into framebuffer space
            Vector2 clip_off = draw_data->DisplayPos;         // (0,0) unless using multi-viewports
            Vector2 clip_scale = draw_data->FramebufferScale; // (1,1) unless using retina display which are often (2,2)

            // Render command lists
            for (int n = 0; n < draw_data->CmdListsCount; n++)
            {
                ImDrawList* cmd_list = draw_data->CmdLists.Data[n];

                // Upload vertex/index buffers
                // - OpenGL drivers are in a very sorry state nowadays....
                //   During 2021 we attempted to switch from glBufferData() to orphaning+glBufferSubData() following reports
                //   of leaks on Intel GPU when using multi-viewports on Windows.
                // - After this we kept hearing of various display corruptions issues. We started disabling on non-Intel GPU, but issues still got reported on Intel.
                // - We are now back to using exclusively glBufferData(). So bd->UseBufferSubData IS ALWAYS FALSE in this code.
                //   We are keeping the old code path for a while in case people finding new issues may want to test the bd->UseBufferSubData path.
                // - See https://github.com/ocornut/imgui/issues/4468 and please report any corruption issues.
                nuint vtx_buffer_size = (nuint)cmd_list->VtxBuffer.Size * (nuint)sizeof(ImDrawVert);
                nuint idx_buffer_size = (nuint)cmd_list->IdxBuffer.Size * sizeof(ImDrawIdx);
                if (bd->UseBufferSubData)
                {
                    if (bd->VertexBufferSize < vtx_buffer_size)
                    {
                        bd->VertexBufferSize = vtx_buffer_size;
                        /*GL_CALL*/
                        GL.BufferData(GLEnum.ArrayBuffer, bd->VertexBufferSize, null, GLEnum.StreamDraw);
                    }
                    if (bd->IndexBufferSize < idx_buffer_size)
                    {
                        bd->IndexBufferSize = idx_buffer_size;
                        /*GL_CALL*/
                        GL.BufferData(GLEnum.ElementArrayBuffer, bd->IndexBufferSize, null, GLEnum.StreamDraw);
                    }
                    /*GL_CALL*/
                    GL.BufferSubData(GLEnum.ArrayBuffer, 0, vtx_buffer_size, cmd_list->VtxBuffer.Data);
                    /*GL_CALL*/
                    GL.BufferSubData(GLEnum.ElementArrayBuffer, 0, idx_buffer_size, cmd_list->IdxBuffer.Data);
                }
                else
                {
                    /*GL_CALL*/
                    GL.BufferData(GLEnum.ArrayBuffer, vtx_buffer_size, cmd_list->VtxBuffer.Data, GLEnum.StreamDraw);

                    /*GL_CALL*/
                    GL.BufferData(GLEnum.ElementArrayBuffer, idx_buffer_size, cmd_list->IdxBuffer.Data, GLEnum.StreamDraw);
                }

                for (int cmd_i = 0; cmd_i < cmd_list->CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmd pcmd = cmd_list->CmdBuffer[cmd_i];
                    if (pcmd.UserCallback != null)
                    {
                        // User callback, registered via ImDrawList::AddCallback()
                        // (ImDrawCallback_ResetRenderState is a special callback value used by the user to request the renderer to reset render state.)
                        if ((nint)pcmd.UserCallback == ImGui.ImDrawCallbackResetRenderState)
                        {
                            SetupRenderState(draw_data, fb_width, fb_height, vertex_array_object);
                        }
                        else
                        {
                            ((delegate*<ImDrawList*, ImDrawCmd*, void>)pcmd.UserCallback)(cmd_list, &pcmd);
                        }
                    }
                    else
                    {
                        // Project scissor/clipping rectangles into framebuffer space
                        Vector2 clip_min = new((pcmd.ClipRect.X - clip_off.X) * clip_scale.X, (pcmd.ClipRect.Y - clip_off.Y) * clip_scale.Y);
                        Vector2 clip_max = new((pcmd.ClipRect.Z - clip_off.X) * clip_scale.X, (pcmd.ClipRect.W - clip_off.Y) * clip_scale.Y);
                        if (clip_max.X <= clip_min.X || clip_max.Y <= clip_min.Y)
                        {
                            continue;
                        }

                        // Apply scissor/clipping rectangle (Y is inverted in OpenGL)
                        /*GL_CALL*/
                        GL.Scissor((int)clip_min.X, (int)(fb_height - clip_max.Y), (uint)(clip_max.X - clip_min.X), (uint)(clip_max.Y - clip_min.Y));

                        // Bind texture, Draw
                        /*GL_CALL*/
                        GL.BindTexture(GLEnum.Texture2D, (uint)pcmd.GetTexID().Handle);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_VTX_OFFSET
                        if (bd->GlVersion >= 320)
                        {
                            /*GL_CALL*/
                            GL.DrawElementsBaseVertex(GLEnum.Triangles, pcmd.ElemCount, sizeof(ImDrawIdx) == 2 ? GLEnum.UnsignedShort : GLEnum.UnsignedInt, (void*)(nint)(pcmd.IdxOffset * sizeof(ImDrawIdx)), (int)pcmd.VtxOffset);
                        }
                        else
#endif
                            /*GL_CALL*/
                            GL.DrawElements(GLEnum.Triangles, pcmd.ElemCount, sizeof(ImDrawIdx) == 2 ? GLEnum.UnsignedShort : GLEnum.UnsignedInt, (void*)(nint)(pcmd.IdxOffset * sizeof(ImDrawIdx)));
                    }
                }
            }

            // Destroy the temporary VAO
#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            /*GL_CALL*/
            GL.DeleteVertexArrays(1, &vertex_array_object);
#endif

            // Restore modified GL state
            // This "glIsProgram()" check is required because if the program is "pending deletion" at the time of binding backup, it will have been deleted by now and will cause an OpenGL error. See #6220.
            if (last_program == 0 || GL.IsProgram(last_program))
            {
                GL.UseProgram(last_program);
            }

            GL.BindTexture(GLEnum.Texture2D, last_texture);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_SAMPLER
            if (bd->GlVersion >= 330 || bd->GlProfileIsES3)
            {
                GL.BindSampler(0, last_sampler);
            }
#endif
            GL.ActiveTexture(last_active_texture);
#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            GL.BindVertexArray(last_vertex_array_object);
#endif
            GL.BindBuffer(GLEnum.ArrayBuffer, last_array_buffer);
#if !IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            GL.BindBuffer(GLEnum.ElementArrayBuffer, (uint)last_element_array_buffer);
            //last_vtx_attrib_state_pos.SetState(bd->AttribLocationVtxPos);
            //last_vtx_attrib_state_uv.SetState(bd->AttribLocationVtxUV);
            //last_vtx_attrib_state_color.SetState(bd->AttribLocationVtxColor);
#endif
            GL.BlendEquationSeparate(last_blend_equation_rgb, last_blend_equation_alpha);
            GL.BlendFuncSeparate(last_blend_src_rgb, last_blend_dst_rgb, last_blend_src_alpha, last_blend_dst_alpha);
            if (last_enable_blend)
            {
                GL.Enable(EnableCap.Blend);
            }
            else
            {
                GL.Disable(EnableCap.Blend);
            }

            if (last_enable_cull_face)
            {
                GL.Enable(GLEnum.CullFace);
            }
            else
            {
                GL.Disable(GLEnum.CullFace);
            }

            if (last_enable_depth_test)
            {
                GL.Enable(GLEnum.DepthTest);
            }
            else
            {
                GL.Disable(GLEnum.DepthTest);
            }

            if (last_enable_stencil_test)
            {
                GL.Enable(GLEnum.StencilTest);
            }
            else
            {
                GL.Disable(GLEnum.StencilTest);
            }

            if (last_enable_scissor_test)
            {
                GL.Enable(GLEnum.ScissorTest);
            }
            else
            {
                GL.Disable(GLEnum.ScissorTest);
            }
#if IMGUI_IMPL_OPENGL_MAY_HAVE_PRIMITIVE_RESTART
            if (bd->GlVersion >= 310)
            {
                if (last_enable_primitive_restart)
                {
                    GL.Enable(GLEnum.PrimitiveRestart);
                }
                else
                {
                    GL.Disable(GLEnum.PrimitiveRestart);
                }
            }
#endif

#if IMGUI_IMPL_OPENGL_MAY_HAVE_POLYGON_MODE
            // Desktop OpenGL 3.0 and OpenGL 3.1 had separate polygon draw modes for front-facing and back-facing faces of polygons
            if (bd->HasPolygonMode) { if (bd->GlVersion <= 310 || bd->GlProfileIsCompat) { GL.PolygonMode(GLEnum.Front, (GLEnum)last_polygon_mode[0]); GL.PolygonMode(GLEnum.Back, (GLEnum)last_polygon_mode[1]); } else { GL.PolygonMode(GLEnum.FrontAndBack, (GLEnum)last_polygon_mode[0]); } }
#endif // IMGUI_IMPL_OPENGL_MAY_HAVE_POLYGON_MODE

            GL.Viewport(last_viewport[0], last_viewport[1], (uint)last_viewport[2], (uint)last_viewport[3]);
            GL.Scissor(last_scissor_box[0], last_scissor_box[1], (uint)last_scissor_box[2], (uint)last_scissor_box[3]);
        }

        private static bool CreateFontsTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            RenderData* bd = GetBackendData();

            // Build texture atlas
            byte* pixels;
            int width, height;
            io.Fonts.GetTexDataAsRGBA32(&pixels, &width, &height);   // Load as RGBA 32-bit (75% of the memory is wasted, but default font is so small) because it is more likely to be compatible with user's existing shaders. If your ImTextureId represent a higher-level concept than just a GL texture id, consider calling GetTexDataAsAlpha8() instead to save on GPU memory.

            // Upload texture to graphics system
            // (Bilinear sampling is required by default. Set 'io.Fonts->Flags |= ImFontAtlasFlags_NoBakedLines' or 'style.AntiAliasedLinesUseTex = false' to allow point/nearest sampling)
            uint last_texture;
            /*GL_CALL*/
            GL.GetInteger(GLEnum.TextureBinding2D, (int*)&last_texture);
            /*GL_CALL*/
            GL.GenTextures(1, &bd->FontTexture);
            /*GL_CALL*/
            GL.BindTexture(GLEnum.Texture2D, bd->FontTexture);
            /*GL_CALL*/
            GL.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Linear);
            /*GL_CALL*/
            GL.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
#if GL_UNPACK_ROW_LENGTH // Not on WebGL/ES
            /*GL_CALL*/(GL.PixelStore(GLEnum.UnpackRowLength, 0));
#endif
            /*GL_CALL*/
            GL.TexImage2D(GLEnum.Texture2D, 0, (int)GLEnum.Rgba, (uint)width, (uint)height, 0, GLEnum.Rgba, GLEnum.UnsignedByte, pixels);

            // Store our identifier
            io.Fonts.SetTexID((ImTextureID)(nint)bd->FontTexture);

            // Restore state
            /*GL_CALL*/
            GL.BindTexture(GLEnum.Texture2D, last_texture);

            return true;
        }

        private static void DestroyFontsTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            RenderData* bd = GetBackendData();
            if (bd->FontTexture != 0)
            {
                GL.DeleteTextures(1, &bd->FontTexture);
                io.Fonts.SetTexID(0);
                bd->FontTexture = 0;
            }
        }

        // If you get an error please report on github. You may try different GL context version or GLSL version. See GL<>GLSL version table at the top of this file.
        private static bool CheckShader(uint handle, string desc)
        {
            RenderData* bd = GetBackendData();
            int status = 0, log_length = 0;
            GL.GetShader(handle, GLEnum.CompileStatus, &status);
            GL.GetShader(handle, GLEnum.InfoLogLength, &log_length);
            if (status == 0)
            {
                Debug.WriteLine($"ERROR: ImGui_ImplOpenGL3_CreateDeviceObjects: failed to compile {desc}! With GLSL: {ToStringFromUTF8(bd->GlslVersionString)}");
            }

            if (log_length > 1)
            {
                UnsafeList<byte> buf = default;
                buf.Resize((uint)log_length + 1);
                GL.GetShaderInfoLog(handle, (uint)log_length, null, buf.Data);
                Debug.WriteLine(ToStringFromUTF8(buf.Data));
                buf.Release();
            }
            return status != 0;
        }

        // If you get an error please report on GitHub. You may try different GL context version or GLSL version.
        private static bool CheckProgram(uint handle, string desc)
        {
            RenderData* bd = GetBackendData();
            int status = 0, log_length = 0;
            GL.GetProgram(handle, GLEnum.LinkStatus, &status);
            GL.GetProgram(handle, GLEnum.InfoLogLength, &log_length);
            if (status == 0)
            {
                Debug.WriteLine($"ERROR: ImGui_ImplOpenGL3_CreateDeviceObjects: failed to link {desc}! With GLSL {ToStringFromUTF8(bd->GlslVersionString)}");
            }

            if (log_length > 1)
            {
                UnsafeList<byte> buf = default;
                buf.Resize((uint)log_length + 1);
                GL.GetShaderInfoLog(handle, (uint)log_length, null, buf.Data);
                Debug.WriteLine(ToStringFromUTF8(buf.Data));
                buf.Release();
            }
            return status != 0;
        }

        private static bool CreateDeviceObjects()
        {
            RenderData* bd = GetBackendData();

            // Backup GL state
            int last_texture, last_array_buffer;
            GL.GetInteger(GLEnum.TextureBinding2D, &last_texture);
            GL.GetInteger(GLEnum.ArrayBufferBinding, &last_array_buffer);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_BUFFER_PIXEL_UNPACK
            int last_pixel_unpack_buffer = 0;
            if (bd->GlVersion >= 210) { GL.GetInteger(GLEnum.PixelUnpackBufferBinding, &last_pixel_unpack_buffer); GL.BindBuffer(GLEnum.PixelUnpackBuffer, 0); }
#endif
#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            int last_vertex_array;
            GL.GetInteger(GLEnum.VertexArrayBinding, &last_vertex_array);
#endif

            // Parse GLSL version string
            int glsl_version = 130;

            if (bd->GlslVersionString != null && int.TryParse(ToStringFromUTF8(bd->GlslVersionString + 9), out var version))
            {
                glsl_version = version;
            }

            const string vertex_shader_glsl_120 = @"
    uniform mat4 ProjMtx;
    attribute vec2 Position;
    attribute vec2 UV;
    attribute vec4 Color;
    varying vec2 Frag_UV;
    varying vec4 Frag_Color;
    void main()
    {
        Frag_UV = UV;
        Frag_Color = Color;
        gl_Position = ProjMtx * vec4(Position.xy, 0, 1);
    }
";

            const string vertex_shader_glsl_130 = @"
    uniform mat4 ProjMtx;
    in vec2 Position;
    in vec2 UV;
    in vec4 Color;
    out vec2 Frag_UV;
    out vec4 Frag_Color;
    void main()
    {
        Frag_UV = UV;
        Frag_Color = Color;
        gl_Position = ProjMtx * vec4(Position.xy, 0, 1);
    }
";

            const string vertex_shader_glsl_300_es = @"
    precision highp float;
    layout (location = 0) in vec2 Position;
    layout (location = 1) in vec2 UV;
    layout (location = 2) in vec4 Color;
    uniform mat4 ProjMtx;
    out vec2 Frag_UV;
    out vec4 Frag_Color;
    void main()
    {
        Frag_UV = UV;
        Frag_Color = Color;
        gl_Position = ProjMtx * vec4(Position.xy, 0, 1);
    }
";

            const string vertex_shader_glsl_410_core = @"
    layout (location = 0) in vec2 Position;
    layout (location = 1) in vec2 UV;
    layout (location = 2) in vec4 Color;
    uniform mat4 ProjMtx;
    out vec2 Frag_UV;
    out vec4 Frag_Color;
    void main()
    {
        Frag_UV = UV;
        Frag_Color = Color;
        gl_Position = ProjMtx * vec4(Position.xy, 0, 1);
    }
";

            const string fragment_shader_glsl_120 = @"
    #ifdef GL_ES
        precision mediump float;
    #endif
    uniform sampler2D Texture;
    varying vec2 Frag_UV;
    varying vec4 Frag_Color;
    void main()
    {
        gl_FragColor = Frag_Color * texture2D(Texture, Frag_UV.st);
    }
";

            const string fragment_shader_glsl_130 = @"
    uniform sampler2D Texture;
    in vec2 Frag_UV;
    in vec4 Frag_Color;
    out vec4 Out_Color;
    void main()
    {
        Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
    }
";

            const string fragment_shader_glsl_300_es = @"
    precision mediump float;
    uniform sampler2D Texture;
    in vec2 Frag_UV;
    in vec4 Frag_Color;
    layout (location = 0) out vec4 Out_Color;
    void main()
    {
        Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
    }
";

            const string fragment_shader_glsl_410_core = @"
    in vec2 Frag_UV;
    in vec4 Frag_Color;
    uniform sampler2D Texture;
    layout (location = 0) out vec4 Out_Color;
    void main()
    {
        Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
    }
";

            // Select shaders matching our GLSL versions
            string? vertex_shader = null;
            string? fragment_shader = null;
            if (glsl_version < 130)
            {
                vertex_shader = vertex_shader_glsl_120;
                fragment_shader = fragment_shader_glsl_120;
            }
            else if (glsl_version >= 410)
            {
                vertex_shader = vertex_shader_glsl_410_core;
                fragment_shader = fragment_shader_glsl_410_core;
            }
            else if (glsl_version == 300)
            {
                vertex_shader = vertex_shader_glsl_300_es;
                fragment_shader = fragment_shader_glsl_300_es;
            }
            else
            {
                vertex_shader = vertex_shader_glsl_130;
                fragment_shader = fragment_shader_glsl_130;
            }

            // Create shaders
            string vertex_shader_with_version = $"{ToStringFromUTF8(bd->GlslVersionString)}\n{vertex_shader}";
            uint vert_handle = GL.CreateShader(GLEnum.VertexShader);
            GL.ShaderSource(vert_handle, vertex_shader_with_version);
            GL.CompileShader(vert_handle);
            CheckShader(vert_handle, "vertex shader");

            string fragment_shader_with_version = $"{ToStringFromUTF8(bd->GlslVersionString)}\n{fragment_shader}";
            uint frag_handle = GL.CreateShader(GLEnum.FragmentShader);
            GL.ShaderSource(frag_handle, fragment_shader_with_version);
            GL.CompileShader(frag_handle);
            CheckShader(frag_handle, "fragment shader");

            // Link
            bd->ShaderHandle = GL.CreateProgram();
            GL.AttachShader(bd->ShaderHandle, vert_handle);
            GL.AttachShader(bd->ShaderHandle, frag_handle);
            GL.LinkProgram(bd->ShaderHandle);
            CheckProgram(bd->ShaderHandle, "shader program");

            GL.DetachShader(bd->ShaderHandle, vert_handle);
            GL.DetachShader(bd->ShaderHandle, frag_handle);
            GL.DeleteShader(vert_handle);
            GL.DeleteShader(frag_handle);

            bd->AttribLocationTex = GL.GetUniformLocation(bd->ShaderHandle, "Texture");
            bd->AttribLocationProjMtx = GL.GetUniformLocation(bd->ShaderHandle, "ProjMtx");
            bd->AttribLocationVtxPos = (uint)GL.GetAttribLocation(bd->ShaderHandle, "Position");
            bd->AttribLocationVtxUV = (uint)GL.GetAttribLocation(bd->ShaderHandle, "UV");
            bd->AttribLocationVtxColor = (uint)GL.GetAttribLocation(bd->ShaderHandle, "Color");

            // Create buffers
            GL.GenBuffers(1, &bd->VboHandle);
            GL.GenBuffers(1, &bd->ElementsHandle);

            CreateFontsTexture();

            // Restore modified GL state
            GL.BindTexture(GLEnum.Texture2D, (uint)last_texture);
            GL.BindBuffer(GLEnum.ArrayBuffer, (uint)last_array_buffer);
#if IMGUI_IMPL_OPENGL_MAY_HAVE_BIND_BUFFER_PIXEL_UNPACK
            if (bd->GlVersion >= 210) { GL.BindBuffer(GLEnum.PixelUnpackBuffer, (uint)last_pixel_unpack_buffer); }
#endif
#if IMGUI_IMPL_OPENGL_USE_VERTEX_ARRAY
            GL.BindVertexArray((uint)last_vertex_array);
#endif

            return true;
        }

        private static void DestroyDeviceObjects()
        {
            RenderData* bd = GetBackendData();
            if (bd->VboHandle != 0) { GL.DeleteBuffers(1, &bd->VboHandle); bd->VboHandle = 0; }
            if (bd->ElementsHandle != 0) { GL.DeleteBuffers(1, &bd->ElementsHandle); bd->ElementsHandle = 0; }
            if (bd->ShaderHandle != 0) { GL.DeleteProgram(bd->ShaderHandle); bd->ShaderHandle = 0; }
            DestroyFontsTexture();
        }

        //--------------------------------------------------------------------------------------------------------
        // MULTI-VIEWPORT / PLATFORM INTERFACE SUPPORT
        // This is an _advanced_ and _optional_ feature, allowing the backend to create and handle multiple viewports simultaneously.
        // If you are new to dear imgui or creating a new binding for dear imgui, it is recommended that you completely ignore this section first..
        //--------------------------------------------------------------------------------------------------------

        private static void RenderWindow(ImGuiViewport* viewport, void* v)
        {
            if ((viewport->Flags & ImGuiViewportFlags.NoRendererClear) == 0)
            {
                Vector4 clear_color = new(0.0f, 0.0f, 0.0f, 1.0f);
                GL.ClearColor(clear_color.X, clear_color.Y, clear_color.Z, clear_color.W);
                GL.Clear(ClearBufferMask.ColorBufferBit);
            }
            RenderDrawData(viewport->DrawData);
        }

        private static void InitPlatformInterface()
        {
            ImGuiPlatformIOPtr platform_io = ImGui.GetPlatformIO();
            platform_io.RendererRenderWindow = (void*)Marshal.GetFunctionPointerForDelegate<RendererRenderWindow>(RenderWindow);
        }

        private static void ShutdownPlatformInterface()
        {
            ImGui.DestroyPlatformWindows();
        }

        //-----------------------------------------------------------------------------
    }
}