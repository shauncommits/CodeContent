using CodeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Diagnostics;
using CodeAPI.Constants;

namespace CodeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CodeContentController: ControllerBase
{
    private ICodeContentFactory _codeContentFactory;
    
    // For OpenTelemetry Instrumentation
    private ILogger<CodeContentController> logger;
    private ActivitySource activitySource;

    public CodeContentController(ICodeContentFactory codeContentFactory, ILogger<CodeContentController> logger, Instrumentation instrumentation)
    {
        this.logger = logger;
        _codeContentFactory = codeContentFactory;
        activitySource = instrumentation.ActivitySource;
    }

    /// <summary>
    /// Gets all CodeContent saved from the database
    /// </summary>
    /// <remarks>This function should get CodeContent and return it with status code 200</remarks>
    /// <response code="200">CodeContent returned</response>
    /// <response code="500">Server or DB is down</response>
    [HttpGet("/GetAll", Name = "GetAll")]   
    public async Task<IActionResult> GetAll()
    {
        using (var myActivity = activitySource.StartActivity(OpenTelemetryConstants.ServiceName))
        {
            var codeContents = await _codeContentFactory.GetAllCodeContents();

            if (codeContents == null)
            {
                StatusCode(500);
                logger.LogError("Server down");
                myActivity.SetTag("Error: Server might be down", StatusCode(500));
                return NotFound("Server might be down, try again after an hour");
            }
            logger.LogInformation("Content successfully retrieved: {result}", codeContents);
            myActivity.SetTag("Content successfully retrieved, the length of the content is: ", codeContents.Count);
            return Ok(codeContents);
        }
    }
    
    /// <summary>
    /// Adds CodeContent into the database
    /// </summary>
    /// <remarks>This function should add CodeContent into the database</remarks>
    /// <response code="200">Added CodeContent returned</response>
    /// <response code="400">CodeCode content is null </response>
    /// <response code="500">Server or DB is down</response>
    [HttpPost("/AddContent", Name = "AddContent")]
    public async Task<IActionResult> AddContent(CodeContent codeContent)
    {
        using (var myActivity = activitySource.StartActivity(OpenTelemetryConstants.ServiceName))
        {
            if (codeContent == null)
            {
                StatusCode(400);
                myActivity.SetTag("CodeContent cannot be null", StatusCode(400));
                return BadRequest("CodeContent cannot be null");
            }

            codeContent.Id = ObjectId.GenerateNewId();

            await _codeContentFactory.AddCodeContent(codeContent);
            myActivity.SetTag("Content successfully added: ", codeContent);
            return CreatedAtAction(nameof(GetCodeContentById), new { id = codeContent.Id }, codeContent);
            
        }
    }

    /// <summary>
    /// Gets CodeContent from the database using an id
    /// </summary>
    /// <remarks>This function should get CodeContent from the database</remarks>
    /// <response code="200">Returns CodeContent</response>
    /// <response code="400">CodeContent missing or user provided incorrect ID</response>
    /// <response code="500">Server or DB is down</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCodeContentById(string id)
    {
        using (var myActivity = activitySource.StartActivity(OpenTelemetryConstants.ServiceName))
        {

            if (!ObjectId.TryParse(id, out var objectId))
            {
                StatusCode(400);
                myActivity.SetTag("Invalid ID format", StatusCode(400));
                return BadRequest("Invalid ID format");
            }

            var codeContent = await _codeContentFactory.GetCodeContentById(objectId);
            if (codeContent == null)
            {
                StatusCode(500);
                myActivity.SetTag("Something went wrong with retrieving CodeContent by ID: ", StatusCode(500));
                return NotFound();
            }
            
            myActivity.SetTag("Content successfully retrieved: ", codeContent);
            return Ok(codeContent);
        }
    }
    
    /// <summary>
    /// Updates CodeContent from the database
    /// </summary>
    /// <remarks>This function should update CodeContent from the database</remarks>
    /// <response code="200">Return status code 200</response>
    /// <response code="400">CodeContent missing/ has incorrect values</response>
    /// <response code="500">Server or DB is down</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCodeContent(string id, CodeContent codeContent)
    {
        using (var myActivity = activitySource.StartActivity(OpenTelemetryConstants.ServiceName))
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                if (codeContent == null)
                {
                    myActivity.SetTag("Supplied null CodeContent", codeContent);
                    return StatusCode(400, "CodeContent cannot be null");
                }

                myActivity.SetTag("User supplied invalid ID format: ", codeContent);
                return BadRequest("Invalid ID format");
            }

            codeContent.Id = objectId;

            await _codeContentFactory.UpdateCodeContent(codeContent);
            myActivity.SetTag("CodeContent retrieved successfully: ", codeContent);
            return Ok();
        }
    }

    /// <summary>
    /// Deletes CodeContent from the database
    /// </summary>
    /// <remarks>This function should delete a CodeContent from the database using an ID</remarks>
    /// <response code="200">True is returned</response>
    /// <response code="400">CodeContent missing/ ID is incorrect</response>
    /// <response code="500">Server or DB is down</response>
    [HttpDelete("DeleteContent/{id}")]
    public async Task<IActionResult> DeleteCodeContent(string id)
    {
        using (var myActivity = activitySource.StartActivity(OpenTelemetryConstants.ServiceName))
        {
            var st = id;
            if (!ObjectId.TryParse(id, out var objectId))
            {
                myActivity.SetTag("Invalid ID format ", StatusCode(400));
                return BadRequest("Invalid ID format");
            }

            var results = await _codeContentFactory.DeleteCodeContent(objectId);

            if (results)
            {
                StatusCode(200);
                myActivity.SetTag("Code content deleted successfully", StatusCode(400));
                return new JsonResult(results);
            }

            StatusCode(400);
            myActivity.SetTag("Code content does not exist in the database or the server is done ", StatusCode(400));
            return new JsonResult(results);
        }
    }
}