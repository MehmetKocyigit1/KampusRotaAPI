using KampusRota.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KampusRota.Services
{
    public interface IYolculukService
    {
         Task<IEnumerable<Yolculuk>> TumYolculuklariGetirAsync();

         Task<IEnumerable<Yolculuk>> MusaitYolculuklariGetirAsync(string kalkisNoktasi, string varisNoktasi, DateTime tarih);

         Task<Yolculuk> YolculukEkleAsync(Yolculuk yolculuk, int kullaniciId);

         Task<Yolculuk?> YolculukGuncelleAsync(int id, Yolculuk guncelYolculuk, int kullaniciId);

         Task<bool> YolculukSilAsync(int id, int silenKullaniciId);

         Task<ServiceResult<YolculukTalebi>> KatilmaTalebiOlusturAsync(int yolculukId, int yolcuId, string talepMesaji);

         Task<IEnumerable<YolculukTalebi>> SurucuTalepleriniGetirAsync(int surucuId);

         Task<IEnumerable<YolculukTalebi>> YolcuTalepleriniGetirAsync(int yolcuId);

         Task<ServiceResult<YolculukTalebi>> TalepDurumuGuncelleAsync(int talepId, int surucuId, bool onaylandi, string? surucuNotu);

         Task<ServiceResult<YolculukYorumu>> YolculukYorumuEkleAsync(int yolculukId, int yorumYapanKullaniciId, int puanlananKullaniciId, YolculukYorumu yorum);

         Task<IEnumerable<YolculukYorumu>> YolculukYorumlariniGetirAsync(int yolculukId);
    }
}
