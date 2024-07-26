namespace CodeAPI.Models;

public class CodeContentFactory: ICodeContentFactory
{
    private readonly CodeContentDBContext _dbContext;
    private IEnumerable<CodeContent> _codeContents;
    private int count;
    
    public CodeContentFactory(CodeContentDBContext dbContext)
    {
        _dbContext = dbContext;
        _codeContents = _dbContext.CodeContents.ToList();
    }
    
    public CodeContent GetCodeContentByStep(int step)
    {
        return _codeContents.FirstOrDefault(_content => _content.Step == step);
    }

    public IEnumerable<CodeContent> GetAllCodeContents()
    {
        return _codeContents;
    }

    public void AddCodeContent(CodeContent codeContent)
    {
        count = _codeContents.Max(code => code.Step) + 1;
        codeContent.Step = count;
        _dbContext.CodeContents.Add(codeContent);
        _dbContext.SaveChangesAsync();
    }

    public void UpdateCodeContent(CodeContent codeContent)
    {
        var codeContentUpdate = _codeContents.FirstOrDefault(codeContent => codeContent.Step == codeContent.Step);

        codeContentUpdate.Title = codeContent.Title;
        codeContentUpdate.Description = codeContent.Description;
        codeContentUpdate.Picture = codeContent.Picture;
        codeContentUpdate.SubCodeContent = codeContent.SubCodeContent;
        codeContentUpdate.CodeModel = codeContent.CodeModel;

        _dbContext.SaveChangesAsync();
    }

    public void DeleteCodeContent(int id)
    {
        var codeContent = _codeContents.FirstOrDefault(codeContent => codeContent.Step == id);

        if (codeContent != null)
        {
            _dbContext.CodeContents.Remove(codeContent);
            _dbContext.SaveChangesAsync();
        }
    }
}