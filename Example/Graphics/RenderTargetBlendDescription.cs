namespace HexaEngine.Core.Graphics
{
    using System;
    using System.ComponentModel;

    public struct RenderTargetBlendDescription : IEquatable<RenderTargetBlendDescription>
    {
        [DefaultValue(false)]
        public bool IsBlendEnabled;

        [DefaultValue(Blend.One)]
        public Blend SourceBlend;

        [DefaultValue(Blend.Zero)]
        public Blend DestinationBlend;

        [DefaultValue(BlendOperation.Add)]
        public BlendOperation BlendOperation;

        [DefaultValue(Blend.One)]
        public Blend SourceBlendAlpha;

        [DefaultValue(Blend.Zero)]
        public Blend DestinationBlendAlpha;

        [DefaultValue(BlendOperation.Add)]
        public BlendOperation BlendOperationAlpha;

        [DefaultValue(ColorWriteEnable.All)]
        public ColorWriteEnable RenderTargetWriteMask;

        public override bool Equals(object? obj)
        {
            return obj is RenderTargetBlendDescription description && Equals(description);
        }

        public bool Equals(RenderTargetBlendDescription other)
        {
            return IsBlendEnabled == other.IsBlendEnabled &&
                   SourceBlend == other.SourceBlend &&
                   DestinationBlend == other.DestinationBlend &&
                   BlendOperation == other.BlendOperation &&
                   SourceBlendAlpha == other.SourceBlendAlpha &&
                   DestinationBlendAlpha == other.DestinationBlendAlpha &&
                   BlendOperationAlpha == other.BlendOperationAlpha &&
                   RenderTargetWriteMask == other.RenderTargetWriteMask;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsBlendEnabled, SourceBlend, DestinationBlend, BlendOperation, SourceBlendAlpha, DestinationBlendAlpha, BlendOperationAlpha, RenderTargetWriteMask);
        }

        public static bool operator ==(RenderTargetBlendDescription left, RenderTargetBlendDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RenderTargetBlendDescription left, RenderTargetBlendDescription right)
        {
            return !(left == right);
        }
    }
}