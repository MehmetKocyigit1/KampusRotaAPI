using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KampusRota.Data;
using KampusRota.Models;
using Microsoft.EntityFrameworkCore;

namespace KampusRota.Services
{
    public class YolculukService : IYolculukService
    {
        private readonly AppDbContext _context;

        public YolculukService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Yolculuk>> TumYolculuklariGetirAsync()
        {
            return await _context.Yolculuklar
                .Include(y => y.Surucu)
                .Where(y => !y.SilindiMi && y.AktifMi && y.BosKoltukSayisi > 0 && y.KalkisZamani >= DateTime.Now)
                .OrderBy(y => y.KalkisZamani)
                .ToListAsync();
        }

        public async Task<IEnumerable<Yolculuk>> MusaitYolculuklariGetirAsync(string kalkisNoktasi, string varisNoktasi, DateTime tarih)
        {
            return await _context.Yolculuklar
                .Include(y => y.Surucu)
                .Where(y => y.AktifMi == true && !y.SilindiMi)
                .Where(y => y.BosKoltukSayisi > 0)
                .Where(y => y.KalkisZamani.Date == tarih.Date)
                .Where(y => y.KalkisNoktasi.ToLower().Contains(kalkisNoktasi.ToLower()))
                .Where(y => y.VarisNoktasi.ToLower().Contains(varisNoktasi.ToLower()))
                .OrderBy(y => y.KalkisZamani)
                .ToListAsync();
        }

        public async Task<Yolculuk> YolculukEkleAsync(Yolculuk yolculuk, int kullaniciId)
        {
            ValidateRide(yolculuk);

            yolculuk.OlusturulmaTarihi = DateTime.UtcNow;
            yolculuk.OlusturanKullaniciId = kullaniciId;
            yolculuk.SurucuId = kullaniciId;
            yolculuk.AktifMi = true;
            yolculuk.SilindiMi = false;

            _context.Yolculuklar.Add(yolculuk);
            await _context.SaveChangesAsync();
            return yolculuk;
        }

        public async Task<Yolculuk?> YolculukGuncelleAsync(int id, Yolculuk guncelYolculuk, int kullaniciId)
        {
            ValidateRide(guncelYolculuk);

            var mevcutYolculuk = await _context.Yolculuklar.FindAsync(id);

            if (mevcutYolculuk == null || mevcutYolculuk.SilindiMi) return null;

            mevcutYolculuk.GuncellenmeTarihi = DateTime.UtcNow;
            mevcutYolculuk.GuncelleyenKullaniciId = kullaniciId;

            mevcutYolculuk.KalkisNoktasi = guncelYolculuk.KalkisNoktasi;
            mevcutYolculuk.VarisNoktasi = guncelYolculuk.VarisNoktasi;
            mevcutYolculuk.KalkisZamani = guncelYolculuk.KalkisZamani;
            mevcutYolculuk.BosKoltukSayisi = guncelYolculuk.BosKoltukSayisi;
            mevcutYolculuk.KisiBasiUcret = guncelYolculuk.KisiBasiUcret;
            mevcutYolculuk.Aciklama = guncelYolculuk.Aciklama;
            mevcutYolculuk.IletisimTelefonu = guncelYolculuk.IletisimTelefonu;
            mevcutYolculuk.SadeceKadinlarMi = guncelYolculuk.SadeceKadinlarMi;

            await _context.SaveChangesAsync();
            return mevcutYolculuk;
        }

