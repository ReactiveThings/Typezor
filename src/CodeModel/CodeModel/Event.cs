using System.Collections.Generic;
using Typezor.Abstractions;


namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents an event.
    /// </summary>
    public abstract class Event : Item, IAnnotated
    {
        /// <summary>
        /// All attributes defined on the event.
        /// </summary>
        public abstract IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// The XML documentation comment of the event.
        /// </summary>
        public abstract DocComment DocComment { get; }

        /// <summary>
        /// The full original name of the event including namespace and containing class names.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// The name of the event (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The parent context of the event.
        /// </summary>
        public abstract Item Parent { get; }

        /// <summary>
        /// The type of the event.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        public static implicit operator string (Event instance)
        {
            return instance.ToString();
        }
    }

}
