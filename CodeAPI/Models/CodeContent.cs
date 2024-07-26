namespace CodeAPI.Models;

public class CodeContent
{
    private string Title { get; set; }
    private int Step { get; set; }
    private string Description { get; set; }
    private object[] Picture { get; set; }
    private CodeContent[] SubCodeContent { get; set; }
}