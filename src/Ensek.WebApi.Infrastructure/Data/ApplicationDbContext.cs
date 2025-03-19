using Ensek.Application.Common.Interfaces;
using Ensek.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ensek.WebApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public DbSet<MeterReading> MeterReadings { get; set; }

	public DbSet<Account> Accounts { get; set; }

	public Task<bool> EnsureCreatedAsync()
	{
		return Database.EnsureCreatedAsync();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}