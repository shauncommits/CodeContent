namespace CodeAPI.Models;

public class CodeModel: ICodeModel
{
    public string Language { get; set; }
    public string Value { get; set; }
    public string Uri { get; set; }
}