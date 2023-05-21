//based on https://github.com/ocornut/imgui/blob/master/examples/imgui_impl_dx11.cpp
#nullable disable

namespace HexaEngine.Rendering
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Windows;
    using HexaEngine.Mathematics;
    using HexaEngine.ImGui;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using ImDrawIdx = System.UInt16;

    public unsafe class ImGuiRenderer
    {
        private const int VertexConstantBufferSize = 16 * 4;

        private IGraphicsDevice device;
        private IGraphicsContext context;
        private IGraphicsPipeline pipeline;
        private ISwapChain swapChain;
        private ImGuiInputHandler inputHandler;
        private IBuffer vertexBuffer;
        private IBuffer indexBuffer;
        private IBuffer constantBuffer;
        private ISamplerState fontSampler;
        private IShaderResourceView fontTextureView;
        private int vertexBufferSize = 5000, indexBufferSize = 10000;
        private ImGuiContext* guiContext;
        public static readonly Dictionary<nint, ISamplerState> Samplers = new();

        public ImGuiRenderer(SdlWindow window, IGraphicsDevice device, ISwapChain swapChain)
        {
            guiContext = ImGui.CreateContext(null);
            ImGui.SetCurrentContext(guiContext);

            this.device = device;
            this.swapChain = swapChain;
            context = device.Context;

            var io = ImGui.GetIO();

            var config = ImGui.ImFontConfig();
            ImGui.AddFontDefault(io->Fonts, config);

            config->MergeMode = true;
            config->GlyphMinAdvanceX = 18;
            config->GlyphOffset = new(0, 4);
            var range = new char[] { (char)0xE700, (char)0xF800, (char)0 };
            fixed (char* buffer = range)
            {
                var bytes = File.ReadAllBytes("assets/fonts/SEGMDL2.TTF");
                fixed (byte* buffer2 = bytes)
                {
                    ImGui.AddFontFromMemoryTTF(io->Fonts, buffer2, bytes.Length, 14, config, buffer);
                }
            }

            io->ConfigFlags |= ImGuiConfigFlags.DockingEnable | ImGuiConfigFlags.NavEnableGamepad;
            io->BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset | ImGuiBackendFlags.HasMouseCursors;
            ImGui.StyleColorsDark(ImGui.GetStyle());
            CreateDeviceObjects();

            var style = ImGui.GetStyle();
            var colors = &style->Colors_0;

            colors[(int)ImGuiCol.Text] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
            colors[(int)ImGuiCol.WindowBg] = new Vector4(0.10f, 0.10f, 0.10f, 1.00f);
            colors[(int)ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.PopupBg] = new Vector4(0.19f, 0.19f, 0.19f, 0.92f);
            colors[(int)ImGuiCol.Border] = new Vector4(0.19f, 0.19f, 0.19f, 0.29f);
            colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.24f);
            colors[(int)ImGuiCol.FrameBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
            colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
            colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.TitleBg] = new Vector4(0.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.06f, 0.06f, 0.06f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
            colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.40f, 0.40f, 0.40f, 0.54f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);
            colors[(int)ImGuiCol.CheckMark] = new Vector4(0.33f, 0.67f, 0.86f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
            colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);
            colors[(int)ImGuiCol.Button] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
            colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
            colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.Header] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
            colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.00f, 0.00f, 0.00f, 0.36f);
            colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.20f, 0.22f, 0.23f, 0.33f);
            colors[(int)ImGuiCol.Separator] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
            colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.44f, 0.44f, 0.44f, 0.29f);
            colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.40f, 0.44f, 0.47f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
            colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.44f, 0.44f, 0.44f, 0.29f);
            colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.40f, 0.44f, 0.47f, 1.00f);
            colors[(int)ImGuiCol.Tab] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
            colors[(int)ImGuiCol.TabHovered] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
            colors[(int)ImGuiCol.TabActive] = new Vector4(0.20f, 0.20f, 0.20f, 0.36f);
            colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
            colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.33f, 0.67f, 0.86f, 1.00f);
            colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotLines] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
            colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
            colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
            colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.00f, 1.00f, 1.00f, 0.06f);
            colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
            colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.33f, 0.67f, 0.86f, 1.00f);
            colors[(int)ImGuiCol.NavHighlight] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 0.00f, 0.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(1.00f, 0.00f, 0.00f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.10f, 0.10f, 0.10f, 0.00f);

            style->WindowPadding = new Vector2(8.00f, 8.00f);
            style->FramePadding = new Vector2(5.00f, 2.00f);
            style->CellPadding = new Vector2(6.00f, 6.00f);
            style->ItemSpacing = new Vector2(6.00f, 6.00f);
            style->ItemInnerSpacing = new Vector2(6.00f, 6.00f);
            style->TouchExtraPadding = new Vector2(0.00f, 0.00f);
            style->IndentSpacing = 25;
            style->ScrollbarSize = 15;
            style->GrabMinSize = 10;
            style->WindowBorderSize = 1;
            style->ChildBorderSize = 1;
            style->PopupBorderSize = 1;
            style->FrameBorderSize = 1;
            style->TabBorderSize = 1;
            style->WindowRounding = 7;
            style->ChildRounding = 4;
            style->FrameRounding = 3;
            style->PopupRounding = 4;
            style->ScrollbarRounding = 9;
            style->GrabRounding = 3;
            style->LogSliderDeadzone = 4;
            style->TabRounding = 4;

            inputHandler = new(window);
        }

        public void BeginDraw()
        {
            ImGui.SetCurrentContext(guiContext);

            inputHandler.Update();
            ImGui.NewFrame();

            ImGui.PushStyleColor(ImGuiCol.WindowBg, Vector4.Zero);
            ImGui.DockSpaceOverViewport(null, ImGuiDockNodeFlags.PassthruCentralNode, null);
            ImGui.PopStyleColor(1);
        }

        public void EndDraw()
        {
            ImGui.Render();
            ImGui.EndFrame();
            context.SetRenderTarget(swapChain.BackbufferRTV, null);
            Render(ImGui.GetDrawData());
        }

        public void Render(ImDrawData* data)
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

            if (vertexBuffer == null || vertexBufferSize < data->TotalVtxCount)
            {
                vertexBuffer?.Dispose();

                vertexBufferSize = (int)(data->TotalVtxCount * 1.5f);
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

                indexBufferSize = (int)(data->TotalIdxCount * 1.5f);

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
                var cmdlList = data->CmdLists[n];

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
            // Our visible imgui space lies from draw_data.DisplayPos (top left) to draw_data.DisplayPos+data_data.DisplaySize (bottom right). DisplayPos is (0,0) for single viewport apps.

            var constResource = ctx.Map(constantBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
            var span = constResource.AsSpan<byte>(VertexConstantBufferSize);
            ImGuiIO* io = ImGui.GetIO();
            Matrix4x4 mvp = MathUtil.OrthoOffCenterLH(0f, io->DisplaySize.X, io->DisplaySize.Y, 0, -1, 1);
            MemoryMarshal.Write(span, ref mvp);
            ctx.Unmap(constantBuffer, 0);

            SetupRenderState(data, ctx);

            ImGui.ScaleClipRects(data, io->DisplayFramebufferScale);

            // Render command lists
            // (Because we merged all buffers into a single one, we maintain our own offset into them)
            int vtx_offset = 0;
            uint idx_offset = 0;

            for (int n = 0; n < data->CmdListsCount; n++)
            {
                var cmdList = data->CmdLists[n];

                for (int i = 0; i < cmdList->CmdBuffer.Size; i++)
                {
                    var cmd = cmdList->CmdBuffer.Data[i];
                    if (cmd.UserCallback != null)
                    {
                        cmd.UserCallback(cmdList, &cmd);
                    }
                    else
                    {
                        ctx.SetScissorRect((int)cmd.ClipRect.X, (int)cmd.ClipRect.Y, (int)cmd.ClipRect.Z, (int)cmd.ClipRect.W);

                        ctx.PSSetShaderResource((void*)cmd.TextureId.Handle, 0);
                        if (Samplers.TryGetValue(cmd.TextureId.Handle, out var sampler))
                        {
                            ctx.PSSetSampler(sampler, 0);
                        }
                        else
                        {
                            ctx.PSSetSampler(fontSampler, 0);
                        }

                        ctx.DrawIndexedInstanced(cmd.ElemCount, 1, idx_offset, vtx_offset, 0);
                    }
                    idx_offset += cmd.ElemCount;
                }
                vtx_offset += cmdList->VtxBuffer.Size;
            }

            ctx.ClearState();
        }

        public void Dispose()
        {
            ImGui.SaveIniSettingsToDisk("imgui.ini");
            if (device == null)
            {
                return;
            }

            InvalidateDeviceObjects();
        }

        private void SetupRenderState(ImDrawData* drawData, IGraphicsContext ctx)
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

        private void CreateFontsTexture()
        {
            var io = ImGui.GetIO();
            byte* pixels;
            int width;
            int height;
            ImGui.GetTexDataAsRGBA32(io->Fonts, &pixels, &width, &height, null);

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
                DataPointer = (IntPtr)pixels,
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

            io->Fonts->TexID = fontTextureView.NativePointer;

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

        private void CreateDeviceObjects()
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
                ByteWidth = VertexConstantBufferSize,
                Usage = Usage.Dynamic,
                BindFlags = BindFlags.ConstantBuffer,
                CPUAccessFlags = CpuAccessFlags.Write,
            };
            constantBuffer = device.CreateBuffer(constBufferDesc);

            CreateFontsTexture();
        }

        private void InvalidateDeviceObjects()
        {
            pipeline.Dispose();
            fontSampler.Dispose();
            fontTextureView.Dispose();
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
            constantBuffer.Dispose();
        }
    }
}