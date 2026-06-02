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
            ValidateUser(kullanici);

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
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sifre))
            {
                return null;
            }

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == email.Trim() && k.SifreHash == sifre && !k.SilindiMi && k.AktifMi);

            return kullanici;
        }

        public async Task<bool> SifreDegistirAsync(int kullaniciId, SifreDegistirmeIstegi istek)
        {
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
                return false;
            }

            kullanici.SifreHash = istek.YeniSifre;
            kullanici.GuncellenmeTarihi = DateTime.UtcNow;
            kullanici.GuncelleyenKullaniciId = kullaniciId;

            try
            {
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

        public async Task<Kullanici?> KullaniciGetirAsync(int kullaniciId)
        {
            return await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId && !k.SilindiMi);
        }

        public async Task<Kullanici?> KullaniciGuncelleAsync(int kullaniciId, Kullanici guncelKullanici)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId && !k.SilindiMi);
            if (kullanici == null)
            {
                return null;
            }

            ValidateProfileUpdate(guncelKullanici);

            kullanici.Ad = guncelKullanici.Ad.Trim();
            kullanici.Soyad = guncelKullanici.Soyad.Trim();
            kullanici.TelefonNumarasi = guncelKullanici.TelefonNumarasi?.Trim() ?? string.Empty;
            kullanici.Cinsiyet = guncelKullanici.Cinsiyet?.Trim() ?? string.Empty;
            kullanici.Biyografi = guncelKullanici.Biyografi?.Trim() ?? string.Empty;
            kullanici.ProfilFotografiUrl = guncelKullanici.ProfilFotografiUrl?.Trim() ?? string.Empty;
            kullanici.GuncellenmeTarihi = DateTime.UtcNow;
            kullanici.GuncelleyenKullaniciId = kullaniciId;

            await _context.SaveChangesAsync();
            return kullanici;
        }

        public async Task<bool> KullaniciSilAsync(int kullaniciId)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId && !k.SilindiMi);
            if (kullanici == null)
            {
                return false;
            }

            kullanici.SilindiMi = true;
            kullanici.AktifMi = false;
            kullanici.SilinmeTarihi = DateTime.UtcNow;
            kullanici.SilenKullaniciId = kullaniciId;

            await _context.SaveChangesAsync();
            return true;
        }

        private static void ValidateUser(Kullanici kullanici)
        {
            if (string.IsNullOrWhiteSpace(kullanici.Ad) || string.IsNullOrWhiteSpace(kullanici.Soyad))
                throw new ArgumentException("Ad ve soyad boş bırakılamaz.");
            if (string.IsNullOrWhiteSpace(kullanici.Email) || !kullanici.Email.Trim().EndsWith(".edu.tr", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Sadece .edu.tr uzantılı üniversite e-postası kabul edilir.");
            if (string.IsNullOrWhiteSpace(kullanici.SifreHash) || kullanici.SifreHash.Length < 6)
                throw new ArgumentException("Şifre en az 6 karakter olmalıdır.");
            if (string.IsNullOrWhiteSpace(kullanici.OgrenciNumarasi) || kullanici.OgrenciNumarasi.Length < 5)
                throw new ArgumentException("Öğrenci numarası geçerli olmalıdır.");

            kullanici.Ad = kullanici.Ad.Trim();
            kullanici.Soyad = kullanici.Soyad.Trim();
            kullanici.Email = kullanici.Email.Trim();
            kullanici.OgrenciNumarasi = kullanici.OgrenciNumarasi.Trim();
        }

        private static void ValidateProfileUpdate(Kullanici kullanici)
        {
            if (string.IsNullOrWhiteSpace(kullanici.Ad) || string.IsNullOrWhiteSpace(kullanici.Soyad))
                throw new ArgumentException("Ad ve soyad boş bırakılamaz.");
            if (kullanici.Ad.Trim().Length is < 2 or > 40 || kullanici.Soyad.Trim().Length is < 2 or > 40)
                throw new ArgumentException("Ad ve soyad 2-40 karakter arasında olmalıdır.");
            if ((kullanici.TelefonNumarasi ?? string.Empty).Trim().Length > 20)
                throw new ArgumentException("Telefon numarası en fazla 20 karakter olabilir.");
            if ((kullanici.Biyografi ?? string.Empty).Trim().Length > 250)
                throw new ArgumentException("Biyografi en fazla 250 karakter olabilir.");
            if ((kullanici.ProfilFotografiUrl ?? string.Empty).Trim().Length > 300)
                throw new ArgumentException("Profil fotoğrafı bağlantısı en fazla 300 karakter olabilir.");
        }
    }
}
