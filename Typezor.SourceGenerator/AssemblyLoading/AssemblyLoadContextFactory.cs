using System;
using System.Runtime.InteropServices;
using Typezor.AssemblyLoading;

namespace Typezor.SourceGenerator.AssemblyLoading
{
    public class AssemblyLoadContextFactory
    {
        public static IAssemblyLoadContext Create()
        {
            if (IsRunningInDotNetFramework())
            {
                return new AppDomainAssemblyLoadContext();
            }

            return GetAssemblyLoadContext();
        }

        private static IAssemblyLoadContext GetAssemblyLoadContext()
        {
            Type t = Type.GetType("Typezor.SourceGenerator.AssemblyLoading.ModAssemblyLoadContext");
            return (IAssemblyLoadContext)Activator.CreateInstance(t);
        }

        private static bool IsRunningInDotNetFramework()
        {
            var frameworkDescription = RuntimeInformation.FrameworkDescription;
            var platform = frameworkDescription.Substring(0, frameworkDescription.LastIndexOf(' '));

            return platform == ".NET Framework";
        }
    }
}