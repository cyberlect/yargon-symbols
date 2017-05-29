using System;
using System.Collections.Generic;
using System.Text;

namespace Yargon.Symbols
{
    /// <summary>
    /// Specifies the kind of reference.
    /// </summary>
    public enum ReferenceKind
    {
        /// <summary>
        /// Textual reference.
        /// </summary>
        Text = 0,
        /// <summary>
        /// Used as an identifier.
        /// </summary>
        Identifier,
        /// <summary>
        /// Reads from the symbol.
        /// </summary>
        Read,
        /// <summary>
        /// Writes to the symbol.
        /// </summary>
        Write,
        /// <summary>
        /// Reads from and writes to the symbol.
        /// </summary>
        ReadWrite,
    }
}
