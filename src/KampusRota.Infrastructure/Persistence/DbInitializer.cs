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
            var seedUnis = GetSeedUniversities();
            if (!await context.Universities.AnyAsync())
            {
                await context.Universities.AddRangeAsync(seedUnis);
                await context.SaveChangesAsync();
            }
            else
            {
                var existingUnis = await context.Universities.Include(u => u.Locations).ToListAsync();
                bool hasChanges = false;

                foreach (var seed in seedUnis)
                {
                    var existing = existingUnis.FirstOrDefault(u => u.EmailDomain == seed.EmailDomain || u.Name == seed.Name);
                    if (existing != null)
                    {
                        if (existing.Name != seed.Name || existing.City != seed.City)
                        {
                            existing.Name = seed.Name;
                            existing.City = seed.City;
                            hasChanges = true;
                        }

                        if (seed.Locations != null)
                        {
                            foreach (var seedLoc in seed.Locations)
                            {
                                var existingLoc = existing.Locations?.FirstOrDefault(l => l.LocationKey == seedLoc.LocationKey);
                                if (existingLoc != null)
                                {
                                    if (existingLoc.Title != seedLoc.Title || existingLoc.Category != seedLoc.Category)
                                    {
                                        existingLoc.Title = seedLoc.Title;
                                        existingLoc.Category = seedLoc.Category;
                                        hasChanges = true;
                                    }
                                }
                                else
                                {
                                    seedLoc.UniversityId = existing.Id;
                                    context.CampusLocations.Add(seedLoc);
                                    hasChanges = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        await context.Universities.AddAsync(seed);
                        hasChanges = true;
                    }
                }

                if (hasChanges)
                {
                    await context.SaveChangesAsync();
                }
            }
        }

        public static List<University> GetSeedUniversities()
        {
            return new List<University>
            {
                // 1. ISPARTA - Süleyman Demirel Üniversitesi
                new University
                {
                    Name = "Süleyman Demirel Üniversitesi",
                    City = "Isparta",
                    Latitude = 37.8285,
                    Longitude = 30.5345,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "dogu-kampusu", Title = "Doğu Kampüsü (Rektörlük & Müh.)", Latitude = 37.8285, Longitude = 30.5345, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bati-kampusu", Title = "Batı Kampüsü (İİBF & İlahiyat)", Latitude = 37.8294, Longitude = 30.5264, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "100-yil", Title = "100. Yıl Sağlık Yerleşkesi", Latitude = 37.7597, Longitude = 30.5482, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "sdu-tip", Title = "SDÜ Tıp Fakültesi Hastanesi", Latitude = 37.831, Longitude = 30.531, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "erkek-yurdu", Title = "Isparta KYK Erkek Öğrenci Yurdu", Latitude = 37.7823, Longitude = 30.5619, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "mihrihatun-kiz-yurdu", Title = "MihriHatun KYK Kız Yurdu", Latitude = 37.855, Longitude = 30.5315, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bediuzzaman-kyk", Title = "Bediüzzaman KYK Yurdu (Doğu Kampüs)", Latitude = 37.835, Longitude = 30.528, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "gulkent-kyk", Title = "Gülkent KYK Kız Yurdu", Latitude = 37.778, Longitude = 30.548, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "otogar", Title = "Isparta Şehirlerarası Otogarı", Latitude = 37.8103, Longitude = 30.5372, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "tren-gari", Title = "Isparta Tren Garı (Göller Ekspresi)", Latitude = 37.769, Longitude = 30.559, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "cunur-kavsagi", Title = "Çünür Kavşağı & Otobüs Aktarma", Latitude = 37.821, Longitude = 30.535, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "iyas", Title = "Iyaş Park AVM & Yaşam Merkezi", Latitude = 37.782, Longitude = 30.5446, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "meydan-avm", Title = "Meydan AVM", Latitude = 37.7647, Longitude = 30.5509, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sehir-merkezi", Title = "Şehir Merkezi (Hükümet Meydanı)", Latitude = 37.763, Longitude = 30.5547, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kafeler-caddesi", Title = "Kafeler Caddesi (Öğrenci Buluşma)", Latitude = 37.7655, Longitude = 30.552, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sehir-hastanesi", Title = "Isparta Şehir Hastanesi", Latitude = 37.7769, Longitude = 30.5612, Category = LocationCategory.Sosyal },
                    }
                },

                // 2. İSTANBUL - İstanbul Teknik Üniversitesi
                new University
                {
                    Name = "İstanbul Teknik Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.1055,
                    Longitude = 29.0242,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "itu-ayazaga", Title = "Ayazağa Kampüsü (Maslak)", Latitude = 41.1055, Longitude = 29.0242, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-kutuphane", Title = "Mustafa İnan Kütüphanesi (7/24)", Latitude = 41.104, Longitude = 29.023, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-gumussuyu", Title = "Gümüşsuyu Kampüsü (Taksim)", Latitude = 41.0384, Longitude = 28.989, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-taskisla", Title = "Taşkışla Kampüsü (Mimarlık)", Latitude = 41.0412, Longitude = 28.988, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-macka", Title = "Maçka Kampüsü (İşletme)", Latitude = 41.0455, Longitude = 28.995, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "itu-maslak-kyk", Title = "Maslak KYK Öğrenci Yurdu", Latitude = 41.108, Longitude = 29.028, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-golet-yurtlari", Title = "İTÜ Gölet Yurtları", Latitude = 41.1015, Longitude = 29.0205, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-vadi-yurtlari", Title = "İTÜ Vadi Yurtları Kompleksi", Latitude = 41.1095, Longitude = 29.0195, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-ferhunde-yurdu", Title = "Ferhunde Birkan Kız Yurdu", Latitude = 41.106, Longitude = 29.021, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "itu-metro", Title = "İTÜ-Ayazağa Metro İstasyonu (M2)", Latitude = 41.106, Longitude = 29.022, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "maslak-otobus", Title = "Maslak Otobüs & Minibüs Durakları", Latitude = 41.1075, Longitude = 29.0255, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "taksim-metro", Title = "Taksim Metro & Meydan", Latitude = 41.037, Longitude = 28.985, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "zincirlikuyu-metrobus", Title = "Zincirlikuyu Metrobüs Aktarma", Latitude = 41.068, Longitude = 29.013, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "istinyepark", Title = "İstinyePark AVM", Latitude = 41.111, Longitude = 29.033, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "vadi-istanbul", Title = "Vadistanbul AVM & Yaşam Vadisi", Latitude = 41.103, Longitude = 28.987, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "ari-teknokent", Title = "İTÜ ARI Teknokent", Latitude = 41.102, Longitude = 29.026, Category = LocationCategory.Sosyal },
                    }
                },

                // 3. İSTANBUL - Boğaziçi Üniversitesi
                new University
                {
                    Name = "Boğaziçi Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.0836,
                    Longitude = 29.0506,
                    DefaultZoom = 15,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "boun-guney", Title = "Güney Kampüs (Tarihi Bina & Meydan)", Latitude = 41.0836, Longitude = 29.0506, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kuzey", Title = "Kuzey Kampüs (Kütüphane & Lablar)", Latitude = 41.0865, Longitude = 29.046, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-kandilli", Title = "Kandilli Kampüsü & Rasathane", Latitude = 41.063, Longitude = 29.062, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-hisar", Title = "Hisar Kampüsü", Latitude = 41.082, Longitude = 29.043, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-ucaksavar", Title = "Uçaksavar Kampüsü (Spor Salonu)", Latitude = 41.078, Longitude = 29.038, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "boun-superdorm", Title = "Süperdorm Öğrenci Yurdu", Latitude = 41.079, Longitude = 29.039, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "fsm-kyk-yurdu", Title = "Fatih Sultan Mehmet KYK Yurdu", Latitude = 41.092, Longitude = 29.041, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "kilyos-kyk-yurdu", Title = "Kilyos Sarıtepe KYK Yurdu", Latitude = 41.245, Longitude = 29.028, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "boun-metro", Title = "Boğaziçi Üniversitesi Metro (M6)", Latitude = 41.085, Longitude = 29.047, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "levent-metro", Title = "Levent Metro & Metrobüs Aktarma", Latitude = 41.076, Longitude = 29.014, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hisarustu", Title = "Rumeli Hisarüstü Kafeler", Latitude = 41.084, Longitude = 29.048, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bebek-sahil", Title = "Bebek Parkı & Sahil Yürüyüş Yolu", Latitude = 41.077, Longitude = 29.044, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "rumeli-hisari", Title = "Rumeli Hisarı Meydanı", Latitude = 41.086, Longitude = 29.056, Category = LocationCategory.Sosyal },
                    }
                },

                // 4. İSTANBUL - Yıldız Teknik Üniversitesi
                new University
                {
                    Name = "Yıldız Teknik Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.0255,
                    Longitude = 28.8911,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "ytu-davutpasa", Title = "Davutpaşa Kampüsü (Ana Yerleşke)", Latitude = 41.0255, Longitude = 28.8911, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-yildiz", Title = "Yıldız Merkez Kampüsü (Beşiktaş)", Latitude = 41.05, Longitude = 29.01, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ytu-kutuphane", Title = "YTÜ Şevket Sabancı Kütüphanesi", Latitude = 41.027, Longitude = 28.892, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "fsm-erkek-kyk", Title = "FSM Erkek KYK Öğrenci Yurdu", Latitude = 41.023, Longitude = 28.889, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "cevizlibag-ataturk-kyk", Title = "Cevizlibağ Atatürk KYK Kız Yurdu", Latitude = 41.018, Longitude = 28.918, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "besiktas-sair-nedim", Title = "Beşiktaş Şair Nedim KYK Yurdu", Latitude = 41.043, Longitude = 28.998, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "davutpasa-metro", Title = "Davutpaşa-YTÜ Metro İstasyonu (M1A)", Latitude = 41.024, Longitude = 28.894, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "cevizlibag-metrobus", Title = "Cevizlibağ Metrobüs İstasyonu", Latitude = 41.017, Longitude = 28.915, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "ytu-teknopark", Title = "Yıldız Teknopark & İnovasyon", Latitude = 41.029, Longitude = 28.887, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "besiktas-iskele", Title = "Beşiktaş İskele & Çarşı Meydanı", Latitude = 41.042, Longitude = 29.006, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "forum-istanbul", Title = "Forum İstanbul AVM & IKEA", Latitude = 41.048, Longitude = 28.897, Category = LocationCategory.Sosyal },
                    }
                },

                // 5. İSTANBUL - İstanbul Üniversitesi
                new University
                {
                    Name = "İstanbul Üniversitesi",
                    City = "İstanbul",
                    Latitude = 41.013,
                    Longitude = 28.964,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "iu-beyazit", Title = "Beyazıt Merkez Kampüsü (Tarihi Kapı)", Latitude = 41.013, Longitude = 28.964, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "iu-cerrahpasa", Title = "Cerrahpaşa Tıp Yerleşkesi", Latitude = 41.005, Longitude = 28.941, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "iu-avcilar", Title = "Avcılar Yerleşkesi (Müh. & İşletme)", Latitude = 40.99, Longitude = 28.724, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "iu-hukuk-iktisat", Title = "Hukuk & İktisat Fakültesi Binası", Latitude = 41.0125, Longitude = 28.9635, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "vezneciler-kiz-kyk", Title = "Vezneciler KYK Kız Öğrenci Yurdu", Latitude = 41.0105, Longitude = 28.959, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "avcilar-erkek-kyk", Title = "Avcılar Erkek KYK Öğrenci Yurdu", Latitude = 40.991, Longitude = 28.725, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "kadirga-kyk-yurdu", Title = "Kadırga Erkek KYK Yurdu (Sultanahmet)", Latitude = 41.004, Longitude = 28.968, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "vezneciler-metro", Title = "Vezneciler Metro İstasyonu (M2)", Latitude = 41.012, Longitude = 28.96, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "beyazit-tramvay", Title = "Beyazıt-Kapalıçarşı Tramvay Durağı (T1)", Latitude = 41.01, Longitude = 28.965, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "avcilar-metrobus", Title = "Avcılar Kampüs Metrobüs Durağı", Latitude = 40.988, Longitude = 28.722, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kapalicarsi", Title = "Beyazıt Meydanı & Sahaflar Çarşısı", Latitude = 41.011, Longitude = 28.966, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "suleymaniye-kafeler", Title = "Süleymaniye Çay Bahçeleri & Kafeler", Latitude = 41.016, Longitude = 28.964, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sultanahmet-meydani", Title = "Tarihi Sultanahmet Meydanı", Latitude = 41.006, Longitude = 28.977, Category = LocationCategory.Sosyal },
                    }
                },

                // 6. ANKARA - Orta Doğu Teknik Üniversitesi
                new University
                {
                    Name = "Orta Doğu Teknik Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.8913,
                    Longitude = 32.7806,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "odtu-a1", Title = "ODTÜ A1 Girişi (Eskişehir Yolu)", Latitude = 39.8913, Longitude = 32.7806, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-kutuphane", Title = "ODTÜ Merkez Kütüphanesi", Latitude = 39.888, Longitude = 32.784, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-devrim", Title = "ODTÜ Devrim Stadyumu", Latitude = 39.885, Longitude = 32.78, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-carsi", Title = "ODTÜ Çarşı & Sosyal Bina", Latitude = 39.889, Longitude = 32.781, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "odtu-19-yurt", Title = "ODTÜ 19. Yurt Bölgesi", Latitude = 39.892, Longitude = 32.775, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "odtu-8-yurt", Title = "ODTÜ 8. ve 9. Yurtlar", Latitude = 39.893, Longitude = 32.779, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "faik-hiziroglu-kyk", Title = "Faik Hızıroğlu KYK Yurdu", Latitude = 39.888, Longitude = 32.772, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "odtu-metro", Title = "ODTÜ Metro İstasyonu (M2)", Latitude = 39.893, Longitude = 32.786, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "odtu-a2-kapisi", Title = "A2 Kapısı & 100. Yıl Minibüsleri", Latitude = 39.881, Longitude = 32.795, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "asti-otogar-ankara", Title = "AŞTİ Şehirlerarası Otobüs Terminali", Latitude = 39.917, Longitude = 32.8125, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "cepa-kentpark", Title = "CEPA & Kentpark AVM", Latitude = 39.91, Longitude = 32.778, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "yuzuncu-yil-carsi", Title = "100. Yıl İşçi Blokları & Kafeler", Latitude = 39.883, Longitude = 32.802, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "eymir-golu-giris", Title = "ODTÜ Ormanı & Eymir Girişi", Latitude = 39.832, Longitude = 32.825, Category = LocationCategory.Sosyal },
                    }
                },

                // 7. ANKARA - Hacettepe Üniversitesi
                new University
                {
                    Name = "Hacettepe Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.866,
                    Longitude = 32.735,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "hacettepe-beytepe", Title = "Beytepe Kampüsü (Ana Giriş)", Latitude = 39.866, Longitude = 32.735, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "hacettepe-sihhiye", Title = "Sıhhiye Merkez Kampüsü (Tıp)", Latitude = 39.932, Longitude = 32.862, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "beytepe-kutuphane", Title = "Beytepe Merkez Kütüphanesi", Latitude = 39.869, Longitude = 32.738, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "emine-serife-kyk", Title = "Emine Şerife Hanım KYK Kız Yurdu", Latitude = 39.861, Longitude = 32.732, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "beytepe-erkek-kyk", Title = "Beytepe Erkek KYK Öğrenci Yurdu", Latitude = 39.863, Longitude = 32.731, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "sihhiye-kyk-yurdu", Title = "Sıhhiye KYK Öğrenci Yurdu", Latitude = 39.934, Longitude = 32.86, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "beytepe-metro", Title = "Beytepe Metro İstasyonu & Ring Durağı", Latitude = 39.897, Longitude = 32.748, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "sihhiye-metro", Title = "Sıhhiye Metro & Başkentray İstasyonu", Latitude = 39.928, Longitude = 32.855, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "beytepe-city", Title = "Beytepe City Sosyal Tesisleri", Latitude = 39.871, Longitude = 32.74, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "arcadium-avm", Title = "Arcadium AVM & Çayyolu Kafeler", Latitude = 39.882, Longitude = 32.705, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kizilay-meydani", Title = "Kızılay Meydanı & Yüksel Caddesi", Latitude = 39.92, Longitude = 32.854, Category = LocationCategory.Sosyal },
                    }
                },

                // 8. ANKARA - Ankara Üniversitesi
                new University
                {
                    Name = "Ankara Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.9365,
                    Longitude = 32.831,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "ankara-tandogan", Title = "Tandoğan Kampüsü (Fen & Eczacılık)", Latitude = 39.9365, Longitude = 32.831, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ankara-cebeci", Title = "Cebeci Kampüsü (SBF, Hukuk & İletişim)", Latitude = 39.932, Longitude = 32.875, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ankara-tip-ibnisina", Title = "İbn-i Sina Araştırma Hastanesi", Latitude = 39.9315, Longitude = 32.864, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "cebeci-kiz-kyk", Title = "Cebeci KYK Kız Öğrenci Yurdu", Latitude = 39.931, Longitude = 32.879, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "baskent-erkek-kyk", Title = "Başkent Erkek KYK Öğrenci Yurdu", Latitude = 39.938, Longitude = 32.822, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "yildirim-beyazit-kyk", Title = "Yıldırım Beyazıt KYK Yurdu (Dışkapı)", Latitude = 39.945, Longitude = 32.865, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "besevler-metro", Title = "Beşevler Ankaray İstasyonu", Latitude = 39.933, Longitude = 32.827, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kurtulus-parki", Title = "Kurtuluş Parkı & Metro İstasyonu", Latitude = 39.928, Longitude = 32.868, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "dikimevi-ankaray", Title = "Dikimevi Ankaray Son Durak", Latitude = 39.93, Longitude = 32.881, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hamamonu", Title = "Tarihi Hamamönü Meydanı & Kafeler", Latitude = 39.934, Longitude = 32.865, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "tandogan-meydani", Title = "Anadolu (Tandoğan) Meydanı", Latitude = 39.9345, Longitude = 32.837, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "tunali-hilmi", Title = "Kuğulu Park & Tunalı Hilmi Caddesi", Latitude = 39.904, Longitude = 32.861, Category = LocationCategory.Sosyal },
                    }
                },

                // 9. İZMIR - Ege Üniversitesi
                new University
                {
                    Name = "Ege Üniversitesi",
                    City = "İzmir",
                    Latitude = 38.46,
                    Longitude = 27.228,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "ege-merkez", Title = "Ege Kampüs Merkez (Rektörlük & Müh.)", Latitude = 38.46, Longitude = 27.228, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ege-tip", Title = "Ege Üniversitesi Tıp Fakültesi Hastanesi", Latitude = 38.463, Longitude = 27.218, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ege-kutuphane", Title = "Ege Üniversitesi Merkez Kütüphanesi", Latitude = 38.461, Longitude = 27.229, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ege-ogrenci-koyu", Title = "Ege Üniversitesi Öğrenci Köyü Yurtları", Latitude = 38.455, Longitude = 27.234, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "zubeyde-hanim-kyk", Title = "Zübeyde Hanım KYK Kız Yurdu", Latitude = 38.465, Longitude = 27.212, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bornova-erkek-kyk", Title = "Bornova Erkek KYK Öğrenci Yurdu", Latitude = 38.455, Longitude = 27.218, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ege-metro", Title = "Ege Üniversitesi Metro İstasyonu", Latitude = 38.459, Longitude = 27.226, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bornova-metro", Title = "Bornova Metro & Aktarma Merkezi", Latitude = 38.462, Longitude = 27.214, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmir-otogar", Title = "İzmir Şehirlerarası Otobüs Terminali", Latitude = 38.432, Longitude = 27.208, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kucukpark", Title = "Küçükpark Bornova (Öğrenci Meydanı)", Latitude = 38.463, Longitude = 27.217, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "buyukpark", Title = "Büyükpark Meydanı & Açık Hava Tiyatrosu", Latitude = 38.466, Longitude = 27.219, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "forum-bornova", Title = "Forum Bornova AVM & IKEA", Latitude = 38.453, Longitude = 27.238, Category = LocationCategory.Sosyal },
                    }
                },

                // 10. İZMIR - Dokuz Eylül Üniversitesi
                new University
                {
                    Name = "Dokuz Eylül Üniversitesi",
                    City = "İzmir",
                    Latitude = 38.371,
                    Longitude = 27.199,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "deu-tinaztepe", Title = "Tınaztepe Merkez Kampüsü (Buca)", Latitude = 38.371, Longitude = 27.199, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "deu-dokuzcesmeler", Title = "Dokuzçeşmeler İİBF Kampüsü", Latitude = 38.388, Longitude = 27.172, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "deu-alsancak", Title = "Alsancak Tarihi Rektörlük", Latitude = 38.434, Longitude = 27.141, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "buca-kiz-kyk", Title = "Buca KYK Kız Öğrenci Yurdu", Latitude = 38.374, Longitude = 27.193, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "hoca-ahmet-yesevi-kyk", Title = "Hoca Ahmet Yesevi KYK Erkek Yurdu", Latitude = 38.378, Longitude = 27.189, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ataturk-inciralti-kyk", Title = "Atatürk İnciraltı KYK Yurdu", Latitude = 38.399, Longitude = 27.035, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "sirinyer-izban", Title = "Şirinyer İZBAN İstasyonu", Latitude = 38.397, Longitude = 27.159, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hilal-izban-metro", Title = "Hilal İZBAN & Metro Aktarma", Latitude = 38.421, Longitude = 27.158, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "hasanaga-bahcesi", Title = "Buca Hasanağa Bahçesi & Kafeler", Latitude = 38.384, Longitude = 27.178, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "konak-meydan", Title = "Konak Meydanı & Saat Kulesi", Latitude = 38.419, Longitude = 27.1285, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "alsancak-kordon", Title = "Alsancak Kordon & Kıbrıs Şehitleri", Latitude = 38.436, Longitude = 27.14, Category = LocationCategory.Sosyal },
                    }
                },

                // 11. ESKIŞEHIR - Anadolu Üniversitesi
                new University
                {
                    Name = "Anadolu Üniversitesi",
                    City = "Eskişehir",
                    Latitude = 39.7915,
                    Longitude = 30.5005,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "anadolu-yunus-emre", Title = "Yunus Emre Kampüsü (Ana Giriş)", Latitude = 39.7915, Longitude = 30.5005, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "anadolu-iki-eylul", Title = "İki Eylül Kampüsü (Havacılık & Müh.)", Latitude = 39.814, Longitude = 30.528, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "anadolu-kutuphane", Title = "Öğrenci Merkezi & 7/24 Kütüphane", Latitude = 39.793, Longitude = 30.503, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "yunus-emre-kyk", Title = "Yunus Emre KYK Yurdu", Latitude = 39.789, Longitude = 30.498, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "cumhuriyet-kyk", Title = "Cumhuriyet KYK Kız Yurdu", Latitude = 39.792, Longitude = 30.508, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "dogan-aslan-kyk", Title = "Doğan Aslan Bey KYK Yurdu", Latitude = 39.795, Longitude = 30.505, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "anadolu-tramvay", Title = "Anadolu Üniversitesi Tramvay Durağı", Latitude = 39.79, Longitude = 30.501, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "eskisehir-gari", Title = "Eskişehir YHT Garı", Latitude = 39.779, Longitude = 30.514, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "eskisehir-otogar", Title = "Eskişehir Şehirlerarası Otogar", Latitude = 39.771, Longitude = 30.573, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "espark-avm", Title = "Espark AVM & Üniversite Caddesi", Latitude = 39.786, Longitude = 30.506, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "baglar-caddesi", Title = "Bağlar Öğrenci Kafeleri", Latitude = 39.788, Longitude = 30.504, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "adalar-porsuk", Title = "Adalar & Porsuk Çayı Çevresi", Latitude = 39.775, Longitude = 30.518, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "haller-genclik", Title = "Haller Gençlik Merkezi", Latitude = 39.781, Longitude = 30.512, Category = LocationCategory.Sosyal },
                    }
                },

                // 12. ESKIŞEHIR - Eskişehir Osmangazi Üniversitesi
                new University
                {
                    Name = "Eskişehir Osmangazi Üniversitesi",
                    City = "Eskişehir",
                    Latitude = 39.7505,
                    Longitude = 30.484,
                    DefaultZoom = 14,
                    EmailDomain = "gazi.edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "esogu-meselik", Title = "Meşelik Kampüsü (Ana Giriş & Rektörlük)", Latitude = 39.7505, Longitude = 30.484, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "esogu-tip", Title = "ESOGÜ Tıp Fakültesi Hastanesi", Latitude = 39.753, Longitude = 30.486, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "dumlupinar-kyk", Title = "Dumlupınar KYK Öğrenci Yurdu", Latitude = 39.756, Longitude = 30.491, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bala-hatun-kyk", Title = "Bala Hatun KYK Kız Yurdu", Latitude = 39.754, Longitude = 30.495, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "yunus-emre-erkek-kyk", Title = "Yunus Emre KYK Erkek Yurdu (Meşelik)", Latitude = 39.758, Longitude = 30.488, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "esogu-tramvay", Title = "ESOGÜ Tramvay Son Durak & Aktarma", Latitude = 39.749, Longitude = 30.483, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "meselik-otobus", Title = "Meşelik Kampüs Otobüs Peronları", Latitude = 39.748, Longitude = 30.485, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "sazova-parki", Title = "Sazova Bilim, Sanat ve Kültür Parkı", Latitude = 39.768, Longitude = 30.472, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "hamamyolu", Title = "Hamamyolu Caddesi & Odunpazarı", Latitude = 39.771, Longitude = 30.521, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "odunpazari-evleri", Title = "Tarihi Odunpazarı Evleri & Meydanı", Latitude = 39.76, Longitude = 30.525, Category = LocationCategory.Sosyal },
                    }
                },

                // 13. ANTALYA - Akdeniz Üniversitesi
                new University
                {
                    Name = "Akdeniz Üniversitesi",
                    City = "Antalya",
                    Latitude = 36.8965,
                    Longitude = 30.655,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "akdeniz-meltem", Title = "Akdeniz Kampüs (Meltem Kapısı)", Latitude = 36.8965, Longitude = 30.655, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "akdeniz-hastane", Title = "Akdeniz Üniversitesi Tıp Hastanesi", Latitude = 36.891, Longitude = 30.652, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "akdeniz-kutuphane", Title = "Akdeniz Üniversitesi Merkez Kütüphane", Latitude = 36.898, Longitude = 30.658, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "elmalili-hamdi-kyk", Title = "Elmalılı Hamdi Yazır KYK Yurdu", Latitude = 36.893, Longitude = 30.665, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "akdeniz-ogrenci-yurdu", Title = "Akdeniz Öğrenci Yurtları Kompleksi", Latitude = 36.895, Longitude = 30.661, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "muratpasa-kiz-kyk", Title = "Muratpaşa KYK Kız Yurdu", Latitude = 36.892, Longitude = 30.662, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "antray-universite", Title = "Antray Akdeniz Üniversitesi Tramvayı", Latitude = 36.899, Longitude = 30.667, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "antalya-otogar", Title = "Antalya Şehirlerarası Otobüs Terminali", Latitude = 36.921, Longitude = 30.664, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "konyaalti-sahil", Title = "Konyaaltı Sahili & Kent Meydanı", Latitude = 36.879, Longitude = 30.648, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "5m-migros", Title = "5M Migros AVM & Aktur Park", Latitude = 36.884, Longitude = 30.658, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kaleici", Title = "Kaleiçi Tarihi Çarşı & Yat Limanı", Latitude = 36.886, Longitude = 30.706, Category = LocationCategory.Sosyal },
                    }
                },

                // 14. BURSA - Bursa Uludağ Üniversitesi
                new University
                {
                    Name = "Bursa Uludağ Üniversitesi",
                    City = "Bursa",
                    Latitude = 40.224,
                    Longitude = 28.871,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "uludag-gorukle", Title = "Görükle Merkez Kampüsü (Rektörlük)", Latitude = 40.224, Longitude = 28.871, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "uludag-tip", Title = "Uludağ Üniversitesi Tıp Fakültesi", Latitude = 40.228, Longitude = 28.868, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "uludag-kutuphane", Title = "Uludağ Merkez Kütüphanesi", Latitude = 40.225, Longitude = 28.873, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "halime-hatun-kyk", Title = "Halime Hatun KYK Kız Yurdu", Latitude = 40.221, Longitude = 28.874, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "emir-sultan-kyk", Title = "Emir Sultan KYK Erkek Yurdu (Görükle)", Latitude = 40.229, Longitude = 28.865, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "uludag-kiz-kyk", Title = "Uludağ KYK Kız Yurdu Kompleksi", Latitude = 40.226, Longitude = 28.875, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bursaray-universite", Title = "Bursaray Üniversite İstasyonu (Son Durak)", Latitude = 40.223, Longitude = 28.875, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bursa-otogar", Title = "Bursa Şehirlerarası Otobüs Terminali", Latitude = 40.264, Longitude = 29.055, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "gorukle-trio", Title = "Görükle Trio Meydanı & Öğrenci Kafeleri", Latitude = 40.227, Longitude = 28.852, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "gorukle-yerlesim", Title = "Görükle Yerleşim Çarşısı", Latitude = 40.229, Longitude = 28.856, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "sur-yapi-marka", Title = "Sur Yapı Marka AVM & Nilüfer Park", Latitude = 40.208, Longitude = 28.988, Category = LocationCategory.Sosyal },
                    }
                },

                // 15. KONYA - Selçuk Üniversitesi
                new University
                {
                    Name = "Selçuk Üniversitesi",
                    City = "Konya",
                    Latitude = 38.026,
                    Longitude = 32.511,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "selcuk-alaeddin", Title = "Alaeddin Keykubat Kampüsü (Ana Kapı)", Latitude = 38.026, Longitude = 32.511, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "selcuk-tip", Title = "Selçuk Üniversitesi Tıp Fakültesi Hastanesi", Latitude = 38.031, Longitude = 32.514, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "selcuk-kutuphane", Title = "Erol Güngör Merkez Kütüphanesi", Latitude = 38.028, Longitude = 32.509, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "alaeddin-kyk", Title = "Alaeddin KYK Öğrenci Yurdu", Latitude = 38.023, Longitude = 32.516, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "kutalmisoglu-kyk", Title = "Kutalmışoğlu KYK Erkek Yurdu (Bosna)", Latitude = 38.019, Longitude = 32.518, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "fahrunnisa-kyk", Title = "Fahrunnisa KYK Kız Yurdu", Latitude = 38.024, Longitude = 32.508, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "kampus-tramvay", Title = "Kampüs Tramvay Son Durak", Latitude = 38.025, Longitude = 32.512, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "konya-otogar", Title = "Konya Şehirlerarası Otobüs Terminali", Latitude = 37.951, Longitude = 32.508, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "konya-yht-gari", Title = "Konya YHT Hızlı Tren Garı", Latitude = 37.868, Longitude = 32.482, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bosna-hersek-carsi", Title = "Bosna Hersek Mahallesi Öğrenci Çarşısı", Latitude = 38.016, Longitude = 32.522, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "m1-konya", Title = "M1 Konya AVM & Real", Latitude = 37.955, Longitude = 32.512, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "zafer-meydani-konya", Title = "Zafer Meydanı & Alaaddin Tepesi", Latitude = 37.873, Longitude = 32.493, Category = LocationCategory.Sosyal },
                    }
                },

                // 16. KOCAELI - Kocaeli Üniversitesi
                new University
                {
                    Name = "Kocaeli Üniversitesi",
                    City = "Kocaeli",
                    Latitude = 40.821,
                    Longitude = 29.923,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "kou-umuttepe", Title = "Umuttepe Merkez Kampüsü (Rektörlük)", Latitude = 40.821, Longitude = 29.923, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "kou-hastane", Title = "KOÜ Araştırma ve Uygulama Hastanesi", Latitude = 40.824, Longitude = 29.921, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "kou-kutuphane", Title = "Umuttepe Merkez Kütüphanesi", Latitude = 40.822, Longitude = 29.925, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gazi-suleyman-kyk", Title = "Gazi Süleyman Paşa KYK Erkek Yurdu", Latitude = 40.816, Longitude = 29.928, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "samiha-ayverdi-kyk", Title = "Samiha Ayverdi KYK Kız Yurdu", Latitude = 40.825, Longitude = 29.918, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ali-fuat-cebesoy-kyk", Title = "Ali Fuat Cebesoy KYK Yurdu", Latitude = 40.818, Longitude = 29.929, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "umuttepe-otobus", Title = "Umuttepe Otobüs Peronları (Son Durak)", Latitude = 40.82, Longitude = 29.924, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmit-otogar", Title = "İzmit Şehirlerarası Otobüs Terminali", Latitude = 40.774, Longitude = 29.965, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmit-yht-gari", Title = "İzmit Tren Garı (YHT Durağı)", Latitude = 40.762, Longitude = 29.918, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "izmit-yuruyus-yolu", Title = "İzmit Tarihi Yürüyüş Yolu & Kafeler", Latitude = 40.765, Longitude = 29.928, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "symbol-avm", Title = "Symbol Kocaeli AVM & Medical Park", Latitude = 40.762, Longitude = 29.972, Category = LocationCategory.Sosyal },
                    }
                },

                // 17. SAKARYA - Sakarya Üniversitesi
                new University
                {
                    Name = "Sakarya Üniversitesi",
                    City = "Sakarya",
                    Latitude = 40.741,
                    Longitude = 30.334,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "sau-esentepe", Title = "Esentepe Kampüsü (Ana Giriş & Rektörlük)", Latitude = 40.741, Longitude = 30.334, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "sau-kutuphane", Title = "SAÜ Merkez Kütüphanesi", Latitude = 40.7425, Longitude = 30.3355, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "sabahattin-zaim-kyk", Title = "Sabahattin Zaim KYK Yurdu", Latitude = 40.738, Longitude = 30.338, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ayse-humeyra-kyk", Title = "Ayşe Hümeyra Ökten KYK Kız Yurdu", Latitude = 40.738, Longitude = 30.339, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "rahime-sultan-kyk", Title = "Rahime Sultan KYK Yurdu", Latitude = 40.745, Longitude = 30.331, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "kampus-otobus-sau", Title = "Kampüs Otobüs & Minibüs Durakları", Latitude = 40.739, Longitude = 30.336, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "sakarya-otogar", Title = "Sakarya Şehirlerarası Otobüs Terminali", Latitude = 40.718, Longitude = 30.402, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "mavi-durak", Title = "Serdivan Mavi Durak (Öğrenci Kafeleri)", Latitude = 40.762, Longitude = 30.365, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "serdivan-avm", Title = "Serdivan AVM & Cadde54", Latitude = 40.765, Longitude = 30.362, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "cark-caddesi", Title = "Adapazarı Çark Caddesi Meydanı", Latitude = 40.772, Longitude = 30.395, Category = LocationCategory.Sosyal },
                    }
                },

                // 18. TRABZON - Karadeniz Teknik Üniversitesi
                new University
                {
                    Name = "Karadeniz Teknik Üniversitesi",
                    City = "Trabzon",
                    Latitude = 40.995,
                    Longitude = 39.771,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "ktu-kanuni", Title = "Kanuni Kampüsü (Ana Giriş & Rektörlük)", Latitude = 40.995, Longitude = 39.771, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ktu-farabi", Title = "KTÜ Farabi Hastanesi", Latitude = 40.997, Longitude = 39.774, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "ktu-kutuphane", Title = "Faik Ahmet Barutçu Kütüphanesi", Latitude = 40.996, Longitude = 39.772, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "dogu-karadeniz-kyk", Title = "Doğu Karadeniz KYK Yurdu", Latitude = 40.993, Longitude = 39.778, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "yomra-kanuni-kyk", Title = "Yomra Kanuni KYK Kız Yurdu", Latitude = 40.958, Longitude = 39.845, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "trabzon-merkez-kyk", Title = "Trabzon Merkez Erkek KYK Yurdu", Latitude = 40.998, Longitude = 39.765, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "ktu-dolmus", Title = "KTÜ Dolmuş & Otobüs Durakları", Latitude = 40.994, Longitude = 39.769, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "trabzon-otogar", Title = "Trabzon Şehirlerarası Otobüs Terminali", Latitude = 40.996, Longitude = 39.748, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "trabzon-havalimani", Title = "Trabzon Havalimanı (Kampüs Yanı)", Latitude = 40.995, Longitude = 39.789, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "forum-trabzon", Title = "Forum Trabzon AVM", Latitude = 40.998, Longitude = 39.761, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "meydan-parki-trabzon", Title = "Trabzon Meydan Parkı (Atatürk Alanı)", Latitude = 41.003, Longitude = 39.728, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "boztepe-trabzon", Title = "Boztepe Çay Bahçesi & Seyir Terası", Latitude = 40.998, Longitude = 39.731, Category = LocationCategory.Sosyal },
                    }
                },

                // 19. ANKARA - Gazi Üniversitesi
                new University
                {
                    Name = "Gazi Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.9392,
                    Longitude = 32.8228,
                    DefaultZoom = 14,
                    EmailDomain = "gazi.edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "gazi-merkez", Title = "Gazi Merkez Kampüs (Beşevler / Rektörlük)", Latitude = 39.9392, Longitude = 32.8228, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gazi-muhendislik", Title = "Gazi Mühendislik Fakültesi (Maltepe)", Latitude = 39.928, Longitude = 32.845, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gazi-teknoloji", Title = "Teknoloji Fakültesi Yerleşkesi", Latitude = 39.938, Longitude = 32.824, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gazi-hastane", Title = "Gazi Üniversitesi Tıp Fakültesi Hastanesi", Latitude = 39.934, Longitude = 32.821, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gazi-kyk-erkek", Title = "Gazi KYK Erkek Öğrenci Yurdu", Latitude = 39.941, Longitude = 32.825, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "serife-baci-kyk", Title = "Şerife Bacı KYK Kız Yurdu", Latitude = 39.937, Longitude = 32.821, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "mahmut-nedim-kyk", Title = "Mahmut Nedim Zabcı KYK Yurdu", Latitude = 39.9405, Longitude = 32.828, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "beseveler-gazi-metro", Title = "Beşevler Ankaray İstasyonu (Gazi Çıkışı)", Latitude = 39.933, Longitude = 32.827, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "maltepe-gazi-metro", Title = "Maltepe Ankaray İstasyonu", Latitude = 39.9275, Longitude = 32.8465, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "asti-gazi-ulasim", Title = "AŞTİ Otobüs Terminali (5 dk Mesafe)", Latitude = 39.917, Longitude = 32.8125, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bahcelievler-7-cadde", Title = "Bahçelievler 7. Cadde (Öğrenci Kafeleri)", Latitude = 39.92, Longitude = 32.8235, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "anitkabir-parki", Title = "Anıtkabir Barış Parkı & Yürüyüş Yolu", Latitude = 39.925, Longitude = 32.836, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "armada-avm", Title = "Armada AVM & Yaşam Sokağı", Latitude = 39.913, Longitude = 32.809, Category = LocationCategory.Sosyal },
                    }
                },

                // 20. İSTANBUL - Marmara Üniversitesi
                new University
                {
                    Name = "Marmara Üniversitesi",
                    City = "İstanbul",
                    Latitude = 40.985,
                    Longitude = 29.055,
                    DefaultZoom = 14,
                    EmailDomain = "marmara.edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "marmara-kulliye", Title = "Recep Tayyip Erdoğan Külliyesi (Maltepe Ana Kampüs)", Latitude = 40.954, Longitude = 29.142, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "marmara-goztepe", Title = "Göztepe Kampüsü (Kadıköy)", Latitude = 40.985, Longitude = 29.055, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "marmara-haydarpasa", Title = "Tarihi Haydarpaşa Yerleşkesi", Latitude = 40.998, Longitude = 29.022, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "mimar-sinan-kyk-maltepe", Title = "Mimar Sinan KYK Yurdu (Maltepe)", Latitude = 40.952, Longitude = 29.141, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "handan-ziya-kyk", Title = "Handan Ziya Öniş KYK Kız Yurdu", Latitude = 40.982, Longitude = 29.058, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "maltepe-kiz-kyk", Title = "Maltepe KYK Kız Yurdu", Latitude = 40.948, Longitude = 29.135, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "kadikoy-metro-iskele", Title = "Kadıköy Metro & Şehir Hatları İskelesi", Latitude = 40.991, Longitude = 29.023, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "goztepe-marmaray", Title = "Göztepe Marmaray İstasyonu", Latitude = 40.978, Longitude = 29.057, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "sogutlucesme-metrobus", Title = "Söğütlüçeşme Metrobüs & YHT İstasyonu", Latitude = 40.99, Longitude = 29.038, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kadikoy-moda-sahil", Title = "Moda Sahili & Kadıköy Çarşısı", Latitude = 40.983, Longitude = 29.027, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "bagdat-caddesi-marmara", Title = "Bağdat Caddesi & Erenköy", Latitude = 40.963, Longitude = 29.072, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "piazza-maltepe", Title = "Maltepe Piazza & Ritim AVM", Latitude = 40.932, Longitude = 29.155, Category = LocationCategory.Sosyal },
                    }
                },

                // 21. ADANA - Çukurova Üniversitesi
                new University
                {
                    Name = "Çukurova Üniversitesi",
                    City = "Adana",
                    Latitude = 37.0545,
                    Longitude = 35.3525,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "cukurova-balcali", Title = "Balcalı Merkez Kampüsü (Rektörlük)", Latitude = 37.0545, Longitude = 35.3525, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "cukurova-tip-balcali", Title = "Balcalı Tıp Fakültesi Hastanesi", Latitude = 37.056, Longitude = 35.356, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "mithat-ozsan-amfi", Title = "Mithat Özsan Amfisi & Kütüphane", Latitude = 37.0535, Longitude = 35.351, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "fevzi-cakmak-kyk-adana", Title = "Fevzi Çakmak KYK Erkek Yurdu", Latitude = 37.051, Longitude = 35.358, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "cukurova-kiz-kyk", Title = "Çukurova KYK Kız Öğrenci Yurdu", Latitude = 37.058, Longitude = 35.349, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "toroslar-kyk-adana", Title = "Toroslar KYK Yurdu (Balcalı)", Latitude = 37.049, Longitude = 35.361, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "adana-otogar", Title = "Adana Şehirlerarası Otobüs Terminali", Latitude = 36.985, Longitude = 35.265, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "adana-tren-gari", Title = "Tarihi Adana Tren Garı", Latitude = 36.999, Longitude = 35.321, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "balcali-otobus-ring", Title = "Balcalı Otobüs Ring & Aktarma Alanı", Latitude = 37.053, Longitude = 35.351, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "barajyolu-caddesi", Title = "Barajyolu Gençlik Meydanı & Kafeler", Latitude = 37.025, Longitude = 35.317, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "seyhan-baraj-golu", Title = "Seyhan Baraj Gölü Sahili & Çamlık", Latitude = 37.045, Longitude = 35.335, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "m1-adana-avm", Title = "M1 Adana AVM & Real", Latitude = 36.992, Longitude = 35.258, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "ataturk-parki-adana", Title = "Atatürk Parkı & Gazipaşa Bulvarı", Latitude = 36.997, Longitude = 35.324, Category = LocationCategory.Sosyal },
                    }
                },

                // 22. SAMSUN - Ondokuz Mayıs Üniversitesi
                new University
                {
                    Name = "Ondokuz Mayıs Üniversitesi",
                    City = "Samsun",
                    Latitude = 41.3665,
                    Longitude = 36.1925,
                    DefaultZoom = 14,
                    EmailDomain = "edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "omu-kurupelit", Title = "Kurupelit Merkez Kampüsü (Rektörlük)", Latitude = 41.3665, Longitude = 36.1925, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "omu-tip-hastanesi", Title = "OMÜ Tıp Fakültesi Hastanesi", Latitude = 41.369, Longitude = 36.195, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "omu-merkez-kutuphane", Title = "OMÜ Merkez Kütüphanesi", Latitude = 41.365, Longitude = 36.191, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "karadeniz-kyk-samsun", Title = "Karadeniz KYK Erkek Yurdu", Latitude = 41.362, Longitude = 36.198, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "munevver-ayasli-kyk", Title = "Münevver Ayaşlı KYK Kız Yurdu", Latitude = 41.359, Longitude = 36.205, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "atakum-kyk-yurdu", Title = "Atakum KYK Öğrenci Yurdu", Latitude = 41.341, Longitude = 36.255, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "omu-tramvay-durak", Title = "OMÜ Tramvay Son Durak & Aktarma", Latitude = 41.368, Longitude = 36.189, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "samsun-otogar", Title = "Samsun Yusuf Ziya Yılmaz Şehirlerarası Otogar", Latitude = 41.272, Longitude = 36.315, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "samsun-tren-gari", Title = "Samsun Tarihi Tren Garı", Latitude = 41.288, Longitude = 36.341, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "atakum-sahil-kafeler", Title = "Atakum Sahil Şeridi & Öğrenci Kafeleri", Latitude = 41.332, Longitude = 36.275, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "citymall-samsun", Title = "CityMall AVM Atakum", Latitude = 41.328, Longitude = 36.282, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "piazza-samsun", Title = "Piazza AVM & Doğu Park", Latitude = 41.284, Longitude = 36.355, Category = LocationCategory.Sosyal },
                    }
                },

                // 23. KAYSERI - Erciyes Üniversitesi
                new University
                {
                    Name = "Erciyes Üniversitesi",
                    City = "Kayseri",
                    Latitude = 38.705,
                    Longitude = 35.528,
                    DefaultZoom = 14,
                    EmailDomain = "erciyes.edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "erces-merkez", Title = "Merkez Kampüs (Talas Yolu / Rektörlük)", Latitude = 38.705, Longitude = 35.528, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "erces-gevher-nesibe", Title = "Gevher Nesibe Tıp Fakültesi Hastanesi", Latitude = 38.703, Longitude = 35.525, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "erces-kutuphane", Title = "Sabancı Kültür Sitesi & Kütüphane", Latitude = 38.706, Longitude = 35.531, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "gevher-nesibe-kiz-kyk", Title = "Gevher Nesibe KYK Kız Yurdu", Latitude = 38.702, Longitude = 35.534, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "melikgazi-erkek-kyk", Title = "Melikgazi KYK Erkek Öğrenci Yurdu", Latitude = 38.709, Longitude = 35.522, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "seyyid-burhaneddin-kyk", Title = "Seyyid Burhaneddin KYK Yurdu", Latitude = 38.712, Longitude = 35.518, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "eru-tramvay", Title = "ERÜ Tramvay İstasyonu", Latitude = 38.707, Longitude = 35.526, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kayseri-otogar", Title = "Kayseri Şehirlerarası Otobüs Terminali", Latitude = 38.742, Longitude = 35.438, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "kayseri-gari", Title = "Kayseri Tren Garı", Latitude = 38.729, Longitude = 35.476, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "talas-anayurt-kafeler", Title = "Talas Anayurt Öğrenci Kafeleri", Latitude = 38.692, Longitude = 35.552, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "kayseri-park-avm", Title = "Kayseri Park AVM", Latitude = 38.721, Longitude = 35.509, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "cumhuriyet-meydani-kayseri", Title = "Cumhuriyet Meydanı & Kayseri Kalesi", Latitude = 38.722, Longitude = 35.487, Category = LocationCategory.Sosyal },
                    }
                },

                // 24. ANKARA - Bilkent Üniversitesi
                new University
                {
                    Name = "Bilkent Üniversitesi",
                    City = "Ankara",
                    Latitude = 39.873,
                    Longitude = 32.748,
                    DefaultZoom = 14,
                    EmailDomain = "bilkent.edu.tr",
                    IletisimEmail = "info@edu.tr",
                    Locations = new List<CampusLocation>
                    {
                        new CampusLocation { LocationKey = "bilkent-merkez-kampus", Title = "Bilkent Merkez Kampüs (Rektörlük)", Latitude = 39.873, Longitude = 32.748, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bilkent-dogu-kampus", Title = "Bilkent Doğu Kampüsü", Latitude = 39.877, Longitude = 32.756, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bilkent-kutuphane", Title = "Bilkent Merkez Kütüphanesi", Latitude = 39.8705, Longitude = 32.747, Category = LocationCategory.Kampus },
                        new CampusLocation { LocationKey = "bilkent-yurtlar", Title = "Bilkent Öğrenci Yurtları Kompleksi", Latitude = 39.871, Longitude = 32.744, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bilkent-76-yurt", Title = "76. Yurt Bölgesi", Latitude = 39.874, Longitude = 32.741, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "odtu-bilkent-kyk", Title = "ODTÜ-Bilkent KYK Öğrenci Yurdu", Latitude = 39.882, Longitude = 32.761, Category = LocationCategory.Yurt },
                        new CampusLocation { LocationKey = "bilkent-metro-istasyonu", Title = "Bilkent Metro İstasyonu & Ring Durağı", Latitude = 39.905, Longitude = 32.756, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "eskisehir-yolu-aktarma", Title = "Eskişehir Yolu Otobüs Aktarma Noktası", Latitude = 39.907, Longitude = 32.758, Category = LocationCategory.Ulasim },
                        new CampusLocation { LocationKey = "bilkent-center", Title = "Bilkent Center & Ankuva AVM", Latitude = 39.886, Longitude = 32.753, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "tepe-prime", Title = "Tepe Prime Avenue Kafeler & Restoranlar", Latitude = 39.908, Longitude = 32.763, Category = LocationCategory.Sosyal },
                        new CampusLocation { LocationKey = "maidan-carsi", Title = "Maidan İş ve Yaşam Meydanı", Latitude = 39.911, Longitude = 32.765, Category = LocationCategory.Sosyal },
                    }
                },

            };
        }
    }
}
