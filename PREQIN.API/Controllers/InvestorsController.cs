using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PREQIN.API.DTO;
using PREQIN.API.Repositories;

namespace PREQIN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestorController : ControllerBase
    {
        private readonly IInvestorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestorController> _logger;

        public InvestorController(IInvestorRepository repository, IMapper mapper, ILogger<InvestorController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestorDto>>> GetAllInvestors()
        {
            try
            {
                var investors = await _repository.GetAllInvestorsAsync();
                var investorDtos = _mapper.Map<IEnumerable<InvestorDto>>(investors);
                return Ok(investorDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all investors");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}/commitments")]
        public async Task<ActionResult<IEnumerable<CommitmentDto>>> GetInvestorCommitments(int id, [FromQuery] string assetClass = null)
        {
            try
            {
                var commitments = await _repository.GetInvestorCommitmentsAsync(id, assetClass);
                var commitmentDtos = _mapper.Map<IEnumerable<CommitmentDto>>(commitments);

                var groupedCommitments = commitmentDtos
                    .GroupBy(c => c.AssetClass)
                    .Select(g => new
                    {
                        AssetClass = g.Key,
                        Commitments = g.ToList(),
                        TotalAmount = g.Sum(c => c.Amount)
                    })
                    .ToList();

                return Ok(new
                {
                    Commitments = commitmentDtos,
                    GroupedByAssetClass = groupedCommitments
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting commitments for investor {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("investors-with-grouped-commitments")]
        public async Task<ActionResult<IEnumerable<InvestorWithGroupedCommitmentsDto>>> GetAllInvestorsWithGroupedCommitments()
        {
            try
            {
                var investors = await _repository.GetAllInvestorsWithCommitmentsAsync();

                var result = investors.Select(investor => new InvestorWithGroupedCommitmentsDto
                {
                    Id = investor.Id,
                    Name = investor.Name,
                    Type = investor.Type,
                    Country = investor.Country,
                    DateAdded = investor.DateAdded,
                    CommitmentsByAssetClass = investor.Commitments
                        .GroupBy(c => c.AssetClass)
                        .Select(g => new AssetClassCommitmentDto
                        {
                            AssetClass = g.Key,
                            TotalAmount = g.Sum(c => c.Amount),
                            Commitments = _mapper.Map<List<CommitmentDto>>(g.ToList())
                        })
                        .OrderByDescending(g => g.TotalAmount)
                        .ToList(),
                    TotalCommitments = investor.Commitments.Sum(c => c.Amount)
                })
                .OrderByDescending(i => i.TotalCommitments)
                .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all investors with grouped commitments");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("investors-commitments-by-asset-class/{assetClass}")]
        public async Task<ActionResult<IEnumerable<InvestorWithCommitmentsByAssetClassDto>>> GetAllInvestorsWithCommitmentsByAssetClass(string assetClass)
        {
            try
            {
                var investors = await _repository.GetAllInvestorsWithCommitmentsByAssetClassAsync(assetClass);

                var result = investors.Select(investor => new InvestorWithCommitmentsByAssetClassDto
                {
                    Id = investor.Id,
                    Name = investor.Name,
                    Type = investor.Type,
                    Country = investor.Country,
                    DateAdded = investor.DateAdded,
                    Commitments = _mapper.Map<List<CommitmentDto>>(investor.Commitments),
                    TotalCommitmentAmount = investor.Commitments.Sum(c => c.Amount)
                })
                .Where(i => i.Commitments.Any()) 
                .OrderByDescending(i => i.TotalCommitmentAmount)
                .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting investors with commitments for asset class: {assetClass}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("commitments")]
        public async Task<ActionResult<IEnumerable<CommitmentDetailsDto>>> GetAllCommitments()
        {
            try
            {
                var commitments = await _repository.GetAllCommitmentsAsync();
                var commitmentDtos = _mapper.Map<IEnumerable<CommitmentDetailsDto>>(commitments);
                return Ok(commitmentDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting all commitments for asset class");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

