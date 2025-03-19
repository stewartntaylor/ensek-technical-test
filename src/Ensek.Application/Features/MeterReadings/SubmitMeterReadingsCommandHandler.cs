using Ensek.Application.Common.Interfaces;
using Ensek.Application.Common.Files;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ensek.Application.Features.MeterReadings;

public class SubmitMeterReadingsCommandHandler(
	IApplicationDbContext applicationDbContext,
	ILogger<SubmitMeterReadingsCommandHandler> logger)
	: IRequestHandler<SubmitMeterReadingsCommand, SubmitMeterReadingsCommandResult>
{
	private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
	private readonly ILogger<SubmitMeterReadingsCommandHandler> _logger = logger;
	private const int BatchSize = 10;

	public async Task<SubmitMeterReadingsCommandResult> Handle(SubmitMeterReadingsCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Processing meter readings file: {UploadedFilepath}", request.UploadedFilepath);

		using var meterReadings = MeterReadingsFile.Create(request.UploadedFilepath, _logger);

		var processedReadings = 0;
		var failedReadings = 0;

		await foreach (var meterReading in meterReadings.ExtractReadingsAsync(cancellationToken))
		{
			try
			{
				// Ensure the account exists
				if (!_applicationDbContext.Accounts.Any(x => x.AccountId == meterReading.AccountId))
				{
					_logger.LogWarning("Account not found: {AccountId}", meterReading.AccountId);
					failedReadings++;
					continue;
				}

				// Validate the reading (reading value must be in format NNNNN)
				if (meterReading.MeterReadValue < 0 || meterReading.MeterReadValue >= 99999)
				{
					_logger.LogWarning("MeterReading invalid. Value must be between 0 and 99999. MeterReading {MeterReading}", meterReading);
					failedReadings++;
					continue;
				}

				var lastMeterReading = _applicationDbContext.MeterReadings
					.Where(x => x.AccountId == meterReading.AccountId)
					.OrderByDescending(x => x.MeterReadingDateTime)
					.FirstOrDefault();

				if (lastMeterReading != null)
				{
					// When an account has an existing read, ensure the new read isn't older than the existing read
					if (meterReading.MeterReadingDateTime <= lastMeterReading.MeterReadingDateTime)
					{
						_logger.LogWarning("MeterReading invalid. New reading date is before previous reading - CurrentReadingDate:{CurrentReadingDate}, MeterReading {MeterReading}",
							lastMeterReading.MeterReadingDateTime,
							meterReading);
						failedReadings++;
						continue;
					}

					// We should ensure the new read is greater than or equal to the previous read
					if (meterReading.MeterReadValue < lastMeterReading.MeterReadValue)
					{
						_logger.LogWarning("MeterReading invalid. New reading value is less than previous reading - CurrentReading:{CurrentReadingValue}, MeterReading {MeterReading}",
							lastMeterReading.MeterReadValue,
							meterReading);
						failedReadings++;
						continue;
					}
				}

				await _applicationDbContext.MeterReadings.AddAsync(meterReading, cancellationToken);
				processedReadings++;

				if (processedReadings % BatchSize == 0)
				{
					// Write the batch to the DB
					await _applicationDbContext.SaveChangesAsync(cancellationToken);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error processing meter readings. MeterReading {MeterReading}", meterReading);
				failedReadings++;
				continue;
			}
		}

		// Flush the last batch
		await _applicationDbContext.SaveChangesAsync(cancellationToken);

		return new SubmitMeterReadingsCommandResult(processedReadings, failedReadings);
	}
}
