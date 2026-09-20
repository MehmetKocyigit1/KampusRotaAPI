using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KampusRota.Application.Common.Interfaces;
using KampusRota.Domain.Entities;

namespace KampusRota.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<University> Universities { get; set; } = null!;
        public DbSet<CampusLocation> CampusLocations { get; set; } = null!;
        public DbSet<Kullanici> Kullanicilar { get; set; } = null!;
        public DbSet<Yolculuk> Yolculuklar { get; set; } = null!;
        public DbSet<YolculukTalebi> YolculukTalepleri { get; set; } = null!;
        public DbSet<YolculukYorumu> YolculukYorumlari { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // University
            modelBuilder.Entity<University>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
                entity.Property(u => u.City).IsRequired().HasMaxLength(100);
                entity.Property(u => u.EmailDomain).HasMaxLength(100);
            });

            // CampusLocation
            modelBuilder.Entity<CampusLocation>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Title).IsRequired().HasMaxLength(200);
                entity.Property(l => l.LocationKey).HasMaxLength(100);

                entity.HasOne(l => l.University)
                    .WithMany(u => u.Locations)
                    .HasForeignKey(l => l.UniversityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Yolculuk
            modelBuilder.Entity<Yolculuk>(entity =>
            {
                entity.HasKey(y => y.Id);
                entity.Property(y => y.KisiBasiUcret).HasColumnType("decimal(18,2)");

                entity.HasOne(y => y.Surucu)
                    .WithMany(k => k.SurucuYolculuklari)
                    .HasForeignKey(y => y.SurucuId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(y => y.University)
                    .WithMany(u => u.Yolculuklar)
                    .HasForeignKey(y => y.UniversityId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Kullanici
            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(k => k.Email).IsRequired().HasMaxLength(150);

                entity.HasOne(k => k.University)
                    .WithMany(u => u.Kullanicilar)
                    .HasForeignKey(k => k.UniversityId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // YolculukTalebi
            modelBuilder.Entity<YolculukTalebi>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasOne(t => t.Yolculuk)
                    .WithMany(y => y.Talepler)
                    .HasForeignKey(t => t.YolculukId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Yolcu)
                    .WithMany(k => k.Talepleri)
                    .HasForeignKey(t => t.YolcuId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // YolculukYorumu
            modelBuilder.Entity<YolculukYorumu>(entity =>
            {
                entity.HasKey(y => y.Id);

                entity.HasOne(y => y.Yolculuk)
                    .WithMany(r => r.Yorumlar)
                    .HasForeignKey(y => y.YolculukId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(y => y.YorumYapanKullanici)
                    .WithMany()
                    .HasForeignKey(y => y.YorumYapanKullaniciId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(y => y.PuanlananKullanici)
                    .WithMany()
                    .HasForeignKey(y => y.PuanlananKullaniciId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
