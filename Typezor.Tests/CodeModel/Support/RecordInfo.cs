using Typezor.Tests.CodeModel.Support.Interfaces;

#pragma warning disable 67
namespace Typezor.Tests.CodeModel.Support.Record
{
    /// <summary>
    /// summary
    /// </summary>
    [AttributeInfo]
    public record RecordInfo(string ImmutableProperty) : BaseRecordInfo(ImmutableProperty), IInterfaceInfo
    {

        public const string PublicConstant = "";
        internal const string InternalConstant = "";

        public delegate void PublicDelegate<T>(string param1, T param2);
        internal delegate void InternalDelegate();

        public event Delegate PublicEvent;
        internal event Delegate InternalEvent;

        public string PublicField = "";
        internal string InternalField = "";

        public static string PublicStaticField = "";
        internal static string InternalStaticField = "";

        public void PublicMethod() { }
        internal void InternalMethod() { }

        public static void PublicStaticMethod() { }
        internal static void InternalStaticMethod() { }

        public string PublicProperty { get; set; }
        internal string InternalProperty { get; set; }

        public static string PublicStaticProperty { get; set; }
        internal static string InternalStaticProperty { get; set; }

        public class NestedClassInfo
        {
            public string PublicNestedProperty { get; set; }
        }

        public record NestedRecordInfo
        {
            public string PublicNestedProperty { get; set; }
        }

        public interface INestedInterfaceInfo
        {
            string PublicNestedProperty { get; set; }
        }

        public enum NestedEnumInfo
        {
            NestedValue
        }

    }

    public record BaseRecordInfo(string ImmutableBaseProperty)
    {
        public string PublicBaseProperty { get; set; }
    }

    public record GenericRecordInfo<T>
    {
    }

    public record InheritGenericRecordInfo : GenericRecordInfo<string>
    {
    }
}

namespace Typezor.Tests.CodeModel.Support.Class.NestedNamespace
{
    public record RecordInNestedNamespaceInfo
    {

    }
}

namespace Typezor.Tests.CodeModel.Support.Class.NestedNamespace.NestedNamespace
{
    public record RecordInTwoNestedNamespaceInfo
    {

    }
}