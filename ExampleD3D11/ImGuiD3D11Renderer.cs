//based on https://github.com/ocornut/imgui/blob/master/backends/imgui_impl_dx11.cpp
#nullable disable

namespace ExampleD3D11
{
    using Hexa.NET.ImGui;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D.Compilers;
    using Silk.NET.Direct3D11;
    using Silk.NET.DXGI;
    using Silk.NET.Maths;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using ImDrawIdx = UInt16;

    [Obsolete("Use ImGuiImplD3D11 instead")]
    public static class ImGuiD3D11Renderer
    {
        private static readonly D3DCompiler D3DCompiler = D3DCompiler.GetApi();

        /// <summary>
        /// Renderer data
        /// </summary>
        private struct RendererData
        {
            public ComPtr<ID3D11Device> device;
            public ComPtr<ID3D11DeviceContext> context;
            public ComPtr<IDXGIFactory> factory;
            public ComPtr<ID3D11Buffer> vertexBuffer;
            public ComPtr<ID3D11Buffer> indexBuffer;
            public ComPtr<ID3D11VertexShader> vertexShader;
            public ComPtr<ID3D11InputLayout> inputLayout;
            public ComPtr<ID3D11Buffer> constantBuffer;
            public ComPtr<ID3D11PixelShader> pixelShader;
            public ComPtr<ID3D11SamplerState> fontSampler;
            public ComPtr<ID3D11ShaderResourceView> fontTextureView;
            public ComPtr<ID3D11RasterizerState> rasterizerState;
            public ComPtr<ID3D11BlendState> blendState;
            public ComPtr<ID3D11DepthStencilState> depthStencilState;
            public int vertexBufferSize = 5000, indexBufferSize = 10000;

            public RendererData()
            {
            }
        }

        // Backend data stored in io.BackendRendererUserData to allow support for multiple Dear ImGui contexts
        // It is STRONGLY preferred that you use docking branch with multi-viewports (== single Dear ImGui context + multiple windows) instead of multiple Dear ImGui contexts.
        private static unsafe RendererData* GetBackendData()
        {
            return !ImGui.GetCurrentContext().IsNull ? (RendererData*)ImGui.GetIO().BackendRendererUserData : null;
        }

