namespace PREQIN.API.DTO
{
    public class AssetClassCommitmentDto
    {
        public string AssetClass { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CommitmentDto> Commitments { get; set; }
    }
}
