using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KampusRota.Domain.Entities;
using KampusRota.Domain.Enums;

namespace KampusRota.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.Universities.AnyAsync())
            {
                var universities = GetSeedUniversities();
                await context.Universities.AddRangeAsync(universities);
                await context.SaveChangesAsync();
            }
        }

        public static List<University> GetSeedUniversities()
        {
            return new List<University>
            {
                // 1. ISPARTA - SDU
                new University
                {
                    Name = "Süleyman Demirel Üniversitesi",
                    City = "Isparta",
                    Latitude = 37.8285,
                    Longitude = 30.5345,
                    DefaultZoom = 14,
                    EmailDomain = "sdu.edu.tr",
                    IletisimEmail = "info@sdu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        // Kampüs
                        new CampusLocation { LocationKey = "dogu-kampusu", Title = "Doğu Kampüsü", Latitude = 37.8285, Longitude = 30.5345, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bati-kampusu", Title = "Batı Kampüsü", Latitude = 37.829387, Longitude = 30.526435, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "100-yil", Title = "100. Yıl Yerleşkesi", Latitude = 37.759684, Longitude = 30.548229, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "sdu-tip-hastanesi", Title = "SDÜ Tıp Fakültesi Hastanesi", Latitude = 37.8310, Longitude = 30.5310, Category = LocationCategory.Kampus },
                        // Yurtlar
                        new CampusLocation { LocationKey = "erkek-yurdu", Title = "Erkek Yurdu", Latitude = 37.782313, Longitude = 30.561886, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "mihrihatun-kiz-yurdu", Title = "MihriHatun Kız Yurdu", Latitude = 37.854985, Longitude = 30.531456, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bediuzzaman-kyk", Title = "Bediüzzaman KYK Yurdu", Latitude = 37.8350, Longitude = 30.5280, Category = LocationCategory.Yurt },
                        // Ulaşım
                        new CampusLocation { LocationKey = "otogar", Title = "Isparta Otogarı", Latitude = 37.810293, Longitude = 30.537214, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "tren-gari", Title = "Isparta Tren Garı", Latitude = 37.7690, Longitude = 30.5590, Category = LocationCategory.Ulasim },
                        // Sosyal & Şehir
                        new CampusLocation { LocationKey = "iyas", Title = "Iyaş Park AVM", Latitude = 37.782036, Longitude = 30.544647, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "meydan-avm", Title = "Meydan AVM", Latitude = 37.764732, Longitude = 30.550920, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sehir-merkezi", Title = "Şehir Merkezi (Valilik Önü)", Latitude = 37.7630, Longitude = 30.5547, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kafeler-caddesi", Title = "Kafeler Caddesi", Latitude = 37.7655, Longitude = 30.5520, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "gokcay", Title = "Gökçay Mesireliği", Latitude = 37.7481, Longitude = 30.5465, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sehir-hastanesi", Title = "Isparta Şehir Hastanesi", Latitude = 37.776919, Longitude = 30.561183, Category = LocationCategory.Sosyal }
                    }
                },

                // 2. İSTANBUL - İTÜ
                new University
                {
                    Name = "İstanbul Teknik Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.1055,
                    Longitude = 29.0242,
                    DefaultZoom = 14,
                    EmailDomain = "itu.edu.tr",
                    IletisimEmail = "bilgi@itu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        // Kampüs
                        new CampusLocation { LocationKey = "itu-ayazaga-ana-giris", Title = "Ayazağa Kampüsü (Ana Giriş)", Latitude = 41.1055, Longitude = 29.0242, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-kutuphane", Title = "Mustafa İnan Kütüphanesi", Latitude = 41.1040, Longitude = 29.0230, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-gumussuyu", Title = "Gümüşsuyu Kampüsü", Latitude = 41.0384, Longitude = 28.9890, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-taskisla", Title = "Taşkışla Kampüsü", Latitude = 41.0412, Longitude = 28.9880, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-macka", Title = "Maçka Kampüsü", Latitude = 41.0455, Longitude = 28.9950, Category = LocationCategory.Kampus },
                        // Yurtlar
                        new CampusLocation { LocationKey = "itu-maslak-kyk", Title = "Maslak KYK Yurdu", Latitude = 41.1080, Longitude = 29.0280, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-golet-yurtlari", Title = "İTÜ Gölet Yurtları", Latitude = 41.1015, Longitude = 29.0205, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-vadi-yurtlari", Title = "İTÜ Vadi Yurtları", Latitude = 41.1095, Longitude = 29.0195, Category = LocationCategory.Yurt },
                        // Ulaşım
                        new CampusLocation { LocationKey = "itu-metro", Title = "İTÜ - Ayazağa Metro İstasyonu", Latitude = 41.1032, Longitude = 29.0210, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "alibeykoy-otogar", Title = "Alibeyköy Cep Otogarı", Latitude = 41.0740, Longitude = 28.9480, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "esenler-otogar", Title = "Esenler Otogarı (Büyük İstanbul)", Latitude = 41.0415, Longitude = 28.8950, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kadikoy-iskele", Title = "Kadıköy Vapur İskelesi", Latitude = 40.9910, Longitude = 29.0235, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "istanbul-havalimani", Title = "İstanbul Havalimanı (İGA)", Latitude = 41.2750, Longitude = 28.7520, Category = LocationCategory.Ulasim },
                        // Sosyal
                        new CampusLocation { LocationKey = "maslak-1453", Title = "Maslak 1453 Caddesi", Latitude = 41.1170, Longitude = 29.0150, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "besiktas-meydan", Title = "Beşiktaş Meydanı", Latitude = 41.0425, Longitude = 29.0065, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "taksim-meydan", Title = "Taksim Meydanı", Latitude = 41.0370, Longitude = 28.9850, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "cevahir-avm", Title = "Mecidiyeköy Cevahir AVM", Latitude = 41.0630, Longitude = 28.9925, Category = LocationCategory.Sosyal }
                    }
                },

                // 3. ANKARA - ODTÜ
                new University
                {
                    Name = "Orta Doğu Teknik Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.8913,
                    Longitude = 32.7806,
                    DefaultZoom = 14,
                    EmailDomain = "metu.edu.tr",
                    IletisimEmail = "info@metu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        // Kampüs
                        new CampusLocation { LocationKey = "odtu-a1-kapisi", Title = "ODTÜ A1 Kapısı (Eskişehir Yolu)", Latitude = 39.9050, Longitude = 32.7830, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-a4-kapisi", Title = "ODTÜ A4 Kapısı (100. Yıl)", Latitude = 39.8935, Longitude = 32.8020, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-kutuphane", Title = "ODTÜ Merkez Kütüphane", Latitude = 39.8913, Longitude = 32.7806, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-fizik-cimleri", Title = "Fizik Çimleri", Latitude = 39.8895, Longitude = 32.7795, Category = LocationCategory.Kampus },
                        // Yurtlar
                        new CampusLocation { LocationKey = "odtu-yurtlar", Title = "ODTÜ Yurtlar Bölgesi", Latitude = 39.8870, Longitude = 32.7750, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "tahsin-banguoglu-kyk", Title = "Tahsin Banguoğlu KYK Yurdu", Latitude = 39.8980, Longitude = 32.8120, Category = LocationCategory.Yurt },
                        // Ulaşım
                        new CampusLocation { LocationKey = "odtu-metro", Title = "ODTÜ Metro İstasyonu", Latitude = 39.9065, Longitude = 32.7820, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "asti-otogar", Title = "AŞTİ Şehirlerarası Otobüs Terminali", Latitude = 39.9175, Longitude = 32.8140, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "ankara-yht-gari", Title = "Ankara YHT Hızlı Tren Garı", Latitude = 39.9360, Longitude = 32.8440, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kizilay-metro", Title = "Kızılay Metro Aktarma Merkezi", Latitude = 39.9208, Longitude = 32.8540, Category = LocationCategory.Ulasim },
                        // Sosyal
                        new CampusLocation { LocationKey = "odtu-carsi", Title = "ODTÜ Çarşı", Latitude = 39.8890, Longitude = 32.7815, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kizilay-meydani", Title = "Kızılay Meydanı", Latitude = 39.9208, Longitude = 32.8540, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "tunali-hilmi", Title = "Tunalı Hilmi Caddesi", Latitude = 39.9030, Longitude = 32.8600, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bahcelievler-7-cadde", Title = "Bahçelievler 7. Cadde", Latitude = 39.9220, Longitude = 32.8220, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "armada-avm", Title = "Armada AVM", Latitude = 39.9135, Longitude = 32.8090, Category = LocationCategory.Sosyal }
                    }
                },

                // 4. İSTANBUL - BOĞAZİÇİ
                new University
                {
                    Name = "Boğaziçi Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.0836,
                    Longitude = 29.0506,
                    DefaultZoom = 15,
                    EmailDomain = "boun.edu.tr",
                    IletisimEmail = "iletisim@boun.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        // Kampüs
                        new CampusLocation { LocationKey = "boun-guney-kampus", Title = "Güney Kampüs (Tarihi Giriş)", Latitude = 41.0836, Longitude = 29.0506, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kuzey-kampus", Title = "Kuzey Kampüs", Latitude = 41.0872, Longitude = 29.0441, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kandilli", Title = "Kandilli Kampüsü (Rasathane)", Latitude = 41.0620, Longitude = 29.0625, Category = LocationCategory.Kampus },
                        // Yurtlar
                        new CampusLocation { LocationKey = "boun-ucaksavar-yurt", Title = "Uçaksavar Yurdu", Latitude = 41.0820, Longitude = 29.0380, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "boun-kuzey-yurt", Title = "1. Kuzey Yurdu", Latitude = 41.0875, Longitude = 29.0430, Category = LocationCategory.Yurt },
                        // Ulaşım
                        new CampusLocation { LocationKey = "boun-hisarustu-metro", Title = "Boğaziçi Üni. - Hisarüstü Metro", Latitude = 41.0848, Longitude = 29.0435, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "levent-metro", Title = "Levent Metro İstasyonu", Latitude = 41.0770, Longitude = 29.0140, Category = LocationCategory.Ulasim },
                        // Sosyal
                        new CampusLocation { LocationKey = "hisarustu-carsi", Title = "Rumeli Hisarüstü Çarşı", Latitude = 41.0840, Longitude = 29.0470, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bebek-sahili", Title = "Bebek Sahili", Latitude = 41.0760, Longitude = 29.0440, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "akmerkez-avm", Title = "Etiler Akmerkez AVM", Latitude = 41.0775, Longitude = 29.0305, Category = LocationCategory.Sosyal }
                    }
                },

                // 5. İSTANBUL - YTÜ
                new University
                {
                    Name = "Yıldız Teknik Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.0255,
                    Longitude = 28.8911,
                    DefaultZoom = 14,
                    EmailDomain = "yildiz.edu.tr",
                    IletisimEmail = "rektorluk@yildiz.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        // Kampüs
                        new CampusLocation { LocationKey = "ytu-davutpasa-kampus", Title = "Davutpaşa Kampüsü (Kışla)", Latitude = 41.0255, Longitude = 28.8911, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-besiktas-kampus", Title = "Yıldız / Beşiktaş Merkez Kampüsü", Latitude = 41.0503, Longitude = 29.0105, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-kutuphane", Title = "Davutpaşa Merkez Kütüphane", Latitude = 41.0245, Longitude = 28.8920, Category = LocationCategory.Kampus },
                        // Yurtlar
                        new CampusLocation { LocationKey = "ytu-davutpasa-kyk", Title = "Davutpaşa KYK Öğrenci Yurdu", Latitude = 41.0280, Longitude = 28.8870, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "fsm-kyk-yurdu", Title = "FSM Kız Öğrenci Yurdu", Latitude = 41.0230, Longitude = 28.8980, Category = LocationCategory.Yurt },
                        // Ulaşım
                        new CampusLocation { LocationKey = "ytu-davutpasa-metro", Title = "Davutpaşa - YTÜ Metro İstasyonu", Latitude = 41.0240, Longitude = 28.8950, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "cevizlibag-metrobus", Title = "Cevizlibağ Metrobüs İstasyonu", Latitude = 41.0145, Longitude = 28.9180, Category = LocationCategory.Ulasim },
                        // Sosyal
                        new CampusLocation { LocationKey = "forum-istanbul", Title = "Bayrampaşa Forum İstanbul AVM", Latitude = 41.0470, Longitude = 28.8960, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bakirkoy-meydan", Title = "Bakırköy Özgürlük Meydanı", Latitude = 40.9800, Longitude = 28.8720, Category = LocationCategory.Sosyal }
                    }
                },

                // 6. İZMİR - EGE ÜNİVERSİTESİ
                new University
                {
                    Name = "Ege Üniversitesi",
                    City = "İzmir",
                    Latitude = 38.4600,
                    Longitude = 27.2280,
                    DefaultZoom = 14,
                    EmailDomain = "ege.edu.tr",
                    IletisimEmail = "bilgi@ege.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        // Kampüs
                        new CampusLocation { LocationKey = "ege-kutuphane", Title = "Ege Üniversitesi Merkez Kütüphane", Latitude = 38.4615, Longitude = 27.2305, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ege-hastanesi", Title = "Ege Tıp Fakültesi Hastanesi", Latitude = 38.4570, Longitude = 27.2240, Category = LocationCategory.Kampus },
                        // Yurtlar
                        new CampusLocation { LocationKey = "ege-kyk-yurdu", Title = "KYK Ege Öğrenci Yurdu", Latitude = 38.4550, Longitude = 27.2350, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "zubeyde-hanim-yurdu", Title = "Zübeyde Hanım Kız Yurdu", Latitude = 38.4630, Longitude = 27.2380, Category = LocationCategory.Yurt },
                        // Ulaşım
                        new CampusLocation { LocationKey = "ege-metro", Title = "Ege Üniversitesi Metro İstasyonu", Latitude = 38.4600, Longitude = 27.2280, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bornova-metro", Title = "Bornova Metro İstasyonu", Latitude = 38.4640, Longitude = 27.2150, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "halkapinar-izban", Title = "Halkapınar Aktarma (İZBAN/Metro)", Latitude = 38.4350, Longitude = 27.1780, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmir-otogar", Title = "İzmir Otogarı (İZOTAŞ)", Latitude = 38.4300, Longitude = 27.2220, Category = LocationCategory.Ulasim },
                        // Sosyal
                        new CampusLocation { LocationKey = "bornova-kucukpark", Title = "Bornova Küçükpark (Kafeler Bölgesi)", Latitude = 38.4635, Longitude = 27.2195, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bornova-buyukpark", Title = "Bornova Büyükpark", Latitude = 38.4650, Longitude = 27.2140, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "forum-bornova", Title = "Forum Bornova AVM & IKEA", Latitude = 38.4575, Longitude = 27.2400, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "alsancak-kibris-sehitleri", Title = "Alsancak Kıbrıs Şehitleri Caddesi", Latitude = 38.4385, Longitude = 27.1430, Category = LocationCategory.Sosyal }
                    }
                }
            };
        }
    }
}
