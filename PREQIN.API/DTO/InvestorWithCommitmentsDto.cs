namespace PREQIN.API.DTO
{
    public class InvestorWithCommitmentsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CommitmentDto> Commitments { get; set; }
        public Dictionary<string, decimal> CommitmentsByAssetClass { get; set; }
    }

}
