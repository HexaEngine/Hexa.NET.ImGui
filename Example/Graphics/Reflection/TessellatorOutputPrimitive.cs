namespace HexaEngine.Core.Graphics.Reflection
{
    using System;

    [Flags]
    public enum TessellatorOutputPrimitive
    {
        Undefined = 0x0,
        Point = 0x1,
        Line = 0x2,
        TriangleCW = 0x3,
        TriangleCcw = 0x4,
    }
}