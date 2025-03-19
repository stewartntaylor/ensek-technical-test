using Ensek.Domain.Entities;

namespace Ensek.Application.Common.Interfaces;

public interface IAccountsFile
{
	Task<IEnumerable<Account>> ExtractAccountsAsync(string filePath, CancellationToken cancellationToken);
}