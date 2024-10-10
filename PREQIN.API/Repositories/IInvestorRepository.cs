
using PREQIN.API.Models;
namespace PREQIN.API.Repositories
{
    public interface IInvestorRepository
    {
        Task<IEnumerable<Investor>> GetAllInvestorsAsync();
        Task<IEnumerable<Commitment>> GetInvestorCommitmentsAsync(int investorId, string assetClass = null);
        Task<IEnumerable<Investor>> GetAllInvestorsWithCommitmentsAsync();
        Task<IEnumerable<Investor>> GetAllInvestorsWithCommitmentsByAssetClassAsync(string assetClass);
        Task<IEnumerable<Commitment>> GetAllCommitmentsAsync();
    }

}

