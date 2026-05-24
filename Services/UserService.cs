using KampusRota.Data;
using KampusRota.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KampusRota.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly AppDbContext _context;

        public KullaniciService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Kullanici?> KayitOlAsync(Kullanici kullanici)
        {
            // Aynı email ile daha önce kayıt olunmuş mu ve bu hesap silinmemiş mi kontrolü
            if (await _context.Kullanicilar.AnyAsync(k => k.Email == kullanici.Email && !k.SilindiMi))
                return null;

            kullanici.OlusturulmaTarihi = DateTime.UtcNow;
            kullanici.AktifMi = true;
            kullanici.SilindiMi = false;

            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();
            return kullanici;
        }

        public async Task<Kullanici?> GirisYapAsync(string email, string sifre)
        {
            // Kullanıcı girişi yaparken hesabın SİLİNMEMİŞ ve AKTİF olması şartını arıyoruz
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == email && k.SifreHash == sifre && !k.SilindiMi && k.AktifMi);

            return kullanici;
        }

        public async Task<bool> SifreDegistirAsync(int kullaniciId, SifreDegistirmeIstegi istek)
        {
            // SifreDegistirmeIstegi (Record) modeli üzerinden verileri alıyoruz
            if (string.IsNullOrEmpty(istek.EskiSifre) || string.IsNullOrEmpty(istek.YeniSifre))
                return false;

            if (istek.YeniSifre != istek.YeniSifreTekrar)
                return false;

            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId && !k.SilindiMi);

            if (kullanici == null)
            {
                return false;
            }

            if (kullanici.SifreHash.Trim() != istek.EskiSifre.Trim())
            {
                // Eğer veritabanındaki şifre ile girilen eski şifre tutmuyorsa işlemi reddet
                return false;
            }

            // Güncelleme İşlemi
            kullanici.SifreHash = istek.YeniSifre;
            kullanici.GuncellenmeTarihi = DateTime.UtcNow;
            kullanici.GuncelleyenKullaniciId = kullaniciId; // Şifreyi kendi güncellediği için ID'si kendisi

            try
            {
                // Değişikliği Entity Framework'e bildir
                _context.Entry(kullanici).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }
    }
}
