using Shouldly;
using System.Net;

namespace Ensek.WebApi.IntegrationTests;

[Collection("Collection 1")]
public class EndpointTests : ApiFixture
{
	public EndpointTests(TestWebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task Health_Should_Return_Ok()
	{
		// Act
		var response = await Client.GetAsync("health");

		// Assert
		response.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task NonExistent_Endpoint_Returns_Json_Problem_Not_Found()
	{
		// Act
		var response = await Client.GetAsync("does-not-exist");

		// Assert
		response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		response.Content.Headers.ContentType?.ToString()
			.ShouldContain("application/problem+json");

		var content = await response.Content.ReadAsStringAsync();
		content.ShouldContain("https://tools.ietf.org/html/rfc9110");
	}
}