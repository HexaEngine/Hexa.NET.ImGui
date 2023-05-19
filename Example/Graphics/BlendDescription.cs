namespace HexaEngine.Core.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public struct BlendDescription : IEquatable<BlendDescription>
    {
        public static readonly BlendDescription Opaque = new(Blend.One, Blend.Zero);

        public static readonly BlendDescription AlphaBlend = new(Blend.One, Blend.InverseSourceAlpha);

        public static readonly BlendDescription Additive = new(Blend.SourceAlpha, Blend.One);

        public static readonly BlendDescription NonPremultiplied = new(Blend.SourceAlpha, Blend.InverseSourceAlpha);

        public const int SimultaneousRenderTargetCount = unchecked(8);

        [DefaultValue(false)]
        public bool AlphaToCoverageEnable = false;

        [DefaultValue(false)]
        public bool IndependentBlendEnable = false;

        public RenderTargetBlendDescription[] RenderTarget = new RenderTargetBlendDescription[SimultaneousRenderTargetCount];

        public BlendDescription()
        {
        }

        public BlendDescription(Blend sourceBlend, Blend destinationBlend) : this(sourceBlend, destinationBlend, sourceBlend, destinationBlend)
        {
        }

        public BlendDescription(Blend sourceBlend, Blend destinationBlend, Blend srcBlendAlpha, Blend destBlendAlpha)
            : this()
        {
            AlphaToCoverageEnable = false;
            IndependentBlendEnable = false;

            for (int i = 0; i < SimultaneousRenderTargetCount; i++)
            {
                RenderTarget[i].SourceBlend = sourceBlend;
                RenderTarget[i].DestinationBlend = destinationBlend;
                RenderTarget[i].BlendOperation = BlendOperation.Add;
                RenderTarget[i].SourceBlendAlpha = srcBlendAlpha;
                RenderTarget[i].DestinationBlendAlpha = destBlendAlpha;
                RenderTarget[i].BlendOperationAlpha = BlendOperation.Add;
                RenderTarget[i].RenderTargetWriteMask = ColorWriteEnable.All;
                RenderTarget[i].IsBlendEnabled = IsBlendEnabled(ref RenderTarget[i]);
            }
        }

        private static bool IsBlendEnabled(ref RenderTargetBlendDescription renderTarget)
        {
            return renderTarget.BlendOperationAlpha != BlendOperation.Add
                    || renderTarget.SourceBlendAlpha != Blend.One
                    || renderTarget.DestinationBlendAlpha != Blend.Zero
                    || renderTarget.BlendOperation != BlendOperation.Add
                    || renderTarget.SourceBlend != Blend.One
                    || renderTarget.DestinationBlend != Blend.Zero;
        }

        public override bool Equals(object? obj)
        {
            return obj is BlendDescription description && Equals(description);
        }

        public bool Equals(BlendDescription other)
        {
            return AlphaToCoverageEnable == other.AlphaToCoverageEnable &&
                   IndependentBlendEnable == other.IndependentBlendEnable &&
                   EqualityComparer<RenderTargetBlendDescription[]>.Default.Equals(RenderTarget, other.RenderTarget);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AlphaToCoverageEnable, IndependentBlendEnable, RenderTarget);
        }

        public static bool operator ==(BlendDescription left, BlendDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlendDescription left, BlendDescription right)
        {
            return !(left == right);
        }
    }
}