using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Yargon.Symbols
{
    /// <summary>
    /// Describes a symbol.
    /// </summary>
    public interface ISymbol<TName, TSymbol>
    {
        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        /// <value>The name of the symbol.</value>
        TName Name { get; }

        /// <summary>
        /// Gets the scope for symbols nested in this symbol.
        /// </summary>
        /// <value>The nested scope; or <see langword="null"/> when there is none.</value>
        /// <remarks>
        /// For example, the symbol of a class definition will have a nested scope
        /// with the symbols defined in the class.
        /// </remarks>
        [CanBeNull]
        IScope<TName, TSymbol> Scope { get; }

        /// <summary>
        /// Gets the kind of symbol.
        /// </summary>
        /// <value>A member of the <see cref="SymbolKind"/> enum.</value>
        SymbolKind Kind { get; }

        /// <summary>
        /// Gets a summary of the symbol's documentation.
        /// </summary>
        /// <value>A documentation summary.</value>
        [CanBeNull]
        string Summary { get; }

        /// <summary>
        /// Gets the symbol's documentation.
        /// </summary>
        /// <value>The documentation.</value>
        [CanBeNull]
        string Documentation { get; }
    }
}
