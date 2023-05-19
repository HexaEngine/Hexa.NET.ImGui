namespace HexaEngine.Core.Graphics
{
    public enum ComparisonFunction : int
    {
        Never = unchecked(1),
        Less = unchecked(2),
        Equal = unchecked(3),
        LessEqual = unchecked(4),
        Greater = unchecked(5),
        NotEqual = unchecked(6),
        GreaterEqual = unchecked(7),
        Always = unchecked(8)
    }
}