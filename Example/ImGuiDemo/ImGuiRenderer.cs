//based on https://github.com/ocornut/imgui/blob/master/examples/imgui_impl_dx11.cpp
#nullable disable

namespace Example.ImGuiDemo
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Unsafes;
    using HexaEngine.ImGuiNET;
    using HexaEngine.Mathematics;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using ImDrawIdx = UInt16;

    public static class ImGuiRenderer
    {
        private static IGraphicsDevice device;
        private static IGraphicsContext context;
        private static IGraphicsPipeline pipeline;
        private static IBuffer vertexBuffer;
        private static IBuffer indexBuffer;
        private static IBuffer constantBuffer;
        private static ISamplerState fontSampler;
        private static IShaderResourceView fontTextureView;
        private static int vertexBufferSize = 5000, indexBufferSize = 10000;

        /// <summary>
        /// Renderer data
        /// </summary>
        private struct RendererData
        {
            public int Dummy;
        }

        // Backend data stored in io.BackendRendererUserData to allow support for multiple Dear ImGui contexts
        // It is STRONGLY preferred that you use docking branch with multi-viewports (== single Dear ImGui context + multiple windows) instead of multiple Dear ImGui contexts.
        private static unsafe RendererData* GetBackendData()
        {
            return !ImGui.GetCurrentContext().IsNull ? (RendererData*)ImGui.GetIO().BackendRendererUserData : null;
        }

        private static unsafe void SetupRenderState(ImDrawData* drawData, IGraphicsContext ctx)
        {
            var viewport = new Viewport(drawData->DisplaySize.X, drawData->DisplaySize.Y);

            uint stride = (uint)sizeof(ImDrawVert);
            uint offset = 0;
            ctx.SetGraphicsPipeline(pipeline);
            ctx.SetViewport(viewport);
            ctx.SetVertexBuffer(vertexBuffer, stride, offset);
            ctx.SetIndexBuffer(indexBuffer, sizeof(ushort) == 2 ? Format.R16UInt : Format.R32UInt, 0);
            ctx.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
            ctx.VSSetConstantBuffer(constantBuffer, 0);
            ctx.PSSetSampler(fontSampler, 0);
        }

        /// <summary>
        /// Render function
        /// </summary>
        /// <param name="data"></param>
        public static unsafe void RenderDrawData(ImDrawData* data)
        {
            // Avoid rendering when minimized
            if (data->DisplaySize.X <= 0.0f || data->DisplaySize.Y <= 0.0f)
            {
                return;
            }

            if (data->CmdListsCount == 0)
            {
                return;
            }

            IGraphicsContext ctx = context;

            // Create and grow vertex/index buffers if needed
            if (vertexBuffer == null || vertexBufferSize < data->TotalVtxCount)
            {
                vertexBuffer?.Dispose();
                vertexBufferSize = data->TotalVtxCount + 5000;
                BufferDescription desc = new();
                desc.Usage = Usage.Dynamic;
                desc.ByteWidth = vertexBufferSize * sizeof(ImDrawVert);
                desc.BindFlags = BindFlags.VertexBuffer;
                desc.CPUAccessFlags = CpuAccessFlags.Write;
                vertexBuffer = device.CreateBuffer(desc);
            }

            if (indexBuffer == null || indexBufferSize < data->TotalIdxCount)
            {
                indexBuffer?.Dispose();
                indexBufferSize = data->TotalIdxCount + 10000;
                BufferDescription desc = new();
                desc.Usage = Usage.Dynamic;
                desc.ByteWidth = indexBufferSize * sizeof(ImDrawIdx);
                desc.BindFlags = BindFlags.IndexBuffer;
                desc.CPUAccessFlags = CpuAccessFlags.Write;
                indexBuffer = device.CreateBuffer(desc);
            }

            // Upload vertex/index data into a single contiguous GPU buffer
            var vertexResource = ctx.Map(vertexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
            var indexResource = ctx.Map(indexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
            var vertexResourcePointer = (ImDrawVert*)vertexResource.PData;
            var indexResourcePointer = (ImDrawIdx*)indexResource.PData;
            for (int n = 0; n < data->CmdListsCount; n++)
            {
                var cmdlList = data->CmdLists.Data[n];

                var vertBytes = cmdlList->VtxBuffer.Size * sizeof(ImDrawVert);
                Buffer.MemoryCopy(cmdlList->VtxBuffer.Data, vertexResourcePointer, vertBytes, vertBytes);

                var idxBytes = cmdlList->IdxBuffer.Size * sizeof(ImDrawIdx);
                Buffer.MemoryCopy(cmdlList->IdxBuffer.Data, indexResourcePointer, idxBytes, idxBytes);

                vertexResourcePointer += cmdlList->VtxBuffer.Size;
                indexResourcePointer += cmdlList->IdxBuffer.Size;
            }
            ctx.Unmap(vertexBuffer, 0);
            ctx.Unmap(indexBuffer, 0);

            // Setup orthographic projection matrix into our constant buffer
            // Our visible imgui space lies from draw_data->DisplayPos (top left) to draw_data->DisplayPos+data_data->DisplaySize (bottom right). DisplayPos is (0,0) for single viewport apps.
            {
                var mappedResource = ctx.Map(constantBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                Matrix4x4* constant_buffer = (Matrix4x4*)mappedResource.PData;

                float L = data->DisplayPos.X;
                float R = data->DisplayPos.X + data->DisplaySize.X;
                float T = data->DisplayPos.Y;
                float B = data->DisplayPos.Y + data->DisplaySize.Y;
                Matrix4x4 mvp = new
                    (
                     2.0f / (R - L), 0.0f, 0.0f, 0.0f,
                     0.0f, 2.0f / (T - B), 0.0f, 0.0f,
                     0.0f, 0.0f, 0.5f, 0.0f,
                     (R + L) / (L - R), (T + B) / (B - T), 0.5f, 1.0f
                     );
                Buffer.MemoryCopy(&mvp, constant_buffer, sizeof(Matrix4x4), sizeof(Matrix4x4));
                ctx.Unmap(constantBuffer, 0);
            }

            // Setup desired state
            SetupRenderState(data, ctx);

            // Render command lists
            // (Because we merged all buffers into a single one, we maintain our own offset into them)
            int global_idx_offset = 0;
            int global_vtx_offset = 0;
            Vector2 clip_off = data->DisplayPos;
            for (int n = 0; n < data->CmdListsCount; n++)
            {
                var cmdList = data->CmdLists.Data[n];

                for (int i = 0; i < cmdList->CmdBuffer.Size; i++)
                {
                    var cmd = cmdList->CmdBuffer.Data[i];
                    if (cmd.UserCallback != null)
                    {
                        // User callback, registered via ImDrawList::AddCallback()
                        // (ImDrawCallback_ResetRenderState is a special callback value used by the user to request the renderer to reset render state.)
                        if ((nint)cmd.UserCallback == -1)
                        {
                            SetupRenderState(data, ctx);
                        }
                        else
                        {
                            Marshal.GetDelegateForFunctionPointer<UserCallback>((nint)cmd.UserCallback)(cmdList, &cmd);
                        }
                    }
                    else
                    {
                        // Project scissor/clipping rectangles into framebuffer space
                        Vector2 clip_min = new(cmd.ClipRect.X - clip_off.X, cmd.ClipRect.Y - clip_off.Y);
                        Vector2 clip_max = new(cmd.ClipRect.Z - clip_off.X, cmd.ClipRect.W - clip_off.Y);
                        if (clip_max.X <= clip_min.X || clip_max.Y <= clip_min.Y)
                            continue;

                        // Apply scissor/clipping rectangle
                        ctx.SetScissorRect((int)clip_min.X, (int)clip_min.Y, (int)clip_max.X, (int)clip_max.Y);

                        // Bind texture, Draw
                        ctx.PSSetShaderResource((void*)cmd.TextureId.Handle, 0);
                        ctx.DrawIndexedInstanced(cmd.ElemCount, 1, (uint)(cmd.IdxOffset + global_idx_offset), (int)(cmd.VtxOffset + global_vtx_offset), 0);
                    }
                }
                global_idx_offset += cmdList->IdxBuffer.Size;
                global_vtx_offset += cmdList->VtxBuffer.Size;
            }

            ctx.SetGraphicsPipeline(null);
            ctx.SetViewport(default);
            ctx.SetVertexBuffer(null, 0, 0);
            ctx.SetIndexBuffer(null, default, 0);
            ctx.SetPrimitiveTopology(PrimitiveTopology.Undefined);
            ctx.VSSetConstantBuffer(null, 0);
            ctx.PSSetSampler(null, 0);
            ctx.PSSetShaderResource((void*)null, 0);
        }

        private static unsafe void CreateFontsTexture()
        {
            var io = ImGui.GetIO();
            byte* pixels;
            int width;
            int height;
            ImGui.GetTexDataAsRGBA32(io.Fonts, &pixels, &width, &height, null);

            var texDesc = new Texture2DDescription
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8UNorm,
                SampleDescription = new SampleDescription { Count = 1 },
                Usage = Usage.Default,
                BindFlags = BindFlags.ShaderResource,
                CPUAccessFlags = CpuAccessFlags.None
            };

            var subResource = new SubresourceData
            {
                DataPointer = (nint)pixels,
                RowPitch = texDesc.Width * 4,
                SlicePitch = 0
            };

            var texture = device.CreateTexture2D(texDesc, new[] { subResource });

            var resViewDesc = new ShaderResourceViewDescription
            {
                Format = Format.R8G8B8A8UNorm,
                ViewDimension = ShaderResourceViewDimension.Texture2D,
                Texture2D = new Texture2DShaderResourceView { MipLevels = texDesc.MipLevels, MostDetailedMip = 0 }
            };
            fontTextureView = device.CreateShaderResourceView(texture, resViewDesc);
            texture.Dispose();

            io.Fonts.TexID = fontTextureView.NativePointer;

            var samplerDesc = new SamplerDescription
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipLODBias = 0f,
                ComparisonFunction = ComparisonFunction.Always,
                MinLOD = 0f,
                MaxLOD = 0f
            };
            fontSampler = device.CreateSamplerState(samplerDesc);
        }

        private static unsafe void CreateDeviceObjects()
        {
            var blendDesc = new BlendDescription
            {
                AlphaToCoverageEnable = false
            };

            blendDesc.RenderTarget[0] = new RenderTargetBlendDescription
            {
                BlendOperationAlpha = BlendOperation.Add,
                IsBlendEnabled = true,
                BlendOperation = BlendOperation.Add,
                DestinationBlendAlpha = Blend.InverseSourceAlpha,
                DestinationBlend = Blend.InverseSourceAlpha,
                SourceBlend = Blend.SourceAlpha,
                SourceBlendAlpha = Blend.SourceAlpha,
                RenderTargetWriteMask = ColorWriteEnable.All
            };

            var rasterDesc = new RasterizerDescription
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                ScissorEnable = true,
                DepthClipEnable = false,
            };

            var stencilOpDesc = new DepthStencilOperationDescription(StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Keep, ComparisonFunction.Never);
            var depthDesc = new DepthStencilDescription
            {
                DepthEnable = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthFunc = ComparisonFunction.Always,
                StencilEnable = false,
                FrontFace = stencilOpDesc,
                BackFace = stencilOpDesc
            };

            var inputElements = new[]
          {
                new InputElementDescription( "POSITION", 0, Format.R32G32Float,   0, 0, InputClassification.PerVertexData, 0 ),
                new InputElementDescription( "TEXCOORD", 0, Format.R32G32Float,   8,  0, InputClassification.PerVertexData, 0 ),
                new InputElementDescription( "COLOR",    0, Format.R8G8B8A8UNorm, 16, 0, InputClassification.PerVertexData, 0 ),
            };

            pipeline = device.CreateGraphicsPipeline(new()
            {
                VertexShader = "assets/shaders/internal/imgui/vs.hlsl",
                PixelShader = "assets/shaders/internal/imgui/ps.hlsl",
            },
           new GraphicsPipelineState()
           {
               Blend = blendDesc,
               DepthStencil = depthDesc,
               Rasterizer = rasterDesc,
           }, inputElements);

            var constBufferDesc = new BufferDescription
            {
                ByteWidth = sizeof(Matrix4x4),
                Usage = Usage.Dynamic,
                BindFlags = BindFlags.ConstantBuffer,
                CPUAccessFlags = CpuAccessFlags.Write,
            };
            constantBuffer = device.CreateBuffer(constBufferDesc);

            CreateFontsTexture();
        }

        private static void InvalidateDeviceObjects()
        {
            pipeline.Dispose();
            fontSampler.Dispose();
            fontTextureView.Dispose();
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
            constantBuffer.Dispose();
        }

        public static unsafe bool Init(IGraphicsDevice device, IGraphicsContext context)
        {
            var io = ImGui.GetIO();
            Trace.Assert(io.BackendRendererUserData == null, "Already initialized a renderer backend!");

            // Setup backend capabilities flags
            var bd = Alloc<RendererData>();
            io.BackendRendererUserData = bd;
            io.BackendRendererName = "ImGui_Generic_Renderer".ToUTF8();
            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset; // We can honor the ImDrawCmd::VtxOffset field, allowing for large meshes.
            io.BackendFlags |= ImGuiBackendFlags.RendererHasViewports; // We can create multi-viewports on the Renderer side (optional)

            ImGuiRenderer.device = device;
            ImGuiRenderer.context = context;

            CreateDeviceObjects();

            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
                InitPlatformInterface();

            return true;
        }

        public static unsafe void Shutdown()
        {
            RendererData* bd = GetBackendData();
            Trace.Assert(bd != null, "No renderer backend to shutdown, or already shutdown?");
            var io = ImGui.GetIO();

            ShutdownPlatformInterface();
            InvalidateDeviceObjects();

            io.BackendRendererName = null;
            io.BackendRendererUserData = null;
            io.BackendFlags &= ~(ImGuiBackendFlags.RendererHasVtxOffset | ImGuiBackendFlags.RendererHasViewports);
            Free(bd);
        }

        public static unsafe void NewFrame()
        {
            RendererData* bd = GetBackendData();
            Trace.Assert(bd != null, "Did you call ImGui_ImplDX11_Init()?");

            if (fontSampler == null)
                CreateDeviceObjects();
        }

        //--------------------------------------------------------------------------------------------------------
        // MULTI-VIEWPORT / PLATFORM INTERFACE SUPPORT
        // This is an _advanced_ and _optional_ feature, allowing the backend to create and handle multiple viewports simultaneously.
        // If you are new to dear imgui or creating a new binding for dear imgui, it is recommended that you completely ignore this section first..
        //--------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Helper structure we store in the void* RendererUserData field of each ImGuiViewport to easily retrieve our backend data.
        /// </summary>
        private class ViewportData
        {
            public ISwapChain SwapChain;
            public IRenderTargetView RTView;
        };

        private struct ViewportDataHandle
        {
            private nint size;
        }

        private static readonly Dictionary<Pointer<ViewportDataHandle>, ViewportData> viewportData = new();

        private static unsafe void CreateWindow(ImGuiViewport* viewport)
        {
            ViewportData vd = new();
            ViewportDataHandle* vh = Alloc<ViewportDataHandle>();
            viewportData.Add(vh, vd);
            viewport->RendererUserData = vh;

            // PlatformHandleRaw should always be a HWND, whereas PlatformHandle might be a higher-level handle (e.g. GLFWWindow*, SDL_Window*).
            // Some backends will leave PlatformHandleRaw == 0, in which case we assume PlatformHandle will contain the HWND.
            Silk.NET.SDL.Window* window = (Silk.NET.SDL.Window*)viewport->PlatformHandle;

            // Create swap chain
            vd.SwapChain = device.CreateSwapChain(window);

            // Create the render target
            if (vd.SwapChain != null)
            {
                vd.RTView = vd.SwapChain.BackbufferRTV;
            }
        }

        private static unsafe void DestroyWindow(ImGuiViewport* viewport)
        {
            // The main viewport (owned by the application) will always have RendererUserData == nullptr since we didn't create the data for it.
            ViewportDataHandle* vh = (ViewportDataHandle*)viewport->RendererUserData;
            if (vh != null)
            {
                ViewportData vd = viewportData[vh];
                vd.SwapChain?.Dispose();
                vd.SwapChain = null;
                vd.RTView = null;
                viewportData.Remove(vh);
                Free(vh);
            }
            viewport->RendererUserData = null;
        }

        private static unsafe void SetWindowSize(ImGuiViewport* viewport, Vector2 size)
        {
            ViewportDataHandle* vh = (ViewportDataHandle*)viewport->RendererUserData;
            ViewportData vd = viewportData[vh];

            vd.RTView = null;

            if (vd.SwapChain != null)
            {
                vd.SwapChain.Resize((int)size.X, (int)size.Y);
                vd.SwapChain.Active = true;
                vd.SwapChain.VSync = false;
                vd.SwapChain.LimitFPS = false;
                vd.RTView = vd.SwapChain.BackbufferRTV;
            }
        }

        private static unsafe void RenderWindow(ImGuiViewport* viewport, void* userdata)
        {
            ViewportDataHandle* vh = (ViewportDataHandle*)viewport->RendererUserData;
            ViewportData vd = viewportData[vh];
            context.SetRenderTarget(vd.RTView, null);
            if ((viewport->Flags & ImGuiViewportFlags.NoRendererClear) != 0)
                context.ClearRenderTargetView(vd.RTView, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
            RenderDrawData(viewport->DrawData);
        }

        private static unsafe void SwapBuffers(ImGuiViewport* viewport, void* userdata)
        {
            ViewportDataHandle* vh = (ViewportDataHandle*)viewport->RendererUserData;
            ViewportData vd = viewportData[vh];
            vd.SwapChain.Present(0, 0); // Present without vsync
        }

        private static unsafe void InitPlatformInterface()
        {
            ImGuiPlatformIOPtr platform_io = ImGui.GetPlatformIO();
            platform_io.RendererCreateWindow = (void*)Marshal.GetFunctionPointerForDelegate<RendererCreateWindow>(CreateWindow);
            platform_io.RendererDestroyWindow = (void*)Marshal.GetFunctionPointerForDelegate<RendererDestroyWindow>(DestroyWindow);
            platform_io.RendererSetWindowSize = (void*)Marshal.GetFunctionPointerForDelegate<RendererSetWindowSize>(SetWindowSize);
            platform_io.RendererRenderWindow = (void*)Marshal.GetFunctionPointerForDelegate<RendererRenderWindow>(RenderWindow);
            platform_io.RendererSwapBuffers = (void*)Marshal.GetFunctionPointerForDelegate<RendererSwapBuffers>(SwapBuffers);
        }

        private static unsafe void ShutdownPlatformInterface()
        {
            ImGui.DestroyPlatformWindows();
        }
    }
}