

using System.Collections.Generic;
using Typezor.Abstractions;

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a delegate.
    /// </summary>
    public abstract class Delegate : Item, IAnnotated
    {
        /// <summary>
        /// All attributes defined on the delegate.
        /// </summary>
        public abstract IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// The XML documentation comment of the delegate.
        /// </summary>
        public abstract DocComment DocComment { get; }

        /// <summary>
        /// The full original name of the delegate including namespace and containing class names.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Determines if the delegate is generic.
        /// </summary>
        public abstract bool IsGeneric { get; }

        /// <summary>
        /// The name of the delegate (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the delegate.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// All parameters of the delegate.
        /// </summary>
        public abstract IEnumerable<Parameter> Parameters { get; }

        /// <summary>
        /// The parent context of the delegate.
        /// </summary>
        public abstract Item Parent { get; }

        /// <summary>
        /// The type of the delegate.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// All generic type parameters of the delegate.
        /// TypeParameters are the type placeholders of a generic delegate e.g. &lt;T&gt;.
        /// </summary>
        public abstract IEnumerable<TypeParameter> TypeParameters { get; }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        public static implicit operator string (Delegate instance)
        {
            return instance.ToString();
        }
    }
}
