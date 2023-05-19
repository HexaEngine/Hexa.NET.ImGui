namespace HexaEngine.Core.Graphics
{
    [Flags]
    public enum MapFlags : int
    {
        DoNotWait = unchecked(1048576),
        None = unchecked(0)
    }

    [Flags]
    public enum Map : int
    {
        Read = 0x1,
        Write = 0x2,
        ReadWrite = 0x3,
        WriteDiscard = 0x4,
        WriteNoOverwrite = 0x5
    }
}