        private static unsafe void SetupRenderState(ImDrawData* drawData, ComPtr<ID3D11DeviceContext> ctx)
        {
            RendererData* bd = GetBackendData();
            var viewport = new Viewport(0, 0, drawData->DisplaySize.X, drawData->DisplaySize.Y, 0, 1);

            ctx.RSSetViewports(1, &viewport);

            uint stride = (uint)sizeof(ImDrawVert);
            uint offset = 0;

            ctx.IASetInputLayout(bd->inputLayout);
            ctx.IASetVertexBuffers(0, 1, bd->vertexBuffer, &stride, &offset);
            ctx.IASetIndexBuffer(bd->indexBuffer, sizeof(ushort) == 2 ? Format.FormatR16Uint : Format.FormatR32Uint, 0);
            ctx.IASetPrimitiveTopology(D3DPrimitiveTopology.D3DPrimitiveTopologyTrianglelist);
            ctx.VSSetShader(bd->vertexShader, null, 0);
            ctx.VSSetConstantBuffers(0, 1, bd->constantBuffer);
            ctx.PSSetShader(bd->pixelShader, null, 0);
            ctx.PSSetSamplers(0, 1, bd->fontSampler);
            ctx.GSSetShader((ID3D11GeometryShader*)null, null, 0);
            ctx.HSSetShader((ID3D11HullShader*)null, null, 0); // In theory we should backup and restore this as well.. very infrequently used..
            ctx.DSSetShader((ID3D11DomainShader*)null, null, 0); // In theory we should backup and restore this as well.. very infrequently used..
            ctx.CSSetShader((ID3D11ComputeShader*)null, null, 0); // In theory we should backup and restore this as well.. very infrequently used..

            // Setup blend state
            Vector4 blend_factor = Vector4.Zero;
            ctx.OMSetBlendState(bd->blendState, (float*)&blend_factor, 0xffffffff);
            ctx.OMSetDepthStencilState(bd->depthStencilState, 0);
            ctx.RSSetState(bd->rasterizerState);
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

            RendererData* bd = GetBackendData();
            ComPtr<ID3D11DeviceContext> ctx = bd->context;

            // Create and grow vertex/index buffers if needed
            if (bd->vertexBuffer.Handle == null || bd->vertexBufferSize < data->TotalVtxCount)
            {
                if (bd->vertexBuffer.Handle != null)
                {
                    bd->vertexBuffer.Release();
                }

                bd->vertexBufferSize = data->TotalVtxCount + 5000;
                BufferDesc desc = new();
                desc.Usage = Usage.Dynamic;
                desc.ByteWidth = (uint)(bd->vertexBufferSize * sizeof(ImDrawVert));
                desc.BindFlags = (uint)BindFlag.VertexBuffer;
                desc.CPUAccessFlags = (uint)CpuAccessFlag.Write;
                bd->device.CreateBuffer(desc, null, ref bd->vertexBuffer);
            }

            if (bd->indexBuffer.Handle == null || bd->indexBufferSize < data->TotalIdxCount)
            {
                if (bd->indexBuffer.Handle != null)
                {
                    bd->indexBuffer.Release();
                }

                bd->indexBufferSize = data->TotalIdxCount + 10000;
                BufferDesc desc = new();
                desc.Usage = Usage.Dynamic;
                desc.ByteWidth = (uint)(bd->indexBufferSize * sizeof(ImDrawIdx));
                desc.BindFlags = (uint)BindFlag.IndexBuffer;
                desc.CPUAccessFlags = (uint)CpuAccessFlag.Write;
                bd->device.CreateBuffer(desc, null, ref bd->indexBuffer);
            }

            // Upload vertex/index data into a single contiguous GPU buffer
            MappedSubresource vertexResource;
            ctx.Map(bd->vertexBuffer, 0, Map.WriteDiscard, 0, &vertexResource);
            MappedSubresource indexResource;
            ctx.Map(bd->indexBuffer, 0, Map.WriteDiscard, 0, &indexResource);
            var vertexResourcePointer = (ImDrawVert*)vertexResource.PData;
            var indexResourcePointer = (ImDrawIdx*)indexResource.PData;
            for (int n = 0; n < data->CmdListsCount; n++)
            {
                var cmdlList = data->CmdLists.Data[n];

                var vertBytes = cmdlList.VtxBuffer.Size * sizeof(ImDrawVert);
                System.Buffer.MemoryCopy(cmdlList.VtxBuffer.Data, vertexResourcePointer, vertBytes, vertBytes);

                var idxBytes = cmdlList.IdxBuffer.Size * sizeof(ImDrawIdx);
                System.Buffer.MemoryCopy(cmdlList.IdxBuffer.Data, indexResourcePointer, idxBytes, idxBytes);

                vertexResourcePointer += cmdlList.VtxBuffer.Size;
                indexResourcePointer += cmdlList.IdxBuffer.Size;
            }
            ctx.Unmap(bd->vertexBuffer, 0);
            ctx.Unmap(bd->indexBuffer, 0);

            // Setup orthographic projection matrix into our constant buffer
            // Our visible imgui space lies from draw_data->DisplayPos (top left) to draw_data->DisplayPos+data_data->DisplaySize (bottom right). DisplayPos is (0,0) for single viewport apps.
            {
                MappedSubresource mappedResource;
                ctx.Map(bd->constantBuffer, 0, Map.WriteDiscard, 0, &mappedResource);
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
                System.Buffer.MemoryCopy(&mvp, constant_buffer, sizeof(Matrix4x4), sizeof(Matrix4x4));
                ctx.Unmap(bd->constantBuffer, 0);
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
                var cmdList = data->CmdLists[n];

                for (int i = 0; i < cmdList.CmdBuffer.Size; i++)
                {
                    var cmd = cmdList.CmdBuffer[i];
                    if (cmd.UserCallback != null)
                    {
                        // User callback, registered via ImDrawList::AddCallback()
                        // (ImDrawCallback_ResetRenderState is a special callback value used by the user to request the renderer to reset render state.)
                        if ((nint)cmd.UserCallback == ImGui.ImDrawCallbackResetRenderState)
                        {
                            SetupRenderState(data, ctx);
                        }
                        else
                        {
                            ((delegate*<ImDrawList*, ImDrawCmd*, void>)cmd.UserCallback)(cmdList, &cmd);
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
                        Box2D<int> rect = new((int)clip_min.X, (int)clip_min.Y, (int)clip_max.X, (int)clip_max.Y);
                        ctx.RSSetScissorRects(1, &rect);

                        // Bind texture, Draw
                        var srv = cmd.TextureId.Handle;
                        ctx.PSSetShaderResources(0, 1, (ID3D11ShaderResourceView**)&srv);
                        ctx.DrawIndexedInstanced(cmd.ElemCount, 1, (uint)(cmd.IdxOffset + global_idx_offset), (int)(cmd.VtxOffset + global_vtx_offset), 0);
                    }
                }
                global_idx_offset += cmdList.IdxBuffer.Size;
                global_vtx_offset += cmdList.VtxBuffer.Size;
            }

            void* nullObj = null;

            ctx.IASetInputLayout((ID3D11InputLayout*)null);
            ctx.VSSetShader((ID3D11VertexShader*)null, null, 0);
            ctx.PSSetShader((ID3D11PixelShader*)null, null, 0);
            ctx.GSSetShader((ID3D11GeometryShader*)null, null, 0);
            ctx.HSSetShader((ID3D11HullShader*)null, null, 0);
            ctx.DSSetShader((ID3D11DomainShader*)null, null, 0);
            ctx.CSSetShader((ID3D11ComputeShader*)null, null, 0);

            Vector4 blend_factor = Vector4.Zero;
            ctx.OMSetBlendState((ID3D11BlendState*)null, (float*)&blend_factor, 0xffffffff);
            ctx.OMSetDepthStencilState((ID3D11DepthStencilState*)null, 0);
            ctx.RSSetState((ID3D11RasterizerState*)null);

            Viewport viewport = default;
            ctx.RSSetViewports(1, &viewport);
            ctx.IASetVertexBuffers(0, 1, (ID3D11Buffer**)&nullObj, 0, 0);
            ctx.IASetIndexBuffer((ID3D11Buffer*)null, default, 0);
            ctx.IASetPrimitiveTopology(D3DPrimitiveTopology.D3DPrimitiveTopologyUndefined);
            ctx.VSSetConstantBuffers(0, 1, (ID3D11Buffer**)&nullObj);
            ctx.PSSetSamplers(0, 1, (ID3D11SamplerState**)&nullObj);
            ctx.PSSetShaderResources(0, 1, (ID3D11ShaderResourceView**)&nullObj);
        }

        private static unsafe void CreateFontsTexture()
        {
            var io = ImGui.GetIO();
            RendererData* bd = GetBackendData();
            byte* pixels;
            int width;
            int height;
            ImGui.GetTexDataAsRGBA32(io.Fonts, &pixels, &width, &height, null);

            var texDesc = new Texture2DDesc
            {
                Width = (uint)width,
                Height = (uint)height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.FormatR8G8B8A8Unorm,
                SampleDesc = new SampleDesc { Count = 1 },
                Usage = Usage.Default,
                BindFlags = (uint)BindFlag.ShaderResource,
                CPUAccessFlags = (uint)CpuAccessFlag.None
            };

            var subResource = new SubresourceData
            {
                PSysMem = pixels,
                SysMemPitch = texDesc.Width * 4,
                SysMemSlicePitch = 0
            };

            ComPtr<ID3D11Texture2D> texture = default;
            bd->device.CreateTexture2D(&texDesc, &subResource, ref texture);

            var resViewDesc = new ShaderResourceViewDesc
            {
                Format = Format.FormatR8G8B8A8Unorm,
                ViewDimension = D3DSrvDimension.D3DSrvDimensionTexture2D,
                Texture2D = new() { MipLevels = texDesc.MipLevels, MostDetailedMip = 0 }
            };
            ;
            bd->device.CreateShaderResourceView(texture, &resViewDesc, ref bd->fontTextureView);
            texture.Dispose();

            io.Fonts.TexID = new((nint)bd->fontTextureView.Handle);

            var samplerDesc = new SamplerDesc
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipLODBias = 0f,
                ComparisonFunc = ComparisonFunc.Always,
                MinLOD = 0f,
                MaxLOD = 0f
            };

            bd->device.CreateSamplerState(&samplerDesc, ref bd->fontSampler);
        }

