using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IFileMetadata
    {
        IEnumerable<IClassMetadata> Classes { get; }
        IEnumerable<IDelegateMetadata> Delegates { get; }
        IEnumerable<IEnumMetadata> Enums { get; }
        IEnumerable<IInterfaceMetadata> Interfaces { get; }
    }
}