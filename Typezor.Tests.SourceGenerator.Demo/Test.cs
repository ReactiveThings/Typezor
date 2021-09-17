using System;
using System.Collections.Generic;
using System.Linq;

namespace Typezor.Tests.SourceGenerator.Demo
{
    public enum TestA
    {
        A = 1
    }

    public enum TestB
    {
        A = 1
    }

    public class GenerateMapping : Attribute
    {

    }

    public class OverrideMapping : Attribute
    {

    }

    public class MapA
    {
        public string Regular { get; set; }
        public string OnlyGetter { get; set; }
        public string OnlySetter { set{} }

        public string regular;
        public string readonlyField;

        public IEnumerable<string> PrimitiveEnumerable { get; set; }
        public IEnumerable<string> PrimitiveEnumerableOnlyGetter { get; set; }
        public IEnumerable<string> PrimitiveEnumerableOnlySetter { set { } }

        public string[] PrimitiveArray { get; set; }
        public List<string> PrimitiveList { get; set; }

        public MapA Complex { get; set; }
        public IEnumerable<MapA> ComplexEnumerable { get; set; }

        public string FieldFromProperty { get; set; }
        public string PropertyFromField;
        public MapB ComplexDiffrentType { get; set; }
        public string RegularDiffrentType { get; set; }

        public ICollection<string> ICollectionWithoutSetter { get; }
        public List<string> PrimitiveListWithoutSetter { get; }
        public IReadOnlyCollection<string> IReadOnlyCollectionWithoutSetter { get; }

        public TestA Enum { get; set; }
        public TestA? NullableEnum { get; set; }
        public TestA EnumMapping { get; set; }
        public TestA? EnumMappingNullable { get; set; }

        public int Integer { get; set; }
        public int? NullableInteger { get; set; }
        public int? SourceNullableInteger { get; set; }

        public MyCollectionT<TestA> CustomCollection { get; set; }
        public MyCollectionT<IEnumerable<TestA>> NestedCollection { get; set; }
    }

    public class MapB
    {
        public string Regular { get; set; }
        public string OnlyGetter { get; }
        //public string OnlySetter { get; set; }

        public string regular;
        public readonly string readonlyField;

        public IEnumerable<string> PrimitiveEnumerable { get; set; }
        public IEnumerable<string> PrimitiveEnumerableOnlyGetter { get; }
        //public IEnumerable<string> PrimitiveEnumerableOnlySetter { get; set; }
        public IEnumerable<string> PrimitiveArray { get; set; }
        public List<string> PrimitiveList { get; set; }
        public MapB Complex { get; set; }
        public IEnumerable<MapB> ComplexEnumerable { get; set; }
        public string FieldFromProperty;
        public string PropertyFromField { get; set; }
        public MapA ComplexDiffrentType { get; set; }

        public int RegularDiffrentType { get; set; }

        public ICollection<string> ICollectionWithoutSetter { get; }
        public List<string> PrimitiveListWithoutSetter { get; }
        public IReadOnlyCollection<string> IReadOnlyCollectionWithoutSetter { get; set; }

        public TestA Enum { get; set; }
        public TestA? NullableEnum { get; set; }
        public TestB EnumMapping { get; set; }
        public TestB? EnumMappingNullable { get; set; }
        public int Integer { get; set; }
        public int? NullableInteger { get; set; }
        public int SourceNullableInteger { get; set; }

        public MyCollectionT<TestB> CustomCollection { get; set; }
        //public MyCollectionT<TestB>[] NestedCollection { get; set; }
    }

    public class MyCollectionT<T> : List<T>
    {

    }
    [GenerateMapping]
    public static partial class MappingExtensions
    {
        [GenerateMapping] 
        public static partial MapB MapTo(this MapA from, MapB to = default);

        [GenerateMapping]
        public static partial List<MapB> MapTo(this MapA[] from, List<MapB> to = default);

        public static partial Int32 MapTo(this String from, Int32 to)
        {
            return int.Parse(from);
        }

        public static partial String MapTo(this Int32 from, String to)
        {
            return from.ToString();
        }

        //[OverrideMapping]
        //public static partial Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB>[] MapTo(this Typezor.Tests.SourceGenerator.Demo.MyCollectionT<System.Collections.Generic.IEnumerable<Typezor.Tests.SourceGenerator.Demo.TestA>> from, Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB>[] to)
        //{
        //    return from?.Select(p => p.MapTo(default(Typezor.Tests.SourceGenerator.Demo.MyCollectionT<Typezor.Tests.SourceGenerator.Demo.TestB>))).ToArray();
        //}
        //[OverrideMapping]
        //public static MyCollectionT<TestB>[] MapTo(this MyCollectionT<IEnumerable<TestA>> from, MyCollectionT<TestB>[] to)
        //{
        //    return from?.Select(p => p.MapTo(default(MyCollectionT<TestB>))).ToArray();
        //}
    }

    //public static partial class MappingExtensions
    //{

    //    public static partial void MapTo(this string from, int to)
    //    {

    //    }
    //}

    //public partial class NotStaticMappingExtensions
    //{
    //    [GenerateMapping]
    //    public virtual partial MapB MapTo(MapA from, MapB to = default);

    //}

    //public partial class NotStaticMappingExtensions
    //{

    //    public virtual partial MapB MapTo(MapA from, MapB to = default)
    //    {
    //        return null;
    //    }

    //}

    public class Test1
    {
        public Test1()
        {
            var a = new MapA().MapTo(new MapB());
            //a.ICollectionWithoutSetter = new List<string>();
        }
    }
}
