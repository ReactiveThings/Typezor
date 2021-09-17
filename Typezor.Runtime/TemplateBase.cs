using System.Threading;
using System.Threading.Tasks;

namespace Typezor;

/// <summary>
/// Base implementation of a Razor template.
/// </summary>
public abstract class TemplateBase<TModel> : ITemplate
{
    private string? _lastAttributeSuffix;

    /// <summary>
    /// Template Path
    /// </summary>
    public string TemplatePath { get; set; }

    /// <inheritdoc />
    public ITemplateOutput Output { get; set; }

    /// <inheritdoc cref="ITemplate.Model" />
    public TModel Model { get; set; } = default!;

    object? ITemplate.Model
    {
        get => Model;
        set => Model = (TModel) value!;
    }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Writes a template literal.
    /// </summary>
    protected void WriteLiteral(string? literal) => Output?.Write(literal);

    /// <summary>
    /// Writes a string with encoding.
    /// </summary>
    protected void Write(string? str)
    {
        if (str is not null)
            WriteLiteral(str);
    }


    /// <summary>
    /// Writes an object.
    /// </summary>
    protected void Write(object? obj)
    {
        if (obj is string str)
        {
            Write(str);
        }
        else
        {
            Write(obj?.ToString());
        }
    }

    /// <summary>
    /// Begins writing attribute.
    /// </summary>
    protected void BeginWriteAttribute(
        string? name,
        string? prefix,
        int prefixOffset,
        string? suffix,
        int suffixOffset,
        int attributeValuesCount)
    {
        _lastAttributeSuffix = suffix;
        WriteLiteral(prefix);
    }

    /// <summary>
    /// Writes attribute value.
    /// </summary>
    protected void WriteAttributeValue(
        string? prefix,
        int prefixOffset,
        object? value,
        int valueOffset,
        int valueLength,
        bool isLiteral)
    {
        WriteLiteral(prefix);
        Write(value);
    }

    /// <summary>
    /// Ends writing attribute.
    /// </summary>
    protected void EndWriteAttribute()
    {
        WriteLiteral(_lastAttributeSuffix);
        _lastAttributeSuffix = null;
    }

    /// <inheritdoc />
    public virtual Task ExecuteAsync() { return Task.CompletedTask;}
}