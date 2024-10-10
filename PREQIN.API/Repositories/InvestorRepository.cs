using Microsoft.EntityFrameworkCore;
using PREQIN.API.Data;
using PREQIN.API.Models;
using System;

namespace PREQIN.API.Repositories
{
    public class InvestorRepository : IInvestorRepository
    {
        private readonly ApplicationDbContext _context;

        public InvestorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Investor>> GetAllInvestorsAsync()
        {
            return await _context.Investors
                .Include(i => i.Commitments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commitment>> GetInvestorCommitmentsAsync(int investorId, string assetClass = null)
        {
            var query = _context.Commitments
                .Where(c => c.InvestorId == investorId);

            if (!string.IsNullOrEmpty(assetClass))
            {
                query = query.Where(c => c.AssetClass == assetClass);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Investor>> GetAllInvestorsWithCommitmentsAsync()
        {
            return await _context.Investors
                .Include(i => i.Commitments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Investor>> GetAllInvestorsWithCommitmentsByAssetClassAsync(string assetClass)
        {
            return await _context.Investors
                .Include(i => i.Commitments.Where(c => c.AssetClass == assetClass))
                .ToListAsync();
        }

        public async Task<IEnumerable<Commitment>> GetAllCommitmentsAsync()
        {
            return await _context.Commitments.ToListAsync();
        }
    }
}
