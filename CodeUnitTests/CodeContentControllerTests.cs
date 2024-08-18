namespace CodeAPIUnitTests;

using System.Text.Json;
using CodeAPI.Controllers;
using CodeAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using NSubstitute;
using OpenTelemetry;
using OpenTelemetry.Trace;


public class Tests
{

    private CodeContentController _codeContentController;
    private ICodeContentFactory _codeContentFactory;
    private ILogger<CodeContentController> _logger;
    private Instrumentation _instrumentation;
    private TracerProvider _tracerProvider;

    
    [SetUp]
    public void Setup()
    {
        _codeContentFactory = Substitute.For<ICodeContentFactory>();
        _logger = Substitute.For<ILogger<CodeContentController>>();
        _instrumentation = new Instrumentation();
        // Configure OpenTelemetry with AlwaysOffSampler for tests
        _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("codeContent")
            .SetSampler(new AlwaysOffSampler())
            .Build();        
        _codeContentController = new CodeContentController(_codeContentFactory, _logger, _instrumentation);
    }

    [Test]
    public async Task GetAll_GivenValidCodeContents_WhenHittingEndPointGetAll_ThenReturnStatusCodeOk()
    {
        var codeContents = new List<CodeContent>
        {
            new CodeContent()
            {
                Title = "Testing"
            }
        };
        _codeContentFactory.GetAllCodeContents().Returns(codeContents);

        // Act
        var result = await _codeContentController.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().Be(codeContents);
    }

    [Test]
    public async Task GetAll_GivenNullCodeContents_WhenHittingEndPointGetAll_ThenReturnStatusCodeNotFound()
    {
        // Arrange
        _codeContentFactory.GetAllCodeContents().Returns((List<CodeContent>)null);

        // Act
        var result = await _codeContentController.GetAll();

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.Value.Should().Be("Server might be down, try again after an hour");
    }

    [Test]
    public async Task AddContent_GivenValidCodeContent_WhenCallingAddContent_ThenReturnCodeContent()
    {
        // Arrange
        var codeContent = new CodeContent()
        {
            Id = new ObjectId(),
            Description = "Testing",
            Picture = new JsonElement()
            {

            },
            SubCodeContent = new JsonElement()
            {
                
            },
            Title = "Testing"
        };

        // Act
        var result = _codeContentController.AddContent(codeContent);

        // Assert
        result.Should().NotBeNull();
    }
    
    [Test]
    public async Task AddContent_GivenNullCodeContent_WhenCallingAddContent_ThenReturnStatusCode400()
    {
        // Arrange Act
        var result = await _codeContentController.AddContent(null);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result;
        badRequestResult.Value.Should().Be("CodeContent cannot be null");
    }
    
    [Test]
    public async Task AddContent_GivenValidCodeContent_WhenHittingEndPointAddContent_ThenReturnCreatedAtAction()
    {
        // Arrange
        var codeContent = new CodeContent
        {
            Title = "Testing"
        };
        _codeContentFactory.AddCodeContent(codeContent).Returns(Task.CompletedTask);

        // Act
        var result = await _codeContentController.AddContent(codeContent);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtActionResult = (CreatedAtActionResult)result;
        createdAtActionResult.ActionName.Should().Be(nameof(_codeContentController.GetCodeContentById));
        createdAtActionResult.Value.Should().Be(codeContent);
    }

    [Test]
    public async Task GetCodeContentById_GivenValidIdWhenHittingEndPointGetCodeContentById_ThenReturnsOk()
    {
        // Arrange
        var objectId = ObjectId.GenerateNewId();
        var codeContent = new CodeContent
        {
            Title = "Test",
            Description = "Testing",
            Id = ObjectId.Empty,
            Picture = new JsonElement(){},
            SubCodeContent = new JsonElement()
        };
        _codeContentFactory.GetCodeContentById(objectId).Returns(codeContent);

        // Act
        var result = await _codeContentController.GetCodeContentById(objectId.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().Be(codeContent);
    }

    [Test]
    public async Task GetCodeContentById_GivenInvalidId_WhenHittingEndPointGetCodeContentById_ThenReturnBadRequest()
    {
        // Arrange
        var invalidId = "invalid-id";

        // Act
        var result = await _codeContentController.GetCodeContentById(invalidId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result;
        badRequestResult.Value.Should().Be("Invalid ID format");
    }

    [Test]
    public async Task GetCodeContentById_GivenNullCodeContent_WhenHittingEndPointGetCodeContentById_ReturnStatusNotFound()
    {
        // Arrange
        var objectId = ObjectId.GenerateNewId();
        _codeContentFactory.GetCodeContentById(objectId).Returns((CodeContent)null);

        // Act
        var result = await _codeContentController.GetCodeContentById(objectId.ToString());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}