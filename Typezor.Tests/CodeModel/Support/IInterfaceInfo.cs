namespace Typezor.Tests.CodeModel.Support.Interfaces
{
    /// <summary>
    /// summary
    /// </summary>
    [AttributeInfo]
    public interface IInterfaceInfo : IBaseInterfaceInfo
    {
        event Delegate PublicEvent;
        void PublicMethod();
        string PublicProperty { get; set; }
    }

    public interface IBaseInterfaceInfo
    {
        string PublicBaseProperty { get; set; }
    }

    public interface IGenericInterface<T>
    {
    }

    public interface IInheritGenericInterfaceInfo : IGenericInterface<string>
    {
    }

    public class InterfaceContiningClassInfo
    {
        public interface INestedInterfaceInfo
        {
        }
    }
}

namespace Typezor.Tests.CodeModel.Support.Interfaces.NestedNamespace
{
    public interface InterfaceInNestedNamespaceInfo
    {

    }
}

namespace Typezor.Tests.CodeModel.Support.Interfaces.NestedNamespace.Nested
{
    public interface InterfaceInTwoNestedNamespaceInfo
    {

    }
}