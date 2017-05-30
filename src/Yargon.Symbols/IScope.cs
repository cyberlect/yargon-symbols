using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Yargon.Symbols
{
    /// <summary>
    /// Represents a scope in the program, and tracks the symbols declared in it.
    /// </summary>
    /// <remarks>
    /// <para>Adding a symbol to this scope will shadow any symbols with the same name
    /// from the parent scopes.</para>
    /// <para>Similarly, removing a symbol or clearing all symbols from this scope
    /// will un-shadow the symbols with the same names from the parent scopes.</para>
    /// <para>Most scopes have a single parent scope, but some may have more than one.</para>
    /// <para>The members of the <see cref="IDictionary{TName,TValue}"/> interface
    /// work only on this scope. To find a symbol in this scope or parent scopes,
    /// use the <see cref="Lookup"/> method.</para>
    /// </remarks>
    /// <typeparam name="TName">The type of symbol name.</typeparam>
    /// <typeparam name="TSymbol">The type of symbol.</typeparam>
    public interface IScope<TName, TSymbol> : IDictionary<TName, TSymbol>
    {
        /// <summary>
        /// Gets the parent symbol tables.
        /// </summary>
        /// <value>The parent symbol tables;
        /// or an empty list when the table has no parents.</value>
        /// <remarks>
        /// Most symbol tables 
        /// </remarks>
        IReadOnlyList<IScope<TName, TSymbol>> Parents { get; }

        /// <summary>
        /// Adds the specified symbol to the scope.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><see langword="true"/> when the symbol was added;
        /// otherwise, <see langword="false"/> when it already exists.</returns>
        new bool Add(TName key, TSymbol value);

        /// <summary>
        /// Find symbol with the specified name in this
        /// scope or any parent scopes. 
        /// </summary>
        /// <param name="name">The name to look for.</param>
        /// <returns>A list of symbols found.</returns>
        /// <remarks>
        /// The returned list is empty when the symbol wasn't found,
        /// contains a single symbol when the symbol was unambiguously found,
        /// and contains more than one symbol when the symbol was ambiguous.
        /// </remarks>
        IReadOnlyList<TSymbol> Lookup(TName name);
    }
}
