namespace HexaEngine.Core.Graphics.Buffers
{
    using HexaEngine.Core.Graphics;
    using System;
    using System.Runtime.CompilerServices;

    public unsafe class UavBuffer : IBuffer
    {
        private readonly IGraphicsDevice device;
        private readonly Format format;
        private readonly bool canWrite;
        private readonly bool canRead;
#pragma warning disable CS0649 // Field 'UavBuffer.stride' is never assigned to, and will always have its default value 0
        private readonly int stride;
#pragma warning restore CS0649 // Field 'UavBuffer.stride' is never assigned to, and will always have its default value 0
        private readonly BufferUnorderedAccessViewFlags uavflags;
        private readonly BufferExtendedShaderResourceViewFlags srvFlags;
        private readonly string dbgName;
        private BufferDescription bufferDescription;
        private BufferDescription copyDescription;
        private IBuffer buffer;
        private IBuffer? copyBuffer;
        private IUnorderedAccessView uav;
        private IShaderResourceView srv;

        private uint length;
        private bool disposedValue;

        public UavBuffer(IGraphicsDevice device, int stride, uint length, Format format, bool canWrite, bool canRead, BufferUnorderedAccessViewFlags uavflags = BufferUnorderedAccessViewFlags.None, BufferExtendedShaderResourceViewFlags srvFlags = BufferExtendedShaderResourceViewFlags.None, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            this.device = device;
            this.format = format;
            this.canWrite = canWrite;
            this.canRead = canRead;
            this.uavflags = uavflags;
            this.srvFlags = srvFlags;
            dbgName = $"UavBuffer: {filename}, Line:{lineNumber}";
            this.length = length;
            bufferDescription = new(stride * (int)length, BindFlags.UnorderedAccess | BindFlags.ShaderResource, Usage.Default, CpuAccessFlags.None, ResourceMiscFlag.None, 0);
            buffer = device.CreateBuffer(bufferDescription);
            buffer.DebugName = dbgName;
            if (canWrite)
            {
                copyDescription = new(stride * (int)length, BindFlags.ShaderResource, Usage.Dynamic, CpuAccessFlags.Write, ResourceMiscFlag.None, 0);
                if (canRead)
                {
                    copyDescription.BindFlags = BindFlags.None;
                    copyDescription.Usage = Usage.Staging;
                    copyDescription.CPUAccessFlags = CpuAccessFlags.RW;
                }
                copyBuffer = device.CreateBuffer(copyDescription);
                copyBuffer.DebugName = dbgName + ".CopyBuffer";
            }

            uav = device.CreateUnorderedAccessView(buffer, new(buffer, format, 0, (int)length, uavflags));
            uav.DebugName = dbgName + ".UAV";
            srv = device.CreateShaderResourceView(buffer, new(buffer, format, 0, (int)length, srvFlags));
            srv.DebugName = dbgName + ".SRV";
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

        /// <summary>
        /// Gets the uav.
        /// </summary>
        /// <value>
        /// The uav.
        /// </value>
        public IUnorderedAccessView UAV => uav;

        /// <summary>
        /// Gets the SRV.
        /// </summary>
        /// <value>
        /// The SRV.
        /// </value>
        public IShaderResourceView SRV => srv;

        public IBuffer Buffer => buffer;

        /// <summary>
        /// Not null when CanWrite is true
        /// </summary>
        public IBuffer? CopyBuffer => copyBuffer;

        /// <summary>
        /// Gets a value indicating whether this instance can write.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can write; otherwise, <c>false</c>.
        /// </value>
        public bool CanWrite => canWrite;

        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can read; otherwise, <c>false</c>.
        /// </value>
        public bool CanRead => canRead;

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public uint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => length;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value == 0)
                {
                    return;
                }

                if (value == length)
                {
                    return;
                }

                if (value < length)
                {
                    return;
                }

                length = value;
                srv.Dispose();
                uav.Dispose();
                buffer.Dispose();
                copyBuffer?.Dispose();
                bufferDescription.ByteWidth = stride * (int)length;
                buffer = device.CreateBuffer(bufferDescription);
                buffer.DebugName = dbgName;
                if (canWrite)
                {
                    copyDescription.ByteWidth = stride * (int)value;
                    copyBuffer = device.CreateBuffer(copyDescription);
                    copyBuffer.DebugName = dbgName + ".CopyBuffer";
                }

