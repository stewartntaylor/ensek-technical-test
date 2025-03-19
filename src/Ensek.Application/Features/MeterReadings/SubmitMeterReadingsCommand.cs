using MediatR;

namespace Ensek.Application.Features.MeterReadings;

public record SubmitMeterReadingsCommand(string UploadedFilepath) 
	: IRequest<SubmitMeterReadingsCommandResult>;
