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
                        new CampusLocation { LocationKey = "dogu-kampusu", Title = "Doğu Kampüsü", Latitude = 37.8285, Longitude = 30.5345, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bati-kampusu", Title = "Batı Kampüsü", Latitude = 37.8294, Longitude = 30.5264, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "100-yil", Title = "100. Yıl Yerleşkesi", Latitude = 37.7597, Longitude = 30.5482, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "sdu-tip", Title = "SDÜ Tıp Fakültesi Hastanesi", Latitude = 37.8310, Longitude = 30.5310, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "erkek-yurdu", Title = "Erkek Öğrenci Yurdu", Latitude = 37.7823, Longitude = 30.5619, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "mihrihatun-kiz-yurdu", Title = "MihriHatun Kız Yurdu", Latitude = 37.8550, Longitude = 30.5315, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bediuzzaman-kyk", Title = "Bediüzzaman KYK Yurdu", Latitude = 37.8350, Longitude = 30.5280, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "otogar", Title = "Isparta Otogarı", Latitude = 37.8103, Longitude = 30.5372, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "tren-gari", Title = "Isparta Tren Garı", Latitude = 37.7690, Longitude = 30.5590, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "iyas", Title = "Iyaş Park AVM", Latitude = 37.7820, Longitude = 30.5446, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "meydan-avm", Title = "Meydan AVM", Latitude = 37.7647, Longitude = 30.5509, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sehir-merkezi", Title = "Şehir Merkezi (Meydan)", Latitude = 37.7630, Longitude = 30.5547, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kafeler-caddesi", Title = "Kafeler Caddesi", Latitude = 37.7655, Longitude = 30.5520, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sehir-hastanesi", Title = "Isparta Şehir Hastanesi", Latitude = 37.7769, Longitude = 30.5612, Category = LocationCategory.Sosyal }
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
                        new CampusLocation { LocationKey = "itu-ayazaga", Title = "Ayazağa Kampüsü (Maslak)", Latitude = 41.1055, Longitude = 29.0242, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-kutuphane", Title = "Mustafa İnan Kütüphanesi", Latitude = 41.1040, Longitude = 29.0230, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-gumussuyu", Title = "Gümüşsuyu Kampüsü (Taksim)", Latitude = 41.0384, Longitude = 28.9890, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-taskisla", Title = "Taşkışla Kampüsü", Latitude = 41.0412, Longitude = 28.9880, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-macka", Title = "Maçka Kampüsü", Latitude = 41.0455, Longitude = 28.9950, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-maslak-kyk", Title = "Maslak KYK Yurdu", Latitude = 41.1080, Longitude = 29.0280, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-golet-yurtlari", Title = "İTÜ Gölet Yurtları", Latitude = 41.1015, Longitude = 29.0205, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-vadi-yurtlari", Title = "İTÜ Vadi Yurtları", Latitude = 41.1095, Longitude = 29.0195, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-metro", Title = "İTÜ - Ayazağa Metro İstasyonu", Latitude = 41.1032, Longitude = 29.0210, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "alibeykoy-otogar", Title = "Alibeyköy Cep Otogarı", Latitude = 41.0740, Longitude = 28.9480, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "esenler-otogar", Title = "Esenler Büyük Otogar", Latitude = 41.0415, Longitude = 28.8950, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kadikoy-iskele", Title = "Kadıköy Vapur İskelesi", Latitude = 40.9910, Longitude = 29.0235, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "maslak-1453", Title = "Maslak 1453 Caddesi", Latitude = 41.1170, Longitude = 29.0150, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "besiktas-meydan", Title = "Beşiktaş Meydanı", Latitude = 41.0425, Longitude = 29.0065, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "taksim-meydan", Title = "Taksim Meydanı", Latitude = 41.0370, Longitude = 28.9850, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "cevahir-avm", Title = "Mecidiyeköy Cevahir AVM", Latitude = 41.0630, Longitude = 28.9925, Category = LocationCategory.Sosyal }
                    }
                },

                // 3. İSTANBUL - BOĞAZİÇİ
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
                        new CampusLocation { LocationKey = "boun-guney", Title = "Güney Kampüs (Bebek)", Latitude = 41.0836, Longitude = 29.0506, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kuzey", Title = "Kuzey Kampüs (Aşiyan)", Latitude = 41.0865, Longitude = 29.0450, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-hisar", Title = "Hisar Kampüsü", Latitude = 41.0850, Longitude = 29.0390, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kandilli", Title = "Kandilli Kampüsü", Latitude = 41.0610, Longitude = 29.0620, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kilyos", Title = "Kilyos Sarıtepe Kampüsü", Latitude = 41.2460, Longitude = 29.0350, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-1-yurt", Title = "1. Kuzey Öğrenci Yurdu", Latitude = 41.0870, Longitude = 29.0460, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "boun-metro", Title = "Boğaziçi Üni Metro İstasyonu", Latitude = 41.0855, Longitude = 29.0440, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bebek-sahil", Title = "Bebek Sahili & Parkı", Latitude = 41.0770, Longitude = 29.0435, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "rumeli-hisari", Title = "Rumeli Hisarı Kordon", Latitude = 41.0850, Longitude = 29.0570, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "levent-kanyon", Title = "Levent Kanyon AVM", Latitude = 41.0785, Longitude = 29.0115, Category = LocationCategory.Sosyal }
                    }
                },

                // 4. İSTANBUL - YILDIZ TEKNİK
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
                        new CampusLocation { LocationKey = "ytu-davutpasa", Title = "Davutpaşa Kampüsü (Esenler)", Latitude = 41.0255, Longitude = 28.8911, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-besiktas", Title = "Yıldız Beşiktaş Kampüsü", Latitude = 41.0505, Longitude = 29.0115, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-kutuphane", Title = "Davutpaşa Kütüphanesi", Latitude = 41.0260, Longitude = 28.8930, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-fsm-yurdu", Title = "FSM Kız Öğrenci Yurdu", Latitude = 41.0230, Longitude = 28.8980, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ytu-metro", Title = "Davutpaşa - YTÜ Metro İstasyonu", Latitude = 41.0240, Longitude = 28.8950, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "cevizlibag-metrobus", Title = "Cevizlibağ Metrobüs İstasyonu", Latitude = 41.0145, Longitude = 28.9180, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "forum-istanbul", Title = "Bayrampaşa Forum İstanbul AVM", Latitude = 41.0470, Longitude = 28.8960, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bakirkoy-meydan", Title = "Bakırköy Özgürlük Meydanı", Latitude = 40.9800, Longitude = 28.8720, Category = LocationCategory.Sosyal }
                    }
                },

                // 5. İSTANBUL - İSTANBUL ÜNİVERSİTESİ
                new University
                {
                    Name = "İstanbul Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.0130,
                    Longitude = 28.9640,
                    DefaultZoom = 14,
                    EmailDomain = "istanbul.edu.tr",
                    IletisimEmail = "iletisim@istanbul.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "iu-beyazit", Title = "Beyazıt Tarihi Ana Kampüs", Latitude = 41.0130, Longitude = 28.9640, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "iu-avcilar", Title = "Avcılar Kampüsü", Latitude = 40.9905, Longitude = 28.7215, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "iu-cerrahpasa", Title = "Cerrahpaşa Yerleşkesi", Latitude = 41.0050, Longitude = 28.9410, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "iu-capa", Title = "Çapa Tıp Fakültesi", Latitude = 41.0155, Longitude = 28.9380, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "vezneciler-metro", Title = "Vezneciler - İ.Ü. Metro İstasyonu", Latitude = 41.0125, Longitude = 28.9590, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "beyazit-tramvay", Title = "Beyazıt Tramvay Durağı", Latitude = 41.0105, Longitude = 28.9650, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "avcilar-metrobus", Title = "Avcılar Metrobüs İstasyonu", Latitude = 40.9890, Longitude = 28.7230, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "beyazit-meydani", Title = "Beyazıt Meydanı", Latitude = 41.0110, Longitude = 28.9655, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kadikoy-rihtim", Title = "Kadıköy Rıhtım Meydanı", Latitude = 40.9925, Longitude = 29.0230, Category = LocationCategory.Sosyal }
                    }
                },

                // 6. ANKARA - ODTÜ
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
                        new CampusLocation { LocationKey = "odtu-a1", Title = "ODTÜ A1 Ana Giriş Kapısı", Latitude = 39.8913, Longitude = 32.7806, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-kutuphane", Title = "ODTÜ Kütüphanesi", Latitude = 39.8910, Longitude = 32.7845, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-carsi", Title = "ODTÜ Çarşı & Stadyum", Latitude = 39.8870, Longitude = 32.7820, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-19-yurt", Title = "ODTÜ 19. Yurt", Latitude = 39.8885, Longitude = 32.7790, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "faik-hiziroglu-kyk", Title = "Faik Hızıroğlu KYK Yurdu", Latitude = 39.9020, Longitude = 32.7720, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "odtu-metro", Title = "ODTÜ Metro İstasyonu", Latitude = 39.9035, Longitude = 32.7765, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "asti-otogar", Title = "AŞTİ Şehirlerarası Otobüs Terminali", Latitude = 39.9175, Longitude = 32.8130, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "ankara-yht", Title = "Ankara YHT Hızlı Tren Garı", Latitude = 39.9360, Longitude = 32.8440, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kizilay-meydan", Title = "Kızılay Meydanı", Latitude = 39.9208, Longitude = 32.8541, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "tunali-hilmi", Title = "Tunalı Hilmi Caddesi", Latitude = 39.9065, Longitude = 32.8605, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bahcelievler-7", Title = "Bahçelievler 7. Cadde", Latitude = 39.9215, Longitude = 32.8245, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "armada-avm", Title = "Armada AVM", Latitude = 39.9125, Longitude = 32.8080, Category = LocationCategory.Sosyal }
                    }
                },

                // 7. ANKARA - HACETTEPE
                new University
                {
                    Name = "Hacettepe Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.8660,
                    Longitude = 32.7350,
                    DefaultZoom = 14,
                    EmailDomain = "hacettepe.edu.tr",
                    IletisimEmail = "iletisim@hacettepe.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "hacettepe-beytepe", Title = "Beytepe Kampüsü (Çankaya)", Latitude = 39.8660, Longitude = 32.7350, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "hacettepe-sihhiye", Title = "Sıhhiye Merkez Kampüsü", Latitude = 39.9300, Longitude = 32.8620, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "beytepe-kutuphane", Title = "Beytepe Kütüphanesi", Latitude = 39.8680, Longitude = 32.7370, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "beytepe-kyk", Title = "Beytepe KYK Öğrenci Yurdu", Latitude = 39.8640, Longitude = 32.7320, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "beytepe-metro", Title = "Beytepe Metro İstasyonu", Latitude = 39.8880, Longitude = 32.7480, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kizilay-guvenpark", Title = "Kızılay Güvenpark", Latitude = 39.9200, Longitude = 32.8530, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kugulu-park", Title = "Kuğulu Park & Tunalı", Latitude = 39.9040, Longitude = 32.8610, Category = LocationCategory.Sosyal }
                    }
                },

                // 8. ANKARA - ANKARA ÜNİVERSİTESİ
                new University
                {
                    Name = "Ankara Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.9365,
                    Longitude = 32.8310,
                    DefaultZoom = 14,
                    EmailDomain = "ankara.edu.tr",
                    IletisimEmail = "ankara@ankara.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "ankara-tandogan", Title = "Tandoğan Kampüsü (Beşevler)", Latitude = 39.9365, Longitude = 32.8310, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ankara-cebeci", Title = "Cebeci Kampüsü (SBF & Hukuk)", Latitude = 39.9320, Longitude = 32.8750, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "besevler-metro", Title = "Beşevler Ankaray İstasyonu", Latitude = 39.9330, Longitude = 32.8270, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kurtulus-parki", Title = "Kurtuluş Parkı & Metro", Latitude = 39.9280, Longitude = 32.8680, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hamamonu", Title = "Tarihi Hamamönü Meydanı", Latitude = 39.9340, Longitude = 32.8650, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "tandogan-meydani", Title = "Anadolu (Tandoğan) Meydanı", Latitude = 39.9345, Longitude = 32.8370, Category = LocationCategory.Sosyal }
                    }
                },

                // 9. İZMİR - EGE ÜNİVERSİTESİ
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
                        new CampusLocation { LocationKey = "ege-kutuphane", Title = "Ege Üniversitesi Merkez Kütüphane", Latitude = 38.4615, Longitude = 27.2305, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ege-tip", Title = "Ege Tıp Fakültesi Hastanesi", Latitude = 38.4570, Longitude = 27.2240, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ege-kyk-yurdu", Title = "KYK Ege Öğrenci Yurdu", Latitude = 38.4550, Longitude = 27.2350, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "zubeyde-hanim-kyk", Title = "Zübeyde Hanım Kız Yurdu", Latitude = 38.4630, Longitude = 27.2380, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ege-metro", Title = "Ege Üniversitesi Metro İstasyonu", Latitude = 38.4600, Longitude = 27.2280, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bornova-metro", Title = "Bornova Metro İstasyonu", Latitude = 38.4640, Longitude = 27.2150, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "halkapinar-aktarma", Title = "Halkapınar Aktarma (İZBAN/Metro)", Latitude = 38.4350, Longitude = 27.1780, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmir-otogar", Title = "İzmir Otogarı (İZOTAŞ)", Latitude = 38.4300, Longitude = 27.2220, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bornova-kucukpark", Title = "Bornova Küçükpark (Kafeler)", Latitude = 38.4635, Longitude = 27.2195, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bornova-buyukpark", Title = "Bornova Büyükpark", Latitude = 38.4650, Longitude = 27.2140, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "forum-bornova", Title = "Forum Bornova AVM & IKEA", Latitude = 38.4575, Longitude = 27.2400, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "alsancak-kibris", Title = "Alsancak Kıbrıs Şehitleri", Latitude = 38.4385, Longitude = 27.1430, Category = LocationCategory.Sosyal }
                    }
                },

                // 10. İZMİR - DOKUZ EYLÜL ÜNİVERSİTESİ
                new University
                {
                    Name = "Dokuz Eylül Üniversitesi",
                    City = "İzmir",
                    Latitude = 38.3710,
                    Longitude = 27.1990,
                    DefaultZoom = 14,
                    EmailDomain = "deu.edu.tr",
                    IletisimEmail = "bilgi@deu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "deu-tinaztepe", Title = "Tınaztepe Merkez Kampüsü (Buca)", Latitude = 38.3710, Longitude = 27.1990, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "deu-dokuzcesmeler", Title = "Dokuzçeşmeler İİBF Kampüsü", Latitude = 38.3880, Longitude = 27.1720, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "deu-alsancak", Title = "Alsancak Rektörlük", Latitude = 38.4340, Longitude = 27.1410, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "buca-kyk", Title = "Buca Kız Öğrenci Yurdu", Latitude = 38.3740, Longitude = 27.1930, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "sirinyer-izban", Title = "Şirinyer İZBAN İstasyonu", Latitude = 38.3970, Longitude = 27.1590, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hasanaga-bahcesi", Title = "Buca Hasanağa Bahçesi", Latitude = 38.3840, Longitude = 27.1780, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "konak-meydan", Title = "Konak Meydanı & Saat Kulesi", Latitude = 38.4190, Longitude = 27.1285, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "alsancak-kordon", Title = "Alsancak Kordon Sahil", Latitude = 38.4360, Longitude = 27.1400, Category = LocationCategory.Sosyal }
                    }
                },

                // 11. ESKİŞEHİR - ANADOLU ÜNİVERSİTESİ
                new University
                {
                    Name = "Anadolu Üniversitesi",
                    City = "Eskişehir",
                    Latitude = 39.7915,
                    Longitude = 30.5005,
                    DefaultZoom = 14,
                    EmailDomain = "anadolu.edu.tr",
                    IletisimEmail = "bilgi@anadolu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "anadolu-yunusemre", Title = "Yunus Emre Kampüsü (Ana Giriş)", Latitude = 39.7915, Longitude = 30.5005, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "anadolu-kutuphane", Title = "Anadolu Üniversitesi Kütüphanesi", Latitude = 39.7930, Longitude = 30.4990, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "anadolu-ogrenci-merkezi", Title = "Öğrenci Merkezi & Kongre", Latitude = 39.7920, Longitude = 30.4980, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "yunusemre-kyk", Title = "Yunus Emre KYK Öğrenci Yurdu", Latitude = 39.7890, Longitude = 30.5040, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "gunduzalp-kyk", Title = "Gündüzalp Erkek Yurdu", Latitude = 39.7950, Longitude = 30.4850, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "eskisehir-baglar", Title = "Bağlar (Üniversite Caddesi)", Latitude = 39.7845, Longitude = 30.5080, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "espark-avm", Title = "Espark AVM", Latitude = 39.7815, Longitude = 30.5105, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "porsuk-adalar", Title = "Adalar / Porsuk Çayı Kıyısı", Latitude = 39.7750, Longitude = 30.5180, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "barlar-sokagi", Title = "Barlar Sokağı", Latitude = 39.7765, Longitude = 30.5160, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "haller-genclik", Title = "Haller Gençlik Merkezi", Latitude = 39.7830, Longitude = 30.5150, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "eskisehir-tren-gari", Title = "Eskişehir Tren Garı (YHT)", Latitude = 39.7780, Longitude = 30.5120, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "eskisehir-otogar", Title = "Eskişehir Şehirlerarası Otogar", Latitude = 39.7680, Longitude = 30.5650, Category = LocationCategory.Ulasim }
                    }
                },

                // 12. ESKİŞEHİR - ESOGÜ
                new University
                {
                    Name = "Eskişehir Osmangazi Üniversitesi",
                    City = "Eskişehir",
                    Latitude = 39.7505,
                    Longitude = 30.4840,
                    DefaultZoom = 14,
                    EmailDomain = "ogu.edu.tr",
                    IletisimEmail = "bilgi@ogu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "esogu-meselik", Title = "Meşelik Kampüsü (Ana Giriş)", Latitude = 39.7505, Longitude = 30.4840, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "esogu-tip", Title = "ESOGÜ Tıp Fakültesi Hastanesi", Latitude = 39.7530, Longitude = 30.4860, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "dumlupinar-kyk", Title = "Dumlupınar KYK Öğrenci Yurdu", Latitude = 39.7560, Longitude = 30.4910, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "esogu-tramvay", Title = "ESOGÜ Tramvay Son Durak", Latitude = 39.7490, Longitude = 30.4830, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hamamyolu", Title = "Hamamyolu Caddesi", Latitude = 39.7710, Longitude = 30.5210, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sazova-parki", Title = "Sazova Bilim Kültür Parkı", Latitude = 39.7680, Longitude = 30.4720, Category = LocationCategory.Sosyal }
                    }
                },

                // 13. ANTALYA - AKDENİZ
                new University
                {
                    Name = "Akdeniz Üniversitesi",
                    City = "Antalya",
                    Latitude = 36.8965,
                    Longitude = 30.6550,
                    DefaultZoom = 14,
                    EmailDomain = "akdeniz.edu.tr",
                    IletisimEmail = "iletisim@akdeniz.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "akdeniz-merkez", Title = "Akdeniz Üniversitesi Merkez Kampüs", Latitude = 36.8965, Longitude = 30.6550, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "olbia-carsi", Title = "Olbia Çarşısı & Kütüphane", Latitude = 36.8980, Longitude = 30.6540, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "akdeniz-tip", Title = "Akdeniz Tıp Fakültesi Hastanesi", Latitude = 36.8920, Longitude = 30.6520, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "elmalili-kyk", Title = "Elmalılı Hamdi Yazır KYK Yurdu", Latitude = 36.9030, Longitude = 30.6580, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "sehzade-korkut-kyk", Title = "Şehzade Korkut Erkek Yurdu", Latitude = 36.9010, Longitude = 30.6620, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "akdeniz-tramvay", Title = "Kampüs Tramvay İstasyonu", Latitude = 36.8985, Longitude = 30.6620, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "antalya-otogar", Title = "Antalya Şehirlerarası Otogar", Latitude = 36.9210, Longitude = 30.6670, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kultur-mahallesi", Title = "Kültür Mahallesi (Kafeler)", Latitude = 36.9025, Longitude = 30.6510, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "meltem-mahallesi", Title = "Meltem Mahallesi", Latitude = 36.8910, Longitude = 30.6650, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "konyaalti-sahili", Title = "Konyaaltı Sahili", Latitude = 36.8790, Longitude = 30.6480, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "markantalya-avm", Title = "MarkAntalya AVM", Latitude = 36.8940, Longitude = 30.7040, Category = LocationCategory.Sosyal }
                    }
                },

                // 14. BURSA - ULUDAĞ
                new University
                {
                    Name = "Bursa Uludağ Üniversitesi",
                    City = "Bursa",
                    Latitude = 40.2240,
                    Longitude = 28.8710,
                    DefaultZoom = 14,
                    EmailDomain = "uludag.edu.tr",
                    IletisimEmail = "bilgi@uludag.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "uludag-gorukle", Title = "Görükle Merkez Kampüsü", Latitude = 40.2240, Longitude = 28.8710, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "uludag-kutuphane", Title = "Halil İnalcık Kütüphanesi", Latitude = 40.2260, Longitude = 28.8730, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "mete-cengiz-kultur", Title = "Mete Cengiz Kültür Merkezi", Latitude = 40.2230, Longitude = 28.8680, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gorukle-yerlesim", Title = "Görükle Yerleşim (Öğrenci Meydanı)", Latitude = 40.2285, Longitude = 28.8470, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "nilufer-kyk", Title = "Nilüfer KYK Öğrenci Yurdu", Latitude = 40.2210, Longitude = 28.8750, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "uludag-metro", Title = "Üniversite Bursaray İstasyonu", Latitude = 40.2250, Longitude = 28.8760, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bursa-otogar", Title = "Bursa Şehirlerarası Otogar", Latitude = 40.2640, Longitude = 29.0550, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "fsm-bulvari", Title = "FSM Bulvarı Nilüfer", Latitude = 40.2130, Longitude = 28.9870, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "podyumpark", Title = "PodyumPark AVM", Latitude = 40.2190, Longitude = 28.9680, Category = LocationCategory.Sosyal }
                    }
                },

                // 15. KONYA - SELÇUK
                new University
                {
                    Name = "Selçuk Üniversitesi",
                    City = "Konya",
                    Latitude = 38.0260,
                    Longitude = 32.5110,
                    DefaultZoom = 14,
                    EmailDomain = "selcuk.edu.tr",
                    IletisimEmail = "bilgi@selcuk.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "selcuk-alaeddin", Title = "Alaeddin Keykubat Kampüsü", Latitude = 38.0260, Longitude = 32.5110, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "selcuk-kutuphane", Title = "Selçuklu Merkez Kütüphanesi", Latitude = 38.0275, Longitude = 32.5130, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "selcuk-tip", Title = "Selçuk Tıp Fakültesi Hastanesi", Latitude = 38.0230, Longitude = 32.5080, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bosna-hersek", Title = "Bosna Hersek Mah. (Öğrenci Çarşısı)", Latitude = 38.0120, Longitude = 32.5270, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "alaeddin-kyk", Title = "Alaeddin KYK Öğrenci Yurdu", Latitude = 38.0310, Longitude = 32.5070, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "selcuk-tramvay", Title = "Kampüs Tramvay Son Durak", Latitude = 38.0240, Longitude = 32.5150, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "konya-otogar", Title = "Konya Otogarı", Latitude = 37.9540, Longitude = 32.5100, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "konya-yht", Title = "Konya YHT Hızlı Tren Garı", Latitude = 37.8680, Longitude = 32.4830, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "zafer-meydani", Title = "Alaeddin Tepesi & Zafer", Latitude = 37.8730, Longitude = 32.4930, Category = LocationCategory.Sosyal }
                    }
                },

                // 16. KOCAELİ - KOCAELİ ÜNİVERSİTESİ
                new University
                {
                    Name = "Kocaeli Üniversitesi",
                    City = "Kocaeli",
                    Latitude = 40.8210,
                    Longitude = 29.9230,
                    DefaultZoom = 14,
                    EmailDomain = "kocaeli.edu.tr",
                    IletisimEmail = "bilgi@kocaeli.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "kou-umuttepe", Title = "Umuttepe Merkez Kampüsü", Latitude = 40.8210, Longitude = 29.9230, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "kou-kultur-merkezi", Title = "Prof. Dr. Baki Komsuoğlu Kültür Mrk.", Latitude = 40.8225, Longitude = 29.9250, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "kou-hastane", Title = "KOÜ Araştırma ve Uygulama Hastanesi", Latitude = 40.8190, Longitude = 29.9210, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "umuttepe-kyk", Title = "Umuttepe KYK Öğrenci Yurdu", Latitude = 40.8250, Longitude = 29.9270, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "izmit-otogar", Title = "İzmit Şehirlerarası Otogarı", Latitude = 40.7740, Longitude = 29.9720, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmit-yht", Title = "İzmit YHT Tren Garı", Latitude = 40.7630, Longitude = 29.9210, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "yuruyus-yolu", Title = "İzmit Yürüyüş Yolu & Belsa", Latitude = 40.7650, Longitude = 29.9320, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "symbol-avm", Title = "Symbol Kocaeli AVM", Latitude = 40.7680, Longitude = 29.9650, Category = LocationCategory.Sosyal }
                    }
                },

                // 17. SAKARYA - SAKARYA ÜNİVERSİTESİ
                new University
                {
                    Name = "Sakarya Üniversitesi",
                    City = "Sakarya",
                    Latitude = 40.7410,
                    Longitude = 30.3340,
                    DefaultZoom = 14,
                    EmailDomain = "sakarya.edu.tr",
                    IletisimEmail = "iletisim@sakarya.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "sau-esentepe", Title = "Esentepe Kampüsü (Ana Giriş)", Latitude = 40.7410, Longitude = 30.3340, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "sau-kutuphane", Title = "Turgut Özal Kütüphanesi", Latitude = 40.7425, Longitude = 30.3325, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "zaim-kyk", Title = "Sabahattin Zaim KYK Yurdu", Latitude = 40.7380, Longitude = 30.3380, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "mavi-durak", Title = "Serdivan Mavi Durak (Kafeler)", Latitude = 40.7620, Longitude = 30.3650, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "serdivan-avm", Title = "Serdivan AVM & Cadde54", Latitude = 40.7650, Longitude = 30.3620, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sakarya-otogar", Title = "Sakarya Şehirlerarası Otogar", Latitude = 40.7180, Longitude = 30.4020, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "cark-caddesi", Title = "Adapazarı Çark Caddesi", Latitude = 40.7720, Longitude = 30.3950, Category = LocationCategory.Sosyal }
                    }
                },

                // 18. TRABZON - KTÜ
                new University
                {
                    Name = "Karadeniz Teknik Üniversitesi",
                    City = "Trabzon",
                    Latitude = 40.9950,
                    Longitude = 39.7710,
                    DefaultZoom = 14,
                    EmailDomain = "ktu.edu.tr",
                    IletisimEmail = "bilgi@ktu.edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "ktu-kanuni", Title = "Kanuni Merkez Kampüsü", Latitude = 40.9950, Longitude = 39.7710, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ktu-kutuphane", Title = "Faik Ahmet Barutçu Kütüphanesi", Latitude = 40.9965, Longitude = 39.7730, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ktu-farabi", Title = "Farabi Tıp Fakültesi Hastanesi", Latitude = 40.9930, Longitude = 39.7680, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ktu-kyk", Title = "Doğu Karadeniz KYK Yurdu", Latitude = 40.9980, Longitude = 39.7740, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "trabzon-havalimani", Title = "Trabzon Havalimanı", Latitude = 40.9910, Longitude = 39.7890, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "trabzon-otogar", Title = "Trabzon Şehirlerarası Otogar", Latitude = 41.0020, Longitude = 39.7480, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "trabzon-meydan", Title = "Trabzon Meydan Parkı (Maraş Cad.)", Latitude = 41.0060, Longitude = 39.7270, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "forum-trabzon", Title = "Forum Trabzon AVM", Latitude = 40.9980, Longitude = 39.7560, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kalkinma-mahallesi", Title = "Kalkınma Mah. (Öğrenci Kafeleri)", Latitude = 40.9970, Longitude = 39.7640, Category = LocationCategory.Sosyal }
                    }
                }
            };
        }
    }
}
