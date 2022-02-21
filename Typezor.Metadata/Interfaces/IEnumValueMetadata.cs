using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IEnumValueMetadata : INamedItem
    {
        string DocComment { get; }
        IEnumerable<IAttributeMetadata> Attributes { get; }
        long Value { get; }
    }
}