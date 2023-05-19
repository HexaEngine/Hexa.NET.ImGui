namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using System;
    using Viewport = Mathematics.Viewport;

    public unsafe class GraphicsPipeline : IGraphicsPipeline
    {
        private readonly string dbgName;
        private bool disposedValue;
        protected readonly D3D11GraphicsDevice device;
        protected InputElementDescription[]? inputElements;
        protected readonly GraphicsPipelineDesc desc;
        protected readonly ShaderMacro[] macros = Array.Empty<ShaderMacro>();
        protected ComPtr<ID3D11VertexShader> vs;
        protected ComPtr<ID3D11HullShader> hs;
        protected ComPtr<ID3D11DomainShader> ds;
        protected ComPtr<ID3D11GeometryShader> gs;
        protected ComPtr<ID3D11PixelShader> ps;
        protected ComPtr<ID3D11InputLayout> layout;
        protected ComPtr<ID3D11RasterizerState> rasterizerState;
        protected ComPtr<ID3D11DepthStencilState> depthStencilState;
        protected ComPtr<ID3D11BlendState> blendState;
        protected GraphicsPipelineState state = GraphicsPipelineState.Default;
        protected bool valid;
        protected volatile bool initialized;

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, ShaderMacro[] macros, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            this.macros = macros;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, InputElementDescription[] inputElements, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            this.inputElements = inputElements;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, InputElementDescription[] inputElements, ShaderMacro[] macros, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            this.inputElements = inputElements;
            this.macros = macros;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, GraphicsPipelineState state, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            State = state;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, GraphicsPipelineState state, ShaderMacro[] macros, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            State = state;
            this.macros = macros;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, GraphicsPipelineState state, InputElementDescription[] inputElements, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            this.inputElements = inputElements;
            State = state;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public GraphicsPipeline(D3D11GraphicsDevice device, GraphicsPipelineDesc desc, GraphicsPipelineState state, InputElementDescription[] inputElements, ShaderMacro[] macros, string dbgName = "")
        {
            this.device = device;
            this.desc = desc;
            this.inputElements = inputElements;
            State = state;
            this.macros = macros;
            this.dbgName = dbgName;
            Compile();
            PipelineManager.Register(this);
            initialized = true;
        }

        public string DebugName => dbgName;

        public GraphicsPipelineDesc Description => desc;

        public bool IsInitialized => initialized;

        public bool IsValid => valid;

        public GraphicsPipelineState State
        {
            get => state;
            set
            {
                state = value;
                if (rasterizerState.Handle != null)
                {
                    rasterizerState.Release();
                }

                rasterizerState = null;
                if (depthStencilState.Handle != null)
                {
                    depthStencilState.Release();
                }

                depthStencilState.Handle = null;
                if (blendState.Handle != null)
                {
                    blendState.Release();
                }

                blendState = null;

                ComPtr<ID3D11RasterizerState> rs;
                var rsDesc = Helper.Convert(value.Rasterizer);
                device.Device.CreateRasterizerState(&rsDesc, &rs.Handle);
                rasterizerState = rs;
                Utils.SetDebugName(rasterizerState, $"{dbgName}.{nameof(rasterizerState)}");

                ComPtr<ID3D11DepthStencilState> ds;
                var dsDesc = Helper.Convert(value.DepthStencil);
                device.Device.CreateDepthStencilState(&dsDesc, &ds.Handle);
                depthStencilState = ds;
                Utils.SetDebugName(depthStencilState, $"{dbgName}.{nameof(depthStencilState)}");

                ComPtr<ID3D11BlendState> bs;
                var bsDesc = Helper.Convert(value.Blend);
                device.Device.CreateBlendState(&bsDesc, &bs.Handle);
                blendState = bs;
                Utils.SetDebugName(blendState, $"{dbgName}.{nameof(blendState)}");
            }
        }

        public void Recompile()
        {
            initialized = false;

            if (vs.Handle != null)
            {
                vs.Release();
            }

            vs = null;
            if (hs.Handle != null)
            {
                hs.Release();
            }

            hs = null;
            if (ds.Handle != null)
            {
                ds.Release();
            }

            ds = null;
            if (gs.Handle != null)
            {
                gs.Release();
            }

            gs = null;
            if (ps.Handle != null)
            {
                ps.Release();
            }

            ps = null;
            layout.Release();
            layout = null;
            Compile(true);
            initialized = true;
        }

        private unsafe void Compile(bool bypassCache = false)
        {
            if (desc.VertexShader != null)
            {
                Shader* shader;
                D3D11GraphicsDevice.Compiler.GetShaderOrCompileFileWithInputSignature(desc.VertexShaderEntrypoint, desc.VertexShader, "vs_5_0", macros, &shader, out var elements, out var signature, bypassCache);
                if (shader == null || signature == null || inputElements == null && elements == null)
                {
                    valid = false;
                    return;
                }

                ComPtr<ID3D11VertexShader> vertexShader;
                device.Device.CreateVertexShader(shader->Bytecode, shader->Length, (ID3D11ClassLinkage*)null, &vertexShader.Handle);
                vs = vertexShader;
                Utils.SetDebugName(vs, $"{dbgName}.{nameof(vs)}");

                inputElements ??= elements;

                ComPtr<ID3D11InputLayout> il;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                InputElementDesc* descs = Alloc<InputElementDesc>(inputElements.Length);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                Helper.Convert(inputElements, descs);
                device.Device.CreateInputLayout(descs, (uint)inputElements.Length, (void*)signature.BufferPointer, signature.PointerSize, &il.Handle);
                Helper.Free(descs, inputElements.Length);
                Free(descs);
                layout = il;

                Utils.SetDebugName(layout, $"{dbgName}.{nameof(layout)}");

                Free(shader);
            }

            if (desc.HullShader != null)
            {
                Shader* shader;
                D3D11GraphicsDevice.Compiler.GetShaderOrCompileFile(desc.HullShaderEntrypoint, desc.HullShader, "hs_5_0", macros, &shader, bypassCache);
                if (shader == null)
                {
                    valid = false;
                    return;
                }

                ComPtr<ID3D11HullShader> hullShader;
                device.Device.CreateHullShader(shader->Bytecode, shader->Length, (ID3D11ClassLinkage*)null, &hullShader.Handle);
                hs = hullShader;
                Utils.SetDebugName(hs, $"{dbgName}.{nameof(hs)}");

                Free(shader);
            }

            if (desc.DomainShader != null)
            {
                Shader* shader;
                D3D11GraphicsDevice.Compiler.GetShaderOrCompileFile(desc.DomainShaderEntrypoint, desc.DomainShader, "ds_5_0", macros, &shader, bypassCache);
                if (shader == null)
                {
                    valid = false;
                    return;
                }

                ComPtr<ID3D11DomainShader> domainShader;
                device.Device.CreateDomainShader(shader->Bytecode, shader->Length, (ID3D11ClassLinkage*)null, &domainShader.Handle);
                ds = domainShader;
                Utils.SetDebugName(ds, $"{dbgName}.{nameof(hs)}");

                Free(shader);
            }

            if (desc.GeometryShader != null)
            {
                Shader* shader;
                D3D11GraphicsDevice.Compiler.GetShaderOrCompileFile(desc.GeometryShaderEntrypoint, desc.GeometryShader, "gs_5_0", macros, &shader, bypassCache);
                if (shader == null)
                {
                    valid = false;
                    return;
                }

                ComPtr<ID3D11GeometryShader> geometryShader;
                device.Device.CreateGeometryShader(shader->Bytecode, shader->Length, (ID3D11ClassLinkage*)null, &geometryShader.Handle);
                gs = geometryShader;
                Utils.SetDebugName(gs, $"{dbgName}.{nameof(gs)}");

                Free(shader);
            }

            if (desc.PixelShader != null)
            {
                Shader* shader;
                D3D11GraphicsDevice.Compiler.GetShaderOrCompileFile(desc.PixelShaderEntrypoint, desc.PixelShader, "ps_5_0", macros, &shader, bypassCache);
                if (shader == null)
                {
                    valid = false;
                    return;
                }

                ComPtr<ID3D11PixelShader> pixelShader;
                device.Device.CreatePixelShader(shader->Bytecode, shader->Length, (ID3D11ClassLinkage*)null, &pixelShader.Handle);
                ps = pixelShader;
                Utils.SetDebugName(ps, $"{dbgName}.{nameof(ps)}");

                Free(shader);
            }

            valid = true;
        }

        public void BeginDraw(IGraphicsContext context)
        {
            if (context is not D3D11GraphicsContext contextd3d11)
            {
                return;
            }

            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            ComPtr<ID3D11DeviceContext1> ctx = contextd3d11.DeviceContext;
            ctx.VSSetShader(vs, null, 0);
            ctx.HSSetShader(hs, null, 0);
            ctx.DSSetShader(ds, null, 0);
            ctx.GSSetShader(gs, null, 0);
            ctx.PSSetShader(ps, null, 0);

            ctx.RSSetState(rasterizerState);

            var factor = State.BlendFactor;
            float* fac = (float*)&factor;

            ctx.OMSetBlendState(blendState, fac, uint.MaxValue);
            ctx.OMSetDepthStencilState(depthStencilState, state.StencilRef);
            ctx.IASetInputLayout(layout);
            ctx.IASetPrimitiveTopology(Helper.Convert(state.Topology));
        }

        public void SetGraphicsPipeline(ComPtr<ID3D11DeviceContext1> context, Viewport viewport)
        {
            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            context.VSSetShader(vs, null, 0);
            context.HSSetShader(hs, null, 0);
            context.DSSetShader(ds, null, 0);
            context.GSSetShader(gs, null, 0);
            context.PSSetShader(ps, null, 0);

            var dViewport = Helper.Convert(viewport);
            context.RSSetViewports(1, &dViewport);
            context.RSSetState(rasterizerState);

            var factor = State.BlendFactor;
            float* fac = (float*)&factor;

            context.OMSetBlendState(blendState, fac, uint.MaxValue);
            context.OMSetDepthStencilState(depthStencilState, state.StencilRef);
            context.IASetInputLayout(layout);
            context.IASetPrimitiveTopology(Helper.Convert(state.Topology));
        }

        public void SetGraphicsPipeline(ComPtr<ID3D11DeviceContext1> context)
        {
            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            context.VSSetShader(vs, null, 0);
            context.HSSetShader(hs, null, 0);
            context.DSSetShader(ds, null, 0);
            context.GSSetShader(gs, null, 0);
            context.PSSetShader(ps, null, 0);

            context.RSSetState(rasterizerState);

            var factor = State.BlendFactor;
            float* fac = (float*)&factor;

            context.OMSetBlendState(blendState, fac, uint.MaxValue);
            context.OMSetDepthStencilState(depthStencilState, state.StencilRef);
            context.IASetInputLayout(layout);
            context.IASetPrimitiveTopology(Helper.Convert(state.Topology));
        }

        public void EndDraw(IGraphicsContext context)
        {
        }

        public void DrawInstanced(IGraphicsContext context, uint vertexCount, uint instanceCount, uint vertexOffset, uint instanceOffset)
        {
            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            BeginDraw(context);
            context.DrawInstanced(vertexCount, instanceCount, vertexOffset, instanceOffset);
            EndDraw(context);
        }

        public void DrawIndexedInstanced(IGraphicsContext context, uint indexCount, uint instanceCount, uint indexOffset, int vertexOffset, uint instanceOffset)
        {
            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            BeginDraw(context);
            context.DrawIndexedInstanced(indexCount, instanceCount, indexOffset, vertexOffset, instanceOffset);
            EndDraw(context);
        }

        public void DrawInstanced(IGraphicsContext context, IBuffer args, uint stride)
        {
            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            BeginDraw(context);
            context.DrawInstancedIndirect(args, stride);
            EndDraw(context);
        }

        public void DrawIndexedInstancedIndirect(IGraphicsContext context, IBuffer args, uint stride)
        {
            if (!initialized)
            {
                return;
            }

            if (!valid)
            {
                return;
            }

            BeginDraw(context);
            context.DrawIndexedInstancedIndirect(args, stride);
            EndDraw(context);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                PipelineManager.Unregister(this);

                if (vs.Handle != null)
                {
                    vs.Release();
                }

                if (hs.Handle != null)
                {
                    hs.Release();
                }

                if (ds.Handle != null)
                {
                    ds.Release();
                }

                if (gs.Handle != null)
                {
                    gs.Release();
                }

                if (ps.Handle != null)
                {
                    ps.Release();
                }

                if (layout.Handle != null)
                {
                    layout.Release();
                }

                if (rasterizerState.Handle != null)
                {
                    rasterizerState.Release();
                }

                if (rasterizerState.Handle != null)
                {
                    depthStencilState.Release();
                }

                if (rasterizerState.Handle != null)
                {
                    blendState.Release();
                }

                disposedValue = true;
            }
        }

        ~GraphicsPipeline()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}