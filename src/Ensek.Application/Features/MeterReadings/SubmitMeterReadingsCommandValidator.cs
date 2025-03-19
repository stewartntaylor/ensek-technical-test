using FluentValidation;

namespace Ensek.Application.Features.MeterReadings;

public class SubmitMeterReadingsCommandValidator : AbstractValidator<SubmitMeterReadingsCommand>
{
	public SubmitMeterReadingsCommandValidator()
	{
		RuleFor(x => x.UploadedFilepath).NotEmpty();
	}
}