                uav = device.CreateUnorderedAccessView(buffer, new(buffer, format, 0, (int)length, uavflags));
                uav.DebugName = dbgName + ".UAV";
                srv = device.CreateShaderResourceView(buffer, new(buffer, format, 0, (int)length, srvFlags));
                srv.DebugName = dbgName + ".SRV";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public BufferDescription Description => buffer.Description;

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <value>
        /// The dimension.
        /// </value>
        public ResourceDimension Dimension => buffer.Dimension;

        /// <summary>
        /// Gets the native pointer.
        /// </summary>
        /// <value>
        /// The native pointer.
        /// </value>
        public nint NativePointer => buffer.NativePointer;

        /// <summary>
        /// Gets or sets the dbgName of the debug.
        /// </summary>
        /// <value>
        /// The dbgName of the debug.
        /// </value>
        public string? DebugName { get => buffer.DebugName; set => buffer.DebugName = value; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed => buffer.IsDisposed;

        int IBuffer.Length => buffer.Length;

        public void Clear(IGraphicsContext context)
        {
            context.ClearUnorderedAccessViewUint(uav, 0, 0, 0, 0);
        }

        public void Write(IGraphicsContext context, void* src, int length)
        {
            if (copyBuffer == null)
            {
                throw new InvalidOperationException();
            }

            if (!canWrite)
            {
                throw new InvalidOperationException();
            }

            if (canRead)
            {
                context.Write(copyBuffer, src, length, Map.Write);
                context.CopyResource(buffer, copyBuffer);
            }
            else
            {
                context.Write(copyBuffer, src, length);
                context.CopyResource(buffer, copyBuffer);
            }
        }

        public void Read(IGraphicsContext context, void* dst, int length)
        {
            if (copyBuffer == null)
            {
                throw new InvalidOperationException();
            }

            if (!canRead)
            {
                throw new InvalidOperationException();
            }

            context.CopyResource(copyBuffer, buffer);
            context.Read(copyBuffer, dst, length);
        }

        public void CopyTo(IGraphicsContext context, IBuffer buffer)
        {
            context.CopyResource(buffer, this.buffer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                srv.Dispose();
                uav.Dispose();
                buffer.Dispose();
                copyBuffer?.Dispose();
                length = 0;
                disposedValue = true;
            }
        }

        ~UavBuffer()
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

    public unsafe class UavBuffer<T> : IBuffer where T : unmanaged
    {
        private const int DefaultCapacity = 64;
        private readonly IGraphicsDevice device;
        private readonly bool canWrite;
        private readonly bool canRead;
        private readonly BufferUnorderedAccessViewFlags flags;
        private readonly string dbgName;
        private BufferDescription bufferDescription;
        private BufferDescription copyDescription;
        private IBuffer buffer;
        private IBuffer? copyBuffer;
        private IUnorderedAccessView uav;
        private IShaderResourceView srv;

        private T* items;
        private uint count;
        private bool isDirty;
        private uint capacity;

        private bool disposedValue;

        public UavBuffer(IGraphicsDevice device, bool canWrite, bool canRead, Format format, BufferUnorderedAccessViewFlags uavflags = BufferUnorderedAccessViewFlags.None, BufferExtendedShaderResourceViewFlags srvFlags = BufferExtendedShaderResourceViewFlags.None, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            this.device = device;
            this.canWrite = canWrite;
            this.canRead = canRead;
            this.flags = uavflags;
            dbgName = $"UavBuffer: {filename}, Line:{lineNumber}";
            capacity = DefaultCapacity;
            items = Alloc<T>(DefaultCapacity);
            Zero(items, DefaultCapacity * sizeof(T));
            bufferDescription = new(sizeof(T) * DefaultCapacity, BindFlags.UnorderedAccess | BindFlags.ShaderResource, Usage.Default, CpuAccessFlags.None, ResourceMiscFlag.None, 0);
            buffer = device.CreateBuffer(items, DefaultCapacity, bufferDescription);
            buffer.DebugName = dbgName;
            if (canWrite)
            {
                copyDescription = new(sizeof(T) * DefaultCapacity, BindFlags.ShaderResource, Usage.Dynamic, CpuAccessFlags.Write, ResourceMiscFlag.None, 0);
                if (canRead)
                {
                    copyDescription.BindFlags = BindFlags.None;
                    copyDescription.Usage = Usage.Staging;
                    copyDescription.CPUAccessFlags = CpuAccessFlags.RW;
                }
                copyBuffer = device.CreateBuffer(copyDescription);
                copyBuffer.DebugName = dbgName + ".CopyBuffer";
            }

            uav = device.CreateUnorderedAccessView(buffer, new(buffer, format, 0, DefaultCapacity, uavflags));
            uav.DebugName = dbgName + ".UAV";
            srv = device.CreateShaderResourceView(buffer, new(buffer, format, 0, DefaultCapacity, srvFlags));
            srv.DebugName = dbgName + ".SRV";
        }

        public UavBuffer(IGraphicsDevice device, int intialCapacity, bool canWrite, bool canRead, Format format, BufferUnorderedAccessViewFlags uavflags = BufferUnorderedAccessViewFlags.None, BufferExtendedShaderResourceViewFlags srvFlags = BufferExtendedShaderResourceViewFlags.None, [CallerFilePath] string filename = "", [CallerLineNumber] int lineNumber = 0)
        {
            this.device = device;
            this.canWrite = canWrite;
            this.canRead = canRead;
            this.flags = uavflags;
            dbgName = $"UavBuffer: {filename}, Line:{lineNumber}";
            capacity = (uint)intialCapacity;
            items = Alloc<T>(intialCapacity);
            Zero(items, intialCapacity * sizeof(T));
            bufferDescription = new(sizeof(T) * intialCapacity, BindFlags.UnorderedAccess | BindFlags.ShaderResource, Usage.Default, CpuAccessFlags.None, ResourceMiscFlag.None, 0);
            buffer = device.CreateBuffer(items, (uint)intialCapacity, bufferDescription);
            buffer.DebugName = dbgName;
            if (canWrite)
            {
                copyDescription = new(sizeof(T) * intialCapacity, BindFlags.ShaderResource, Usage.Dynamic, CpuAccessFlags.Write, ResourceMiscFlag.None, 0);
                if (canRead)
                {
                    copyDescription.BindFlags = BindFlags.None;
                    copyDescription.Usage = Usage.Staging;
                    copyDescription.CPUAccessFlags = CpuAccessFlags.RW;
                }
                copyBuffer = device.CreateBuffer(copyDescription);
                copyBuffer.DebugName = dbgName + ".CopyBuffer";
            }

            uav = device.CreateUnorderedAccessView(buffer, new(buffer, format, 0, intialCapacity, uavflags));
            uav.DebugName = dbgName + ".UAV";
            srv = device.CreateShaderResourceView(buffer, new(buffer, format, 0, intialCapacity, srvFlags));
            srv.DebugName = dbgName + ".SRV";
        }

        public T this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; isDirty = true; }
        }

