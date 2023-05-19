// Ternary Search Tree Implementation for C#
//
// Rewritten by Eric Domke
//
// Code adapted from implementation by Jonathan de Halleux
//   at http://www.codeproject.com/Articles/5819/Ternary-Search-Tree-Dictionary-in-C-Faster-String
//
// Rewrite focused on
// - removing fields from the TstDictionaryEntry class to reduce memory usage
// - decreasing the number of nodes to reduce memory usage (used some of the
//   ideas from http://hackthology.com/ternary-search-tries-for-fast-flexible-string-search-part-1.html)
// - implementing the modern IDictionary<string, T> interface
// - supporting case-insensitive matching and retrieval
// - supporting "starts with" style searching of the tree
// - adding utilities for creating a balanced tree using the algorithm at
//   http://www.drdobbs.com/database/ternary-search-trees/184410528?pgno=2
// - removing unnecessary public members

using System.Collections;

namespace HexaEngine.Core.Collections
{
    /// <summary>
    /// Ternary Search Tree Dictionary
    /// </summary>
    /// <remarks>
    /// <para>
    /// This dictionary is an implementation of the <b>Ternary Search Tree</b>
    /// data structure proposed by J. L. Bentley and R. Sedgewick in their
    /// paper:  Fast algorithms for sorting and searching strings
    /// in Proceedings of the Eighth Annual ACM-SIAM Symposium on Discrete Algorithms,
    /// New Orleans Louisiana, January 5-7, 1997.
    /// </para>
    /// <para>
    /// This dictionary acts as a symbol table: the keys must be string. It
    /// is generally faster to find symbol than the <see cref="Hashtable"/> or
    /// <see cref="SortedList"/> classes.
    /// </para>
    /// <para>
    /// Please read the paper to get some insight on the stucture used below.
    /// </para>
    /// </remarks>
    public class TernarySearchTreeDictionary<T> : IDictionary<string, T>, ICollection, IReadOnlyDictionary<string, T>
    {
        private readonly Func<char, char, int> _compare;
        private readonly IComparer<string> _comparer;
        private TstDictionaryEntry<T>? _root;
        private long _version;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Construct an empty ternary search tree with an ordinal comparer
        /// </remarks>
        public TernarySearchTreeDictionary() : this(StringComparer.Ordinal) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comparer">Comparer used to compare keys</param>
        /// <remarks>
        /// Construct an empty ternary search tree with the specified comparer
        /// </remarks>
        public TernarySearchTreeDictionary(IComparer<string> comparer)
        {
            _root = null;
            _version = 0;
            _comparer = comparer;

            if (_comparer == StringComparer.Ordinal)
            {
                _compare = (x, y) => x - y;
            }
            else if (_comparer == StringComparer.OrdinalIgnoreCase)
            {
                _compare = (x, y) => char.ToLower(x) - char.ToLower(y);
            }
            else
            {
                _compare = (x, y) => _comparer.Compare(x.ToString(), y.ToString());
            }
        }

        /// <summary>
        /// Constructor that adds multiple elements into the <see cref="TstDictionary"/>
        /// </summary>
        /// <param name="values">The elements to add</param>
        /// <remarks>
        /// Construct and populate a ternary search tree.
        /// </remarks>
        public TernarySearchTreeDictionary(IEnumerable<KeyValuePair<string, T>> values) : this(values, StringComparer.Ordinal) { }

        /// <summary>
        /// Constructor that adds multiple elements into the <see cref="TstDictionary"/>
        /// </summary>
        /// <param name="values">The elements to add</param>
        /// <param name="comparer">Comparer used to compare keys</param>
        /// <remarks>
        /// Construct and populate a ternary search tree.
        /// </remarks>
        public TernarySearchTreeDictionary(IEnumerable<KeyValuePair<string, T>> values, IComparer<string> comparer) : this(comparer)
        {
            AddRange(values);
        }

        /// <summary>
        /// Returns the comparer used to compare keys in the <see cref="TstDictionary"/>
        /// </summary>
        public IComparer<string>? Comparer
        {
            get { return _comparer; }
        }

