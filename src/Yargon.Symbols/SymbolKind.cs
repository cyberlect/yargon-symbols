namespace Yargon.Symbols
{
    /// <summary>
    /// Specifies the kind of symbol.
    /// </summary>
    public enum SymbolKind
    {
        /// <summary>
        /// Unspecified.
        /// </summary>
        None = 0,
        /// <summary>
        /// A local variable.
        /// </summary>
        Variable,
        /// <summary>
        /// A function or method (type) parameter.
        /// </summary>
        Parameter,
        /// <summary>
        /// A class attribute, such as a field, event, enum member, or property.
        /// </summary>
        Attribute,
        /// <summary>
        /// An operation, such as a function, method, procedure, constructor, destructor, extension method, or operator overload.
        /// </summary>
        Function,
        /// <summary>
        /// A class or class-like symbol, including enum, trait, record, mixin, struct, delegate, and annotation.
        /// </summary>
        Class,
        /// <summary>
        /// A file.
        /// </summary>
        File,
        /// <summary>
        /// A folder.
        /// </summary>
        Folder,
        /// <summary>
        /// The project. This is the root symbol of all symbols.
        /// </summary>
        Project
    }
}