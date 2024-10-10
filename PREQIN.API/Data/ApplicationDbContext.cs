using Microsoft.EntityFrameworkCore;
using PREQIN.API.Models;

namespace PREQIN.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Investor> Investors { get; set; }
        public DbSet<Commitment> Commitments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Investor>()
                .HasMany(i => i.Commitments)
                .WithOne(c => c.Investor)
                .HasForeignKey(c => c.InvestorId);
        }
    }
}
