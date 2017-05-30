using System;
using JetBrains.Annotations;

namespace Yargon.Symbols
{
    /// <summary>
    /// A symbol.
    /// </summary>
    public class Symbol<TName, TSymbol> : ISymbol<TName, TSymbol>
    {
        /// <inheritdoc />
        public TName Name { get; }

        /// <inheritdoc />
        public IScope<TName, TSymbol> Scope { get; }

        /// <inheritdoc />
        public SymbolKind Kind { get; }

        /// <inheritdoc />
        [CanBeNull]
        public string Summary { get; }

        /// <inheritdoc />
        [CanBeNull]
        public string Documentation { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="S2.Symbol"/> class.
        /// </summary>
        /// <param name="name">The fully-qualified name of the symbol.</param>
        /// <param name="scope">The nested scope; or <see langword="null"/>.</param>
        /// <param name="kind">The kind of symbol.</param>
        /// <param name="summary">A documentation summary; or <see langword="null"/>.</param>
        /// <param name="documentation">The documentation; or <see langword="null"/>.</param>
        public Symbol(TName name, [CanBeNull] IScope<TName, TSymbol> scope, SymbolKind kind, [CanBeNull] string summary, [CanBeNull] string documentation)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (Enum.IsDefined(typeof(SymbolKind), kind))
                throw new ArgumentException("Value is not a member of the enum.", nameof(kind));
            #endregion

            this.Name = name;
            this.Scope = scope;
            this.Kind = kind;
            this.Summary = summary;
            this.Documentation = documentation;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Symbol<TName, TSymbol> other)
        {
            return !Object.ReferenceEquals(other, null)
                && Object.Equals(this.Name, other.Name)
                && this.Kind == other.Kind
                && this.Summary == other.Summary
                && this.Documentation == other.Documentation;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Name.GetHashCode();
                hash = hash * 29 + this.Kind.GetHashCode();
                // Documentation is not hashed.
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Symbol<TName, TSymbol>);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Symbol"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Symbol<TName, TSymbol> left, Symbol<TName, TSymbol> right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Symbol"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Symbol<TName, TSymbol> left, Symbol<TName, TSymbol> right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Name} ({this.Kind.ToString().ToLowerInvariant()})";
        }
    }
}
