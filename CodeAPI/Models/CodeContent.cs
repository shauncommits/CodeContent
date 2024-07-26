namespace CodeAPI.Models;

public class CodeContent
{
    public string Title { get; set; }
    public int Step { get; set; }
    public string Description { get; set; }
    public object[] Picture { get; set; }
    public CodeContent[] SubCodeContent { get; set; }
    public CodeModel CodeModel { get; set; }
}