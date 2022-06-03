using Microsoft.EntityFrameworkCore;

namespace Weather.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }
        public DbSet<Sky> Skys { get; set; }
    }
}
