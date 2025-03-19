using Ensek.Domain.Entities;

namespace Ensek.Application.Common.Interfaces;

public interface IMeterReadingsFile : IDisposable
{
	IAsyncEnumerable<MeterReading> ExtractReadingsAsync(CancellationToken cancellationToken);
}
