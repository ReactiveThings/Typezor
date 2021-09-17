using System.IO;
using System.Reflection;

namespace Typezor.AssemblyLoading;

public interface IAssemblyLoadContext
{
    Assembly LoadFromAssemblyPath(string filePath);
    Assembly LoadFromAssemblyName(AssemblyName getName);
    Assembly LoadFromStream(Stream assemblyStream);
}