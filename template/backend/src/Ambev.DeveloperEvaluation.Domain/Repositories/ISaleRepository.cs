using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale> GetByIdAsync(Guid id);
        Task<Sale> GetBySaleNumberAsync(string saleNumber);
        Task<bool> ExistsBySaleNumberAsync(string saleNumber);
        Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId);
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Sale> AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Guid id);
    }
} 