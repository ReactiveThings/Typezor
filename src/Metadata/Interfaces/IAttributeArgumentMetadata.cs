namespace Typezor.Metadata.Interfaces
{
    public interface IAttributeArgumentMetadata
    {
        ITypeMetadata Type { get; }

        ITypeMetadata TypeValue { get; }

        object Value { get; }
    }
}
