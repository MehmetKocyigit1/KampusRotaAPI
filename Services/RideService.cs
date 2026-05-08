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

        // Dependency Injection (OOP Prensibi)
        public YolculukService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Yolculuk>> TumYolculuklariGetirAsync()
        {
            // Sadece SİLİNMEMİŞ ilanları sürücü bilgisiyle getir
            return await _context.Yolculuklar
                .Include(y => y.Surucu)
                .Where(y => !y.SilindiMi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Yolculuk>> MusaitYolculuklariGetirAsync(string kalkisNoktasi, string varisNoktasi, DateTime tarih)
        {
            return await _context.Yolculuklar
                .Include(y => y.Surucu)
                .Where(y => y.AktifMi == true && !y.SilindiMi) // Aktif ve silinmemiş olmalı
                .Where(y => y.BosKoltukSayisi > 0) // Boş yer olmalı
                .Where(y => y.KalkisZamani.Date == tarih.Date)
                .Where(y => y.KalkisNoktasi.ToLower().Contains(kalkisNoktasi.ToLower()))
                .Where(y => y.VarisNoktasi.ToLower().Contains(varisNoktasi.ToLower()))
                .OrderBy(y => y.KalkisZamani) // Saate göre sırala
                .ToListAsync();
        }

        public async Task<Yolculuk> YolculukEkleAsync(Yolculuk yolculuk, int kullaniciId)
        {
            // BaseEntity'den gelen Audit Log (Takip) bilgileri
            yolculuk.OlusturulmaTarihi = DateTime.UtcNow;
            yolculuk.OlusturanKullaniciId = kullaniciId;
            yolculuk.SurucuId = kullaniciId;
            yolculuk.AktifMi = true;
            yolculuk.SilindiMi = false;

            _context.Yolculuklar.Add(yolculuk);
            await _context.SaveChangesAsync();
            return yolculuk;
        }

        public async Task<Yolculuk> YolculukGuncelleAsync(int id, Yolculuk guncelYolculuk, int kullaniciId)
        {
            var mevcutYolculuk = await _context.Yolculuklar.FindAsync(id);

            // Eğer ilan yoksa veya silinmişse işlemi iptal et
            if (mevcutYolculuk == null || mevcutYolculuk.SilindiMi) return null;

            // BaseEntity'den gelen güncelleme logları
            mevcutYolculuk.GuncellenmeTarihi = DateTime.UtcNow;
            mevcutYolculuk.GuncelleyenKullaniciId = kullaniciId;

            // İlan detaylarını güncelle (Yeni eklenen özellikler dahil)
            mevcutYolculuk.KalkisNoktasi = guncelYolculuk.KalkisNoktasi;
            mevcutYolculuk.VarisNoktasi = guncelYolculuk.VarisNoktasi;
            mevcutYolculuk.KalkisZamani = guncelYolculuk.KalkisZamani;
            mevcutYolculuk.BosKoltukSayisi = guncelYolculuk.BosKoltukSayisi;
            mevcutYolculuk.KisiBasiUcret = guncelYolculuk.KisiBasiUcret;
            mevcutYolculuk.Aciklama = guncelYolculuk.Aciklama;
            mevcutYolculuk.SadeceKadinlarMi = guncelYolculuk.SadeceKadinlarMi;

            await _context.SaveChangesAsync();
            return mevcutYolculuk;
        }

        public async Task<bool> YolculukSilAsync(int id, int silenKullaniciId)
        {
            var yolculuk = await _context.Yolculuklar.FindAsync(id);
            if (yolculuk == null || yolculuk.SilindiMi) return false;

            // DİKKAT: Gerçek silme (Remove) işlemi yapmıyoruz! Soft Delete uyguluyoruz.
            yolculuk.SilindiMi = true;
            yolculuk.SilinmeTarihi = DateTime.UtcNow;
            yolculuk.SilenKullaniciId = silenKullaniciId;
            yolculuk.AktifMi = false; // İlanı yayından kaldırıyoruz

            await _context.SaveChangesAsync();
            return true;
        }
    }
}