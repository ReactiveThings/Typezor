using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IEventMetadata : INamedItem
    {
        string DocComment { get; }
        IEnumerable<IAttributeMetadata> Attributes { get; }
        ITypeMetadata Type { get; }
    }
}