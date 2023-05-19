namespace HexaEngine.Core.Graphics
{
    public enum MapMode : int
    {
        Read = unchecked(1),
        Write = unchecked(2),
        ReadWrite = unchecked(3),
        WriteDiscard = unchecked(4),
        WriteNoOverwrite = unchecked(5)
    }
}