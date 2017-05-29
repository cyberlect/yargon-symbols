using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Virtlink.Utilib.Collections;
using Virtlink.Utilib.Collections.Graphs;

namespace Yargon.Symbols
{
    /// <summary>
    /// A simple symbol index implementation.
    /// </summary>
    public sealed class SimpleSymbolTable : ISymbolTable
    {
        private readonly List<SymbolNode> roots = new List<SymbolNode>();
        private readonly Dictionary<Symbol, SymbolNode> symbols = new Dictionary<Symbol, SymbolNode>();

        /// <inheritdoc />
        public IReadOnlyList<Symbol> GetSymbols(
            Symbol ancestor = null,
            string prefix = null,
            SymbolKind? kind = null)
        {
            IEnumerable<SymbolNode> ancestorNodes;
            if (ancestor != null)
            {
                if (!this.symbols.TryGetValue(ancestor, out var ancestorNode))
                    throw new InvalidOperationException("The ancestor symbol is not in the table.");
                ancestorNodes = new[] { ancestorNode };
            }
            else
            {
                ancestorNodes = this.roots;
            }

            return ancestorNodes
                .SelectMany(a => BreadthFirstTraversal.Traverse(a, node => node.Children).Skip(ancestor != null ? /* Skip the root */ 1 : 0))
                .Where(n => prefix == null || n.Symbol.Name.StartsWith(prefix, StringComparison.Ordinal))
                .Where(n => kind == null || n.Symbol.Kind == kind)
                .Select(n => n.Symbol)
                .ToList();
        }

        /// <inheritdoc />
        public IReadOnlyList<Declaration> GetDeclarations(
            Symbol symbol = null,
            Uri document = null,
            Range? range = null)
        {
            IEnumerable<Declaration> declarations;
            if (symbol != null)
            {
                if (!this.symbols.TryGetValue(symbol, out var node))
                    throw new InvalidOperationException("The symbol is not in the table.");
                declarations = node.Declarations;
            }
            else
            {
                declarations = this.symbols.Values.SelectMany(s => s.Declarations);
            }
            
            return declarations
                .Where(d => document == null || d.Document == document)
                .Where(d => range == null || ((Range) range).Overlaps(d.Range))
                .ToList();
        }

        /// <inheritdoc />
        public IReadOnlyList<Reference> GetReferences(
            Symbol symbol = null,
            Uri document = null,
            Range? range = null,
            ReferenceKind? kind = null)
        {
            IEnumerable<Reference> references;
            if (symbol != null)
            {
                if (!this.symbols.TryGetValue(symbol, out var node))
                    throw new InvalidOperationException("The symbol is not in the table.");
                references = node.References;
            }
            else
            {
                references = this.symbols.Values.SelectMany(s => s.References);
            }
            
            return references
                .Where(r => document == null || r.Document == document)
                .Where(r => range == null || ((Range) range).Overlaps(r.Range))
                .Where(r => kind == null || r.Kind == kind)
                .ToList();
        }

        /// <inheritdoc />
        public void AddSymbol(Symbol symbol)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            #endregion

            if (this.symbols.ContainsKey(symbol))
            {
                // Symbol is already in the table.
                return;
            }

            var node = new SymbolNode(symbol);
            if (symbol.Parent != null)
            {
                if (!this.symbols.TryGetValue(symbol.Parent, out var parentNode))
                    throw new InvalidOperationException("The parent symbol is not in the table.");
                parentNode.Children.Add(node);
            }
            else
            {
                this.roots.Add(node);
            }

            this.symbols.Add(symbol, node);
        }

