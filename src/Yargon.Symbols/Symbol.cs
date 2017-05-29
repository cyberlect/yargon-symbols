using System;
using JetBrains.Annotations;

namespace Yargon.Symbols
{
    /// <summary>
    /// A symbol.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Gets the fully-qualified name of the symbol.
        /// </summary>
        /// <value>The fully-qualified name of the symbol.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the parent symbol.
        /// </summary>
        /// <value>The parent symbol; or <see langword="null"/>.</value>
        [CanBeNull]
        public Symbol Parent { get; }

        /// <summary>
        /// Gets the kind of symbol.
        /// </summary>
        /// <value>A member of the <see cref="SymbolKind"/> enum.</value>
        public SymbolKind Kind { get; }

        /// <summary>
        /// Gets a summary of the symbol's documentation.
        /// </summary>
        /// <value>A documentation summary.</value>
        [CanBeNull]
        public string Summary { get; }

        /// <summary>
        /// Gets the symbol's documentation.
        /// </summary>
        /// <value>The documentation.</value>
        [CanBeNull]
        public string Documentation { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        /// <param name="name">The fully-qualified name of the symbol.</param>
        /// <param name="parent">The parent symbol; or <see langword="null"/>.</param>
        /// <param name="kind">The kind of symbol.</param>
        /// <param name="summary">A documentation summary; or <see langword="null"/>.</param>
        /// <param name="documentation">The documentation; or <see langword="null"/>.</param>
        public Symbol(string name, [CanBeNull] Symbol parent, SymbolKind kind, [CanBeNull] string summary, [CanBeNull] string documentation)
        {
            #region Contract
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (Enum.IsDefined(typeof(SymbolKind), kind))
                throw new ArgumentException("Value is not a member of the enum.", nameof(kind));
            #endregion

            this.Name = name;
            this.Parent = parent;
            this.Kind = kind;
            this.Summary = summary;
            this.Documentation = documentation;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Symbol other)
        {
            return !Object.ReferenceEquals(other, null)
                && this.Name == other.Name
                && this.Parent == other.Parent
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
                hash = hash * 29 + this.Parent?.GetHashCode() ?? 0;
                hash = hash * 29 + this.Kind.GetHashCode();
                // Documentation is not hashed.
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Symbol);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Symbol"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Symbol left, Symbol right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Symbol"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Symbol left, Symbol right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Name} ({this.Kind.ToString().ToLowerInvariant()})" + (this.Parent != null ? $" in {this.Parent?.Name}" : "");
        }
    }
}
