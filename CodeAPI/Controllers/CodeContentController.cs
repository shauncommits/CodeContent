using CodeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CodeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CodeContentController: ControllerBase
{
    private ICodeContentFactory _codeContentFactory;

    public CodeContentController(ICodeContentFactory codeContentFactory)
    {
        _codeContentFactory = codeContentFactory;
    }

    [HttpGet("/GetAll", Name = "GetAll")]   
    public async Task<IActionResult> GetAll()
    {
        var products = await _codeContentFactory.GetAllCodeContents();
        return Ok(products);
    }

    [HttpPost("/AddContent", Name = "AddContent")]
    public async Task<IActionResult> AddContent(CodeContent codeContent)
    {
        codeContent.Id = ObjectId.GenerateNewId();
        
        await _codeContentFactory.AddCodeContent(codeContent);
        return CreatedAtAction(nameof(GetCodeContentByID), new { id = codeContent.Id }, codeContent);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCodeContentByID(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format");
        }
        var codeContent = await _codeContentFactory.GetCodeContentById(objectId);
        if (codeContent == null)
        {
            return NotFound();
        }

        return Ok(id);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCodeContent(string id, CodeContent codeContent)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format");
        }

        codeContent.Id = objectId;
        await _codeContentFactory.UpdateCodeContent(codeContent);
        return NoContent();
    }

    [HttpDelete("DeleteContent/{id}")]
    public async Task<IActionResult> DeleteCodeContent(string id)
    {
        var st = id;
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format");
        }

        var results = await _codeContentFactory.DeleteCodeContent(objectId);
        return new JsonResult(results);
    }
}