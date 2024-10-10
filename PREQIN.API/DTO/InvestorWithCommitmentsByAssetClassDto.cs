namespace PREQIN.API.DTO
{
    public class InvestorWithCommitmentsByAssetClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public List<CommitmentDto> Commitments { get; set; }
        public decimal TotalCommitmentAmount { get; set; }
    }
}