        private static unsafe bool CreateDeviceObjects()
        {
            RendererData* bd = GetBackendData();
            if (bd->device.Handle == null)
                return false;

            if (bd->fontSampler.Handle != null)
                InvalidateDeviceObjects();

            {
                string vertexShaderCode =
                  @"cbuffer vertexBuffer : register(b0)

            {
              float4x4 ProjectionMatrix;
            };
            struct VS_INPUT
            {
              float2 pos : POSITION;
              float2 uv  : TEXCOORD0;
              float4 col : COLOR0;
            };

            struct PS_INPUT
            {
              float4 pos : SV_POSITION;
              float4 col : COLOR0;
              float2 uv  : TEXCOORD0;
            };

            PS_INPUT main(VS_INPUT input)
            {
              PS_INPUT output;
              output.pos = mul(ProjectionMatrix, float4(input.pos.xy, 0.f, 1.f));
              output.col = input.col;
              output.uv = input.uv;
              return output;
            }
";

                byte* pVertexShaderCode = vertexShaderCode.ToUTF8Ptr();
                byte* pEntryPoint = "main".ToUTF8Ptr();
                byte* pProfile = "vs_4_0".ToUTF8Ptr();

                ID3D10Blob* vertexShaderBlob;

                HResult result = D3DCompiler.Compile(pVertexShaderCode, (nuint)vertexShaderCode.Length, (byte*)null, (D3DShaderMacro*)null, (ID3DInclude*)null, pEntryPoint, pProfile, 0, 0, &vertexShaderBlob, null);

                // avoid leaks even if compilation fails.
                Free(pVertexShaderCode);
                Free(pEntryPoint);
                Free(pProfile);

                if (!result.IsSuccess)
                    return false;   // NB: Pass ID3DBlob* pErrorBlob to D3DCompile() to get error showing in (const char*)pErrorBlob->GetBufferPointer(). Make sure to Release() the blob!

                result = bd->device.CreateVertexShader(vertexShaderBlob->GetBufferPointer(), vertexShaderBlob->GetBufferSize(), (ID3D11ClassLinkage*)null, bd->vertexShader.GetAddressOf());
                if (!result.IsSuccess)
                {
                    vertexShaderBlob->Release();
                    return false;
                }

                // Create the input layout
                InputElementDesc[] local_layout =
                {
                    new("POSITION".ToUTF8Ptr(), 0, Format.FormatR32G32Float,   0, 0,  InputClassification.PerVertexData, 0 )  ,
                    new("TEXCOORD".ToUTF8Ptr(), 0, Format.FormatR32G32Float,   0, 8,  InputClassification.PerVertexData, 0 ),
                    new("COLOR".ToUTF8Ptr(),    0, Format.FormatR8G8B8A8Unorm, 0, 16, InputClassification.PerVertexData, 0),
                };

                fixed (InputElementDesc* pLocal_layout = local_layout)
                {
                    result = bd->device.CreateInputLayout(pLocal_layout, 3, vertexShaderBlob->GetBufferPointer(), vertexShaderBlob->GetBufferSize(), ref bd->inputLayout);
                }

                for (int i = 0; i < local_layout.Length; i++)
                {
                    Free(local_layout[i].SemanticName);
                }

                if (!result.IsSuccess)
                {
                    vertexShaderBlob->Release();
                    return false;
                }
                vertexShaderBlob->Release();

                // Create the constant buffer
                {
                    var constBufferDesc = new BufferDesc
                    {
                        ByteWidth = (uint)sizeof(Matrix4x4),
                        Usage = Usage.Dynamic,
                        BindFlags = (uint)BindFlag.ConstantBuffer,
                        CPUAccessFlags = (uint)CpuAccessFlag.Write,
                    };
                    bd->device.CreateBuffer(&constBufferDesc, null, ref bd->constantBuffer);
                }
            }

            // Create the pixel shader
            {
                string pixelShaderCode =
                     @"struct PS_INPUT
                    {
                    float4 pos : SV_POSITION;
            float4 col : COLOR0;
            float2 uv  : TEXCOORD0;
            };
            sampler sampler0;
            Texture2D texture0;

            float4 main(PS_INPUT input) : SV_Target
            {
            float4 out_col = input.col * texture0.Sample(sampler0, input.uv);
            return out_col;
            }
                ";

                byte* pPixelShaderCode = pixelShaderCode.ToUTF8Ptr();
                byte* pEntryPoint = "main".ToUTF8Ptr();
                byte* pProfile = "ps_4_0".ToUTF8Ptr();

                ID3D10Blob* pixelShaderBlob;
                HResult result = D3DCompiler.Compile(pPixelShaderCode, (nuint)pixelShaderCode.Length, (byte*)null, (D3DShaderMacro*)null, (ID3DInclude*)null, pEntryPoint, pProfile, 0, 0, &pixelShaderBlob, null);

                Free(pPixelShaderCode);
                Free(pEntryPoint);
                Free(pProfile);

                if (!result.IsSuccess)
                    return false; // NB: Pass ID3DBlob* pErrorBlob to D3DCompile() to get error showing in (const char*)pErrorBlob->GetBufferPointer(). Make sure to Release() the blob!

                result = bd->device.CreatePixelShader(pixelShaderBlob->GetBufferPointer(), pixelShaderBlob->GetBufferSize(), (ID3D11ClassLinkage*)null, bd->pixelShader.GetAddressOf());

                if (!result.IsSuccess)
                {
                    pixelShaderBlob->Release();
                    return false;
                }
                pixelShaderBlob->Release();
            }

            // Create the blending setup
            {
                var blendDesc = new BlendDesc
                {
                    AlphaToCoverageEnable = false
                };

                blendDesc.RenderTarget[0] = new()
                {
                    BlendOpAlpha = BlendOp.Add,
                    BlendEnable = true,
                    BlendOp = BlendOp.Add,
                    DestBlendAlpha = Blend.InvSrcAlpha,
                    DestBlend = Blend.InvSrcAlpha,
                    SrcBlend = Blend.SrcAlpha,
                    SrcBlendAlpha = Blend.SrcAlpha,
                    RenderTargetWriteMask = (byte)ColorWriteEnable.All
                };
                bd->device.CreateBlendState(&blendDesc, ref bd->blendState);
            }

            // Create the rasterizer state
            {
                var rasterDesc = new RasterizerDesc
                {
                    FillMode = FillMode.Solid,
                    CullMode = CullMode.None,
                    ScissorEnable = true,
                    DepthClipEnable = false,
                };
                bd->device.CreateRasterizerState(&rasterDesc, ref bd->rasterizerState);
            }

            // Create depth-stencil State
            {
                var stencilOpDesc = new DepthStencilopDesc(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep, ComparisonFunc.Never);
                var depthDesc = new DepthStencilDesc
                {
                    DepthEnable = false,
                    DepthWriteMask = DepthWriteMask.Zero,
                    DepthFunc = ComparisonFunc.Always,
                    StencilEnable = false,
                    FrontFace = stencilOpDesc,
                    BackFace = stencilOpDesc
                };
                bd->device.CreateDepthStencilState(&depthDesc, ref bd->depthStencilState);
            }

            CreateFontsTexture();

            return true;
        }

