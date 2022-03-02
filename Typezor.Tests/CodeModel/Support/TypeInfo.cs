using System.Collections.Generic;
using Typezor.Tests.CodeModel.Support.Class;
using Typezor.Tests.CodeModel.Support.Record;

namespace Typezor.Tests.CodeModel.Support
{
    public class TypeInfo
    {
        public ClassInfo Class { get; set; }
        public RecordInfo Record { get; set; }
        public BaseClassInfo BaseClass { get; set; }
        public GenericClassInfo<string> GenericClass { get; set; }
        public InheritGenericClassInfo InheritGenericClass { get; set; }

        public string String { get; set; }
        public ICollection<ClassInfo> ClassCollection { get; set; }
        public ICollection<string> StringCollection { get; set; }
    }
}
