using System.Collections.Generic;
using Typezor.Metadata.Interfaces;

namespace Typezor.CodeModel.Implementation
{
    public sealed class FileImpl : File
    {
        private readonly IFileMetadata _metadata;

        public FileImpl(IFileMetadata metadata)
        {
            _metadata = metadata;
        }

        private IEnumerable<CodeModel.Class> _classes;
        public override IEnumerable<CodeModel.Class> Classes => _classes ?? (_classes = ClassImpl.FromMetadata(_metadata.Classes, this));

        private IEnumerable<CodeModel.Delegate> _delegates;
        public override IEnumerable<CodeModel.Delegate> Delegates => _delegates ?? (_delegates = DelegateImpl.FromMetadata(_metadata.Delegates, this));

        private IEnumerable<Enum> _enums;
        public override IEnumerable<Enum> Enums => _enums ?? (_enums = EnumImpl.FromMetadata(_metadata.Enums, this));

        private IEnumerable<Interface> _interfaces;
        public override IEnumerable<Interface> Interfaces => _interfaces ?? (_interfaces = InterfaceImpl.FromMetadata(_metadata.Interfaces, this));
        
        public override File GetTypesFromNamespace(params string[] namespaces)
        {
            return new FileImpl(_metadata.GetTypesFromNamespace(namespaces));
        }
    }
}