        public T this[uint index]
        {
            get { return items[index]; }
            set { items[index] = value; isDirty = true; }
        }

        public T* Local => items;

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

        /// <summary>
        /// Gets the uav.
        /// </summary>
        /// <value>
        /// The uav.
        /// </value>
        public IUnorderedAccessView UAV => uav;

        /// <summary>
        /// Gets the SRV.
        /// </summary>
        /// <value>
        /// The SRV.
        /// </value>
        public IShaderResourceView SRV => srv;

        /// <summary>
        /// Not null when CanWrite is true
        /// </summary>
        public IBuffer? CopyBuffer => copyBuffer;

        /// <summary>
        /// Gets a value indicating whether this instance can write.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can write; otherwise, <c>false</c>.
        /// </value>
        public bool CanWrite => canWrite;

        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can read; otherwise, <c>false</c>.
        /// </value>
        public bool CanRead => canRead;

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public uint Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => capacity;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value == 0)
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
                Zero(tmp, DefaultCapacity * sizeof(T));
                var oldsize = count * sizeof(T);
                var newsize = value * sizeof(T);
                Buffer.MemoryCopy(items, tmp, newsize, oldsize > newsize ? newsize : oldsize);
                Free(items);
                items = tmp;
                capacity = value;
                count = capacity < count ? capacity : count;
                srv.Dispose();
                uav.Dispose();
                buffer.Dispose();
                copyBuffer?.Dispose();
                bufferDescription.ByteWidth = sizeof(T) * (int)capacity;
                buffer = device.CreateBuffer(items, capacity, bufferDescription);
                buffer.DebugName = dbgName;
                if (canWrite)
                {
                    copyDescription.ByteWidth = sizeof(T) * (int)value;
                    copyBuffer = device.CreateBuffer(copyDescription);
                    copyBuffer.DebugName = dbgName + ".CopyBuffer";
                }

