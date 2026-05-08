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

         Task<Yolculuk> YolculukGuncelleAsync(int id, Yolculuk guncelYolculuk, int kullaniciId);

         Task<bool> YolculukSilAsync(int id, int silenKullaniciId);
    }
}