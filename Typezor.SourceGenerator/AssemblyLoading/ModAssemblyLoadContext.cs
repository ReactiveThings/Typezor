using System.Reflection;
using System.Runtime.Loader;
using Typezor;
using Typezor.AssemblyLoading;
using Typezor.CodeModel;

namespace Typezor.SourceGenerator.AssemblyLoading
{
    public class ModAssemblyLoadContext : AssemblyLoadContext, IAssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (typeof(File).Assembly.GetName().FullName == assemblyName.FullName)
            {
                return typeof(File).Assembly;
            }

            if (typeof(ITemplate).Assembly.GetName().FullName == assemblyName.FullName)
            {
                return typeof(ITemplate).Assembly;
            }

            return Default.LoadFromAssemblyName(assemblyName);
        }
    }
}