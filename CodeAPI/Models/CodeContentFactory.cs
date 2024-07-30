using MongoDB.Bson;
using MongoDB.Driver;

namespace CodeAPI.Models;

public class CodeContentFactory: ICodeContentFactory
{
    private readonly IMongoCollection<CodeContent> _collection;
    public CodeContentFactory(IMongoDatabase database)
    {
        _collection = database.GetCollection<CodeContent>("CodeContents");
    }
    
    public async Task<CodeContent> GetCodeContentById(ObjectId id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<CodeContent>> GetAllCodeContents()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task AddCodeContent(CodeContent codeContent)
    {
        await _collection.InsertOneAsync(codeContent);
    }

    public async Task UpdateCodeContent(CodeContent codeContent)
    {
        await _collection.ReplaceOneAsync(p => p.Id == codeContent.Id, codeContent);
    }

    public async Task<bool> DeleteCodeContent(ObjectId id)
    {
        var filter = Builders<CodeContent>.Filter.Eq(r => r.Id, id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount == 1;
    }
}