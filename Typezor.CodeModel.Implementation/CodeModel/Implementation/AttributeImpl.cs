using System.Collections.Generic;
using System.Linq;

using Typezor.Metadata.Interfaces;
using static Typezor.CodeModel.Helpers;

namespace Typezor.CodeModel.Implementation
{
    public sealed class AttributeImpl : Attribute
    {
        private readonly IAttributeMetadata _metadata;

        private AttributeImpl(IAttributeMetadata metadata, Item parent)
        {
            _metadata = metadata;
            Parent = parent;
        }

        public override Item Parent { get; }

        public override string name => CamelCase(_metadata.Name.TrimStart('@'));
        public override string Name => _metadata.Name.TrimStart('@');
        public override string FullName => _metadata.FullName;
        public override string Value => GetValue(_metadata.Value);

        private IEnumerable<AttributeArgumentImpl> _arguments;
        public override IEnumerable<AttributeArgument> Arguments => _arguments ?? (_arguments = AttributeArgumentImpl.FromMetadata(_metadata.Arguments, this));

        private static string GetValue(string value)
        {
            if (value == null) return null;

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                var trimmed = value.Substring(1, value.Length - 2);

                if (trimmed.Replace("\\\"", string.Empty).Contains("\"") == false)
                    return trimmed;
            }

            return value;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<AttributeImpl> FromMetadata(IEnumerable<IAttributeMetadata> metadata, Item parent)
        {
            return metadata.Select(a => new AttributeImpl(a, parent));
        }
    }
}