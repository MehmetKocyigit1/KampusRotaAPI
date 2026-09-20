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
    public class YolculukServiceTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task KatilmaTalebiOlustur_SurucuKendiIlaninaTalepGonderemez_DonerBadRequest()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var yolculuk = new Yolculuk
            {
                Id = 1,
                SurucuId = 10,
                KalkisNoktasi = "Doğu Kampüsü",
                VarisNoktasi = "Otogar",
                KalkisZamani = DateTime.Now.AddHours(2),
                BosKoltukSayisi = 3,
                AktifMi = true
            };
            context.Yolculuklar.Add(yolculuk);
            await context.SaveChangesAsync();

            // Act
            var result = await service.KatilmaTalebiOlusturAsync(yolculukId: 1, yolcuId: 10, "Beni de alır mısın?");

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);
            result.Message.Should().Contain("Kendi ilanına katılım talebi gönderemezsin");
        }

        [Fact]
        public async Task KatilmaTalebiOlustur_GecersizYolculukId_DonerNotFound()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            // Act
            var result = await service.KatilmaTalebiOlusturAsync(yolculukId: 999, yolcuId: 5, "Katılmak istiyorum.");

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(404);
            result.Message.Should().Contain("İlan bulunamadı");
        }

        [Fact]
        public async Task KatilmaTalebiOlustur_BosKoltukKalmadiginda_DonerBadRequest()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var yolculuk = new Yolculuk
            {
                Id = 2,
                SurucuId = 1,
                KalkisNoktasi = "Iyaş",
                VarisNoktasi = "Batı Kampüsü",
                KalkisZamani = DateTime.Now.AddHours(3),
                BosKoltukSayisi = 0, // Dolu!
                AktifMi = true
            };
            context.Yolculuklar.Add(yolculuk);
            await context.SaveChangesAsync();

            // Act
            var result = await service.KatilmaTalebiOlusturAsync(yolculukId: 2, yolcuId: 2, "Yer var mı?");

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);
            result.Message.Should().Contain("katılıma uygun değil");
        }

        [Fact]
        public async Task TalepDurumuGuncelle_Onaylandiginda_BosKoltukSayisiAzalir()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var yolculuk = new Yolculuk
            {
                Id = 3,
                SurucuId = 1,
                KalkisNoktasi = "Çarşı",
                VarisNoktasi = "Kampüs",
                KalkisZamani = DateTime.Now.AddHours(4),
                BosKoltukSayisi = 2,
                AktifMi = true
            };
            var talep = new YolculukTalebi
            {
                Id = 1,
                YolculukId = 3,
                YolcuId = 5,
                Durum = "Bekliyor",
                Yolculuk = yolculuk
            };

            context.Yolculuklar.Add(yolculuk);
            context.YolculukTalepleri.Add(talep);
            await context.SaveChangesAsync();

            // Act
            var result = await service.TalepDurumuGuncelleAsync(talepId: 1, surucuId: 1, onaylandi: true, surucuNotu: "Görüşmek üzere");

            // Assert
            result.Success.Should().BeTrue();
            result.Value!.Durum.Should().Be("Onaylandı");
            yolculuk.BosKoltukSayisi.Should().Be(1);
        }

        [Fact]
        public async Task YolculukEkle_GecmisTarihliYolculuk_HataFirlatir()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var gecmisYolculuk = new Yolculuk
            {
                KalkisNoktasi = "Otogar",
                VarisNoktasi = "Kampüs",
                KalkisZamani = DateTime.Now.AddHours(-2), // Geçmiş tarih!
                BosKoltukSayisi = 2
            };

            // Act & Assert
            var act = async () => await service.YolculukEkleAsync(gecmisYolculuk, kullaniciId: 1);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Geçmiş tarihli yolculuk oluşturulamaz*");
        }

        [Fact]
        public async Task YolculukEkle_KalkisVeVarisAyniOldugunda_HataFirlatir()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var hataliYolculuk = new Yolculuk
            {
                KalkisNoktasi = "Doğu Kampüsü",
                VarisNoktasi = "Doğu Kampüsü", // Aynı nokta!
                KalkisZamani = DateTime.Now.AddHours(2),
                BosKoltukSayisi = 2
            };

            // Act & Assert
            var act = async () => await service.YolculukEkleAsync(hataliYolculuk, kullaniciId: 1);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Kalkış ve varış noktası aynı olamaz*");
        }

        [Fact]
        public async Task YolculukYorumuEkle_GecersizPuan_DonerBadRequest()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var yorum = new YolculukYorumu
            {
                Puan = 6, // 1-5 aralığı dışında
                Yorum = "Harika yolculuktu"
            };

            // Act
            var result = await service.YolculukYorumuEkleAsync(yolculukId: 1, yorumYapanKullaniciId: 2, puanlananKullaniciId: 1, yorum);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);
            result.Message.Should().Contain("Puan 1 ile 5 arasında olmalı");
        }

        [Fact]
        public async Task YolculukYorumuEkle_KullaniciKendiniPuanlayamaz_DonerBadRequest()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var service = new YolculukService(context);

            var yorum = new YolculukYorumu
            {
                Puan = 5,
                Yorum = "Kendime 5 veriyorum"
            };

            // Act
            var result = await service.YolculukYorumuEkleAsync(yolculukId: 1, yorumYapanKullaniciId: 5, puanlananKullaniciId: 5, yorum);

            // Assert
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);
            result.Message.Should().Contain("Kullanıcı kendini puanlayamaz");
        }
    }
}
