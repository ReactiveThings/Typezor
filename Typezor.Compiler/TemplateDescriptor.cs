using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Typezor.AssemblyLoading;

namespace Typezor;

/// <summary>
/// Represents a compiled Razor template.
/// </summary>
public class TemplateDescriptor
{
    private readonly Type _templateType;

    /// <summary>
    /// Initializes an instance of <see cref="TemplateDescriptor"/>.
    /// </summary>
    public TemplateDescriptor(Type templateType) => _templateType = templateType;

    private ITemplate CreateTemplateInstance() =>  (ITemplate)(
        Activator.CreateInstance(_templateType) ??
        throw new InvalidOperationException($"Could not instantiate compiled template of type '{_templateType}'.")
    );

    /// <summary>
    /// Renders the template using the specified writer.
    /// </summary>
    public async Task RenderAsync(object model, string templateFilePath, ITemplateOutput templateOutput, CancellationToken cancellationToken = default)
    {
        var template = CreateTemplateInstance();

        template.Output = templateOutput;

        template.Model = model;

        template.CancellationToken = cancellationToken;
        template.TemplatePath = templateFilePath;

        await template.ExecuteAsync();
    }

    /// <summary>
    /// Compiles a Razor template into executable code.
    /// </summary>
    /// <remarks>
    /// Compiled resources are stored in memory and cannot be released.
    /// Use the overload that takes <see cref="AssemblyLoadContext"/> to specify a custom assembly context that can be unloaded.
    /// </remarks>
    public static TemplateDescriptor Compile(
        string source, 
        List<Assembly> razorReferences,
        IAssemblyLoadContext assemblyLoadContext, 
        string templatePath, 
        List<string> razorClasses)
    {
        var csharpCode = Razor.Transpile(source, templatePath);
        var sources = new[] { csharpCode }.Concat(razorClasses);


        var type = new Compiler(assemblyLoadContext).Compile<ITemplate>(
            sources,
            razorReferences
        );

        return new TemplateDescriptor(type);
    }
}