                uav = device.CreateUnorderedAccessView(buffer, new(buffer, Format.Unknown, 0, (int)capacity, flags));
                uav.DebugName = dbgName + ".UAV";
                srv = device.CreateShaderResourceView(buffer);
                srv.DebugName = dbgName + ".SRV";
                isDirty = true;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public BufferDescription Description => buffer.Description;

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length => buffer.Length;

        /// <summary>
        /// Get the item count / counter of the buffer.
        /// </summary>
        public uint Count => count;

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <value>
        /// The dimension.
        /// </value>
        public ResourceDimension Dimension => buffer.Dimension;

        /// <summary>
        /// Gets the native pointer.
        /// </summary>
        /// <value>
        /// The native pointer.
        /// </value>
        public nint NativePointer => buffer.NativePointer;

        /// <summary>
        /// Gets or sets the dbgName of the debug.
        /// </summary>
        /// <value>
        /// The dbgName of the debug.
        /// </value>
        public string? DebugName { get => buffer.DebugName; set => buffer.DebugName = value; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed => buffer.IsDisposed;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add(T args)
        {
            var index = count;
            count++;
            EnsureCapacity(count);
            items[index] = args;
            isDirty = true;
            return ref items[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int index)
        {
            var size = (count - index) * sizeof(T);
            Buffer.MemoryCopy(&items[index + 1], &items[index], size, size);
            isDirty = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(uint index)
        {
            var size = (count - index) * sizeof(T);
            Buffer.MemoryCopy(&items[index + 1], &items[index], size, size);
            isDirty = true;
        }

        public void Clear()
        {
            for (int i = 0; i < count; i++)
            {
                items[i] = default;
            }
            count = 0;
        }

        public void Clear(IGraphicsContext context)
        {
            context.ClearUnorderedAccessViewUint(uav, 0, 0, 0, 0);
        }

        public bool Update(IGraphicsContext context)
        {
            if (copyBuffer == null)
            {
                throw new InvalidOperationException();
            }

            if (!canWrite)
            {
                throw new InvalidOperationException();
            }

            if (isDirty)
            {
                if (canRead)
                {
                    context.Write(copyBuffer, items, (int)(count * sizeof(T)), Map.Write);
                    context.CopyResource(buffer, copyBuffer);
                }
                else
                {
                    context.Write(copyBuffer, items, (int)(count * sizeof(T)));
                    context.CopyResource(buffer, copyBuffer);
                }

                isDirty = false;
                return true;
            }
            return false;
        }

        public void Read(IGraphicsContext context)
        {
            if (copyBuffer == null)
            {
                throw new InvalidOperationException();
            }

            if (!canRead)
            {
                throw new InvalidOperationException();
            }

            context.CopyResource(copyBuffer, buffer);
            context.Read(copyBuffer, items, capacity);
        }

        public void CopyTo(IGraphicsContext context, IBuffer buffer)
        {
            context.CopyResource(buffer, this.buffer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                srv.Dispose();
                uav.Dispose();
                buffer.Dispose();
                copyBuffer?.Dispose();
                count = 0;
                capacity = 0;
                disposedValue = true;
            }
        }

        ~UavBuffer()
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