using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IFieldMetadata : INamedItem
    {
        string DocComment { get; }
        IEnumerable<IAttributeMetadata> Attributes { get; }
        ITypeMetadata Type { get; }
    }

    public interface IClassFieldMetadata : IFieldMetadata
    {
        bool IsReadOnly { get; }
    }
}