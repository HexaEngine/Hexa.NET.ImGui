namespace HexaEngine.Core.Graphics.Reflection
{
    using System;

    [Flags]
    public enum TessellatorDomain
    {
        Undefined = 0x0,
        Isoline = 0x1,
        Tri = 0x2,
        Quad = 0x3,
    }
}