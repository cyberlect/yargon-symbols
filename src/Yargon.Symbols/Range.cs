using System;

namespace Yargon.Symbols
{
    /// <summary>
    /// A source range.
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// Gets the start of the range.
        /// </summary>
        /// <value>The start of the range.</value>
        public Location Start { get; }

        /// <summary>
        /// Gets the end of the range.
        /// </summary>
        /// <value>The end of the range.</value>
        public Location End { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="start">The range start.</param>
        /// <param name="end">The range end.</param>
        public Range(Location start, Location end)
        {
            #region Contract
            if (end < start)
                throw new ArgumentOutOfRangeException(nameof(end), end, "The end is before the start of the range.");
            #endregion

            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="startLine">The zero-based range start line.</param>
        /// <param name="startCharacter">The zero-based range start character.</param>
        /// <param name="endLine">The zero-based range end line.</param>
        /// <param name="endCharacter">The zero-based range end character.</param>
        public Range(int startLine, int startCharacter, int endLine, int endCharacter)
            : this(new Location(startLine, startCharacter), new Location(endLine, endCharacter)) { }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Range other)
        {
            return this.Start == other.Start
                && this.End == other.End;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Start.GetHashCode();
                hash = hash * 29 + this.End.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Range && Equals((Range)obj);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Range"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Range left, Range right) => Object.Equals(left, right);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Range"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Range left, Range right) => !(left == right);
        #endregion

        /// <summary>
        /// Determines whether the specified location is in this range.
        /// </summary>
        /// <param name="location">The location to check.</param>
        /// <returns><see langword="true"/> when the location is at or between the
        /// start and end of the range; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Location location) => this.Start <= location && location <= this.End;

        /// <summary>
        /// Determines whether the specified range is contained within this range.
        /// </summary>
        /// <param name="range">The range to check.</param>
        /// <returns><see langword="true"/> when the range is at or between the
        /// start and end of the range; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Range range) => Contains(range.Start) && Contains(range.End);

        /// <summary>
        /// Determines whether the specified range overlaps this range.
        /// </summary>
        /// <param name="range">The range to check.</param>
        /// <returns><see langword="true"/> when the range is equal to or overlaps this range;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Overlaps(Range range) => (this.Start < range.End && range.Start < this.End) || this == range;

        /// <inheritdoc />
        /// <remarks>
        /// This displays the one-based line and character offsets.
        /// </remarks>
        public override string ToString() => $"({this.Start.Line + 1}:{this.Start.Character + 1}-{this.End.Line + 1}:{this.End.Character + 1})";
    }
}
