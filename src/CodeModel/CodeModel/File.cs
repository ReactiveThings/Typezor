

using System.Collections.Generic;

namespace Typezor.CodeModel
{
    /// <summary>
    /// Represents a file.
    /// </summary>
    public abstract class File : Item
    {
        /// <summary>
        /// All public classes defined in the file.
        /// </summary>
        public abstract IEnumerable<CodeModel.Class> Classes { get; }

        /// <summary>
        /// All public delegates defined in the file.
        /// </summary>
        public abstract IEnumerable<CodeModel.Delegate> Delegates { get; }

        /// <summary>
        /// All public enums defined in the file.
        /// </summary>
        public abstract IEnumerable<Enum> Enums { get; }

        /// <summary>
        /// All public interfaces defined in the file.
        /// </summary>
        public abstract IEnumerable<Interface> Interfaces { get; }

        /// <summary>
        /// All public types.
        /// </summary>
        public abstract File GetTypesFromNamespace(params string[] namespaces);
    }
}