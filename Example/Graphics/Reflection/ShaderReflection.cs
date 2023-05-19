namespace HexaEngine.Core.Graphics.Reflection
{
    using System;

    public struct ShaderReflection
    {
        public uint Version;
        public unsafe byte* Creator;
        public uint Flags;
        public uint ConstantBuffers;
        public uint BoundResources;
        public uint InputParameters;
        public uint OutputParameters;
        public uint InstructionCount;
        public uint TempRegisterCount;
        public uint TempArrayCount;
        public uint DefCount;
        public uint DclCount;
        public uint TextureNormalInstructions;
        public uint TextureLoadInstructions;
        public uint TextureCompInstructions;
        public uint TextureBiasInstructions;
        public uint TextureGradientInstructions;
        public uint FloatInstructionCount;
        public uint IntInstructionCount;
        public uint UintInstructionCount;
        public uint StaticFlowControlCount;
        public uint DynamicFlowControlCount;
        public uint MacroInstructionCount;
        public uint ArrayInstructionCount;
        public uint CutInstructionCount;
        public uint EmitInstructionCount;
        public PrimitiveTopology GSOutputTopology;
        public uint GSMaxOutputVertexCount;
        public Primitive InputPrimitive;
        public uint PatchConstantParameters;
        public uint CGSInstanceCount;
        public uint CControlPoints;
        public TessellatorOutputPrimitive HSOutputPrimitive;
        public TessellatorPartitioning HSPartitioning;
        public TessellatorDomain TessellatorDomain;
        public uint CBarrierInstructions;
        public uint CInterlockedInstructions;
        public uint CTextureStoreInstructions;
    }

    public struct SignatureParameterDescription
    {
        public string SemanticName;
        public uint SemanticIndex;
        public uint Register;
        public Name SystemValueType;
        public RegisterComponentType ComponentType;
        public byte Mask;
        public byte ReadWriteMask;
        public uint Stream;
        public MinPrecision MinPrecision;
    }

    public enum MinPrecision
    {
        Default = 0,
        Float16 = 1,
        Float28 = 2,
        Reserved = 3,
        Sint16 = 4,
        Uint16 = 5,
        Any16 = 240,
        Any10 = 241
    }

    [Flags]
    public enum RegisterComponentType
    {
        Unknown = 0x0,
        Uint32 = 0x1,
        Sint32 = 0x2,
        Float32 = 0x3,
    }

    public enum Name
    {
        Undefined = 0,
        Position = 1,
        ClipDistance = 2,
        CullDistance = 3,
        RenderTargetArrayIndex = 4,
        ViewportArrayIndex = 5,
        VertexID = 6,
        PrimitiveID = 7,
        InstanceID = 8,
        IsFrontFace = 9,
        SampleIndex = 10,
        FinalQuadEdgeTessfactor = 11,
        FinalQuadInsideTessfactor = 12,
        FinalTriEdgeTessfactor = 13,
        FinalTriInsideTessfactor = 14,
        FinalLineDetailTessfactor = 0xF,
        FinalLineDensityTessfactor = 0x10,
        Barycentrics = 23,
        Shadingrate = 24,
        Cullprimitive = 25,
        Target = 0x40,
        Depth = 65,
        Coverage = 66,
        DepthGreaterEqual = 67,
        DepthLessEqual = 68,
        StencilRef = 69,
        InnerCoverage = 70,
    }

    public struct ShaderInputBindDescription
    {
        public string Name;
        public ShaderInputType Type;
        public uint BindPoint;
        public uint BindCount;
        public uint UFlags;
        public ResourceReturnType ReturnType;
        public SrvDimension Dimension;
        public uint NumSamples;
    }

    public enum ShaderInputType
    {
        SitCbuffer = 0,
        SitTbuffer = 1,
        SitTexture = 2,
        SitSampler = 3,
        SitUavRwtyped = 4,
        SitStructured = 5,
        SitUavRwstructured = 6,
        SitByteaddress = 7,
        SitUavRwbyteaddress = 8,
        SitUavAppendStructured = 9,
        SitUavConsumeStructured = 10,
        SitUavRwstructuredWithCounter = 11,
        SitRtaccelerationstructure = 12,
        SitUavFeedbacktexture = 13,
    }

    [Flags]
    public enum ResourceReturnType
    {
        None = 0x0,
        Unorm = 0x1,
        SNorm = 0x2,
        Sint = 0x3,
        Uint = 0x4,
        Float = 0x5,
        Mixed = 0x6,
        Double = 0x7,
        Continued = 0x8,
    }

    public enum SrvDimension
    {
        Unknown = 0,
        Buffer = 1,
        Texture1D = 2,
        Texture1Darray = 3,
        Texture2D = 4,
        Texture2Darray = 5,
        Texture2Dms = 6,
        Texture2Dmsarray = 7,
        Texture3D = 8,
        Texturecube = 9,
        Texturecubearray = 10,
        Bufferex = 11,
    }
}