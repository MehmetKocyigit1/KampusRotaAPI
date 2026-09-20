using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KampusRota.Application.Common;
using KampusRota.Application.Services;
using KampusRota.Domain.Entities;
using KampusRota.Infrastructure.Persistence;

namespace KampusRota.Infrastructure.Services
{
    public class YolculukService : IYolculukService
    {
        private readonly AppDbContext _context;

        public YolculukService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Yolculuk>> TumYolculuklariGetirAsync(int? universityId = null)
        {
            var query = _context.Yolculuklar
                .Include(y => y.Surucu)
                .Include(y => y.University)
                .Where(y => !y.SilindiMi && y.AktifMi && y.BosKoltukSayisi > 0 && y.KalkisZamani >= DateTime.Now);

            if (universityId.HasValue && universityId.Value > 0)
            {
                query = query.Where(y => y.UniversityId == universityId.Value);
            }

            return await query.OrderBy(y => y.KalkisZamani).ToListAsync();
        }

        public async Task<IEnumerable<Yolculuk>> MusaitYolculuklariGetirAsync(string kalkisNoktasi, string varisNoktasi, DateTime tarih, int? universityId = null)
        {
            var query = _context.Yolculuklar
                .Include(y => y.Surucu)
                .Include(y => y.University)
                .Where(y => y.AktifMi && !y.SilindiMi)
                .Where(y => y.BosKoltukSayisi > 0)
                .Where(y => y.KalkisZamani.Date == tarih.Date)
                .Where(y => y.KalkisNoktasi.ToLower().Contains(kalkisNoktasi.ToLower()))
                .Where(y => y.VarisNoktasi.ToLower().Contains(varisNoktasi.ToLower()));

            if (universityId.HasValue && universityId.Value > 0)
            {
                query = query.Where(y => y.UniversityId == universityId.Value);
            }

            return await query.OrderBy(y => y.KalkisZamani).ToListAsync();
        }

        public async Task<Yolculuk?> YolculukGetirByIdAsync(int id)
        {
            return await _context.Yolculuklar
                .Include(y => y.Surucu)
                .Include(y => y.University)
                .FirstOrDefaultAsync(y => y.Id == id && !y.SilindiMi);
        }

        public async Task<Yolculuk> YolculukEkleAsync(Yolculuk yolculuk, int kullaniciId)
        {
            ValidateRide(yolculuk);
            await ValidateWomenOnlyPermissionAsync(kullaniciId, yolculuk.SadeceKadinlarMi);

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
            await ValidateWomenOnlyPermissionAsync(kullaniciId, guncelYolculuk.SadeceKadinlarMi);

            var mevcutYolculuk = await _context.Yolculuklar.FindAsync(id);

            if (mevcutYolculuk == null || mevcutYolculuk.SilindiMi) return null;
            if (mevcutYolculuk.SurucuId != kullaniciId)
                throw new ArgumentException("Bu ilanı yalnızca ilan sahibi düzenleyebilir.");

            mevcutYolculuk.GuncellenmeTarihi = DateTime.UtcNow;
            mevcutYolculuk.GuncelleyenKullaniciId = kullaniciId;

            mevcutYolculuk.KalkisNoktasi = guncelYolculuk.KalkisNoktasi;
            mevcutYolculuk.VarisNoktasi = guncelYolculuk.VarisNoktasi;
            mevcutYolculuk.KalkisLatitude = guncelYolculuk.KalkisLatitude;
            mevcutYolculuk.KalkisLongitude = guncelYolculuk.KalkisLongitude;
            mevcutYolculuk.VarisLatitude = guncelYolculuk.VarisLatitude;
            mevcutYolculuk.VarisLongitude = guncelYolculuk.VarisLongitude;
            mevcutYolculuk.UniversityId = guncelYolculuk.UniversityId;
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
            if (yolculuk == null) return ServiceResult<YolculukTalebi>.Fail("İlan bulunamadı.", 404);
            if (yolculuk.SurucuId == yolcuId) return ServiceResult<YolculukTalebi>.Fail("Kendi ilanına katılım talebi gönderemezsin.", 400);
            if (!yolculuk.AktifMi || yolculuk.BosKoltukSayisi <= 0 || yolculuk.KalkisZamani <= DateTime.Now)
                return ServiceResult<YolculukTalebi>.Fail("Bu ilan katılıma uygun değil.", 400);

            var mevcutTalep = await _context.YolculukTalepleri
                .FirstOrDefaultAsync(t => t.YolculukId == yolculukId && t.YolcuId == yolcuId && !t.SilindiMi && t.Durum != "Reddedildi");
            if (mevcutTalep != null) return ServiceResult<YolculukTalebi>.Ok(mevcutTalep, 200);

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
            return ServiceResult<YolculukTalebi>.Ok(yeniTalep, 201);
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
            if (talep?.Yolculuk == null) return ServiceResult<YolculukTalebi>.Fail("Talep bulunamadı.", 404);
            if (talep.Yolculuk.SurucuId != surucuId) return ServiceResult<YolculukTalebi>.Fail("Bu talebi yalnızca sürücü sonuçlandırabilir.", 403);
            if (talep.Durum != "Bekliyor") return ServiceResult<YolculukTalebi>.Fail("Bu talep daha önce sonuçlandırılmış.", 400);

            talep.Durum = onaylandi ? "Onaylandı" : "Reddedildi";
            talep.SurucuNotu = surucuNotu ?? string.Empty;
            talep.OnayTarihi = DateTime.UtcNow;
            talep.GuncellenmeTarihi = DateTime.UtcNow;
            talep.GuncelleyenKullaniciId = surucuId;

            if (onaylandi)
            {
                if (talep.Yolculuk.BosKoltukSayisi <= 0) return ServiceResult<YolculukTalebi>.Fail("Boş koltuk kalmadı.", 400);

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
            }

            await _context.SaveChangesAsync();
            return ServiceResult<YolculukTalebi>.Ok(talep, 200);
        }

