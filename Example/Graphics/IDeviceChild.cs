namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Core.Unsafes;
    using System.Runtime.CompilerServices;

    public interface IDeviceChild : INative
    {
        string? DebugName { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] set; }

        bool IsDisposed { get; }

        event EventHandler? OnDisposed;
    }
}