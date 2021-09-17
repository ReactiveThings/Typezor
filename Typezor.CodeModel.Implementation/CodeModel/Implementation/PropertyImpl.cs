using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public sealed class PropertyImpl : Property
    {
        private readonly IPropertyMetadata _metadata;

        private PropertyImpl(IPropertyMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }
        
        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');
        public override string FullName => _metadata.FullName;
        public override bool HasGetter => _metadata.HasGetter;
        public override bool HasSetter => _metadata.HasSetter;
        public override bool IsAbstract => _metadata.IsAbstract;

        private IEnumerable<Attribute> _attributes;
        public override IEnumerable<Attribute> Attributes => _attributes ?? (_attributes = AttributeImpl.FromMetadata(_metadata.Attributes, this));

        private DocComment _docComment;
        public override DocComment DocComment => _docComment ?? (_docComment = DocCommentImpl.FromXml(_metadata.DocComment, this));

        private Type _type;
        public override Type Type => _type ?? (_type = TypeImpl.FromMetadata(_metadata.Type, this));

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<CodeModel.Property> FromMetadata(IEnumerable<IPropertyMetadata> metadata, Item parent)
        {
            return metadata.Select(p => new PropertyImpl(p, parent));
        }
    }
}