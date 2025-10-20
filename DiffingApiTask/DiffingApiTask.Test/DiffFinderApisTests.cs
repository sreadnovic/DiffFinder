using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

[TestFixture]
public class DiffFinderApisTests
{
    private WebApplicationFactory<Program> _factory;
    private Mock<IRepo> _mockRepo;
    private Mock<IDiffFinder> _mockDiffFinder;
    private HttpClient _client;

    [OneTimeSetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IRepo>();
        _mockDiffFinder = new Mock<IDiffFinder>();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mockRepo.Object);
                    services.AddSingleton(_mockDiffFinder.Object);
                });
            });

        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task PutLeft_ValidData_ShouldReturnCreated()
    {
        var testData = new Input { data = "test-data" };
        var content = new StringContent(
            JsonSerializer.Serialize(testData),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PutAsync("/v1/diff/123/left", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        _mockRepo.Verify(r => r.Add("left123", "test-data"), Times.Once);
    }

    [Test]
    public async Task PutRight_ValidData_ShouldReturnCreated()
    {
        var testData = new Input { data = "test-data" };
        var content = new StringContent(
            JsonSerializer.Serialize(testData),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PutAsync("/v1/diff/123/right", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        _mockRepo.Verify(r => r.Add("right123", "test-data"), Times.Once);
    }

    [Test]
    public async Task GetDiff_BothSidesExist_ShouldReturnOk()
    {
        var diffResult = new { result = "test-diff" };
        _mockRepo.Setup(r => r.Get("left123")).Returns("leftData");
        _mockRepo.Setup(r => r.Get("right123")).Returns("rightData");
        _mockDiffFinder.Setup(d => d.GetDiff("leftData", "rightData"))
                        .Returns(diffResult);

        var response = await _client.GetAsync("/v1/diff/123");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _mockDiffFinder.Verify(d => d.GetDiff("leftData", "rightData"), Times.Once);
    }

    [Test]
    public async Task GetDiff_MissingData_ShouldReturnNotFound()
    {
        _mockRepo.Setup(r => r.Get("left123")).Returns((string)null);
        _mockRepo.Setup(r => r.Get("right123")).Returns("rightData");

        var response = await _client.GetAsync("/v1/diff/123");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}