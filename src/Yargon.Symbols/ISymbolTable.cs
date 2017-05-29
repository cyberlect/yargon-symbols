using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Yargon.Symbols
{
    /// <summary>
    /// A symbol table.
    /// </summary>
    public interface ISymbolTable
    {
        /// <summary>
        /// Gets the symbols with the specified ancestor that have the specified prefix.
        /// </summary>
        /// <param name="ancestor">The ancestor; or <see langword="null"/> to not filter by ancestor.</param>
        /// <param name="prefix">The symbol prefix; or <see langword="null"/> to not filter by prefix.</param>
        /// <param name="kind">The kind of symbol; or <see langword="null"/> to not filter by kind.</param>
        /// <returns>The symbols.</returns>
        IReadOnlyList<Symbol> GetSymbols(
            [CanBeNull] Symbol ancestor = null,
            [CanBeNull] string prefix = null,
            [CanBeNull] SymbolKind? kind = null);

        /// <summary>
        /// Gets the declarations of the specified symbol in the specified file.
        /// </summary>
        /// <param name="symbol">The symbol; or <see langword="null"/> to not filter by symbol.</param>
        /// <param name="document">The document; or <see langword="null"/> to not filter by document.</param>
        /// <param name="range">The range in which the declarations occur; or <see langword="null"/> to not filter by range.</param>
        /// <returns>The declarations.</returns>
        IReadOnlyList<Declaration> GetDeclarations(
            [CanBeNull] Symbol symbol = null,
            [CanBeNull] Uri document = null,
            [CanBeNull] Range? range = null);

        /// <summary>
        /// Gets the references to the specified symbol in the specified file.
        /// </summary>
        /// <param name="symbol">The symbol; or <see langword="null"/> to not filter by symbol.</param>
        /// <param name="document">The document; or <see langword="null"/> to not filter by document.</param>
        /// <param name="range">The range in which the references occur; or <see langword="null"/> to not filter by range.</param>
        /// <param name="kind">The kind of reference; or <see langword="null"/> to not filter by kind.</param>
        /// <returns>The references.</returns>
        IReadOnlyList<Reference> GetReferences(
            [CanBeNull] Symbol symbol = null,
            [CanBeNull] Uri document = null,
            [CanBeNull] Range? range = null,
            [CanBeNull] ReferenceKind? kind = null);

        /// <summary>
        /// Adds a symbol to the table.
        /// </summary>
        /// <param name="symbol">The symbol to add.</param>
        void AddSymbol(Symbol symbol);

        /// <summary>
        /// Adds a declaration to the table.
        /// </summary>
        /// <param name="declaration">The declaration to add.</param>
        void AddDeclaration(Declaration declaration);

        /// <summary>
        /// Adds a reference to the table.
        /// </summary>
        /// <param name="reference">The reference to add.</param>
        void AddReference(Reference reference);

        /// <summary>
        /// Removes the specified declarations from the table.
        /// </summary>
        /// <param name="declarations">The declarations to remove.</param>
        void RemoveDeclarations(IReadOnlyCollection<Declaration> declarations);

        /// <summary>
        /// Removes the specified references from the table.
        /// </summary>
        /// <param name="references">The references to remove.</param>
        void RemoveReferences(IReadOnlyCollection<Reference> references);

        /// <summary>
        /// Removes the specified symbols from the table.
        /// </summary>
        /// <param name="symbols">The symbols to remove.</param>
        void RemoveSymbols(IReadOnlyCollection<Symbol> symbols);
    }
}
