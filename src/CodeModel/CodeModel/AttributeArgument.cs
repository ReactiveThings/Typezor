

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents an attribute argument.
    /// </summary>
    public abstract class AttributeArgument : Item
    {
        /// <summary>
        /// The type of the argument.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// The type value of the argument.
        /// </summary>
        public abstract Type TypeValue { get; }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public abstract object Value { get; }
    }
}