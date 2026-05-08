using KampusRota.Models;
using System.Threading.Tasks;

namespace KampusRota.Services
{
    public interface IKullaniciService
    {
         Task<Kullanici> KayitOlAsync(Kullanici kullanici);

         Task<Kullanici> GirisYapAsync(string email, string sifre);

         Task<bool> SifreDegistirAsync(int kullaniciId, SifreDegistirmeIstegi istek);
    }
}