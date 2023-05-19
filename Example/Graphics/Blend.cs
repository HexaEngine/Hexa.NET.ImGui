namespace HexaEngine.Core.Graphics
{
    public enum Blend : int
    {
        Zero = unchecked(1),
        One = unchecked(2),
        SourceColor = unchecked(3),
        InverseSourceColor = unchecked(4),
        SourceAlpha = unchecked(5),
        InverseSourceAlpha = unchecked(6),
        DestinationAlpha = unchecked(7),
        InverseDestinationAlpha = unchecked(8),
        DestinationColor = unchecked(9),
        InverseDestinationColor = unchecked(10),
        SourceAlphaSaturate = unchecked(11),
        BlendFactor = unchecked(14),
        InverseBlendFactor = unchecked(15),
        Source1Color = unchecked(16),
        InverseSource1Color = unchecked(17),
        Source1Alpha = unchecked(18),
        InverseSource1Alpha = unchecked(19)
    }
}