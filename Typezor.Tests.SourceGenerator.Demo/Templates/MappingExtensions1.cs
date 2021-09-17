using System.Linq;
namespace Typezor.Tests.SourceGenerator.Demo
{
    public static partial class MappingExtensions {
         public static partial Typezor.Tests.SourceGenerator.Demo.MapB MapTo(this Typezor.Tests.SourceGenerator.Demo.MapA from, Typezor.Tests.SourceGenerator.Demo.MapB to)
         {
             to ??= new Typezor.Tests.SourceGenerator.Demo.MapB();
             to.Regular = from?.Regular.MapTo(to.Regular) ?? default(System.String);
             to.PrimitiveEnumerable = MapTo(from?.PrimitiveEnumerable, to.PrimitiveEnumerable);
             to.PrimitiveArray = MapTo(from?.PrimitiveArray, to.PrimitiveArray);
             to.PrimitiveList = MapTo(from?.PrimitiveList, to.PrimitiveList);
             to.Complex = from?.Complex.MapTo(to.Complex) ?? default(Typezor.Tests.SourceGenerator.Demo.MapB);
             to.ComplexEnumerable = MapTo(from?.ComplexEnumerable, to.ComplexEnumerable);
             to.FieldFromProperty = from?.FieldFromProperty.MapTo(to.FieldFromProperty) ?? default(System.String);
             to.ComplexDiffrentType = from?.ComplexDiffrentType.MapTo(to.ComplexDiffrentType) ?? default(Typezor.Tests.SourceGenerator.Demo.MapA);
             to.RegularDiffrentType = from?.RegularDiffrentType.MapTo(to.RegularDiffrentType) ?? default(System.Int32);
             MapTo(from?.PrimitiveListWithoutSetter, to.PrimitiveListWithoutSetter);
             to.IReadOnlyCollectionWithoutSetter = MapTo(from?.IReadOnlyCollectionWithoutSetter, to.IReadOnlyCollectionWithoutSetter);
             to.Enum = from?.Enum.MapTo(to.Enum) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA);
             to.NullableEnum = from?.NullableEnum.MapTo(to.NullableEnum) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA?);
             to.EnumMapping = from?.EnumMapping.MapTo(to.EnumMapping) ?? default(Typezor.Tests.SourceGenerator.Demo.TestB);
             to.EnumMappingNullable = from?.EnumMappingNullable.MapTo(to.EnumMappingNullable) ?? default(Typezor.Tests.SourceGenerator.Demo.TestB?);
             to.Integer = from?.Integer.MapTo(to.Integer) ?? default(System.Int32);
             to.NullableInteger = from?.NullableInteger.MapTo(to.NullableInteger) ?? default(System.Int32?);
             to.SourceNullableInteger = from?.SourceNullableInteger.MapTo(to.SourceNullableInteger) ?? default(System.Int32);
             to.CustomCollection = MapTo(from?.CustomCollection, to.CustomCollection);
             to.regular = from?.regular.MapTo(to.regular) ?? default(System.String);
             to.PropertyFromField = from?.PropertyFromField.MapTo(to.PropertyFromField) ?? default(System.String);
             return to;
         }
         public static partial System.Collections.Generic.List<Typezor.Tests.SourceGenerator.Demo.MapB> MapTo(this Typezor.Tests.SourceGenerator.Demo.MapA[] from, System.Collections.Generic.List<Typezor.Tests.SourceGenerator.Demo.MapB> to)
         {
             to ??= new System.Collections.Generic.List<Typezor.Tests.SourceGenerator.Demo.MapB>();
             foreach(var p in from) to.Add(p.MapTo(default(Typezor.Tests.SourceGenerator.Demo.MapB)));
             return to;
         }
         public static System.String MapTo(this System.String from, System.String to)
         {
             return from;
         }
         public static System.Collections.Generic.IEnumerable<System.String> MapTo(this System.Collections.Generic.IEnumerable<System.String> from, System.Collections.Generic.IEnumerable<System.String> to)
         {
             return from?.ToArray();
         }
         public static System.Collections.Generic.IEnumerable<System.String> MapTo(this System.String[] from, System.Collections.Generic.IEnumerable<System.String> to)
         {
             return from?.ToArray();
         }
         public static System.Collections.Generic.List<System.String> MapTo(this System.Collections.Generic.List<System.String> from, System.Collections.Generic.List<System.String> to)
         {
             to ??= new System.Collections.Generic.List<System.String>();
             foreach(var p in from) to.Add(p.MapTo(default(System.String)));
             return to;
         }
         public static System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.MapB> MapTo(this System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.MapA> from, System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.MapB> to)
         {
             return from?.Select(p => p.MapTo(default(Typezor.Tests.SourceGenerator.Demo.MapB))).ToArray();
         }
         public static Typezor.Tests.SourceGenerator.Demo.MapA MapTo(this Typezor.Tests.SourceGenerator.Demo.MapB from, Typezor.Tests.SourceGenerator.Demo.MapA to)
         {
             to ??= new Typezor.Tests.SourceGenerator.Demo.MapA();
             to.Regular = from?.Regular.MapTo(to.Regular) ?? default(System.String);
             to.OnlyGetter = from?.OnlyGetter.MapTo(to.OnlyGetter) ?? default(System.String);
             to.PrimitiveEnumerable = MapTo(from?.PrimitiveEnumerable, to.PrimitiveEnumerable);
             to.PrimitiveEnumerableOnlyGetter = MapTo(from?.PrimitiveEnumerableOnlyGetter, to.PrimitiveEnumerableOnlyGetter);
             to.PrimitiveArray = MapTo(from?.PrimitiveArray, to.PrimitiveArray);
             to.PrimitiveList = MapTo(from?.PrimitiveList, to.PrimitiveList);
             to.Complex = from?.Complex.MapTo(to.Complex) ?? default(Typezor.Tests.SourceGenerator.Demo.MapA);
             to.ComplexEnumerable = MapTo(from?.ComplexEnumerable, to.ComplexEnumerable);
             to.PropertyFromField = from?.PropertyFromField.MapTo(to.PropertyFromField) ?? default(System.String);
             to.ComplexDiffrentType = from?.ComplexDiffrentType.MapTo(to.ComplexDiffrentType) ?? default(Typezor.Tests.SourceGenerator.Demo.MapB);
             to.RegularDiffrentType = from?.RegularDiffrentType.MapTo(to.RegularDiffrentType) ?? default(System.String);
             MapTo(from?.PrimitiveListWithoutSetter, to.PrimitiveListWithoutSetter);
             to.Enum = from?.Enum.MapTo(to.Enum) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA);
             to.NullableEnum = from?.NullableEnum.MapTo(to.NullableEnum) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA?);
             to.EnumMapping = from?.EnumMapping.MapTo(to.EnumMapping) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA);
             to.EnumMappingNullable = from?.EnumMappingNullable.MapTo(to.EnumMappingNullable) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA?);
             to.Integer = from?.Integer.MapTo(to.Integer) ?? default(System.Int32);
             to.NullableInteger = from?.NullableInteger.MapTo(to.NullableInteger) ?? default(System.Int32?);
             to.SourceNullableInteger = from?.SourceNullableInteger.MapTo(to.SourceNullableInteger) ?? default(System.Int32?);
             to.CustomCollection = MapTo(from?.CustomCollection, to.CustomCollection);
             to.regular = from?.regular.MapTo(to.regular) ?? default(System.String);
             to.readonlyField = from?.readonlyField.MapTo(to.readonlyField) ?? default(System.String);
             to.FieldFromProperty = from?.FieldFromProperty.MapTo(to.FieldFromProperty) ?? default(System.String);
             return to;
         }
         public static partial System.Int32 MapTo(this System.String from, System.Int32 to)
            ;
         public static System.Collections.Generic.IReadOnlyCollection<System.String> MapTo(this System.Collections.Generic.IReadOnlyCollection<System.String> from, System.Collections.Generic.IReadOnlyCollection<System.String> to)
         {
             return from?.ToArray();
         }
         public static Typezor.Tests.SourceGenerator.Demo.TestA MapTo(this Typezor.Tests.SourceGenerator.Demo.TestA from, Typezor.Tests.SourceGenerator.Demo.TestA to)
         {
             return from;
         }
         public static Typezor.Tests.SourceGenerator.Demo.TestA? MapTo(this Typezor.Tests.SourceGenerator.Demo.TestA? from, Typezor.Tests.SourceGenerator.Demo.TestA? to)
         {
             return from;
         }
         public static Typezor.Tests.SourceGenerator.Demo.TestB MapTo(this Typezor.Tests.SourceGenerator.Demo.TestA from, Typezor.Tests.SourceGenerator.Demo.TestB to)
         {
             return ((Typezor.Tests.SourceGenerator.Demo.TestB?)(int?)from) ?? default(Typezor.Tests.SourceGenerator.Demo.TestB);
         }
         public static Typezor.Tests.SourceGenerator.Demo.TestB? MapTo(this Typezor.Tests.SourceGenerator.Demo.TestA? from, Typezor.Tests.SourceGenerator.Demo.TestB? to)
         {
             return ((Typezor.Tests.SourceGenerator.Demo.TestB?)(int?)from) ?? default(Typezor.Tests.SourceGenerator.Demo.TestB?);
         }
         public static System.Int32 MapTo(this System.Int32 from, System.Int32 to)
         {
             return from;
         }
         public static System.Int32? MapTo(this System.Int32? from, System.Int32? to)
         {
             return from;
         }
         public static System.Int32 MapTo(this System.Int32? from, System.Int32 to)
         {
             return from?? default;
         }
         public static Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB> MapTo(this Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestA> from, Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB> to)
         {
             to ??= new Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB>();
             foreach(var p in from) to.Add(p.MapTo(default(Typezor.Tests.SourceGenerator.Demo.TestB)));
             return to;
         }
         public static System.String[] MapTo(this System.Collections.Generic.IEnumerable<System.String> from, System.String[] to)
         {
             return from?.ToArray();
         }
         public static System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.MapA> MapTo(this System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.MapB> from, System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.MapA> to)
         {
             return from?.Select(p => p.MapTo(default(Typezor.Tests.SourceGenerator.Demo.MapA))).ToArray();
         }
         public static partial System.String MapTo(this System.Int32 from, System.String to)
            ;
         public static Typezor.Tests.SourceGenerator.Demo.TestA MapTo(this Typezor.Tests.SourceGenerator.Demo.TestB from, Typezor.Tests.SourceGenerator.Demo.TestA to)
         {
             return ((Typezor.Tests.SourceGenerator.Demo.TestA?)(int?)from) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA);
         }
         public static Typezor.Tests.SourceGenerator.Demo.TestA? MapTo(this Typezor.Tests.SourceGenerator.Demo.TestB? from, Typezor.Tests.SourceGenerator.Demo.TestA? to)
         {
             return ((Typezor.Tests.SourceGenerator.Demo.TestA?)(int?)from) ?? default(Typezor.Tests.SourceGenerator.Demo.TestA?);
         }
         public static System.Int32? MapTo(this System.Int32 from, System.Int32? to)
         {
             return from;
         }
         public static Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestA> MapTo(this Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB> from, Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestA> to)
         {
             to ??= new Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestA>();
             foreach(var p in from) to.Add(p.MapTo(default(Typezor.Tests.SourceGenerator.Demo.TestA)));
             return to;
         }
    }
}