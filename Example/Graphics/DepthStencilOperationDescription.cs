namespace HexaEngine.Core.Graphics
{
    using System;
    using System.ComponentModel;

    public struct DepthStencilOperationDescription : IEquatable<DepthStencilOperationDescription>
    {
        [DefaultValue(StencilOperation.Keep)]
        public StencilOperation StencilFailOp;

        [DefaultValue(StencilOperation.Keep)]
        public StencilOperation StencilDepthFailOp;

        [DefaultValue(StencilOperation.Keep)]
        public StencilOperation StencilPassOp;

        [DefaultValue(ComparisonFunction.Always)]
        public ComparisonFunction StencilFunc;

        /// <summary>
        /// A built-in description with default values.
        /// </summary>
        public static readonly DepthStencilOperationDescription Default = new(StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Keep, ComparisonFunction.Always);

        /// <summary>
        /// A built-in description with default values.
        /// </summary>
        public static readonly DepthStencilOperationDescription DefaultFront = new(StencilOperation.Keep, StencilOperation.Increment, StencilOperation.Keep, ComparisonFunction.Always);

        /// <summary>
        /// A built-in description with default values.
        /// </summary>
        public static readonly DepthStencilOperationDescription DefaultBack = new(StencilOperation.Keep, StencilOperation.Decrement, StencilOperation.Keep, ComparisonFunction.Always);

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilOperationDescription"/> struct.
        /// </summary>
        /// <param name="stencilFailOp">A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing fails.</param>
        /// <param name="stencilDepthFailOp">A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing passes and depth testing fails.</param>
        /// <param name="stencilPassOp">A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing and depth testing both pass.</param>
        /// <param name="stencilFunc">A <see cref="ComparisonFunction"/> value that identifies the function that compares stencil data against existing stencil data.</param>
        public DepthStencilOperationDescription(StencilOperation stencilFailOp, StencilOperation stencilDepthFailOp, StencilOperation stencilPassOp, ComparisonFunction stencilFunc)
        {
            StencilFailOp = stencilFailOp;
            StencilDepthFailOp = stencilDepthFailOp;
            StencilPassOp = stencilPassOp;
            StencilFunc = stencilFunc;
        }

        public override bool Equals(object? obj)
        {
            return obj is DepthStencilOperationDescription description && Equals(description);
        }

        public bool Equals(DepthStencilOperationDescription other)
        {
            return StencilFailOp == other.StencilFailOp &&
                   StencilDepthFailOp == other.StencilDepthFailOp &&
                   StencilPassOp == other.StencilPassOp &&
                   StencilFunc == other.StencilFunc;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StencilFailOp, StencilDepthFailOp, StencilPassOp, StencilFunc);
        }

        public static bool operator ==(DepthStencilOperationDescription left, DepthStencilOperationDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DepthStencilOperationDescription left, DepthStencilOperationDescription right)
        {
            return !(left == right);
        }
    }
}