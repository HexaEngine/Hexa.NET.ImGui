namespace HexaEngine.Core.Graphics
{
    public struct ShaderBinding
    {
        public ShaderBinding(ShaderStage stage, int slot)
        {
            Stage = stage;
            Slot = slot;
        }

        public ShaderStage Stage;
        public int Slot;
    }

    public struct BoundResource
    {
        public IShaderResourceView? Resource;
        public List<ShaderBinding> Bindings;

        public BoundResource(IShaderResourceView? resource, params ShaderBinding[] bindings)
        {
            Resource = resource;
            Bindings = new(bindings);
        }

        public BoundResource(IShaderResourceView? resource, ShaderStage stage, int slot)
        {
            Resource = resource;
            Bindings = new List<ShaderBinding>();
            Bindings.Add(new ShaderBinding(stage, slot));
        }

        public void Bind(IGraphicsContext context)
        {
            for (int i = 0; i < Bindings.Count; i++)
            {
                var binding = Bindings[i];
                switch (binding.Stage)
                {
                    case ShaderStage.Vertex:
                        context.VSSetShaderResource(Resource, Bindings[i].Slot);
                        break;

                    case ShaderStage.Hull:
                        context.HSSetShaderResource(Resource, Bindings[i].Slot);
                        break;

                    case ShaderStage.Domain:
                        context.DSSetShaderResource(Resource, Bindings[i].Slot);
                        break;

                    case ShaderStage.Geometry:
                        context.GSSetShaderResource(Resource, Bindings[i].Slot);
                        break;

                    case ShaderStage.Pixel:
                        context.PSSetShaderResource(Resource, Bindings[i].Slot);
                        break;

                    case ShaderStage.Compute:
                        context.CSSetShaderResource(Resource, Bindings[i].Slot);
                        break;
                }
            }
        }
    }

    public struct BoundSampler
    {
        public ISamplerState? Sampler;
        public List<ShaderBinding> Bindings;

        public BoundSampler(ISamplerState? sampler, params ShaderBinding[] bindings)
        {
            Sampler = sampler;
            Bindings = new(bindings);
        }

        public BoundSampler(ISamplerState? sampler, ShaderStage stage, int slot)
        {
            Sampler = sampler;
            Bindings = new List<ShaderBinding>();
            Bindings.Add(new ShaderBinding(stage, slot));
        }

        public void Bind(IGraphicsContext context)
        {
            for (int i = 0; i < Bindings.Count; i++)
            {
                var binding = Bindings[i];
                switch (binding.Stage)
                {
                    case ShaderStage.Vertex:
                        context.VSSetSampler(Sampler, Bindings[i].Slot);
                        break;

                    case ShaderStage.Hull:
                        context.HSSetSampler(Sampler, Bindings[i].Slot);
                        break;

                    case ShaderStage.Domain:
                        context.DSSetSampler(Sampler, Bindings[i].Slot);
                        break;

                    case ShaderStage.Geometry:
                        context.GSSetSampler(Sampler, Bindings[i].Slot);
                        break;

                    case ShaderStage.Pixel:
                        context.PSSetSampler(Sampler, Bindings[i].Slot);
                        break;

                    case ShaderStage.Compute:
                        context.CSSetSampler(Sampler, Bindings[i].Slot);
                        break;
                }
            }
        }
    }

    public struct BoundConstant
    {
        public IBuffer? Constant;
        public List<ShaderBinding> Bindings;

        public BoundConstant(IBuffer? constant, params ShaderBinding[] bindings)
        {
            Constant = constant;
            Bindings = new(bindings);
        }

        public BoundConstant(IBuffer? constant, ShaderStage stage, int slot)
        {
            Constant = constant;
            Bindings = new List<ShaderBinding>();
            Bindings.Add(new ShaderBinding(stage, slot));
        }

        public void Bind(IGraphicsContext context)
        {
            for (int i = 0; i < Bindings.Count; i++)
            {
                var binding = Bindings[i];
                switch (binding.Stage)
                {
                    case ShaderStage.Vertex:
                        context.VSSetConstantBuffer(Constant, Bindings[i].Slot);
                        break;

                    case ShaderStage.Hull:
                        context.HSSetConstantBuffer(Constant, Bindings[i].Slot);
                        break;

                    case ShaderStage.Domain:
                        context.DSSetConstantBuffer(Constant, Bindings[i].Slot);
                        break;

                    case ShaderStage.Geometry:
                        context.GSSetConstantBuffer(Constant, Bindings[i].Slot);
                        break;

                    case ShaderStage.Pixel:
                        context.PSSetConstantBuffer(Constant, Bindings[i].Slot);
                        break;

                    case ShaderStage.Compute:
                        context.CSSetConstantBuffer(Constant, Bindings[i].Slot);
                        break;
                }
            }
        }
    }
}