

using System.Collections.Generic;
using Typezor.Abstractions;

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a parameter.
    /// </summary>
    public abstract class Parameter : Item, IAnnotated
    {
        /// <summary>
        /// All attributes defined on the parameter.
        /// </summary>
        public abstract IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// The full original name of the parameter.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// The default value of the parameter if it's optional.
        /// </summary>
        public abstract string DefaultValue { get; }

        /// <summary>
        /// Determines if the parameter has a default value.
        /// </summary>
        public abstract bool HasDefaultValue { get; }

        /// <summary>
        /// The name of the parameter (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The parent context of the parameter.
        /// </summary>
        public abstract Item Parent { get; }

        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        public static implicit operator string (Parameter instance)
        {
            return instance.ToString();
        }
    }
}