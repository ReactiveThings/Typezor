using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public sealed class MethodImpl : Method
    {
        private readonly IMethodMetadata _metadata;

        private MethodImpl(IMethodMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }

        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');
        public override string FullName => _metadata.FullName;
        public override bool IsAbstract => _metadata.IsAbstract;
        public override bool IsGeneric => _metadata.IsGeneric;

        private IEnumerable<Attribute> _attributes;
        public override IEnumerable<Attribute> Attributes => _attributes ?? (_attributes = AttributeImpl.FromMetadata(_metadata.Attributes, this));

        private DocComment _docComment;
        public override DocComment DocComment => _docComment ?? (_docComment = DocCommentImpl.FromXml(_metadata.DocComment, this));

        private IEnumerable<TypeParameter> _typeParameters;
        public override IEnumerable<TypeParameter> TypeParameters => _typeParameters ?? (_typeParameters = TypeParameterImpl.FromMetadata(_metadata.TypeParameters, this));

        private IEnumerable<Parameter> _parameters;
        public override IEnumerable<Parameter> Parameters => _parameters ?? (_parameters = ParameterImpl.FromMetadata(_metadata.Parameters, this));

        private Type _type;
        public override Type Type => _type ?? (_type = TypeImpl.FromMetadata(_metadata.Type, this));

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<Method> FromMetadata(IEnumerable<IMethodMetadata> metadata, Item parent)
        {
            return metadata.Select(m => new MethodImpl(m, parent));
        }
    }
}