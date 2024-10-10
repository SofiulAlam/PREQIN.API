namespace PREQIN.API.Models
{
    public class Commitment
    {
        public int Id { get; set; }
        public string AssetClass { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int InvestorId { get; set; }
        public Investor Investor { get; set; }
    }
}
