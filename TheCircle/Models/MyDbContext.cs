using Microsoft.EntityFrameworkCore;
using TheCircle.Models;

namespace TheCircle
{
    public class MyDbContext : DbContext
    {
        public virtual DbSet<Cargo> Cargos { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>()
                .HasKey(c => c.tipo);
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Data Source=GUYSRV08\\SOAAPPS;Initial Catalog=TheCircle;Integrated Security=False;User ID=thecircle_user;Password=Th3C1rCl3_cI;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}