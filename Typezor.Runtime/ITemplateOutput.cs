namespace Typezor;

public interface ITemplateOutput
{
    void Write(string text);
    string SaveAs(string filePath);
    string AddSource(string hintName);
}