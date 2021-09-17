using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;

namespace Typezor.CodeModel.Implementation
{
    public class AttributeArgumentImpl : AttributeArgument
    {
        private IAttributeArgumentMetadata _metadata;
        private readonly Item parent;

        public AttributeArgumentImpl(IAttributeArgumentMetadata metadata, Item parent)
        {
            _metadata = metadata;
            this.parent = parent;
        }
        public override Type Type => TypeImpl.FromMetadata(_metadata.Type, parent);

        public override Type TypeValue => TypeImpl.FromMetadata(_metadata.TypeValue, parent);

        public override object Value => _metadata.Value;

        public static IEnumerable<AttributeArgumentImpl> FromMetadata(IEnumerable<IAttributeArgumentMetadata> metadata, Item parent)
        {
            return metadata.Select(a => new AttributeArgumentImpl(a, parent));
        }
    }
}
