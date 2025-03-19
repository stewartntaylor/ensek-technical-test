using Ensek.WebApi.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.WebApi.IntegrationTests;

public sealed class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
		builder.UseEnvironment("IntegrationTests");

		base.ConfigureWebHost(builder);

		builder.ConfigureTestServices(services =>
        {
			// Remove the ApplicationDbContext
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbContextOptions<ApplicationDbContext>));

			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			// Add ApplicationDbContext using an in-memory database for testing.
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseInMemoryDatabase("InMemoryDbForTesting");
			});
		});
    }
}