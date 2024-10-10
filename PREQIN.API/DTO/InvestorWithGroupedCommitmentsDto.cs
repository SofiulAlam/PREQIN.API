namespace PREQIN.API.DTO
{
    public class InvestorWithGroupedCommitmentsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public List<AssetClassCommitmentDto> CommitmentsByAssetClass { get; set; }
        public decimal TotalCommitments { get; set; }
    }
}
