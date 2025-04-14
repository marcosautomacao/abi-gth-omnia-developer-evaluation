using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public IQueryable<Sale> GetQueryable()
        {
            return _context.Sales.Include(s => s.Items).AsQueryable();
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Items)
                .ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> GetBySaleNumberAsync(string saleNumber)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
        }

        public async Task<bool> ExistsBySaleNumberAsync(string saleNumber)
        {
            return await _context.Sales
                .AnyAsync(s => s.SaleNumber == saleNumber);
        }

        public async Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Where(s => s.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid branchId)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Where(s => s.BranchId == branchId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();
        }

        public async Task<Sale> AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Entry(sale).State = EntityState.Modified;
            foreach (var item in sale.Items)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
} 