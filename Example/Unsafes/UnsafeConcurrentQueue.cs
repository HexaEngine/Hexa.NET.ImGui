namespace HexaEngine.Core.Unsafes
{
    using System;
    using System.Runtime.CompilerServices;

    public unsafe struct UnsafeConcurrentQueue<T> : IFreeable where T : unmanaged
    {
        private const int DefaultCapacity = 4;

        private Allocator* allocator;
        private T* items;
        private T* head;
        private T* tail;
        private int size;
        private int capacity;

        private object lockObject = new();

        public UnsafeConcurrentQueue()
        {
            allocator = Allocator.Default;
            EnsureCapacity(DefaultCapacity);
        }

        public UnsafeConcurrentQueue(Allocator* customAllocator)
        {
            allocator = customAllocator;
            EnsureCapacity(DefaultCapacity);
        }

        public UnsafeConcurrentQueue(int capacity)
        {
            allocator = Allocator.Default;
            EnsureCapacity(capacity);
        }

        public int Size => size;

        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => capacity;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var tmp = allocator->Allocate<T>(value);
                var oldsize = size * sizeof(T);
                var newsize = value * sizeof(T);
                Buffer.MemoryCopy(items, tmp, newsize, oldsize > newsize ? newsize : oldsize);
                allocator->Free(items);
                head = tail = items = tmp;
                capacity = value;
                size = capacity < size ? capacity : size;
            }
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => items[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => items[index] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Grow(int capacity)
        {
            int newcapacity = size == 0 ? DefaultCapacity : 2 * size;

            if (newcapacity < capacity)
            {
                newcapacity = capacity;
            }

            Capacity = newcapacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int capacity)
        {
            lock (lockObject)
            {
                if (this.capacity < capacity)
                {
                    Grow(capacity);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(T item)
        {
            lock (lockObject)
            {
                EnsureCapacity(size + 1);

                head++;
                *head = item;
                size++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Dequeue()
        {
            lock (lockObject)
            {
                var tmp = *tail;
                *tail = default;
                tail++;
                if (head == tail)
                {
                    head = tail = items;
                }
                size--;
                return tmp;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out T item)
        {
            lock (lockObject)
            {
                if (size == 0)
                {
                    item = default;
                    return false;
                }
                item = *tail;
                *tail = default;
                tail++;
                if (head == tail)
                {
                    head = tail = items;
                }
                size--;
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T* array, uint arrayIndex, uint arraySize)
        {
            lock (lockObject)
            {
                Buffer.MemoryCopy(items, &array[arrayIndex], (arraySize - arrayIndex) * sizeof(T), size * sizeof(T));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T* array, uint arrayIndex, uint arraySize, uint offset, uint count)
        {
            lock (lockObject)
            {
                Buffer.MemoryCopy(&items[offset], &array[arrayIndex], (arraySize - arrayIndex) * sizeof(T), (count - offset) * sizeof(T));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            lock (lockObject)
            {
                var ptr = items;
                for (int i = 0; i < size; i++)
                {
                    ptr[i] = default;
                }
                size = 0;
                head = tail = items;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T* item)
        {
            lock (lockObject)
            {
                for (int i = 0; i < size; i++)
                {
                    var current = &items[i];
                    if (current == null)
                    {
                        break;
                    }

                    if (current == item)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Free()
        {
            allocator->Free(items);
            items = null;
            head = null;
            tail = null;
            capacity = 0;
            size = 0;
        }
    }
}