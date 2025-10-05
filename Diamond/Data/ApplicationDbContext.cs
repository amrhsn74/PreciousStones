using Diamond.Models;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=AMR-HASSAN;Database=PreciousStones;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
