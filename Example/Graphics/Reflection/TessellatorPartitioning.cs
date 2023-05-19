namespace HexaEngine.Core.Graphics.Reflection
{
    using System;

    [Flags]
    public enum TessellatorPartitioning
    {
        Undefined = 0x0,
        Integer = 0x1,
        Pow2 = 0x2,
        FractionalOdd = 0x3,
        FractionalEven = 0x4,
    }
}