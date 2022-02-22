namespace Typezor.Metadata.Interfaces
{
    public interface IConstantMetadata : IFieldMetadata
    {
        string Value { get; }
    }
}