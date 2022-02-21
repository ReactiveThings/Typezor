using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IMethodMetadata : IFieldMetadata
    {
        bool IsAbstract { get; }
        bool IsGeneric { get; }
        IEnumerable<ITypeParameterMetadata> TypeParameters { get; }
        IEnumerable<IParameterMetadata> Parameters { get; }
    }
}