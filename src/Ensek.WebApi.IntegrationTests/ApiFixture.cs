
namespace Ensek.WebApi.IntegrationTests;

public class ApiFixture : IClassFixture<TestWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public ApiFixture(TestWebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }
}