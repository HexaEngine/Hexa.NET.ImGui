namespace HexaEngine.Core.Graphics.Buffers
{
    using HexaEngine.Core.Graphics;
    using System.Runtime.CompilerServices;

    public unsafe class StructuredBuffer<T> : IBuffer where T : unmanaged
    {
        private const int DefaultCapacity = 128;
        private readonly IGraphicsDevice device;

        private readonly string dbgName;
        private IBuffer buffer;
        private IShaderResourceView srv;
        private BufferDescription description;

        private T* items;
        private uint count;
        private volatile bool isDirty;
        private uint capacity;

        private bool disposedValue;

        public StructuredBuffer(IGraphicsDevice device, CpuAccessFlags cpuAccessFlags, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            dbgName = $"StructuredBuffer: {filename}, Line:{lineNumber}";
            description = new(sizeof(T) * DefaultCapacity, BindFlags.ShaderResource, Usage.Default, cpuAccessFlags, ResourceMiscFlag.BufferStructured, sizeof(T));
            if (cpuAccessFlags.HasFlag(CpuAccessFlags.Write))
            {
                description.Usage = Usage.Dynamic;
            }
            if (cpuAccessFlags.HasFlag(CpuAccessFlags.Read))
            {
                description.Usage = Usage.Staging;
            }
            capacity = DefaultCapacity;
            items = Alloc<T>(DefaultCapacity);
            Zero(items, DefaultCapacity * sizeof(T));
            buffer = device.CreateBuffer(items, DefaultCapacity, description);
            buffer.DebugName = dbgName;
            srv = device.CreateShaderResourceView(buffer);
            srv.DebugName = dbgName + ".SRV";
            this.device = device;
        }

        public event EventHandler? OnDisposed
        {
            add
            {
                buffer.OnDisposed += value;
            }

            remove
            {
                buffer.OnDisposed -= value;
            }
        }

        public IShaderResourceView SRV => srv;

        public uint Count => count;

        public uint Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => capacity;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var tmp = Alloc<T>((int)value);
                var oldsize = count * sizeof(T);
                var newsize = value * sizeof(T);
                Buffer.MemoryCopy(items, tmp, newsize, oldsize > newsize ? newsize : oldsize);
                Free(items);
                items = tmp;
                capacity = value;
                count = capacity < count ? capacity : count;
                srv.Dispose();
                buffer.Dispose();
                buffer = device.CreateBuffer(items, capacity, description);
                buffer.DebugName = dbgName;
                srv = device.CreateShaderResourceView(buffer);
                srv.DebugName = dbgName + ".SRV";
            }
        }

        public BufferDescription Description => buffer.Description;

        public int Length => buffer.Length;

        public ResourceDimension Dimension => buffer.Dimension;

        public nint NativePointer => buffer.NativePointer;

        public string? DebugName { get => buffer.DebugName; set => buffer.DebugName = value; }

        public bool IsDisposed => buffer.IsDisposed;

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => items[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                items[index] = value;
                isDirty = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetCounter()
        {
            count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(uint capacity)
        {
            if (this.capacity < capacity)
            {
                Grow(capacity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Grow(uint capacity)
        {
            uint newcapacity = count == 0 ? DefaultCapacity : 2 * count;

            if (newcapacity < capacity)
            {
                newcapacity = capacity;
            }

            Capacity = newcapacity;
        }

        public void Add(T item)
        {
            uint index = count;
            count++;
            EnsureCapacity(count);
            items[index] = item;
            isDirty = true;
        }

        public void Remove(int index)
        {
            var size = (count - index) * sizeof(T);
            Buffer.MemoryCopy(&items[index + 1], &items[index], size, size);
            isDirty = true;
        }

        public bool Update(IGraphicsContext context)
        {
            if (isDirty)
            {
                context.Write(buffer, items, (int)(count * sizeof(T)));
                isDirty = false;
                return true;
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                srv.Dispose();
                buffer.Dispose();
                Free(items);
                count = 0;
                capacity = 0;
                disposedValue = true;
            }
        }

        ~StructuredBuffer()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}