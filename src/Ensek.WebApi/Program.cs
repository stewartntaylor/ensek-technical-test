
using Ensek.Application;
using Ensek.WebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ensek.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(context.Configuration)
			);

			builder.Services.AddProblemDetails();

			builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			builder.Services.AddApplicationServices();
			builder.Services.AddInfrastructureServices(builder.Configuration);

			builder.Services.AddHealthChecks();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
			else
			{
				app.UseHsts();
			}

			app.UseStatusCodePages();
			app.UseExceptionHandler();

			app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

			app.MapHealthChecks("/health");

			using (var scope = app.Services.CreateScope())
			{
				var dataInitialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
				await dataInitialiser.SeedDataAsync(builder.Configuration["AccountsSeedDataFilePath"]!);
			}

			app.Run();
        }
    }
}
