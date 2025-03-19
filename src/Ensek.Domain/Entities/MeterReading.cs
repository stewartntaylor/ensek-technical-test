
namespace Ensek.Domain.Entities;

public class MeterReading
{
	public required int AccountId { get; set; }

	public required DateTime MeterReadingDateTime { get; set; }

	public required int MeterReadValue { get; set; }

	public override string ToString()
	{
		return $"AccountId:{AccountId} MeterReadingDateTime:{MeterReadingDateTime} MeterReadValue:{MeterReadValue}";
	}
}