        private static unsafe void InvalidateDeviceObjects()
        {
            RendererData* bd = GetBackendData();

            if (bd->device.Handle == null)
                return;

            if (bd->fontSampler.Handle != null)
            {
                bd->fontSampler.Release();
                bd->fontSampler = null;
            }

            if (bd->fontTextureView.Handle != null)
            {
                bd->fontTextureView.Release();
                bd->fontTextureView = null;
                ImGui.GetIO().Fonts.SetTexID(0); // We copied data->pFontTextureView to io.Fonts->TexID so let's clear that as well.
            }

            if (bd->indexBuffer.Handle != null)
            {
                bd->indexBuffer.Release();
                bd->indexBuffer = null;
            }

            if (bd->vertexBuffer.Handle != null)
            {
                bd->vertexBuffer.Release();
                bd->vertexBuffer = null;
            }

            if (bd->blendState.Handle != null)
            {
                bd->blendState.Release();
                bd->blendState = null;
            }

            if (bd->depthStencilState.Handle != null)
            {
                bd->depthStencilState.Release();
                bd->depthStencilState = null;
            }

            if (bd->rasterizerState.Handle != null)
            {
                bd->rasterizerState.Release();
                bd->rasterizerState = null;
            }

            if (bd->pixelShader.Handle != null)
            {
                bd->pixelShader.Release();
                bd->pixelShader = null;
            }

            if (bd->constantBuffer.Handle != null)
            {
                bd->constantBuffer.Release();
                bd->constantBuffer = null;
            }

            if (bd->inputLayout.Handle != null)
            {
                bd->inputLayout.Release();
                bd->inputLayout = null;
            }

            if (bd->vertexShader.Handle != null)
            {
                bd->vertexShader.Release();
                bd->vertexShader = null;
            }
        }