        public async Task<ServiceResult<YolculukYorumu>> YolculukYorumuEkleAsync(int yolculukId, int yorumYapanKullaniciId, int puanlananKullaniciId, YolculukYorumu yorum)
        {
            if (yorum.Puan < 1 || yorum.Puan > 5) return ServiceResult<YolculukYorumu>.Fail("Puan 1 ile 5 arasında olmalı.", 400);
            if (yorumYapanKullaniciId == puanlananKullaniciId) return ServiceResult<YolculukYorumu>.Fail("Kullanıcı kendini puanlayamaz.", 400);
            if ((yorum.Yorum ?? string.Empty).Length > 250) return ServiceResult<YolculukYorumu>.Fail("Yorum en fazla 250 karakter olabilir.", 400);

            var yolculuk = await _context.Yolculuklar.FirstOrDefaultAsync(y => y.Id == yolculukId && !y.SilindiMi);
            if (yolculuk == null) return ServiceResult<YolculukYorumu>.Fail("Yolculuk bulunamadı.", 404);

            var katilimVarMi = yolculuk.SurucuId == yorumYapanKullaniciId ||
                await _context.YolculukTalepleri.AnyAsync(t =>
                    t.YolculukId == yolculukId &&
                    t.YolcuId == yorumYapanKullaniciId &&
                    t.Durum == "Onaylandı" &&
                    !t.SilindiMi);
            if (!katilimVarMi) return ServiceResult<YolculukYorumu>.Fail("Sadece yolculuğa katılan kullanıcılar yorum yapabilir.", 400);

            var mevcutYorum = await _context.YolculukYorumlari.FirstOrDefaultAsync(y =>
                y.YolculukId == yolculukId &&
                y.YorumYapanKullaniciId == yorumYapanKullaniciId &&
                y.PuanlananKullaniciId == puanlananKullaniciId &&
                !y.SilindiMi);
            if (mevcutYorum != null) return ServiceResult<YolculukYorumu>.Fail("Bu kullanıcı için bu yolculukta zaten yorum yapılmış.", 400);

            var yeniYorum = new YolculukYorumu
            {
                YolculukId = yolculukId,
                YorumYapanKullaniciId = yorumYapanKullaniciId,
                PuanlananKullaniciId = puanlananKullaniciId,
                Puan = yorum.Puan,
                Yorum = (yorum.Yorum ?? string.Empty).Trim(),
                YorumTarihi = DateTime.UtcNow,
                OlusturulmaTarihi = DateTime.UtcNow,
                OlusturanKullaniciId = yorumYapanKullaniciId,
                AktifMi = true,
                SilindiMi = false
            };

            _context.YolculukYorumlari.Add(yeniYorum);
            await UpdateAverageRatingAsync(puanlananKullaniciId, yorumYapanKullaniciId, yeniYorum.Puan);
            await _context.SaveChangesAsync();
            return ServiceResult<YolculukYorumu>.Ok(yeniYorum, 201);
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

        private async Task ValidateWomenOnlyPermissionAsync(int kullaniciId, bool sadeceKadinlarMi)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == kullaniciId && !k.SilindiMi && k.AktifMi);
            if (kullanici == null)
                throw new ArgumentException("Kullanıcı bulunamadı.");

            if (sadeceKadinlarMi && !string.Equals(kullanici.Cinsiyet?.Trim(), "Kadın", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Sadece kadınlar seçeneğini yalnızca kadın üyeler kullanabilir.");
        }

        private async Task UpdateAverageRatingAsync(int puanlananKullaniciId, int guncelleyenKullaniciId, int yeniPuan)
        {
            var puanlanan = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == puanlananKullaniciId && !k.SilindiMi);
            if (puanlanan == null) return;

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
