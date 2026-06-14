# Kampus Rota API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Minimal%20API-5C2D91)](https://learn.microsoft.com/aspnet/core)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)

Kampus Rota API, kampus ici yolculuk paylasimi icin gelistirilmis ASP.NET Core tabanli bir backend projesidir. Kullanici yonetimi, yolculuk ilani olusturma, yolculuk arama, katilim talebi, talep onay/red akisi ve yolculuk sonrasi puanlama gibi temel islevleri sunar.

## Ozellikler

- Kullanici kayit, giris, profil guncelleme, sifre degistirme ve hesap silme
- Yolculuk ilani listeleme, arama, olusturma, guncelleme ve silme
- Yolculuga katilim talebi olusturma
- Surucu tarafinda gelen talepleri onaylama veya reddetme
- Yolcu ve surucu icin yolculuk talebi gecmisi
- Yolculuk sonrasi puan ve yorum ekleme
- Entity Framework Core Code First migration yapisi
- Swagger/OpenAPI arayuzu

## Teknolojiler

- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core 8
- SQL Server
- Swagger / Swashbuckle

## Proje Yapisi

```text
KampusRotaAPI/
├── Data/              # Entity Framework DbContext
├── Migrations/        # EF Core migration dosyalari
├── Models/            # Domain modelleri
├── Services/          # Is kurallari ve servis katmani
├── Program.cs         # API endpoint tanimlari
└── appsettings.json   # Temel uygulama ayarlari
```

## Kurulum

Gereksinimler:

- .NET 8 SDK
- SQL Server veya SQL Server Express
- Visual Studio 2022, Rider veya VS Code

Projeyi klonlayin:

```bash
git clone https://github.com/MehmetKocyigit1/KampusRotaAPI.git
cd KampusRotaAPI
```

Bagimliliklari yukleyin:

```bash
dotnet restore
```

Veritabani baglanti adresini `appsettings.json` icinde kendi ortaminiza gore duzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=KampusRotaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Migration'lari veritabanina uygulayin:

```bash
dotnet ef database update
```

Uygulamayi calistirin:

```bash
dotnet run
```

Varsayilan gelistirme ortaminda Swagger arayuzu su adreste acilir:

```text
https://localhost:7107/swagger
```

## Temel Endpointler

| Metot | Endpoint | Aciklama |
| --- | --- | --- |
| `GET` | `/api/rides` | Tum yolculuk ilanlarini listeler |
| `GET` | `/api/rides/search` | Kalkis, varis ve tarihe gore arama yapar |
| `POST` | `/api/rides` | Yeni yolculuk ilani olusturur |
| `PUT` | `/api/rides/{id}` | Yolculuk ilanini gunceller |
| `DELETE` | `/api/rides/{id}` | Yolculuk ilanini siler |
| `POST` | `/api/rides/{id}/requests` | Yolculuga katilim talebi gonderir |
| `GET` | `/api/rides/requests/driver/{surucuId}` | Surucuye gelen talepleri listeler |
| `GET` | `/api/rides/requests/passenger/{yolcuId}` | Yolcunun taleplerini listeler |
| `PUT` | `/api/rides/requests/{talepId}/status` | Talep durumunu onaylar veya reddeder |
| `POST` | `/api/rides/{id}/reviews` | Yolculuk yorumu ve puani ekler |
| `POST` | `/api/users/register` | Kullanici kaydi olusturur |
| `POST` | `/api/users/login` | Kullanici girisi yapar |
| `GET` | `/api/users/{id}` | Kullanici profilini getirir |
| `PUT` | `/api/users/{id}` | Kullanici profilini gunceller |
| `PUT` | `/api/users/{id}/change-password` | Kullanici sifresini degistirir |
| `DELETE` | `/api/users/{id}` | Kullanici hesabini siler |

## Kalite Kontrol

Build kontrolu:

```bash
dotnet build
```

Format kontrolu:

```bash
dotnet format --verify-no-changes
```

## Ilgili Repo

Mobil istemci uygulamasi: [KampusRotaUI](https://github.com/MehmetKocyigit1/KampusRotaUI)

## Notlar

- Bu proje egitim ve portfolyo amacli gelistirilmistir.
- Yerel calisma icin SQL Server baglanti adresi gelistirici ortaminda guncellenmelidir.
- Uretim ortamina cikmadan once kimlik dogrulama, sifre hashleme ve ortam bazli gizli ayar yonetimi guclendirilmelidir.
