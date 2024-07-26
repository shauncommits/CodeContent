namespace CodeAPI.Models;

public interface ICodeModel
{
    string Language { get; set; }
    string Value { get; set; }
    string Uri { get; set; }
}