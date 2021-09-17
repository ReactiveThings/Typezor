using System.Threading;
using System.Threading.Tasks;

namespace Typezor;

/// <summary>
/// Non-generic handle for <see cref="TemplateBase{TModel}"/> to simplify usage from reflection.
/// </summary>
public interface ITemplate
{
    /// <summary>
    /// Template Path
    /// </summary>
    string TemplatePath { get; set; }
    /// <summary>
    /// Output associated with this template.
    /// </summary>
    ITemplateOutput Output { get; set; }

    /// <summary>
    /// Model associated with this template.
    /// </summary>
    object? Model { get; set; }

    /// <summary>
    /// Cancellation token associated with this template.
    /// </summary>
    CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Executes the template.
    /// </summary>
    Task ExecuteAsync();
}