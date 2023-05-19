namespace HexaEngine.Core.Unsafes
{
    using System;
    using System.Runtime.CompilerServices;

    public interface INative : IDisposable
    {
        public IntPtr NativePointer { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
    }
}