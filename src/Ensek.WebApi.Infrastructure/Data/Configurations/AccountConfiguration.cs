using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ensek.Domain.Entities;

namespace Ensek.WebApi.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder.ToTable(nameof(Account));
		builder.HasKey(x => x.AccountId);

		builder.Property(x => x.AccountId).ValueGeneratedNever();
		builder.Property(x => x.FirstName).HasMaxLength(50);
		builder.Property(x => x.LastName).HasMaxLength(50);
	}
}
