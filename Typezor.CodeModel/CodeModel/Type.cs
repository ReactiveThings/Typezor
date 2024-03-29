﻿

using System.Collections.Generic;
using Typezor.Abstractions;

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a type.
    /// </summary>
    public abstract class Type : Item, IAnnotated
    {
        /// <summary>
        /// All attributes defined on the type.
        /// </summary>
        public abstract IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// The base class of the type.
        /// </summary>
        public abstract Class BaseClass { get; }

        /// <summary>
        /// All constants defined in the type.
        /// </summary>
        public abstract IEnumerable<CodeModel.Constant> Constants { get; }

        /// <summary>
        /// The containing class of the type if it's nested.
        /// </summary>
        public abstract Class ContainingClass { get; }

        /// <summary>
        /// All delegates defined in the type.
        /// </summary>
        public abstract IEnumerable<CodeModel.Delegate> Delegates { get; }

        /// <summary>
        /// The XML documentation comment of the type.
        /// </summary>
        public abstract DocComment DocComment { get; }

        /// <summary>
        /// All fields defined in the type.
        /// </summary>
        public abstract IEnumerable<CodeModel.Field> Fields { get; }

        /// <summary>
        /// The full original name of the type including namespace and containing class names.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// All interfaces implemented by the type.
        /// (In Visual Studio 2013 Interfaces are not available on Types)
        /// </summary>
        public abstract IEnumerable<Interface> Interfaces { get; }

        /// <summary>
        /// Determines if the type is a DateTime.
        /// </summary>
        public abstract bool IsDate { get; }

        /// <summary>
        /// Determines if the type is defined in the current solution.
        /// (In Visual Studio 2013 IsDefined returns false for generic types)
        /// </summary>
        public abstract bool IsDefined { get; }

        /// <summary>
        /// Determines if the type is an enum.
        /// </summary>
        public abstract bool IsEnum { get; }

        /// <summary>
        /// Determines if the type is enumerable.
        /// </summary>
        public abstract bool IsEnumerable { get; }

        /// <summary>
        /// Determines if the type is generic.
        /// </summary>
        public abstract bool IsGeneric { get; }

        /// <summary>
        /// Determines if the type is a record.
        /// </summary>
        public abstract bool IsRecord { get; }

        /// <summary>
        /// Determines if the type is a Guid.
        /// </summary>
        public abstract bool IsGuid { get; }

        /// <summary>
        /// Determines if the type is nullable.
        /// </summary>
        public abstract bool IsNullable { get; }

        /// <summary>
        /// Determines if the type is primitive.
        /// </summary>
        public abstract bool IsPrimitive { get; }

        /// <summary>
        /// Determines if the type is a Task.
        /// </summary>
        public abstract bool IsTask { get; }

        /// <summary>
        /// Determines if the type is a TimeSpan.
        /// </summary>
        public abstract bool IsTimeSpan { get; }

        /// <summary>
        /// Determines if the type is a ValueTuple.
        /// </summary>
        public abstract bool IsValueTuple { get; }

        /// <summary>
        /// All methods defined in the type.
        /// </summary>
        public abstract IEnumerable<Method> Methods { get; }

        /// <summary>
        /// The name of the type (camelCased).
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name of the type.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The namespace of the type.
        /// </summary>
        public abstract string Namespace { get; }

        /// <summary>
        /// All classes defined in the type.
        /// </summary>
        public abstract IEnumerable<CodeModel.Class> NestedClasses { get; }

        /// <summary>
        /// All enums defined in the type.
        /// </summary>
        public abstract IEnumerable<Enum> NestedEnums { get; }

        /// <summary>
        /// All interfaces defined in the type.
        /// </summary>
        public abstract IEnumerable<Interface> NestedInterfaces { get; }

        /// <summary>
        /// The original C# name of the type.
        /// </summary>
        public abstract string OriginalName { get; }

        /// <summary>
        /// The parent context of the type.
        /// </summary>
        public abstract Item Parent { get; }

        /// <summary>
        /// All properties defined in the type.
        /// </summary>
        public abstract IEnumerable<CodeModel.Property> Properties { get; }

        /// <summary>
        /// All generic type parameters of the type.
        /// TypeArguments are the specified arguments for the TypeParametes on a generic type e.g. &lt;string&gt;.
        /// (In Visual Studio 2013 TypeParameters and TypeArguments are the same)
        /// </summary>
        public abstract IEnumerable<CodeModel.Type> TypeArguments { get; }

        /// <summary>
        /// All generic type parameters of the type.
        /// TypeParameters are the type placeholders of a generic type e.g. &lt;T&gt;.
        /// (In Visual Studio 2013 TypeParameters and TypeArguments are the same)
        /// </summary>
        public abstract IEnumerable<TypeParameter> TypeParameters { get; }

        /// <summary>
        /// The named ValueTuple fields of the type.
        /// </summary>
        public abstract IEnumerable<CodeModel.Field> TupleElements { get; }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        public static implicit operator string (Type instance)
        {
            return instance.ToString();
        }
    }
}