        public async Task<bool> YolculukSilAsync(int id, int silenKullaniciId)
        {
            var yolculuk = await _context.Yolculuklar.FindAsync(id);
            if (yolculuk == null || yolculuk.SilindiMi) return false;

            yolculuk.SilindiMi = true;
            yolculuk.SilinmeTarihi = DateTime.UtcNow;
            yolculuk.SilenKullaniciId = silenKullaniciId;
            yolculuk.AktifMi = false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ServiceResult<YolculukTalebi>> KatilmaTalebiOlusturAsync(int yolculukId, int yolcuId, string talepMesaji)
        {
            var yolculuk = await _context.Yolculuklar.FirstOrDefaultAsync(y => y.Id == yolculukId && !y.SilindiMi);
            if (yolculuk == null) return ServiceResult<YolculukTalebi>.NotFound("İlan bulunamadı.");
            if (yolculuk.SurucuId == yolcuId) return ServiceResult<YolculukTalebi>.BadRequest("Kendi ilanına katılım talebi gönderemezsin.");
            if (!yolculuk.AktifMi || yolculuk.BosKoltukSayisi <= 0 || yolculuk.KalkisZamani <= DateTime.Now)
                return ServiceResult<YolculukTalebi>.BadRequest("Bu ilan katılıma uygun değil.");

            var mevcutTalep = await _context.YolculukTalepleri
                .FirstOrDefaultAsync(t => t.YolculukId == yolculukId && t.YolcuId == yolcuId && !t.SilindiMi && t.Durum != "Reddedildi");
            if (mevcutTalep != null) return ServiceResult<YolculukTalebi>.Ok(mevcutTalep);

            var yeniTalep = new YolculukTalebi
            {
                YolculukId = yolculukId,
                YolcuId = yolcuId,
                TalepMesaji = talepMesaji.Trim(),
                Durum = "Bekliyor",
                TalepTarihi = DateTime.UtcNow,
                OlusturulmaTarihi = DateTime.UtcNow,
                OlusturanKullaniciId = yolcuId,
                AktifMi = true,
                SilindiMi = false
            };

            _context.YolculukTalepleri.Add(yeniTalep);
            await _context.SaveChangesAsync();
            return ServiceResult<YolculukTalebi>.Created(yeniTalep);
        }

        public async Task<IEnumerable<YolculukTalebi>> SurucuTalepleriniGetirAsync(int surucuId)
        {
            return await _context.YolculukTalepleri
                .Include(t => t.Yolculuk)
                .Include(t => t.Yolcu)
                .Where(t => !t.SilindiMi && t.Yolculuk != null && t.Yolculuk!.SurucuId == surucuId)
                .OrderByDescending(t => t.TalepTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<YolculukTalebi>> YolcuTalepleriniGetirAsync(int yolcuId)
        {
            return await _context.YolculukTalepleri
                .Include(t => t.Yolculuk)
                .ThenInclude(y => y!.Surucu)
                .Include(t => t.Yolcu)
                .Where(t => !t.SilindiMi && t.YolcuId == yolcuId)
                .OrderByDescending(t => t.TalepTarihi)
                .ToListAsync();
        }

        public async Task<ServiceResult<YolculukTalebi>> TalepDurumuGuncelleAsync(int talepId, int surucuId, bool onaylandi, string? surucuNotu)
        {
            var talep = await _context.YolculukTalepleri
                .Include(t => t.Yolculuk)
                .FirstOrDefaultAsync(t => t.Id == talepId && !t.SilindiMi);
            if (talep?.Yolculuk == null) return ServiceResult<YolculukTalebi>.NotFound("Talep bulunamadı.");
            if (talep.Yolculuk.SurucuId != surucuId) return ServiceResult<YolculukTalebi>.Forbidden("Bu talebi yalnızca sürücü sonuçlandırabilir.");
            if (talep.Durum != "Bekliyor") return ServiceResult<YolculukTalebi>.BadRequest("Bu talep daha önce sonuçlandırılmış.");

            talep.Durum = onaylandi ? "Onaylandı" : "Reddedildi";
            talep.SurucuNotu = surucuNotu ?? string.Empty;
            talep.OnayTarihi = DateTime.UtcNow;
            talep.GuncellenmeTarihi = DateTime.UtcNow;
            talep.GuncelleyenKullaniciId = surucuId;

            if (onaylandi)
            {
                if (talep.Yolculuk.BosKoltukSayisi <= 0) return ServiceResult<YolculukTalebi>.BadRequest("Boş koltuk kalmadı.");

                talep.Yolculuk.BosKoltukSayisi -= 1;
                talep.Yolculuk.GuncellenmeTarihi = DateTime.UtcNow;
                talep.Yolculuk.GuncelleyenKullaniciId = surucuId;

                if (talep.Yolculuk.BosKoltukSayisi <= 0)
                {
                    talep.Yolculuk.BosKoltukSayisi = 0;
                    talep.Yolculuk.AktifMi = false;
                    talep.Yolculuk.SilindiMi = true;
                    talep.Yolculuk.SilinmeTarihi = DateTime.UtcNow;
                    talep.Yolculuk.SilenKullaniciId = surucuId;

                    var digerBekleyenTalepler = await _context.YolculukTalepleri
                        .Where(t =>
                            t.YolculukId == talep.YolculukId &&
                            t.Id != talep.Id &&
                            !t.SilindiMi &&
                            t.Durum == "Bekliyor")
                        .ToListAsync();

                    foreach (var bekleyenTalep in digerBekleyenTalepler)
                    {
                        bekleyenTalep.Durum = "Reddedildi";
                        bekleyenTalep.SurucuNotu = "Koltuklar dolduğu için talep otomatik reddedildi.";
                        bekleyenTalep.OnayTarihi = DateTime.UtcNow;
                        bekleyenTalep.GuncellenmeTarihi = DateTime.UtcNow;
                        bekleyenTalep.GuncelleyenKullaniciId = surucuId;
                    }
                }
                else
                {
                    talep.Yolculuk.AktifMi = true;
                }
            }

            await _context.SaveChangesAsync();
            return ServiceResult<YolculukTalebi>.Ok(talep);
        }

        public async Task<ServiceResult<YolculukYorumu>> YolculukYorumuEkleAsync(int yolculukId, int yorumYapanKullaniciId, int puanlananKullaniciId, YolculukYorumu yorum)
        {
            if (yorum.Puan < 1 || yorum.Puan > 5) return ServiceResult<YolculukYorumu>.BadRequest("Puan 1 ile 5 arasında olmalı.");
            if (yorumYapanKullaniciId == puanlananKullaniciId) return ServiceResult<YolculukYorumu>.BadRequest("Kullanıcı kendini puanlayamaz.");
            if ((yorum.Yorum ?? string.Empty).Length > 250) return ServiceResult<YolculukYorumu>.BadRequest("Yorum en fazla 250 karakter olabilir.");

            var yolculuk = await _context.Yolculuklar.FirstOrDefaultAsync(y => y.Id == yolculukId && !y.SilindiMi);
            if (yolculuk == null) return ServiceResult<YolculukYorumu>.NotFound("Yolculuk bulunamadı.");

            var katilimVarMi = yolculuk.SurucuId == yorumYapanKullaniciId ||
                await _context.YolculukTalepleri.AnyAsync(t =>
                    t.YolculukId == yolculukId &&
                    t.YolcuId == yorumYapanKullaniciId &&
                    t.Durum == "Onaylandı" &&
                    !t.SilindiMi);
            if (!katilimVarMi) return ServiceResult<YolculukYorumu>.BadRequest("Sadece yolculuğa katılan kullanıcılar yorum yapabilir.");

            var mevcutYorum = await _context.YolculukYorumlari.FirstOrDefaultAsync(y =>
                y.YolculukId == yolculukId &&
                y.YorumYapanKullaniciId == yorumYapanKullaniciId &&
                y.PuanlananKullaniciId == puanlananKullaniciId &&
                !y.SilindiMi);
            if (mevcutYorum != null) return ServiceResult<YolculukYorumu>.BadRequest("Bu kullanıcı için bu yolculukta zaten yorum yapılmış.");

            var yeniYorum = new YolculukYorumu
            {
                YolculukId = yolculukId,
                YorumYapanKullaniciId = yorumYapanKullaniciId,
                PuanlananKullaniciId = puanlananKullaniciId,
                Puan = yorum.Puan,
                Yorum = yorum.Yorum ?? string.Empty,
                YorumTarihi = DateTime.UtcNow,
                OlusturulmaTarihi = DateTime.UtcNow,
                OlusturanKullaniciId = yorumYapanKullaniciId,
                AktifMi = true,
                SilindiMi = false
            };

            _context.YolculukYorumlari.Add(yeniYorum);
            await UpdateAverageRatingAsync(puanlananKullaniciId, yorumYapanKullaniciId, yeniYorum.Puan);
            await _context.SaveChangesAsync();
            return ServiceResult<YolculukYorumu>.Created(yeniYorum);
        }

        public async Task<IEnumerable<YolculukYorumu>> YolculukYorumlariniGetirAsync(int yolculukId)
        {
            return await _context.YolculukYorumlari
                .Include(y => y.YorumYapanKullanici)
                .Include(y => y.PuanlananKullanici)
                .Where(y => y.YolculukId == yolculukId && !y.SilindiMi)
                .OrderByDescending(y => y.YorumTarihi)
                .ToListAsync();
        }

        private static void ValidateRide(Yolculuk yolculuk)
        {
            if (string.IsNullOrWhiteSpace(yolculuk.KalkisNoktasi) || string.IsNullOrWhiteSpace(yolculuk.VarisNoktasi))
                throw new ArgumentException("Kalkış ve varış noktası boş bırakılamaz.");
            if (string.Equals(yolculuk.KalkisNoktasi.Trim(), yolculuk.VarisNoktasi.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Kalkış ve varış noktası aynı olamaz.");
            if (yolculuk.KalkisZamani <= DateTime.Now)
                throw new ArgumentException("Geçmiş tarihli yolculuk oluşturulamaz.");
            if (yolculuk.BosKoltukSayisi is < 1 or > 6)
                throw new ArgumentException("Boş koltuk sayısı 1 ile 6 arasında olmalıdır.");
            if (yolculuk.KisiBasiUcret < 0)
                throw new ArgumentException("Ücret negatif olamaz.");
            if ((yolculuk.Aciklama ?? string.Empty).Length > 250)
                throw new ArgumentException("Açıklama en fazla 250 karakter olabilir.");
            if ((yolculuk.IletisimTelefonu ?? string.Empty).Length > 20)
                throw new ArgumentException("Telefon numarası en fazla 20 karakter olabilir.");
        }

        private async Task UpdateAverageRatingAsync(int puanlananKullaniciId, int guncelleyenKullaniciId, int yeniPuan)
        {
            var puanlanan = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == puanlananKullaniciId && !k.SilindiMi);
            if (puanlanan == null)
            {
                return;
            }

            var puanlar = await _context.YolculukYorumlari
                .Where(y => y.PuanlananKullaniciId == puanlananKullaniciId && !y.SilindiMi)
                .Select(y => y.Puan)
                .ToListAsync();
            puanlar.Add(yeniPuan);

            puanlanan.OrtalamaPuan = Math.Round(puanlar.Average(), 1);
            puanlanan.GuncellenmeTarihi = DateTime.UtcNow;
            puanlanan.GuncelleyenKullaniciId = guncelleyenKullaniciId;
        }
    }
}
