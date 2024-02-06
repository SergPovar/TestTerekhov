using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using System.Reflection.Metadata;


namespace DataAccess
{
    public class ExcelDbContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
      
        public ExcelDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(WebConstants.CONNECTION_STRING);
        }
    }
}
