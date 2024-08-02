using MongoDB.Bson;

namespace CodeAPI.Models;

public interface ICodeContentFactory
{
    Task<CodeContent> GetCodeContentById(ObjectId id);
    Task<List<CodeContent>> GetAllCodeContents();
    Task AddCodeContent(CodeContent codeContent);
    Task UpdateCodeContent(CodeContent codeContent);
    Task<bool> DeleteCodeContent(ObjectId id);
}