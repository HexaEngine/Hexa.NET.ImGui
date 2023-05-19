namespace HexaEngine.Core.Graphics
{
    using System;
    using System.ComponentModel;

    public struct RasterizerDescription : IEquatable<RasterizerDescription>
    {
        public const int DefaultDepthBias = unchecked(0);
        public const float DefaultDepthBiasClamp = unchecked((float)0.0F);
        public const float DefaultSlopeScaledDepthBias = unchecked((float)0.0F);

        [DefaultValue(FillMode.Solid)]
        public FillMode FillMode = FillMode.Solid;

        [DefaultValue(CullMode.Back)]
        public CullMode CullMode = CullMode.Back;

        [DefaultValue(false)]
        public bool FrontCounterClockwise;

        [DefaultValue(DefaultDepthBias)]
        public int DepthBias;

        [DefaultValue(DefaultDepthBiasClamp)]
        public float DepthBiasClamp;

        [DefaultValue(DefaultSlopeScaledDepthBias)]
        public float SlopeScaledDepthBias;

        [DefaultValue(true)]
        public bool DepthClipEnable;

        [DefaultValue(false)]
        public bool ScissorEnable;

        [DefaultValue(true)]
        public bool MultisampleEnable;

        [DefaultValue(false)]
        public bool AntialiasedLineEnable;

        /// <summary>
        /// A built-in description with settings with settings for not culling any primitives.
        /// </summary>
        public static readonly RasterizerDescription CullNone = new(CullMode.None, FillMode.Solid);

        /// <summary>
        /// A built-in description with settings for culling primitives with clockwise winding order.
        /// </summary>
        public static readonly RasterizerDescription CullFront = new(CullMode.Front, FillMode.Solid);

        /// <summary>
        /// A built-in description with settings for culling primitives with counter-clockwise winding order.
        /// </summary>
        public static readonly RasterizerDescription CullBack = new(CullMode.Back, FillMode.Solid);

        /// <summary>
        /// A built-in description with settings for not culling any primitives and wireframe fill mode.
        /// </summary>
        public static readonly RasterizerDescription Wireframe = new(CullMode.None, FillMode.Wireframe);

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerDescription"/> class.
        /// </summary>
        /// <param name="cullMode">A <see cref="CullMode"/> value that specifies that triangles facing the specified direction are not drawn..</param>
        /// <param name="fillMode">A <see cref="FillMode"/> value that specifies the fill mode to use when rendering.</param>
        public RasterizerDescription(CullMode cullMode, FillMode fillMode)
        {
            CullMode = cullMode;
            FillMode = fillMode;
            FrontCounterClockwise = false;
            DepthBias = DefaultDepthBias;
            DepthBiasClamp = DefaultDepthBiasClamp;
            SlopeScaledDepthBias = DefaultSlopeScaledDepthBias;
            DepthClipEnable = true;
            ScissorEnable = false;
            MultisampleEnable = true;
            AntialiasedLineEnable = false;
        }

        public override bool Equals(object? obj)
        {
            return obj is RasterizerDescription description && Equals(description);
        }

        public bool Equals(RasterizerDescription other)
        {
            return FillMode == other.FillMode &&
                   CullMode == other.CullMode &&
                   FrontCounterClockwise == other.FrontCounterClockwise &&
                   DepthBias == other.DepthBias &&
                   DepthBiasClamp == other.DepthBiasClamp &&
                   SlopeScaledDepthBias == other.SlopeScaledDepthBias &&
                   DepthClipEnable == other.DepthClipEnable &&
                   ScissorEnable == other.ScissorEnable &&
                   MultisampleEnable == other.MultisampleEnable &&
                   AntialiasedLineEnable == other.AntialiasedLineEnable;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(FillMode);
            hash.Add(CullMode);
            hash.Add(FrontCounterClockwise);
            hash.Add(DepthBias);
            hash.Add(DepthBiasClamp);
            hash.Add(SlopeScaledDepthBias);
            hash.Add(DepthClipEnable);
            hash.Add(ScissorEnable);
            hash.Add(MultisampleEnable);
            hash.Add(AntialiasedLineEnable);
            return hash.ToHashCode();
        }

        public static bool operator ==(RasterizerDescription left, RasterizerDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RasterizerDescription left, RasterizerDescription right)
        {
            return !(left == right);
        }
    }
}