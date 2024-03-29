﻿using System.Collections.Generic;

namespace Typezor.Metadata.Interfaces
{
    public interface IParameterMetadata : INamedItem
    {
        bool HasDefaultValue { get; }
        string DefaultValue { get; }
        IEnumerable<IAttributeMetadata> Attributes { get; }
        ITypeMetadata Type { get; }
    }
}