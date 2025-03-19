using CsvHelper;
using Ensek.Application.Common.Interfaces;
using Ensek.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Ensek.Application.Common.Files;

/// <summary>
/// Simple Accounts file reader
/// </summary>
/// <param name="logger">Logger</param>
public class AccountsFile(ILogger<MeterReadingsFile> logger) : IAccountsFile
{
	private readonly ILogger<MeterReadingsFile> _logger = logger;

	public async Task<IEnumerable<Account>> ExtractAccountsAsync(string filePath, CancellationToken cancellationToken)
	{
		var records = new List<Account>();

		try
		{
			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				await foreach (var record in csv.GetRecordsAsync<Account>().WithCancellation(cancellationToken))
				{
					records.Add(record);
				}
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error reading accounts file");
			throw;
		}

		_logger.LogInformation("Read {Count} accounts from file", records.Count);
		return records;
	}
}
