

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a generic type parameter.
    /// </summary>
    public abstract class TypeParameter : Item
    {
        /// <summary>
        /// The name of the type parameter (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the type parameter.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The parent context of the type parameter.
        /// </summary>
        public abstract Item Parent { get; }
    }

}
