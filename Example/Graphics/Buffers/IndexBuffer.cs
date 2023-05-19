namespace HexaEngine.Core.Graphics.Buffers
{
    using HexaEngine.Core.Graphics;
    using System.Runtime.CompilerServices;

    public unsafe class IndexBuffer : IBuffer
    {
        private const int DefaultCapacity = 8;

        private readonly IGraphicsDevice device;
        private readonly string dbgName;

        private IBuffer buffer;
        private BufferDescription description;

        private uint* items;
        private uint count;
        private uint capacity;

        private bool isDirty;

        private bool disposedValue;

        public IndexBuffer(IGraphicsDevice device, CpuAccessFlags flags, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            this.device = device;
            dbgName = $"IndexBuffer: {filename}, Line:{lineNumber}";

            items = Alloc<uint>(DefaultCapacity);
            ZeroRange(items, DefaultCapacity);
            capacity = DefaultCapacity;

            description = new(sizeof(uint) * DefaultCapacity, BindFlags.IndexBuffer, Usage.Default, flags);
            if ((flags & CpuAccessFlags.Write) != 0)
            {
                description.Usage = Usage.Dynamic;
            }
            if ((flags & CpuAccessFlags.Read) != 0)
            {
                description.Usage = Usage.Staging;
            }
            if ((flags & CpuAccessFlags.None) != 0)
            {
                throw new InvalidOperationException("If cpu access flags are none initial data must be provided");
            }

            buffer = device.CreateBuffer(description);
        }

        public IndexBuffer(IGraphicsDevice device, CpuAccessFlags flags, uint[] indices, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            this.device = device;
            dbgName = $"IndexBuffer: {filename}, Line:{lineNumber}";

            capacity = (uint)indices.Length;
            count = capacity;

            description = new(sizeof(uint) * (int)capacity, BindFlags.IndexBuffer, Usage.Default, flags);

            if ((flags & CpuAccessFlags.None) != 0)
            {
                description.Usage = Usage.Immutable;
                fixed (uint* ptr = indices)
                {
                    buffer = device.CreateBuffer(ptr, capacity, description);
                }
                return;
            }
            if ((flags & CpuAccessFlags.Write) != 0)
            {
                description.Usage = Usage.Dynamic;
            }
            if ((flags & CpuAccessFlags.Read) != 0)
            {
                description.Usage = Usage.Staging;
            }

            items = AllocCopy(indices);
            buffer = device.CreateBuffer(items, capacity, description);
        }

        public IndexBuffer(IGraphicsDevice device, CpuAccessFlags flags, uint capacity, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            this.device = device;
            dbgName = $"IndexBuffer: {filename}, Line:{lineNumber}";

            this.capacity = capacity;

            description = new(sizeof(uint) * (int)capacity, BindFlags.IndexBuffer, Usage.Default, flags);
            if ((flags & CpuAccessFlags.None) != 0)
            {
                throw new InvalidOperationException("If cpu access flags are none initial data must be provided");
            }
            if ((flags & CpuAccessFlags.Write) != 0)
            {
                description.Usage = Usage.Dynamic;
            }
            if ((flags & CpuAccessFlags.Read) != 0)
            {
                description.Usage = Usage.Staging;
            }

            items = Alloc<uint>(capacity);
            ZeroRange(items, capacity);
            buffer = device.CreateBuffer(items, capacity, description);
            buffer.DebugName = dbgName;
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

        public uint Count => count;

        public uint Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => capacity;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var tmp = Alloc<uint>((int)value);
                var oldsize = count * sizeof(uint);
                var newsize = value * sizeof(uint);
                Buffer.MemoryCopy(items, tmp, newsize, oldsize > newsize ? newsize : oldsize);
                Free(items);
                items = tmp;
                capacity = value;
                count = capacity < count ? capacity : count;
                buffer.Dispose();
                buffer = device.CreateBuffer(items, capacity, description);
                buffer.DebugName = dbgName;
            }
        }

        public BufferDescription Description => buffer.Description;

        public int Length => buffer.Length;

        public ResourceDimension Dimension => buffer.Dimension;

        public nint NativePointer => buffer.NativePointer;

        public string? DebugName { get => buffer.DebugName; set => buffer.DebugName = value; }

        public bool IsDisposed => buffer.IsDisposed;

        public uint this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => items[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (items[index] != value)
                {
                    items[index] = value;
                    isDirty = true;
                }
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

        public void Add(params uint[] indices)
        {
            uint index = count;
            count += (uint)indices.Length;
            EnsureCapacity(count);

            for (int i = 0; i < indices.Length; i++)
            {
                items[index + i] = indices[i];
            }

            isDirty = true;
        }

        public void Remove(int index)
        {
            var size = (count - index) * sizeof(uint);
            Buffer.MemoryCopy(&items[index + 1], &items[index], size, size);
            isDirty = true;
        }

        public bool Update(IGraphicsContext context)
        {
            if (isDirty)
            {
                context.Write(buffer, items, (int)(count * sizeof(uint)));
                isDirty = false;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            count = 0;
            isDirty = true;
        }

        public void FlushMemory()
        {
            Free(items);
        }

        public void CopyTo(IGraphicsContext context, IBuffer buffer)
        {
            context.CopyResource(buffer, this.buffer);
        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                buffer?.Dispose();
                capacity = 0;
                count = 0;
                Free(items);

                disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}