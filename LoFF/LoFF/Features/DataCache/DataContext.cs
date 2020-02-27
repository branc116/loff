using LoFF.Features.DataCache.Models;
using Microsoft.EntityFrameworkCore;

namespace LoFF.Features.DataCache
{
    public class DataContext : DbContext
    {
        public DbSet<Entry> Entries { get; set; }

        public DataContext(DbContextOptions<DataContext> config) : base(config)
        {
        }
    }
}
