${
    Template(Settings settings)
    {
        settings.IncludeProject("Typezor.Tests.SourceGenerator.Demo");
        settings.IncludeProject("Typezor.Tests.SourceGenerator.ReferencedProject");
        settings.OutputExtension = ".cs";
    }
}

$Classes(p => p.Name.StartsWith("Class") && p.Namespace.StartsWith("Typezor.Tests.SourceGenerator"))[ 
namespace Typezor.Tests.SourceGenerator.Demo
{
    public class Generated$Name {
        
    }
}
@@SaveAsFile:Generated$Name@@
]

