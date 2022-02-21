using System.Collections.Generic;
using Typezor.CodeModel;

namespace Typezor.Abstractions
{
    public interface IAnnotated
    {
        /// <summary>
        /// All attributes defined
        /// </summary>
        IEnumerable<Attribute> Attributes { get; }
    }
}