using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public sealed class ConstantImpl : Constant
    {
        private readonly IConstantMetadata _metadata;

        private ConstantImpl(IConstantMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }

        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');
        public override string FullName => _metadata.FullName;

        private IEnumerable<Attribute> _attributes;
        public override IEnumerable<Attribute> Attributes => _attributes ?? (_attributes = AttributeImpl.FromMetadata(_metadata.Attributes, this));

        private DocComment _docComment;
        public override DocComment DocComment => _docComment ?? (_docComment = DocCommentImpl.FromXml(_metadata.DocComment, this));

        private Type _type;
        public override Type Type => _type ?? (_type = TypeImpl.FromMetadata(_metadata.Type, this));

        public override string Value => _metadata.Value;

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<CodeModel.Constant> FromMetadata(IEnumerable<IConstantMetadata> metadata, Item parent)
        {
            return metadata.Select(c => new ConstantImpl(c, parent));
        }
    }
}