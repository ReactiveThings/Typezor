using System;
using System.Collections;
using System.Collections.Generic;
using Typezor.Tests.CodeModel.Support.Class;
using Typezor.Tests.CodeModel.Support.Enums;
using Typezor.Tests.CodeModel.Support.Interfaces;

namespace Typezor.Tests.CodeModel.Support
{
    public class PropertyInfo
    {
        /// <summary>
        /// summary
        /// </summary>
        public string GetterOnly { get; }
        public string SetterOnly { set { } }
        public string PrivateGetter { private get; set; }
        public string PrivateSetter { get; private set; }

        // Primitive types
        [AttributeInfo]
        public bool Bool { get; set; }
        public char Char { get; set; }
        public string String { get; set; }
        public byte Byte { get; set; }
        public sbyte Sbyte { get; set; }
        public int Int { get; set; }
        public uint Uint { get; set; }
        public short Short { get; set; }
        public ushort Ushort { get; set; }
        public long Long { get; set; }
        public ulong Ulong { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }

        // Special types
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public Guid Guid { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public object Object { get; set; }
        public dynamic Dynamic { get; set; }

        // Enums
        public EnumInfo Enum { get; set; }
        public EnumInfo? NullableEnum1 { get; set; }
        public Nullable<EnumInfo> NullableEnum2 { get; set; }

        public Exception Exception { get; set; } // Class

        // Untyped collections
        public Array Array { get; set; }
        public IEnumerable Enumerable { get; set; }

        // Typed collections
        public string[] StringArray { get; set; }
        public IEnumerable<string> EnumerableString { get; set; }
        public List<string> ListString { get; set; }

        // Nullable
        public int? NullableInt1 { get; set; }
        public Nullable<int> NullableInt2 { get; set; }
        public IEnumerable<int> EnumerableInt { get; set; }
        public IEnumerable<int?> EnumerableNullableInt { get; set; }

        // Defined types
        public ClassInfo Class { get; set; }
        public BaseClassInfo BaseClass { get; set; }
        public GenericClassInfo<string> GenericClass { get; set; }
        public IInterfaceInfo Interface { get; set; }

        // Nullable annotations
        public string? NullableString { get; set; }
        public object? NullableObject { get; set; }
        public dynamic? NullableDynamic { get; set; }
        public Exception? NullableException { get; set; } // Class
        public Array? NullableArray { get; set; }
        public IEnumerable? NullableEnumerable { get; set; }
        public string[]? NullableStringArray { get; set; }
        public IEnumerable<string>? NullableEnumerableString { get; set; }
        public List<string>? NullableListString { get; set; }
        public IEnumerable<int>? NullableEnumerableInt { get; set; }
        public IEnumerable<int?>? NullableEnumerableNullableInt { get; set; }
        public ClassInfo? NullableClass { get; set; }
        public BaseClassInfo? NullableBaseClass { get; set; }
        public GenericClassInfo<string>? NullableGenericClass { get; set; }
        public IInterfaceInfo? NullableInterface { get; set; }

        public IEnumerable<string?> EnumerableOfNullableString { get; set; }
        public string?[] ArrayOfNullableString { get; set; }
        public GenericClassInfo<string?> GenericOfNullableClass { get; set; }
    }

    public class GenericPropertyInfo<T>
    {
        public T Generic { get; set; }
        public IEnumerable<T> EnumerableGeneric { get; set; }
    }
}
