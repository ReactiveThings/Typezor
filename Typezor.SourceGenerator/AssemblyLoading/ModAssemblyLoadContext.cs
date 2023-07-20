using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Typezor.AssemblyLoading;

namespace Typezor.SourceGenerator.AssemblyLoading
{
    public class ModAssemblyLoadContext : IAssemblyLoadContext
    {
        private static readonly AssemblyLoadContext currentContext = AssemblyLoadContext.GetLoadContext(typeof(ITemplateOutput).Assembly);
        Assembly IAssemblyLoadContext.LoadFromAssemblyPath(string filePath)
        {
            return currentContext.LoadFromAssemblyPath(filePath);
        }

        Assembly IAssemblyLoadContext.LoadFromAssemblyName(AssemblyName getName)
        {
            return currentContext.LoadFromAssemblyName(getName);
        }

        Assembly IAssemblyLoadContext.LoadFromStream(Stream assemblyStream)
        {
            return currentContext.LoadFromStream(assemblyStream);
        }
    }
}