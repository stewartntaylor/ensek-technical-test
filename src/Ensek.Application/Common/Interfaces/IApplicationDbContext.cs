
using Ensek.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ensek.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<MeterReading> MeterReadings { get; }

	DbSet<Account> Accounts { get; }

	DatabaseFacade Database { get; }

	Task<bool> EnsureCreatedAsync();

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