        /// <inheritdoc />
        public void AddDeclaration(Declaration declaration)
        {
            #region Contract
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));
            #endregion

            if (!this.symbols.TryGetValue(declaration.Symbol, out var node))
                throw new InvalidOperationException("The declaration's symbol is not in the table.");

            node.Declarations.Add(declaration);
        }

        /// <inheritdoc />
        public void AddReference(Reference reference)
        {
            #region Contract
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));
            #endregion

            if (!this.symbols.TryGetValue(reference.Symbol, out var node))
                throw new InvalidOperationException("The reference's symbol is not in the table.");

            node.References.Add(reference);
        }

        /// <inheritdoc />
        public void RemoveDeclarations(IReadOnlyCollection<Declaration> declarations)
        {
            #region Contract
            if (declarations == null)
                throw new ArgumentNullException(nameof(declarations));
            #endregion

            foreach (var declaration in declarations)
            {
                if (!this.symbols.TryGetValue(declaration.Symbol, out var node))
                {
                    // The declaration belongs to a symbol that's no longer/not in this table.
                    continue;
                }

                node.Declarations.Remove(declaration);
            }
        }

        /// <inheritdoc />
        public void RemoveReferences(IReadOnlyCollection<Reference> references)
        {
            #region Contract
            if (references == null)
                throw new ArgumentNullException(nameof(references));
            #endregion

            foreach (var reference in references)
            {
                if (!this.symbols.TryGetValue(reference.Symbol, out var node))
                {
                    // The reference belongs to a symbol that's no longer/not in this table.
                    continue;
                }

                node.References.Remove(reference);
            }
        }

        /// <inheritdoc />
        public void RemoveSymbols(IReadOnlyCollection<Symbol> symbols)
        {
            #region Contract
            if (symbols == null)
                throw new ArgumentNullException(nameof(symbols));
            #endregion

            foreach (var symbol in symbols)
            {
                if (!this.symbols.TryGetValue(symbol, out var node))
                {
                    // The symbol is no longer/not in this table.
                    continue;
                }

                if (node.Symbol.Parent != null)
                {
                    var parentNode = this.symbols[node.Symbol.Parent];
                    parentNode.Children.Remove(node);
                }
                else
                {
                    this.roots.Remove(node);
                }
                this.symbols.Remove(symbol);
            }
        }

        /// <summary>
        /// A node in the symbol tree.
        /// </summary>
        private sealed class SymbolNode
        {
            /// <summary>
            /// Gets the symbol.
            /// </summary>
            /// <value>The symbol.</value>
            public Symbol Symbol { get; }

            /// <summary>
            /// Gets the set of children.
            /// </summary>
            /// <value>The children.</value>
            public HashSet<SymbolNode> Children { get; } = new HashSet<SymbolNode>();

            /// <summary>
            /// Gets the set of declarations.
            /// </summary>
            /// <value>The declarations.</value>
            public HashSet<Declaration> Declarations { get; } = new HashSet<Declaration>();

            /// <summary>
            /// Gets the set of references.
            /// </summary>
            /// <value>The references.</value>
            public HashSet<Reference> References { get; } = new HashSet<Reference>();

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="SymbolNode"/> class.
            /// </summary>
            /// <param name="symbol">The symbol.</param>
            public SymbolNode(Symbol symbol)
            {
                #region Contract
                if (symbol == null)
                    throw new ArgumentNullException(nameof(symbol));
                #endregion

                this.Symbol = symbol;
            }
            #endregion

            #region Equality
            /// <inheritdoc />
            public bool Equals(SymbolNode other)
            {
                return !Object.ReferenceEquals(other, null)
                       && this.Symbol == other.Symbol;
            }

            /// <inheritdoc />
            public override int GetHashCode() => this.Symbol.GetHashCode();

            /// <inheritdoc />
            public override bool Equals(object obj) => Equals(obj as SymbolNode);

            /// <summary>
            /// Returns a value that indicates whether two specified <see cref="SymbolNode"/> objects are equal.
            /// </summary>
            /// <param name="left">The first object to compare.</param>
            /// <param name="right">The second object to compare.</param>
            /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
            /// otherwise, <see langword="false"/>.</returns>
            public static bool operator ==(SymbolNode left, SymbolNode right) => Object.Equals(left, right);

            /// <summary>
            /// Returns a value that indicates whether two specified <see cref="SymbolNode"/> objects are not equal.
            /// </summary>
            /// <param name="left">The first object to compare.</param>
            /// <param name="right">The second object to compare.</param>
            /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
            /// otherwise, <see langword="false"/>.</returns>
            public static bool operator !=(SymbolNode left, SymbolNode right) => !(left == right);
            #endregion
        }
    }
}
