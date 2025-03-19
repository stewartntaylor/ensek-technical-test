using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ensek.Domain.Entities;

namespace Ensek.WebApi.Infrastructure.Data.Configurations;

public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
{
	public void Configure(EntityTypeBuilder<MeterReading> builder)
	{
		builder.ToTable(nameof(MeterReading));
		
		builder.HasKey(nameof(MeterReading.AccountId), nameof(MeterReading.MeterReadingDateTime));

		//builder.HasOne(x => x.Account)
		//	.WithMany()
		//	.HasForeignKey(x => x.AccountId)
		//	.IsRequired();
	}
}
