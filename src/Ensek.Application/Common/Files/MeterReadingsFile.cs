using CsvHelper;
using Ensek.Application.Common.Interfaces;
using Ensek.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Ensek.Application.Common.Files;

/// <summary>
/// MeterReadings file reader
/// </summary>
public class MeterReadingsFile : IMeterReadingsFile, IDisposable
{
	private readonly ILogger _logger;
	private readonly StreamReader _reader;
	private readonly CsvReader _csvReader;

	private MeterReadingsFile(string filePath, ILogger logger)
	{
		_logger = logger;
		_reader = new StreamReader(filePath);
		_csvReader = new CsvReader(_reader, CultureInfo.CurrentCulture);
	}

	public static IMeterReadingsFile Create(string filePath, ILogger logger)
	{
		return new MeterReadingsFile(filePath, logger);
	}

	public IAsyncEnumerable<MeterReading> ExtractReadingsAsync(CancellationToken cancellationToken)
	{
		try
		{
			return _csvReader.GetRecordsAsync<MeterReading>(cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error reading meter readings file");
			throw;
		}
	}

	public void Dispose()
	{
		if (_csvReader != null)
		{
			_csvReader.Dispose();
		}

		if (_reader != null)
		{
			_reader.Dispose();
		}
	}
}
