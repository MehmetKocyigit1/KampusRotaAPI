using Microsoft.EntityFrameworkCore;
using KampusRota.Models;

namespace KampusRota.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Yolculuk> Yolculuklar { get; set; }
        public DbSet<YolculukTalebi> YolculukTalepleri { get; set; }
        public DbSet<YolculukYorumu> YolculukYorumlari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Yolculuk>()
                .Property(y => y.KisiBasiUcret)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<YolculukTalebi>()
                .HasOne(t => t.Yolculuk)
                .WithMany()
                .HasForeignKey(t => t.YolculukId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<YolculukTalebi>()
                .HasOne(t => t.Yolcu)
                .WithMany()
                .HasForeignKey(t => t.YolcuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<YolculukYorumu>()
                .HasOne(y => y.Yolculuk)
                .WithMany()
                .HasForeignKey(y => y.YolculukId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<YolculukYorumu>()
                .HasOne(y => y.YorumYapanKullanici)
                .WithMany()
                .HasForeignKey(y => y.YorumYapanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<YolculukYorumu>()
                .HasOne(y => y.PuanlananKullanici)
                .WithMany()
                .HasForeignKey(y => y.PuanlananKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
