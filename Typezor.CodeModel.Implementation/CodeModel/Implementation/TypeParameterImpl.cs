using System.Collections.Generic;
using System.Linq;
using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public class TypeParameterImpl : TypeParameter
    {
        private readonly ITypeParameterMetadata _metadata;

        public TypeParameterImpl(ITypeParameterMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }
        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');

        public static IEnumerable<TypeParameter> FromMetadata(IEnumerable<ITypeParameterMetadata> metadata, Item parent)
        {
            return metadata.Select(t => new TypeParameterImpl(t, parent));
        }
    }
}