        public static unsafe bool Init(ComPtr<ID3D11Device> device, ComPtr<ID3D11DeviceContext> context)
        {
            var io = ImGui.GetIO();
            Trace.Assert(io.BackendRendererUserData == null, "Already initialized a renderer backend!");

            // Setup backend capabilities flags
            var bd = AllocT<RendererData>();
            io.BackendRendererUserData = bd;
            io.BackendRendererName = "imgui_impl_dx11".ToUTF8Ptr();
            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset; // We can honor the ImDrawCmd::VtxOffset field, allowing for large meshes.
            io.BackendFlags |= ImGuiBackendFlags.RendererHasViewports; // We can create multi-viewports on the Renderer side (optional)

            // Get factory from device
            if (device.QueryInterface(out ComPtr<IDXGIDevice> pDXGIDevice) == 0)
            {
                if (pDXGIDevice.GetParent(out ComPtr<IDXGIAdapter> pDXGIAdapter) == 0)
                {
                    if (pDXGIAdapter.GetParent(out ComPtr<IDXGIFactory> pFactory) == 0)
                    {
                        bd->device = device;
                        bd->context = context;
                        bd->factory = pFactory;
                    }
                }

                if (pDXGIAdapter.Handle != null)
                {
                    pDXGIAdapter.Release();
                }
            }

            if (pDXGIDevice.Handle != null)
            {
                pDXGIDevice.Release();
            }

            bd->device.AddRef();
            bd->context.AddRef();

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
            if (bd->factory.Handle != null) { bd->factory.Release(); }
            if (bd->device.Handle != null) { bd->device.Release(); }
            if (bd->context.Handle != null) { bd->context.Release(); }
            io.BackendRendererName = null;
            io.BackendRendererUserData = null;
            io.BackendFlags &= ~(ImGuiBackendFlags.RendererHasVtxOffset | ImGuiBackendFlags.RendererHasViewports);
            Free(bd);
        }

