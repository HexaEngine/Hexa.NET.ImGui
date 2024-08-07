namespace ExampleFramework.NodeEditor
{
    public enum PinType
    {
        DontCare,
        Flow,
        Bool,
        Int,
        UInt,
        Float,
        Float2,
        Float3,
        Float4,
        VectorAny,
        String,
        Object,
        Function,
        Delegate,
        TextureAny,
        Texture1D,
        Texture1DArray,
        Texture2D,
        Texture2DMS,
        Texture2DArray,
        Texture2DMSArray,
        TextureCube,
        TextureCubeArray,
        Texture3D,
        ShaderResourceView,
        RenderTargetView,
        ConstantBuffer,
        Vertices,
        Buffer,
        Sampler,
    }

    public enum PinFlags
    {
        None = 0,
        ColorEdit,
        ColorPicker,
        Slider,
    }
}