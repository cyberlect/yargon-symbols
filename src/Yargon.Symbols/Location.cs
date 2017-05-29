using System;

namespace Yargon.Symbols
{
    /// <summary>
    /// A source location.
    /// </summary>
    public struct Location : IEquatable<Location>, IComparable<Location>
    {
        /// <summary>
        /// Gets the zero-based line offset.
        /// </summary>
        /// <value>The zero-based line offset.</value>
        public int Line { get; }

        /// <summary>
        /// Gets the zero-based character offset.
        /// </summary>
        /// <value>The zero-based character offset.</value>
        public int Character { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="line">The zero-based line offset.</param>
        /// <param name="character">The zero-based character offset.</param>
        public Location(int line, int character)
        {
            #region Contract
            if (line < 0)
                throw new ArgumentOutOfRangeException(nameof(line));
            if (character < 0)
                throw new ArgumentOutOfRangeException(nameof(character));
            #endregion

            this.Line = line;
            this.Character = character;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Location other)
        {
            return this.Line == other.Line
                && this.Character == other.Character;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Line.GetHashCode();
                hash = hash * 29 + this.Character.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Location && Equals((Location)obj);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Location"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Location left, Location right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Location"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Location left, Location right) => !(left == right);
        #endregion

        #region Comparity
        /// <inheritdoc />
        public int CompareTo(Location other)
        {
            int comparison;

            comparison = this.Line.CompareTo(other.Line);
            if (comparison != 0) return comparison;
            comparison = this.Character.CompareTo(other.Character);
            if (comparison != 0) return comparison;

            return comparison;
        }

        /// <summary>
        /// Returns a value that indicates whether one <see cref="Location"/> objects is less than another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <(Location left, Location right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Returns a value that indicates whether one <see cref="Location"/> objects is less than or equal to another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(Location left, Location right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Returns a value that indicates whether one <see cref="Location"/> objects is greater than another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >(Location left, Location right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Returns a value that indicates whether one <see cref="Location"/> objects is greater than or equal to another.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(Location left, Location right) => left.CompareTo(right) >= 0;
        #endregion

        /// <inheritdoc />
        /// <remarks>
        /// This displays the one-based line and character offsets.
        /// </remarks>
        public override string ToString() => $"({this.Line + 1}:{this.Character + 1})";
    }
}
