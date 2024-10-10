namespace PREQIN.API.DTO
{
    public class InvestorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DateAdded { get; set; }
        public string Country { get; set; }
        public decimal TotalCommitments { get; set; }
    }
}