        /// <summary>
        /// Gets the number of key-and-value pairs contained in the <see cref="TstDictionary"/>.
        /// </summary>
        /// <value>
        /// The number of key-and-value pairs contained in the <see cref="TstDictionary"/>.
        /// </value>
        /// <remarks>
        /// Complexity: O(N)
        /// </remarks>
        public virtual int Count
        {
            get
            {
                var en = GetEnumerator();
                int n = 0;
                while (en.MoveNext())
                {
                    ++n;
                }

                return n;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> containing the keys in the <see cref="TstDictionary"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection"/> containing the keys in the <see cref="TstDictionary"/>.
        /// </returns>
        public virtual ICollection<string> Keys
        {
            get
            {
                var keys = new List<string>();
                foreach (var kvp in this)
                {
                    keys.Add(kvp.Key);
                }

                return keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> containing the values in the <see cref="TstDictionary"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection"/> containing the values in the <see cref="TstDictionary"/>.
        /// </returns>
        public virtual ICollection<T> Values
        {
            get
            {
                var values = new List<T>();
                foreach (var kvp in this)
                {
                    values.Add(kvp.Value);
                }

                return values;
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <remarks>
        /// [C#] In C#, this property is the indexer for the <see cref="TstDictionary"/> class.
        /// </remarks>
        /// <param name="key">The key whose value to get or set.</param>
        /// <value>
        /// The value associated with the specified key.
        /// If the specified key is not found, attempting to get it returns a null reference
        /// (Nothing in Visual Basic), and attempting to set it creates a new element using the specified key.
        /// </value>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference</exception>
        /// <exception cref="ArgumentException">
        /// The property is set and <paramref name="key"/> is an empty string
        /// </exception>
        public virtual T this[string key]
        {
            get
            {
                if (!TryGetNode(key, null, out TernarySearchTreeDictionary<T>.TstDictionaryEntry<T>? entry))
                {
                    throw new KeyNotFoundException();
                }

                if (entry == null)
                {
                    throw new KeyNotFoundException();
                }

                return entry.Value.Value;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (key.Length == 0)
                {
                    throw new ArgumentException("key is an empty string");
                }
                // updating version
                ++_version;

                var de = Find(key);
                if (de == null)
                {
                    Add(key, value);
                }
                else
                {
                    de.Value = new KeyValuePair<string, T>(key, value);
                }
            }
        }

        /// <summary>
        /// Adds an element with the specified key and value into the <see cref="TstDictionary"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be a null reference (Nothing in Visual Basic).</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is an empty string</exception>
        /// <exception cref="ArgumentException">
        /// An element with the same key already exists in the <see cref="TstDictionary"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">The <see cref="TstDictionary"/> is read-only.</exception>
        /// <exception cref="NotSupportedException">The <see cref="TstDictionary"/> has a fixed size.</exception>
        public virtual void Add(string key, T value)
        {
            Add(new KeyValuePair<string, T>(key, value));
        }

        /// <summary>
        /// Adds an element with the specified key and value into the <see cref="TstDictionary"/>.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/>.<c>Key</c> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="item"/>.<c>Key</c> is an empty string</exception>
        /// <exception cref="ArgumentException">
        /// An element with the same key already exists in the <see cref="TstDictionary"/>.
        /// </exception>
        public void Add(KeyValuePair<string, T> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException("key is null");
            }

            if (item.Key.Length == 0)
            {
                throw new ArgumentException("trying to add empty key");
            }
            // updating version
            ++_version;

            // creating root node if needed.
            if (_root == null)
            {
                _root = new TstDictionaryEntry<T>(item.Key[0]);
            }

            // adding key
            var p = _root;
            int i = 0;
            char c;
            while (i <= item.Key.Length)
            {
                c = i < item.Key.Length ? item.Key[i] : '\0';
                var cmp = _compare(c, p.SplitChar);
                if (cmp < 0)
                {
                    if (p.LowChild == null)
                    {
                        p.LowChild = new TstDictionaryEntry<T>(c);
                        p.LowChild.Value = item;
                        return;
                    }
                    p = p.LowChild;
                }
                else if (cmp > 0)
                {
                    if (p.HighChild == null)
                    {
                        p.HighChild = new TstDictionaryEntry<T>(c);
                        p.HighChild.Value = item;
                        return;
                    }
                    p = p.HighChild;
                }
                else
                {
                    ++i;
                    if (i == item.Key.Length)
                    {
                        if (p.IsKey && p.Value.Key.Length == i)
                        {
                            throw new ArgumentException("key already in dictionary");
                        }
                    }

                    if (p.EqChild == null && p.IsKey)
                    {
                        p.EqChild = new TstDictionaryEntry<T>(i < p.Value.Key.Length ? p.Value.Key[i] : '\0');
                        p.EqChild.Value = p.Value;
                        p.Value = default;
                    }
                    else if (p.EqChild == null)
                    {
                        p.EqChild = new TstDictionaryEntry<T>(item.Key[i]);
                        p.EqChild.Value = item;
                        return;
                    }
                    p = p.EqChild;
                }
            }

            p.Value = item;
        }

        /// <summary>
        /// Adds multiple elements into the <see cref="TstDictionary"/>
        /// </summary>
        /// <param name="values">The elements to add</param>
        /// <remarks>This method attempts to create a balanced tree</remarks>
        public void AddRange(IEnumerable<KeyValuePair<string, T>> values)
        {
            var arr = values.OrderBy(v => v.Key, _comparer).ToArray();
            AddRecursive(arr, 0, arr.Length);
        }

        /// <summary>
        /// Removes all elements from the <see cref="TstDictionary"/>.
        /// </summary>
        public virtual void Clear()
        {
            // updating version
            ++_version;
            _root = null;
        }

        /// <summary>
        /// Determines whether the <see cref="TstDictionary"/> contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="TstDictionary"/>.</param>
        /// <returns>true if the <see cref="TstDictionary"/> contains an element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <remarks>
        /// <para>Complexity: Uses a Ternary Search Tree (tst) to find the key.</para>
        /// </remarks>
        public virtual bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var de = Find(key);
            return de != null && de.IsKey;
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="TstDictionary" />.</summary>
        /// <returns>A enumerator structure for the <see cref="TstDictionary" />.</returns>
        public virtual IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return new TstDictionaryEnumerator<T>(this);
        }

        ///<summary>
        /// Removes the element with the specified key from the <see cref="TstDictionary"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is an empty string</exception>
        /// <exception cref="NotSupportedException">The <see cref="TstDictionary"/> is read-only.</exception>
        /// <exception cref="NotSupportedException">The <see cref="TstDictionary"/> has a fixed size.</exception>
        public virtual bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length == 0)
            {
                throw new ArgumentException("key length cannot be 0");
            }
            // updating version
            ++_version;

            var stack = new Stack<TstDictionaryEntry<T>>();
            if (!TryGetNode(key, stack, out TernarySearchTreeDictionary<T>.TstDictionaryEntry<T>? p))
            {
                return false;
            }

            stack.Pop();

            if (p == null)
            {
                return false;
            }

            p.Value = default;

            while (!p.IsKey && !p.HasChildren && stack.Count > 0)
            {
                if (stack.Peek().LowChild == p)
                {
                    stack.Peek().LowChild = null;
                }
                else if (stack.Peek().HighChild == p)
                {
                    stack.Peek().HighChild = null;
                }
                else
                {
                    stack.Peek().EqChild = null;
                }

                p = stack.Pop();
            }

            if (!p.IsKey && !p.HasChildren && p == _root)
            {
                _root = null;
            }

            return true;
        }

        public IEnumerable<KeyValuePair<string, T>> StartingWith(string key)
        {
            if (_root == null)
            {
                return Enumerable.Empty<KeyValuePair<string, T>>();
            }

            if (string.IsNullOrEmpty(key))
            {
                return this;
            }

            var result = new List<KeyValuePair<string, T>>();
            StartingWith(_root.SplitChar, _root.LowChild, _root.EqChild, _root.HighChild, _root.Value, key, 0, result);
            return result;
        }

        private void StartingWith(char split
          , TstDictionaryEntry<T>? low
          , TstDictionaryEntry<T>? eq
          , TstDictionaryEntry<T>? high
          , KeyValuePair<string, T> value
          , string key
          , int index
          , IList<KeyValuePair<string, T>> matches)
        {
            var c = index >= key.Length ? '\0' : key[index];
            var cmp = _compare(c, split);

            if ((c == '\0' || cmp < 0) && low != null)
            {
                StartingWith(low.SplitChar, low.LowChild, low.EqChild, low.HighChild, low.Value, key, index, matches);
            }

            if (c == '\0' || cmp == 0)
            {
                if (eq != null)
                {
                    StartingWith(eq.SplitChar, eq.LowChild, eq.EqChild, eq.HighChild, eq.Value, key, index + 1, matches);
                }
                else if (value.Key != null && index < value.Key.Length - 1)
                {
                    StartingWith(value.Key[index + 1], null, null, null, value, key, index + 1, matches);
                }
                else if (value.Key != null && value.Key.Length >= key.Length)
                {
                    matches.Add(value);
                }
            }

            if ((c == '\0' || cmp > 0) && high != null)
            {
                StartingWith(high.SplitChar, high.LowChild, high.EqChild, high.HighChild, high.Value, key, index, matches);
            }
        }

        /// <summary>Gets the value associated with the specified key.</summary>
        /// <returns><c>true</c> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, <c>false</c>.</returns>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is null.
        /// </exception>
        public bool TryGetValue(string key, out T value)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!TryGetNode(key, null, out TernarySearchTreeDictionary<T>.TstDictionaryEntry<T>? entry))
            {
                return false;
            }

            if (entry == null)
            {
                return false;
            }
            else
            {
                value = entry.Value.Value;
            }

            return true;
        }

