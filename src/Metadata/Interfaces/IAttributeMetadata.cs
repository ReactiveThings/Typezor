using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IAttributeMetadata : INamedItem
    {
        string Value { get; }
        IEnumerable<IAttributeArgumentMetadata> Arguments { get; }
    }
}