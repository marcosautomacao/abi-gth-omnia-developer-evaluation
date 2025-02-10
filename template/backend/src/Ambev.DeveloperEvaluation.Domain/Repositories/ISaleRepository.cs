using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for User entity operations
/// </summary>
public interface ISaleRepository
{
    Task<Sale> AddAsync(Sale Sale, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Sale> UpdateAsync(Sale Sale, CancellationToken cancellationToken);
}
