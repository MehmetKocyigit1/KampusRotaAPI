using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;
using KampusRota.Domain.Entities;
using KampusRota.Infrastructure.Persistence;
using KampusRota.Infrastructure.Services;

namespace KampusRota.UnitTests
{
    public class UniversityServiceTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetUniversityByEmailAsync_EduTrEmailEslestirir_DonerDogruUniversite()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new UniversityService(context);

            context.Universities.AddRange(
                new University { Id = 1, Name = "Süleyman Demirel Üniversitesi", City = "Isparta", EmailDomain = "sdu.edu.tr", AktifMi = true },
                new University { Id = 2, Name = "İstanbul Teknik Üniversitesi", City = "İstanbul", EmailDomain = "itu.edu.tr", AktifMi = true }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetUniversityByEmailAsync("ogrenci123@itu.edu.tr");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("İstanbul Teknik Üniversitesi");
            result.City.Should().Be("İstanbul");
        }

        [Fact]
        public async Task GetUniversityByEmailAsync_GecersizEmail_DonerNull()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new UniversityService(context);

            context.Universities.Add(
                new University { Id = 1, Name = "ODTÜ", City = "Ankara", EmailDomain = "metu.edu.tr", AktifMi = true }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetUniversityByEmailAsync("kullanici@gmail.com");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUniversitiesAsync_SadeceAktifUniversiteleriListeler()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new UniversityService(context);

            context.Universities.AddRange(
                new University { Id = 1, Name = "Aktif Okul", City = "Ankara", EmailDomain = "aktif.edu.tr", AktifMi = true },
                new University { Id = 2, Name = "Pasif Okul", City = "İzmir", EmailDomain = "pasif.edu.tr", AktifMi = false }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllUniversitiesAsync();

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Aktif Okul");
        }
    }
}
