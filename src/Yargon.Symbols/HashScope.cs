using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Virtlink.Utilib.Collections;

namespace Yargon.Symbols
{
    /// <summary>
    /// A scope implemented with a hash table.
    /// </summary>
    public sealed class HashScope<TName, TSymbol> : IScope<TName, TSymbol>
    {
        /// <summary>
        /// The inner dictionary.
        /// </summary>
        private readonly Dictionary<TName, TSymbol> dictionary = new Dictionary<TName, TSymbol>();

        /// <inheritdoc />
        public IReadOnlyList<IScope<TName, TSymbol>> Parents { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HashScope{TName,TSymbol}"/> class.
        /// </summary>
        /// <param name="parents">The parents of this scope.</param>
        public HashScope(params IScope<TName, TSymbol>[] parents)
            : this((IEnumerable<IScope<TName, TSymbol>>)parents) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashScope{TName,TSymbol}"/> class.
        /// </summary>
        /// <param name="parents">The parents of this scope.</param>
        public HashScope(IEnumerable<IScope<TName, TSymbol>> parents)
        {
            #region Contract
            if (parents == null)
                throw new ArgumentNullException(nameof(parents));
            #endregion

            this.Parents = parents.AsSmartList();
        }
        #endregion

        /// <inheritdoc />
        public int Count => this.dictionary.Count;

        /// <inheritdoc />
        public bool Add(TName key, TSymbol value)
        {
            if (this.dictionary.ContainsKey(key))
                return false;
            this.dictionary.Add(key, value);
            return true;
        }

        /// <inheritdoc />
        public bool Remove(TName key)
        {
            return this.dictionary.Remove(key);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <inheritdoc />
        public IReadOnlyList<TSymbol> Lookup(TName name)
        {
            if (this.dictionary.TryGetValue(name, out var symbol))
                return new [] { symbol };
            return this.Parents.SelectMany(p => p.Lookup(name)).ToArray();
        }

        #region IDictionary<TName, TValue>
        /// <inheritdoc />
        /// <remarks>
        /// This only returns the symbols in this scope.
        /// </remarks>
        ICollection<TName> IDictionary<TName, TSymbol>.Keys => this.dictionary.Keys;

        /// <inheritdoc />
        /// <remarks>
        /// This only returns the symbols in this scope.
        /// </remarks>
        ICollection<TSymbol> IDictionary<TName, TSymbol>.Values => this.dictionary.Values;

        /// <inheritdoc />
        /// <remarks>
        /// This only sets the symbols in this scope.
        /// </remarks>
        TSymbol IDictionary<TName, TSymbol>.this[TName key]
        {
            get => this.dictionary[key];
            set => this.dictionary[key] = value;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This only checks the symbols in this scope.
        /// </remarks>
        bool IDictionary<TName, TSymbol>.ContainsKey(TName key)
            => ((IDictionary<TName, TSymbol>)this).TryGetValue(key, out _);

        /// <inheritdoc />
        /// <remarks>
        /// This only checks the symbols in this scope.
        /// </remarks>
        bool IDictionary<TName, TSymbol>.TryGetValue(TName key, out TSymbol value)
            => this.dictionary.TryGetValue(key, out value);

        /// <inheritdoc />
        void IDictionary<TName, TSymbol>.Add(TName key, TSymbol value)
            => Add(key, value);
        #endregion

        #region ICollection<T>
        /// <inheritdoc />
        bool ICollection<KeyValuePair<TName, TSymbol>>.IsReadOnly => false;

        /// <inheritdoc />
        /// <remarks>
        /// This only checks the symbols in this scope.
        /// </remarks>
        bool ICollection<KeyValuePair<TName, TSymbol>>.Contains(KeyValuePair<TName, TSymbol> item)
            => ((IDictionary<TName, TSymbol>)this).TryGetValue(item.Key, out var value)
               && EqualityComparer<TSymbol>.Default.Equals(item.Value, value);

        /// <inheritdoc />
        void ICollection<KeyValuePair<TName, TSymbol>>.Add(KeyValuePair<TName, TSymbol> item)
            => Add(item.Key, item.Value);

        /// <inheritdoc />
        bool ICollection<KeyValuePair<TName, TSymbol>>.Remove(KeyValuePair<TName, TSymbol> item)
        {
            if (((ICollection<KeyValuePair<TName, TSymbol>>)this).Contains(item))
            {
                Remove(item.Key);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This only copies the symbols in this scope.
        /// </remarks>
        void ICollection<KeyValuePair<TName, TSymbol>>.CopyTo(KeyValuePair<TName, TSymbol>[] array, int arrayIndex)
        {
            #region Contract
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < this.Count)
                throw new ArgumentException("Array is too small.", nameof(array));
            #endregion

            int i = arrayIndex;
            foreach (var pair in this)
            {
                array[i] = pair;
                i += 1;
            }
        }
        #endregion

        #region IEnumerable<T>
        /// <inheritdoc />
        /// <remarks>
        /// This only enumerates the symbols in this scope.
        /// </remarks>
        IEnumerator<KeyValuePair<TName, TSymbol>> IEnumerable<KeyValuePair<TName, TSymbol>>.GetEnumerator()
            => this.dictionary.GetEnumerator();

        /// <inheritdoc />
        /// <remarks>
        /// This only enumerates the symbols in this scope.
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<KeyValuePair<TName, TSymbol>>)this).GetEnumerator();
        #endregion
    }
}
