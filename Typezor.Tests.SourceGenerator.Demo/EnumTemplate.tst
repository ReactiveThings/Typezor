${
    Template(Settings settings)
    {
        settings.IncludeProject("Typezor.Tests.SourceGenerator.ReferencedProject");
        settings.OutputExtension = ".cs";
    }
}

$Enums(p => p.Namespace.StartsWith("Typezor.Tests.SourceGenerator"))[ 
namespace Typezor.Tests.SourceGenerator.Demo
{
    public enum Generated$Name {
        
    }
}
@@SaveAsFile:Generated$Name@@
]

