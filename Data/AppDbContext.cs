using Microsoft.EntityFrameworkCore;
using KampusRota.Models;

namespace KampusRota.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

         public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Yolculuk> Yolculuklar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Yolculuk>()
                .Property(y => y.KisiBasiUcret)
                .HasColumnType("decimal(18,2)");
        }
    }
}