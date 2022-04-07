
namespace ChallengeForLife.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<VeryBigSum> VeryBigSum { get; set; }
        public DbSet<DiagonalSum> DiagonalSum { get; set; }
        public DbSet<RatioProblems> RatioProblems { get; set; }

    }
}
