namespace Hexa.NET.Utilities
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents an unsafe list of elements of type T.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public unsafe struct UnsafeList<T> : IEnumerable<T>, IList<T>, IReadOnlyList<T>, IEquatable<UnsafeList<T>> where T : unmanaged
    {
        private const uint DefaultCapacity = 4;

        private T* pointer;
        private uint size;
        private uint capacity;

        /// <summary>
        /// Represents an enumerator for the elements in the <see cref="UnsafeList{T}"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            private readonly T* pointer;
            private readonly uint size;
            private int currentIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct for the specified <see cref="UnsafeList{T}"/>.
            /// </summary>
            /// <param name="list">The <see cref="UnsafeList{T}"/> to enumerate.</param>
            internal Enumerator(UnsafeList<T> list)
            {
                pointer = list.pointer;
                size = list.size;
                currentIndex = -1;
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public T Current => pointer[currentIndex];

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            object IEnumerator.Current => Current;

            /// <summary>
            /// Disposes of the enumerator. Since the enumerator does not own resources, this method does nothing.
            /// </summary>
            public readonly void Dispose()
            {
                // Enumerator does not own resources, so nothing to dispose.
            }

            /// <summary>
            /// Moves the enumerator to the next element in the <see cref="UnsafeList{T}"/>.
            /// </summary>
            /// <returns><see langword="true"/> if the enumerator successfully moved to the next element; <see langword="false"/> if the enumerator has reached the end of the collection.</returns>
            public bool MoveNext()
            {
                if (currentIndex < size - 1)
                {
                    currentIndex++;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Resets the enumerator to its initial position, which is before the first element in the <see cref="UnsafeList{T}"/>.
            /// </summary>
            public void Reset()
            {
                currentIndex = -1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeList{T}"/> struct.
        /// </summary>
        public UnsafeList()
        {
            Capacity = DefaultCapacity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeList{T}"/> class with the specified array of values.
        /// </summary>
        /// <param name="values">An array of values to initialize the list.</param>
        public UnsafeList(T[] values)
        {
            Capacity = (uint)values.Length;
            AppendRange(values);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeList{T}"/> struct with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        public UnsafeList(uint capacity)
        {
            Capacity = capacity;
        }

        /// <summary>
        /// Gets the number of elements in the list.
        /// </summary>
        public readonly uint Size => size;

        /// <summary>
        /// Gets the number of elements in the list.
        /// </summary>
        public readonly int Count => (int)size;

        readonly bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Gets the pointer to the underlying data array.
        /// </summary>
        public readonly T* Data => pointer;

        /// <summary>
        /// Gets a value indicating whether the list is empty.
        /// </summary>
        public readonly bool Empty => size == 0;

        /// <summary>
        /// Gets a pointer to the first element in the list.
        /// </summary>
        public readonly T* Front => pointer;

        /// <summary>
        /// Gets a pointer to the last element in the list.
        /// </summary>
        public readonly T* Back => &pointer[size - 1];

        /// <summary>
        /// Gets or sets the capacity of the list.
        /// </summary>
        public uint Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => capacity;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (pointer == null)
                {
                    pointer = AllocT<T>(value);
                    capacity = value;
                    Erase();
                    return;
                }
                pointer = ReAllocT(pointer, value);
                capacity = value;
                size = capacity < size ? capacity : size;
            }
        }

        T IList<T>.this[int index] { get => this[index]; set => this[index] = value; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        public T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => pointer[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => pointer[index] = value;
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => pointer[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => pointer[index] = value;
        }

        /// <summary>
        /// Gets the element at the specified index, with bounds checking.
        /// </summary>
        /// <param name="index">The index of the element to retrieve.</param>
        /// <returns>The element at the specified index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T At(uint index)
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, size);
#else
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException();
            }
#endif
            return this[index];
        }

        /// <summary>
        /// Gets the element at the specified index, with bounds checking.
        /// </summary>
        /// <param name="index">The index of the element to retrieve.</param>
        /// <returns>The element at the specified index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T At(int index)
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, (int)size);
#else
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException();
            }
#endif
            return this[index];
        }

        /// <summary>
        /// Gets a pointer to the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to retrieve.</param>
        /// <returns>A pointer to the element at the specified index.</returns>
        public T* GetPointer(uint index)
        {
            return &pointer[index];
        }

        /// <summary>
        /// Gets a pointer to the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to retrieve.</param>
        /// <returns>A pointer to the element at the specified index.</returns>
        public T* GetPointer(int index)
        {
            return &pointer[index];
        }

        /// <summary>
        /// Initializes the list with the default capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init()
        {
            Grow(DefaultCapacity);
        }

        /// <summary>
        /// Initializes the list with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(uint capacity)
        {
            Grow(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Grow(uint capacity)
        {
            uint newcapacity = size == 0 ? DefaultCapacity : 2 * size;

            if (newcapacity < capacity)
            {
                newcapacity = capacity;
            }

            Capacity = newcapacity;
        }

        /// <summary>
        /// Ensures that the list has the specified capacity.
        /// </summary>
        /// <param name="capacity">The desired capacity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reserve(uint capacity)
        {
            if (this.capacity < capacity || pointer == null)
            {
                Grow(capacity);
            }
        }

        /// <summary>
        /// Reduces the vector's capacity to match its size.
        /// </summary>
        public void ShrinkToFit()
        {
            Capacity = size;
        }

        /// <summary>
        /// Resizes the vector to the specified size. If the new size is larger than the current capacity, the capacity will be increased accordingly.
        /// </summary>
        /// <param name="newSize">The new size of the vector.</param>
        public void Resize(uint newSize)
        {
            if (size == newSize)
            {
                return;
            }
            Reserve(newSize);
            size = newSize;
        }

        /// <summary>
        /// Sets all elements in the vector to their default values and resets the size to 0.
        /// </summary>
        public readonly void Erase()
        {
            ZeroMemoryT(pointer, capacity);
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushBack(T item)
        {
            Reserve(size + 1);
            pointer[size] = item;
            size++;
        }

        /// <summary>
        /// Adds an element to the end of the vector.
        /// </summary>
        /// <param name="item">The element to add to the vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            PushBack(item);
        }

        /// <summary>
        /// Removes the last element from the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopBack()
        {
            pointer[size - 1] = default;
            size--;
        }

        /// <summary>
        /// Adds a range of items to the list.
        /// </summary>
        /// <param name="values">The array of items to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendRange(T[] values)
        {
            Reserve(size + (uint)values.Length);

            fixed (T* src = values)
            {
                Memcpy(src, &pointer[size], capacity * sizeof(T), values.Length * sizeof(T));
            }
            size += (uint)values.Length;
        }

        /// <summary>
        /// Appends a range of elements to the end of the vector.
        /// </summary>
        /// <param name="values">A pointer to the array of elements to append.</param>
        /// <param name="count">The number of elements to append.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendRange(T* values, uint count)
        {
            Reserve(size + count);

            Memcpy(values, &pointer[size], capacity * sizeof(T), count * sizeof(T));

            size += count;
        }

        /// <summary>
        /// Removes the specified item from the list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            var index = IndexOf(&item);
            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes the item at the specified index from the list.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            if (index == this.size - 1)
            {
                pointer[this.size - 1] = default;
                this.size--;
                return;
            }

            var size = (this.size - index) * sizeof(T);
            Buffer.MemoryCopy(&pointer[index + 1], &pointer[index], size, size);
            this.size--;
        }

        /// <summary>
        /// Removes the item at the specified index from the list.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(uint index)
        {
            if (index == this.size - 1)
            {
                pointer[this.size - 1] = default;
                this.size--;
                return;
            }

            var size = (this.size - index) * sizeof(T);
            Buffer.MemoryCopy(&pointer[index + 1], &pointer[index], size, size);
            this.size--;
        }

        /// <summary>
        /// Inserts an item at the specified index in the list.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            Reserve(this.size + 1);

            var size = (this.size - index) * sizeof(T);
            Buffer.MemoryCopy(&pointer[index], &pointer[index + 1], size, size);
            pointer[index] = item;
            this.size++;
        }

        /// <summary>
        /// Inserts an item at the specified index in the list.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(uint index, T item)
        {
            Reserve(this.size + 1);

            var size = (this.size - index) * sizeof(T);
            Buffer.MemoryCopy(&pointer[index], &pointer[index + 1], size, size);
            pointer[index] = item;
            this.size++;
        }

        /// <summary>
        /// Copies the contents of the list to the specified array.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The starting index in the destination array.</param>
        /// <param name="arraySize">The size of the destination array.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(T* array, uint arrayIndex, uint arraySize)
        {
            Buffer.MemoryCopy(pointer, &array[arrayIndex], (arraySize - arrayIndex) * sizeof(T), size * sizeof(T));
        }

        /// <summary>
        /// Copies a range of elements from the list to the specified array.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The starting index in the destination array.</param>
        /// <param name="arraySize">The size of the destination array.</param>
        /// <param name="offset">The starting index in the list.</param>
        /// <param name="count">The number of elements to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T* array, uint arrayIndex, uint arraySize, uint offset, uint count)
        {
            Buffer.MemoryCopy(&pointer[offset], &array[arrayIndex], (arraySize - arrayIndex) * sizeof(T), (count - offset) * sizeof(T));
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            size = 0;
        }

        /// <summary>
        /// Determines whether the list contains the specified item.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns><c>true</c> if the item is found; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T* item)
        {
            for (int i = 0; i < size; i++)
            {
                var current = &pointer[i];
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

        bool ICollection<T>.Contains(T item)
        {
            return Contains(&item);
        }

        /// <summary>
        /// Searches for the specified item and returns the index of the first occurrence within the entire list.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>The index of the first occurrence of the item, or -1 if not found.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T* item)
        {
            for (int i = 0; i < size; i++)
            {
                var current = &pointer[i];
                if (current == null)
                {
                    break;
                }

                if (current == item)
                {
                    return i;
                }
            }

            return -1;
        }

        int IList<T>.IndexOf(T item)
        {
            return IndexOf(&item);
        }

        /// <summary>
        /// Searches for the first occurrence of an element in the collection that matches the specified condition and returns its index.
        /// </summary>
        /// <param name="comparer">A function that defines the condition to search for an element.</param>
        /// <returns>The index of the first element that matches the condition; otherwise, -1 if no match is found.</returns>
        public int FirstIndexOf(Func<T, bool> comparer)
        {
            for (int i = 0; i < size; i++)
            {
                var current = pointer[i];

                if (comparer(current))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Reverses the order of the elements in the list.
        /// </summary>
        public readonly void Reverse()
        {
            new Span<T>(pointer, (int)size).Reverse();
        }

        /// <summary>
        /// Moves the content of another <see cref="UnsafeList{T}"/> to this list, taking ownership of the memory.
        /// </summary>
        /// <param name="list">The list whose content will be moved to this list.</param>
        public void Move(UnsafeList<T> list)
        {
            Free(pointer);
            pointer = list.pointer;
            capacity = list.capacity;
            size = list.size;
        }

#if NET8_0_OR_GREATER

        /// <summary>
        /// Atomically increments the counter value and returns the result.
        /// </summary>
        /// <returns>The incremented counter value.</returns>
        /// <remarks>Note: The capacity remains unchanged due to race conditions.</remarks>
        public uint InterlockedIncrementCounter()
        {
            return Interlocked.Increment(ref size);
        }

        /// <summary>
        /// Atomically decrements the counter value and returns the result.
        /// </summary>
        /// <returns>The decremented counter value.</returns>
        /// <remarks>Note: The capacity remains unchanged due to race conditions.</remarks>
        public uint InterlockedDecrementCounter()
        {
            return Interlocked.Decrement(ref size);
        }

        /// <summary>
        /// Atomically pushes an element to the end of the list and returns the index of the added element.
        /// </summary>
        /// <param name="value">The value to be added to the list.</param>
        /// <returns>The index of the added element.</returns>
        /// <remarks>Note: The capacity remains unchanged due to race conditions.</remarks>
        public uint InterlockedPushBack(T value)
        {
            uint index = Interlocked.Increment(ref size);
            pointer[index] = value;
            return index;
        }

        /// <summary>
        /// Atomically removes and returns the index of the last element in the list.
        /// </summary>
        /// <returns>The index of the removed element.</returns>
        /// <remarks>Note: The capacity remains unchanged due to race conditions.</remarks>
        public uint InterlockedPopBack()
        {
            uint index = InterlockedDecrementCounter();
            pointer[index + 1] = default;
            return index;
        }

        /// <summary>
        /// Atomically resizes the list to the specified new size and returns the previous size.
        /// </summary>
        /// <param name="newSize">The new size to set for the list.</param>
        /// <returns>The previous size of the list.</returns>
        /// <remarks>Note: The capacity remains unchanged due to race conditions.</remarks>
        public uint InterlockedResize(uint newSize)
        {
            return Interlocked.Exchange(ref size, newSize);
        }

#endif

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the <see cref="UnsafeList{T}"/>.
        /// </summary>
        /// <returns>An enumerator for the <see cref="UnsafeList{T}"/>.</returns>
        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Releases the memory associated with the list.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release()
        {
            if (pointer != null)
            {
                Free(pointer);
                this = default;
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            Insert(index, item);
        }

        void IList<T>.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        void ICollection<T>.Add(T item)
        {
            PushBack(item);
        }

        void ICollection<T>.Clear()
        {
            Clear();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            fixed (T* dst = array)
            {
                MemcpyT(&pointer[arrayIndex], dst, array.Length);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            return Remove(item);
        }

        public readonly Span<T> AsSpan()
        {
            return new(pointer, (int)size);
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is UnsafeList<T> list && Equals(list);
        }

        public readonly bool Equals(UnsafeList<T> other)
        {
            return pointer == other.pointer;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine((nint)pointer);
        }

        public static bool operator ==(UnsafeList<T> left, UnsafeList<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnsafeList<T> left, UnsafeList<T> right)
        {
            return !(left == right);
        }
    }
}