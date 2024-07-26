namespace CodeAPI.Models;

public interface ICodeContentFactory
{
    CodeContent GetCodeContentByStep(int step);
    IEnumerable<CodeContent> GetAllCodeContents();
    void AddCodeContent(CodeContent codeContent);
    void UpdateCodeContent(CodeContent codeContent);
    void DeleteCodeContent(int id);
}