        public static unsafe void NewFrame()
        {
            RendererData* bd = GetBackendData();
            Trace.Assert(bd != null, "Did you call ImGui_ImplDX11_Init()?");

            if (bd->fontSampler.Handle == null)
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
        private struct ViewportData
        {
            public ComPtr<IDXGISwapChain> SwapChain;
            public ComPtr<ID3D11RenderTargetView> RTView;
        };

        private static unsafe void CreateWindow(ImGuiViewport* viewport)
        {
            RendererData* bd = GetBackendData();
            ViewportData* vd = AllocT<ViewportData>();
            viewport->RendererUserData = vd;

            // PlatformHandleRaw should always be a HWND, whereas PlatformHandle might be a higher-level handle (e.g. GLFWWindow*, SDL_Window*).
            // Some backends will leave PlatformHandleRaw == 0, in which case we assume PlatformHandle will contain the HWND.
            void* hwnd = viewport->PlatformHandleRaw != null ? viewport->PlatformHandleRaw : viewport->PlatformHandle;

            // Create swap chain
            SwapChainDesc sd;
            sd.BufferDesc.Width = (uint)viewport->Size.X;
            sd.BufferDesc.Height = (uint)viewport->Size.Y;
            sd.BufferDesc.Format = Format.FormatR8G8B8A8Unorm;
            sd.SampleDesc.Count = 1;
            sd.SampleDesc.Quality = 0;
            sd.BufferUsage = DXGI.UsageRenderTargetOutput;
            sd.BufferCount = 1;
            sd.OutputWindow = (nint)hwnd;
            sd.Windowed = true;
            sd.SwapEffect = SwapEffect.Discard;
            sd.Flags = 0;

            bd->factory.CreateSwapChain(bd->device, &sd, ref vd->SwapChain);

            // Create the render target
            if (vd->SwapChain.Handle != null)
            {
                vd->SwapChain.GetBuffer(0, out ComPtr<ID3D11Texture2D> pBackBuffer);
                bd->device.CreateRenderTargetView(pBackBuffer, null, ref vd->RTView);
                pBackBuffer.Release();
            }
        }

        private static unsafe void DestroyWindow(ImGuiViewport* viewport)
        {
            // The main viewport (owned by the application) will always have RendererUserData == null since we didn't create the data for it.
            ViewportData* vd = (ViewportData*)viewport->RendererUserData;
            if (vd != null)
            {
                if (vd->SwapChain.Handle != null)
                {
                    vd->SwapChain.Release();
                }

                vd->SwapChain = null;
                vd->RTView = null;
                Free(vd);
            }
            viewport->RendererUserData = null;
        }

        private static unsafe void SetWindowSize(ImGuiViewport* viewport, Vector2 size)
        {
            RendererData* bd = GetBackendData();
            ViewportData* vd = (ViewportData*)viewport->RendererUserData;

            if (vd->RTView.Handle != null)
            {
                vd->RTView.Release();
                vd->RTView = null;
            }

            if (vd->SwapChain.Handle != null)
            {
                vd->SwapChain.ResizeBuffers(0, (uint)size.X, (uint)size.Y, Format.FormatUnknown, 0);
                vd->SwapChain.GetBuffer(0, out ComPtr<ID3D11Texture2D> pBackBuffer);
                bd->device.CreateRenderTargetView(pBackBuffer, null, ref vd->RTView);
                pBackBuffer.Release();
            }
        }

        private static unsafe void RenderWindow(ImGuiViewport* viewport, void* userdata)
        {
            RendererData* bd = GetBackendData();
            ViewportData* vd = (ViewportData*)viewport->RendererUserData;
            var rtv = vd->RTView.Handle;
            bd->context.OMSetRenderTargets(1, &rtv, (ID3D11DepthStencilView*)null);
            if ((viewport->Flags & ImGuiViewportFlags.NoRendererClear) == 0)
            {
                var col = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                bd->context.ClearRenderTargetView(rtv, (float*)&col);
            }

            RenderDrawData(viewport->DrawData);
        }

        private static unsafe void SwapBuffers(ImGuiViewport* viewport, void* userdata)
        {
            ViewportData* vd = (ViewportData*)viewport->RendererUserData;
            vd->SwapChain.Present(0, 0); // Present without vsync
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