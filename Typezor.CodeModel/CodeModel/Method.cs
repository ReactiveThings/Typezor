﻿

using System.Collections.Generic;
using Typezor.Abstractions;

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a method.
    /// </summary>
    public abstract class Method : Item, IAnnotated
    {
        /// <summary>
        /// All attributes defined on the method.
        /// </summary>
        public abstract IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// The XML documentation comment of the method.
        /// </summary>
        public abstract DocComment DocComment { get; }

        /// <summary>
        /// The full original name of the method including namespace and containing class names.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Determines if the method is abstract.
        /// </summary>
        public abstract bool IsAbstract { get; }

        /// <summary>
        /// Determines if the method is generic.
        /// </summary>
        public abstract bool IsGeneric { get; }

        /// <summary>
        /// The name of the method (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the method.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// All parameters of the method.
        /// </summary>
        public abstract IEnumerable<Parameter> Parameters { get; }

        /// <summary>
        /// The parent context of the method.
        /// </summary>
        public abstract Item Parent { get; }

        /// <summary>
        /// The type of the method.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// All generic type parameters of the method.
        /// TypeParameters are the type placeholders of a generic method e.g. &lt;T&gt;.
        /// </summary>
        public abstract IEnumerable<TypeParameter> TypeParameters { get; }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        public static implicit operator string (Method instance)
        {
            return instance.ToString();
        }
    }
}