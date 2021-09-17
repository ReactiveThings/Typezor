using System.Collections.Generic;
using Typezor.Abstractions;


namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a constant.
    /// </summary>
    public abstract class Constant : Item, IAnnotated
    {
        /// <summary>
        /// All attributes defined on the constant.
        /// </summary>
        public abstract IEnumerable<CodeModel.Attribute> Attributes { get; }

        /// <summary>
        /// The XML documentation comment of the constant.
        /// </summary>
        public abstract DocComment DocComment { get; }

        /// <summary>
        /// The full original name of the constant including namespace and containing class names.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// The name of the constant (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the constant.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The parent context of the constant.
        /// </summary>
        public abstract Item Parent { get; }

        /// <summary>
        /// The type of the constant.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// The value of the constant.
        /// </summary>
        public abstract string Value { get; }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        public static implicit operator string (Constant instance)
        {
            return instance.ToString();
        }
    }

}