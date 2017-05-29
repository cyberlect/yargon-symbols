using System;

namespace Yargon.Symbols
{
    /// <summary>
    /// A symbol declaration.
    /// </summary>
    public class Declaration
    {
        /// <summary>
        /// Gets the symbol being declared.
        /// </summary>
        /// <value>The symbol being declared.</value>
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

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Declaration"/> class.
        /// </summary>
        /// <param name="symbol">The symbol being declared.</param>
        /// <param name="document">The document with the declaration.</param>
        /// <param name="name">The range of the name of the declaration.</param>
        /// <param name="range">The range of the whole declaration.</param>
        public Declaration(Symbol symbol, Uri document, Range name, Range range)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            #endregion

            this.Symbol = symbol;
            this.Document = document;
            this.Name = name;
            this.Range = range;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Declaration other)
        {
            return !Object.ReferenceEquals(other, null)
                && this.Symbol == other.Symbol
                && this.Document == other.Document
                && this.Name == other.Name
                && this.Range == other.Range;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Symbol.GetHashCode();
                hash = hash * 29 + this.Document.GetHashCode();
                hash = hash * 29 + this.Name.GetHashCode();
                hash = hash * 29 + this.Range.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as Declaration);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Declaration"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Declaration left, Declaration right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Declaration"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Declaration left, Declaration right) => !(left == right);
        #endregion

        /// <inheritdoc />
        public override string ToString() => $"{this.Document}:{this.Range.Start.Line}:{this.Range.Start.Character} · {this.Symbol.Name}";
    }
}