        /// <summary>
        /// Finds the tst node matching the key.
        /// </summary>
        /// <returns>the <see cref="TstDictionaryEntry"/> mathcing the key, null if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        protected virtual TstDictionaryEntry<T>? Find(string key)
        {
            if (TryGetNode(key, null, out TernarySearchTreeDictionary<T>.TstDictionaryEntry<T>? result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Finds the node matching the key.
        /// </summary>
        /// <returns>the <see cref="TstDictionaryEntry"/> mathcing the key, null if not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        protected virtual bool TryGetNode(string key, Stack<TstDictionaryEntry<T>>? stack, out TstDictionaryEntry<T>? entry)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var n = key.Length;
            if (n == 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var p = _root;
            var index = 0;
            int cmp;
            while (index < n && p != null)
            {
                if (stack != null)
                {
                    stack.Push(p);
                }

                cmp = _compare(key[index], p.SplitChar);
                if (cmp < 0)
                {
                    p = p.LowChild;
                }
                else if (cmp > 0)
                {
                    p = p.HighChild;
                }
                else
                {
                    if (p.Value.Key != null)
                    {
                        var res = p.Value.Key.Length == index + 1
                          || _comparer.Compare(p.Value.Key, key) == 0;
                        entry = res ? p : null;
                        return res;
                    }
                    else
                    {
                        ++index;
                        p = p.EqChild;
                    }
                }
            }

            entry = p;
            return p != null;
        }

        private void AddRecursive(KeyValuePair<string, T>[] values, int start, int count)
        {
            switch (count)
            {
                case 0:
                    break;

                case 1:
                    Add(values[start]);
                    break;

                case 2:
                    Add(values[start]);
                    Add(values[start + 1]);
                    break;

                default:
                    var bucket = count / 2 - (count + 1) % 2;
                    for (var i = start + bucket; i < start + count - bucket; i++)
                    {
                        Add(values[i]);
                    }
                    AddRecursive(values, start, bucket);
                    AddRecursive(values, start + count - bucket, bucket);
                    break;
            }
        }

        #region Explicit Interfaces

        /// <summary>
        /// Returns an <see cref="IDictionaryEnumerator"/> that can iterate through the <see cref="TstDictionary"/>.
        /// </summary>
        /// <returns>An <see cref="IDictionaryEnumerator"/> for the <see cref="TstDictionary"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerable<string> IReadOnlyDictionary<string, T>.Keys
        {
            get { return Keys; }
        }

        IEnumerable<T> IReadOnlyDictionary<string, T>.Values
        {
            get { return Values; }
        }

        /// <summary>
        /// Get a value indicating whether access to the <see cref="TstDictionary"/> is synchronized (thread-safe).
        /// </summary>
        /// <value>
        /// true if access to the <see cref="TstDictionary"/> is synchronized (thread-safe);
        /// otherwise, false. The default is false.
        /// </value>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="TstDictionary"/>.
        /// </summary>
        /// <value>
        /// An object that can be used to synchronize access to the <see cref="TstDictionary"/>.
        /// </value>
        object ICollection.SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// Copies the <see cref="TstDictionary"/> elements to a one-dimensional Array instance at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the
        /// <see cref="DictionaryEntry"/>
        /// objects copied from <see cref="TstDictionary"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is a null reference</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="array"/> is multidimensional.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The number of elements in the source <see cref="TstDictionary"/> is greater than
        /// the available space from <paramref name="arrayIndex"/> to the end of the destination array.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The type of the source <see cref="TstDictionary"/> cannot be cast automatically
        /// to the type of the destination array.
        /// </exception>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("index is negative");
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException("array is multi-dimensional");
            }

            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("index >= array.Length");
            }

            var i = arrayIndex;
            foreach (var de in this)
            {
                if (i > array.Length)
                {
                    throw new ArgumentException("The number of elements in the source ICollection is greater than the available space from index to the end of the destination array.");
                }

                array.SetValue(de, i++);
            }
        }

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            return TryGetValue(item.Key, out T? value) && Equals(item.Value, value);
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("index is negative");
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException("array is multi-dimensional");
            }

            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("index >= array.Length");
            }

            var i = arrayIndex;
            foreach (var de in this)
            {
                if (i > array.Length)
                {
                    throw new ArgumentException("The number of elements in the source ICollection is greater than the available space from index to the end of the destination array.");
                }

                array[i++] = de;
            }
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return Remove(item.Key);
        }

        #endregion Explicit Interfaces

        /// <summary>
        /// Enumerates the elements of a <see cref="TstDictionary"/>.
        /// </summary>
        protected sealed class TstDictionaryEnumerator<S> : IEnumerator<KeyValuePair<string, S>>
        {
            private TernarySearchTreeDictionary<S>.TstDictionaryEntry<S>? _currentNode;
            private readonly TernarySearchTreeDictionary<S>? _dictionary;
            private Stack<TernarySearchTreeDictionary<S>.TstDictionaryEntry<S>>? _stack;
            private readonly long _version;

            /// <summary>Constructs an enumerator over <paramref name="tst"/></summary>
            /// <param name="tst">dictionary to enumerate.</param>
            /// <exception cref="ArgumentNullException">tst is null</exception>
            public TstDictionaryEnumerator(TernarySearchTreeDictionary<S> tst)
            {
                _currentNode = null;
                _dictionary = tst ?? throw new ArgumentNullException(nameof(tst));
                _stack = null;
                _version = tst._version;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                ThrowIfChanged();
                _stack?.Clear();
                _stack = null;
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <value>The current element in the collection.</value>
            public KeyValuePair<string, S> Current
            {
                get
                {
                    ThrowIfChanged();
                    if (_currentNode == null)
                    {
                        throw new InvalidOperationException();
                    }

                    return _currentNode.Value;
                }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <value>The current element in the collection.</value>
            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element;
            /// false if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                ThrowIfChanged();

                // we are at the beginning
                if (_stack == null)
                {
                    _stack = new Stack<TernarySearchTreeDictionary<S>.TstDictionaryEntry<S>>();
                    _currentNode = null;
                    if (_dictionary?._root != null)
                    {
                        _stack.Push(_dictionary._root);
                    }
                }
                // we are at the end node, finished
                else if (_currentNode == null)
                {
                    throw new InvalidOperationException("out of range");
                }

                if (_stack.Count == 0)
                {
                    _currentNode = null;
                }

                while (_stack.Count > 0)
                {
                    _currentNode = _stack.Pop();
                    if (_currentNode.HighChild != null)
                    {
                        _stack.Push(_currentNode.HighChild);
                    }

                    if (_currentNode.EqChild != null)
                    {
                        _stack.Push(_currentNode.EqChild);
                    }

                    if (_currentNode.LowChild != null)
                    {
                        _stack.Push(_currentNode.LowChild);
                    }

                    if (_currentNode.IsKey)
                    {
                        break;
                    }
                }

                return _currentNode != null;
            }

            internal void ThrowIfChanged()
            {
                if (_version != _dictionary?._version)
                {
                    throw new InvalidOperationException("Collection changed");
                }
            }

            public void Dispose()
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Defines a Ternary Search Tree node pair that can be set or retrieved.
        /// </summary>
        protected class TstDictionaryEntry<S>
        {
            private TstDictionaryEntry<S>? _eqChild;
            private TstDictionaryEntry<S>? _highChild;
            private TstDictionaryEntry<S>? _lowChild;
            private readonly char _splitChar;
            private KeyValuePair<string, S> _value;

            /// <summary>
            /// Construct a tst node.
            /// </summary>
            /// <param name="parent">parent node</param>
            /// <param name="splitChar">split character</param>
            public TstDictionaryEntry(char splitChar)
            {
                _splitChar = splitChar;
                _lowChild = null;
                _eqChild = null;
                _highChild = null;
            }

            /// <summary>
            /// Gets the split character.
            /// </summary>
            /// <value>
            /// The split character.
            /// </value>
            public char SplitChar
            {
                get { return _splitChar; }
            }

            /// <summary>
            /// Gets a value indicating wheter the node is a key.
            /// </summary>
            /// <value>
            /// true is the node is a key, false otherwize.
            /// </value>
            public bool IsKey
            {
                get { return _value.Key != null; }
            }

            /// <summary>
            /// Gets the node value.
            /// </summary>
            /// <value>
            /// The node value.
            /// </value>
            /// <exception cref="InvalidOperationException">The node does not hold a key-value pair.</exception>
            public KeyValuePair<string, S> Value
            {
                get { return _value; }
                set { _value = value; }
            }

            /// <summary>
            /// Gets the node low child.
            /// </summary>
            /// <value>
            /// The low child.
            /// </value>
            public TstDictionaryEntry<S>? LowChild
            {
                get { return _lowChild; }
                set { _lowChild = value; }
            }

            /// <summary>
            /// Gets the node ep child.
            /// </summary>
            /// <value>
            /// The eq child.
            /// </value>
            public TstDictionaryEntry<S>? EqChild
            {
                get { return _eqChild; }
                set { _eqChild = value; }
            }

            /// <summary>
            /// Gets the node high child.
            /// </summary>
            /// <value>
            /// The high child.
            /// </value>
            public TstDictionaryEntry<S>? HighChild
            {
                get { return _highChild; }
                set { _highChild = value; }
            }

            /// <summary>
            /// Gets a value indicating wheter the node has children.
            /// </summary>
            /// <value>
            /// true if the node has children, false otherwize.
            /// </value>
            public bool HasChildren
            {
                get { return LowChild != null || EqChild != null || HighChild != null; }
            }

            public override string ToString()
            {
                if (IsKey)
                {
                    return string.Format("{0} {1}", SplitChar, _value.Key);
                }
                else
                {
                    return string.Format("{0}", SplitChar);
                }
            }
        }
    }
}