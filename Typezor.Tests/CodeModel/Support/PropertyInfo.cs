using System;
using System.Collections;
using System.Collections.Generic;
using Typezor.Tests.CodeModel.Support.Class;
using Typezor.Tests.CodeModel.Support.Enums;
using Typezor.Tests.CodeModel.Support.Interfaces;
#nullable enable
namespace Typezor.Tests.CodeModel.Support
{
    public class PropertyInfo
    {
        /// <summary>
        /// summary
        /// </summary>
        public string GetterOnly { get; } = null!;
        public string SetterOnly { set { } }
        public string PrivateGetter { private get; set; } = null!;
        public string PrivateSetter { get; private set; } = null!;

        // Primitive types
        [AttributeInfo]
        public bool Bool { get; set; }
        public char Char { get; set; }
        public string String { get; set; } = null!;
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
        public object Object { get; set; } = null!;
        public dynamic Dynamic { get; set; } = null!;

        // Enums
        public EnumInfo Enum { get; set; }
        public EnumInfo? NullableEnum1 { get; set; }
        public Nullable<EnumInfo> NullableEnum2 { get; set; } = null!;

        public Exception Exception { get; set; } // Class

        // Untyped collections
        public Array Array { get; set; } = null!;
        public IEnumerable Enumerable { get; set; } = null!;

        // Typed collections
        public string[] StringArray { get; set; } = null!;
        public IEnumerable<string> EnumerableString { get; set; } = null!;
        public List<string> ListString { get; set; } = null!;

        // Nullable
        public int? NullableInt1 { get; set; } = null!;
        public Nullable<int> NullableInt2 { get; set; } = null!;
        public IEnumerable<int> EnumerableInt { get; set; } = null!;
        public IEnumerable<int?> EnumerableNullableInt { get; set; } = null!;

        // Defined types
        public ClassInfo Class { get; set; } = null!;
        public BaseClassInfo BaseClass { get; set; } = null!;
        public GenericClassInfo<string> GenericClass { get; set; } = null!;
        public IInterfaceInfo Interface { get; set; } = null!;

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

        public IEnumerable<string?> EnumerableOfNullableString { get; set; } = null!;
        public string?[] ArrayOfNullableString { get; set; } = null!;
        public GenericClassInfo<string?> GenericOfNullableClass { get; set; } = null!;
    }

    public class GenericPropertyInfo<T>
    {
        public T? Generic { get; set; }
        public IEnumerable<T> EnumerableGeneric { get; set; } = null!;
    }
}
