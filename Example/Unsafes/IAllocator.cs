namespace HexaEngine.Core.Unsafes
{
    public unsafe interface IAllocator
    {
        void* Allocate(nint width);

        T* Allocate<T>() where T : unmanaged;

        void** Allocate(uint width, uint height);

        T* Allocate<T>(uint count) where T : unmanaged;

        T** AllocateArray<T>(uint count) where T : unmanaged;

        void Free(void* ptr);

        void Free<T>(T* ptr) where T : unmanaged, IFreeable;
    }
}