using System.Net;

[TestFixture]
public class CodeApiTests
{
    private HttpClient? _httpClient;

    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        // Remember to dispose of it.
        _httpClient?.Dispose();
    }

    [Test]
    public async Task GetAll_GivenCorrectUrl_WhenHittingUrlofCodeContent_ThenReturnStatusOk()
    {
        // Arrange Act
        var response = await _httpClient!.GetAsync("http://localhost:8080/GetAll");
        var responseContent = await response.Content.ReadAsStreamAsync();
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

}