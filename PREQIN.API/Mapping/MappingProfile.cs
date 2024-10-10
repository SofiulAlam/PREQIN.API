using AutoMapper;
using PREQIN.API.DTO;
using PREQIN.API.Models;
namespace PREQIN.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Investor, InvestorDto>()
                .ForMember(dest => dest.TotalCommitments, opt => opt.MapFrom(src => src.Commitments.Sum(c => c.Amount)));
            CreateMap<Commitment, CommitmentDto>();
            CreateMap<Commitment, CommitmentDetailsDto>();
        }
    }
}
