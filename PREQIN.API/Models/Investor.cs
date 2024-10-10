namespace PREQIN.API.Models
{
    public class Investor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<Commitment> Commitments { get; set; } = new List<Commitment>();
    }
}
