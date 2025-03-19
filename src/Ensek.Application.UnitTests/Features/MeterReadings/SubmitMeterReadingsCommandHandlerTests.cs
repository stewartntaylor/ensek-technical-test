using Ensek.Application.Common.Interfaces;
using Ensek.Application.Features.MeterReadings;
using Ensek.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;

namespace Ensek.Application.UnitTests.Features.MeterReadings;

[Collection("Collection 1")]
public class SubmitMeterReadingsCommandHandlerTests
{
	private readonly Mock<IApplicationDbContext> _dbContext;
	private readonly Mock<ILogger<SubmitMeterReadingsCommandHandler>> _logger;
	private readonly SubmitMeterReadingsCommandHandler _subject;

	public SubmitMeterReadingsCommandHandlerTests()
	{
		_dbContext = new Mock<IApplicationDbContext>();

		var accounts = new List<Account>()
		{
			new () { AccountId = 1, FirstName = "Fred", LastName = "Jones" },
			new () { AccountId = 2, FirstName = "Jane", LastName = "Davies" }
		};

		var meterReadings = new List<MeterReading>()
		{
			new () { AccountId = 1, MeterReadingDateTime = DateTime.Parse("2019-03-10 10:52"), MeterReadValue = 1000 },
			new () { AccountId = 2, MeterReadingDateTime = DateTime.Parse("2019-03-05 09:10"), MeterReadValue = 2000 }
		};

		_dbContext.Setup(x => x.Accounts).ReturnsDbSet(accounts);
		_dbContext.Setup(x => x.MeterReadings).ReturnsDbSet(meterReadings);

		_logger = new Mock<ILogger<SubmitMeterReadingsCommandHandler>>();

		_subject = new SubmitMeterReadingsCommandHandler(_dbContext.Object, _logger.Object);
	}

	[Fact]
	public async Task Handle_Should_Process_Valid_File_All_Processed()
	{
		// Arrange
		var command = new SubmitMeterReadingsCommand("TestFiles/MeterReadings1_all_valid.csv");

		// Act
		var result = await _subject.Handle(command, CancellationToken.None);

		// Assert
		result.Errors.ShouldBe(0);
		result.RecordsProcessed.ShouldBe(2);
	}

	[Fact]
	public async Task Handle_Should_Process_Valid_File_With_Missing_Account()
	{
		// Arrange
		var command = new SubmitMeterReadingsCommand("TestFiles/MeterReadings2_missing_account.csv");

		// Act
		var result = await _subject.Handle(command, CancellationToken.None);

		// Assert
		result.Errors.ShouldBe(1);
		result.RecordsProcessed.ShouldBe(1);
	}

	[Fact]
	public async Task Handle_Should_Process_Valid_File_With_Duplicate_Data()
	{
		// Arrange
		var command = new SubmitMeterReadingsCommand("TestFiles/MeterReadings3_duplicate_data.csv");

		// Act
		var result = await _subject.Handle(command, CancellationToken.None);

		// Assert
		result.Errors.ShouldBe(1);
		result.RecordsProcessed.ShouldBe(1);
	}

	[Fact]
	public async Task Handle_Should_Process_Valid_File_With_Reduced_ReadingValue()
	{
		// Arrange
		var command = new SubmitMeterReadingsCommand("TestFiles/MeterReadings4_reduced_readingvalue.csv");

		// Act
		var result = await _subject.Handle(command, CancellationToken.None);

		// Assert
		result.Errors.ShouldBe(1);
		result.RecordsProcessed.ShouldBe(1);
	}
}
