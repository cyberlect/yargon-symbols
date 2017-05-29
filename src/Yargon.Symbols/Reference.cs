using System;
using JetBrains.Annotations;

namespace Yargon.Symbols
{
    /// <summary>
    /// A symbol reference.
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// Gets the symbol being referenced.
        /// </summary>
        /// <value>The symbol being referenced; or <see langword="null"/> when the reference is unresolved.</value>
        [CanBeNull]
        public Symbol Symbol { get; }

        /// <summary>
        /// Gets the URI of the document where the declaration occurs.
        /// </summary>
        /// <value>A document URI.</value>
        public Uri Document { get; }

        /// <summary>
        /// Gets the range of the name of the declaration in the file.
        /// </summary>
        /// <value>The name range.</value>
        public Range Name { get; }

        /// <summary>
        /// Gets the range of the body of the declaration in the file.
        /// </summary>
        /// <value>The body range.</value>
        public Range Range { get; }

        /// <summary>
        /// Gets the kind of reference.
        /// </summary>
        /// <value>A member of the <see cref="ReferenceKind"/> enum.</value>
        public ReferenceKind Kind { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Reference"/> class.
        /// </summary>
        /// <param name="symbol">The symbol being referenced; or <see langword="null"/>.</param>
        /// <param name="document">The document with the reference.</param>
        /// <param name="name">The range of the name of the reference.</param>
        /// <param name="range">The range of the whole reference.</param>
        /// <param name="kind">The kind of reference.</param>
        public Reference([CanBeNull] Symbol symbol, Uri document, Range name, Range range, ReferenceKind kind)
        {
            #region Contract
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            if (Enum.IsDefined(typeof(ReferenceKind), kind))
                throw new ArgumentException("Value is not a member of the enum.", nameof(kind));
            #endregion

            this.Symbol = symbol;
            this.Document = document;
            this.Name = name;
            this.Range = range;
            this.Kind = kind;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Reference other)
        {
            return !Object.ReferenceEquals(other, null)
                && this.Symbol == other.Symbol
                && this.Document == other.Document
                && this.Name == other.Name
                && this.Range == other.Range
                && this.Kind == other.Kind;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Symbol?.GetHashCode() ?? 0;
                hash = hash * 29 + this.Document.GetHashCode();
                hash = hash * 29 + this.Name.GetHashCode();
                hash = hash * 29 + this.Range.GetHashCode();
                hash = hash * 29 + this.Kind.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Reference);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Reference"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Reference left, Reference right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Reference"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Reference left, Reference right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"{this.Document}:{this.Range.Start.Line}:{this.Range.Start.Character} → {this.Symbol.Name} ({this.Kind.ToString().ToLowerInvariant()})";
    }
}
