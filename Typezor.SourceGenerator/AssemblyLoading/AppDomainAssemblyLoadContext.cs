using System.IO;
using System.Reflection;
using Typezor.AssemblyLoading;

namespace Typezor.SourceGenerator.AssemblyLoading
{
    public class AppDomainAssemblyLoadContext : IAssemblyLoadContext
    {
        private readonly string _tempDirectory;

        public AppDomainAssemblyLoadContext()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "TypezorSourceGenerator");
            if (Directory.Exists(_tempDirectory) == false)
            {
                Directory.CreateDirectory(_tempDirectory);
            }
        }

        public Assembly LoadFromAssemblyPath(string filePath)
        {
            var assembly = Assembly.LoadFile(filePath);
            CopyAssembly(assembly);
            return assembly;
        }

        public Assembly LoadFromAssemblyName(AssemblyName getName)
        {
            return Assembly.Load(getName);
        }

        public Assembly LoadFromStream(Stream assemblyStream)
        {
            byte[] data = new byte[assemblyStream.Length];
            assemblyStream.Read(data, 0, data.Length);

            var filename = Path.GetRandomFileName();
            var path = Path.Combine(_tempDirectory, filename);
            File.WriteAllBytes(path, data);
            return Assembly.LoadFrom(path);
        }

        private void CopyAssembly(Assembly assembly)
        {
            var asmSourcePath = assembly.Location;
            var asmDestPath = Path.Combine(_tempDirectory, Path.GetFileName(asmSourcePath));
            try
            {
                File.Copy(asmSourcePath, asmDestPath, true);
            }
            catch
            {
                // File may be in use
            }
        }
    }
}