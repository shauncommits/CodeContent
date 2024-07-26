using Microsoft.EntityFrameworkCore;

namespace CodeAPI.Models;

public class CodeContentDBContext: DbContext
{
    public CodeContentDBContext(DbContextOptions<CodeContentDBContext> options) : base(options) { }
    
    public DbSet<CodeContent> CodeContents { get; set; }
}