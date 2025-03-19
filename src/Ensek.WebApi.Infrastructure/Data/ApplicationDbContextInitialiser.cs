using Ensek.Application.Common.Files;
using Ensek.Application.Common.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ensek.WebApi.Infrastructure.Data;

public class ApplicationDbContextInitialiser(
	IApplicationDbContext dbContext, 
	IAccountsFile customerAccountsFile)
{
	private readonly IApplicationDbContext _dbContext = dbContext;
	private readonly IAccountsFile _customerAccountsFile = customerAccountsFile;

	public async Task SeedDataAsync(string accountsFilePath)
	{
		await _dbContext.EnsureCreatedAsync();

		if (!_dbContext.Accounts.Any())
		{
			var accounts = await _customerAccountsFile.ExtractAccountsAsync(accountsFilePath, default);

			await _dbContext.Accounts.AddRangeAsync(accounts);
			await _dbContext.SaveChangesAsync(default);
		}
	}
}
