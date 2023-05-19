namespace HexaEngine.Core.Graphics
{
    using System;
    using System.Runtime.CompilerServices;

    public unsafe class DrawIndirectArgsBuffer<T> : IDisposable where T : unmanaged
    {
        private const int DefaultCapacity = 64;
        private readonly IGraphicsDevice device;

        private IBuffer buffer;
        private string dbgName;
        private BufferDescription description;

        private T* items;
        private uint count;
        private bool isDirty;
        private uint capacity;

        private bool disposedValue;

        public DrawIndirectArgsBuffer(IGraphicsDevice device, CpuAccessFlags cpuAccessFlags, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            dbgName = $"CB: {filename}, Line:{lineNumber}";
            description = new(sizeof(T) * DefaultCapacity, BindFlags.ShaderResource, Usage.Default, cpuAccessFlags, ResourceMiscFlag.DrawIndirectArguments, sizeof(T));
            if (cpuAccessFlags.HasFlag(CpuAccessFlags.Write))
            {
                description.Usage = Usage.Dynamic;
            }
            if (cpuAccessFlags.HasFlag(CpuAccessFlags.Read))
            {
                description.Usage = Usage.Staging;
            }

            capacity = DefaultCapacity;
            this.device = device;
            items = Alloc<T>(DefaultCapacity);
            Zero(items, (uint)(capacity * sizeof(T)));
            buffer = device.CreateBuffer(items, DefaultCapacity, description);
            buffer.DebugName = dbgName;
        }

        public IBuffer Buffer => buffer;

        public uint Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => capacity;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value <= 0)
                {
                    return;
                }

                if (value == capacity)
                {
                    return;
                }

                if (value < capacity)
                {
                    return;
                }

                var tmp = Alloc<T>((int)value);
                System.Buffer.MemoryCopy(items, tmp, value * sizeof(T), count * sizeof(T) > value * sizeof(T) ? value * sizeof(T) : count * sizeof(T));
                Free(items);
                items = tmp;

                capacity = value;
                count = capacity < count ? capacity : count;

                buffer.Dispose();
                buffer = device.CreateBuffer(items, capacity, description);
                buffer.DebugName = dbgName;
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

        public void Add(T args)
        {
            uint index = count;
            count++;
            EnsureCapacity(count);
            items[index] = args;
            isDirty = true;
        }

        public void Remove(int index)
        {
            var size = (count - index) * sizeof(T);
            System.Buffer.MemoryCopy(&items[index + 1], &items[index], size, size);
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

        public void CopyTo(IGraphicsContext context, IBuffer buffer)
        {
            context.CopyResource(buffer, this.buffer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                buffer.Dispose();
                count = 0;
                capacity = 0;
                disposedValue = true;
            }
        }

        ~DrawIndirectArgsBuffer()
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