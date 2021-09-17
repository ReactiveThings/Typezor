using System;

namespace Typezor.Metadata.Interfaces
{
    public interface IPropertyMetadata : IFieldMetadata
    {
        bool IsAbstract { get; }
        bool HasGetter { get; }
        bool HasSetter { get; }
    }
}