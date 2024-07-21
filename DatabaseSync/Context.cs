using Microsoft.EntityFrameworkCore;

namespace DatabaseSync
{
    public class CentralDbContext : DbContext
    {
        public DbSet<DataItem> DataItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CentralServer;Initial Catalog=CentralDB;Integrated Security=True");
        }
    }

    public class ClientDbContext : DbContext
    {
        public DbSet<DataItem> DataItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=ClientServer;Initial Catalog=ClientDB;Integrated Security=True");
        }
    }

    public class DataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
