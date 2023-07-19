using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Typezor.AssemblyLoading;

namespace Typezor.SourceGenerator.AssemblyLoading
{
    public class ModAssemblyLoadContext : IAssemblyLoadContext
    {
        Assembly IAssemblyLoadContext.LoadFromAssemblyPath(string filePath)
        {
            
            return AssemblyLoadContext.GetLoadContext(typeof(ITemplateOutput).Assembly).LoadFromAssemblyPath(filePath);
        }

        Assembly IAssemblyLoadContext.LoadFromAssemblyName(AssemblyName getName)
        {
            return AssemblyLoadContext.GetLoadContext(typeof(ITemplateOutput).Assembly).LoadFromAssemblyName(getName);
        }

        Assembly IAssemblyLoadContext.LoadFromStream(Stream assemblyStream)
        {
            return AssemblyLoadContext.GetLoadContext(typeof(ITemplateOutput).Assembly).LoadFromStream(assemblyStream);
        }
    }
}