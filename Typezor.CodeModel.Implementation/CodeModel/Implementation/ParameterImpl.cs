using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public sealed class ParameterImpl : Parameter
    {
        private readonly IParameterMetadata _metadata;

        private ParameterImpl(IParameterMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }

        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');
        public override string FullName => _metadata.FullName;
        public override bool HasDefaultValue => _metadata.HasDefaultValue;
        public override string DefaultValue => _metadata.DefaultValue;

        private IEnumerable<Attribute> _attributes;
        public override IEnumerable<Attribute> Attributes => _attributes ?? (_attributes = AttributeImpl.FromMetadata(_metadata.Attributes, this));

        private Type _type;
        public override Type Type => _type ?? (_type = TypeImpl.FromMetadata(_metadata.Type, this));

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<Parameter> FromMetadata(IEnumerable<IParameterMetadata> metadata, Item parent)
        {
            return metadata.Select(p => new ParameterImpl(p, parent));
        }
    }
}