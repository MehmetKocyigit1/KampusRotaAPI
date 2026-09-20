# Kampus Rota API (Enterprise Clean Architecture)

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-5C2D91)](https://learn.microsoft.com/aspnet/core)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![Map](https://img.shields.io/badge/Map-OpenStreetMap%20%7C%20MapLibre-007AFF)](https://maplibre.org/)
[![Tests](https://img.shields.io/badge/Tests-xUnit%20100%25%20Passing-brightgreen)](https://xunit.net/)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue?logo=docker)](https://www.docker.com/)

**Kampus Rota**, üniversite kampüsleri ve şehir merkezleri arasında güvenli, ekonomik ve sürdürülebilir yolculuk paylaşımı (carpooling) sağlayan bir backend platformudur.

Proje, kurumsal ölçeklenebilirlik standartlarına uygun olarak **Clean Architecture (Onion Architecture)** prensipleriyle yapılandırılmış, test odaklı (TDD) ve çoklu üniversite (multi-campus) destekli olarak geliştirilmiştir.

---

## 🏛️ Mimari Yapı (Clean Architecture)

Proje katmanları sorumlulukların ayrımı (Separation of Concerns) ve SOLID prensiplerine göre organize edilmiştir:

```text
KampusRotaAPI/
├── src/
│   ├── KampusRota.Domain/          # Çekirdek iş varlıkları (University, CampusLocation, Yolculuk, Kullanici)
│   ├── KampusRota.Application/     # Servis arayüzleri, DTO'lar, ServiceResult, validasyonlar
│   ├── KampusRota.Infrastructure/  # EF Core AppDbContext, DbInitializer (Seed Data), Servis implementasyonları
│   └── KampusRota.API/             # Web API Endpoint'leri, GlobalExceptionMiddleware, Swagger
├── tests/
│   └── KampusRota.UnitTests/       # xUnit, FluentAssertions ve Moq ile iş kuralı testleri
├── Dockerfile                      # Çok aşamalı (multi-stage) Docker imajı
└── docker-compose.yml              # SQL Server + API tek komutla çalıştırma
```

---

## 🚀 Öne Çıkan Özellikler

- **Çoklu Üniversite & Dinamik Harita (Multi-Campus OSM):**
  - Isparta kısıtı kaldırılmış, **tüm Türkiye genelinde** harita desteği.
  - Açık kaynak ve **%100 ücretsiz MapLibre GL + OpenStreetMap** altyapısı (kredi kartı veya API anahtarı gerekmez).
  - Üniversiteler (`/api/universities`) ve her üniversiteye ait duraklar/kampüs noktaları (`/api/universities/{id}/locations`) dinamik olarak yüklenir.
- **Akıllı E-posta Eşleştirme:**
  - Öğrenci kayıt esnasında e-postasını girdiğinde (`@itu.edu.tr`, `@sdu.edu.tr`, `@metu.edu.tr`), sistem otomatik olarak kullanıcının üniversitesini tespit eder ve haritayı o kampüse odaklar.
- **Güvenli & Esnek Yolculuk Paylaşımı:**
  - İlan oluşturma, arama, filtreleme, katılım talepleri ve sürücü onay/red mekanizması.
  - Sadece kadınlara özel yolculuk seçeneği ve yetki doğrulaması.
  - Yolculuk sonrası karşılıklı puanlama ve yorum sistemi.
- **Global Exception Handling (RFC 7807):**
  - Tüm hatalar standart `application/problem+json` formatında döndürülür.
- **Otomatik Veritabanı Başlatma & Seed Data:**
  - Popüler üniversiteler (SDU, İTÜ, ODTÜ, Boğaziçi, YTÜ, Ege vb.) koordinatları ve popüler kampüs duraklarıyla otomatik olarak veritabanına eklenir.

---

## 🧪 Testler (Unit Tests)

Proje içerisinde iş kurallarını garanti altına alan birim testleri mevcuttur:

```bash
dotnet test tests/KampusRota.UnitTests/KampusRota.UnitTests.csproj
```

**Kapsanan Senaryolar:**
- Sürücünün kendi ilanına başvuru yapamaması
- Kontenjan dolduğunda rezervasyonun engellenmesi
- Talep onaylandığında boş koltuk sayısının otomatik azaltılması
- Geçmiş tarihli veya kalkış-varış noktası aynı olan ilanların engellenmesi
- Puanlama sınırları (1-5) ve kullanıcının kendini puanlayamaması
- `@edu.tr` uzantılı e-postaların doğru üniversite ile eşleştirilmesi

---

## 🛠️ Kurulum & Çalıştırma

### 1. Seçenek: Docker ile Çalıştırma (Önerilen)
```bash
docker compose up -d --build
```
Swagger arayüzüne `http://localhost:7107/swagger` adresinden erişebilirsiniz.

### 2. Seçenek: .NET CLI ile Çalıştırma
Bağımlılıkları yükleyin:
```bash
dotnet restore
```

API projesini başlatın:
```bash
dotnet run --project src/KampusRota.API/KampusRota.API.csproj
```
Swagger adresi:
`https://localhost:7107/swagger`

---

## 📡 Temel API Endpoint'leri

| Metot | Endpoint | Açıklama |
| :--- | :--- | :--- |
| `GET` | `/api/universities` | Tüm üniversiteleri ve durak sayılarını listeler |
| `GET` | `/api/universities/{id}` | Üniversite detayını ve duraklarını getirir |
| `GET` | `/api/universities/{id}/locations` | Üniversiteye ait durak/kampüs koordinatlarını getirir |
| `GET` | `/api/universities/by-email` | E-posta uzantısından üniversiteyi bulur |
| `GET` | `/api/rides` | Tüm aktif yolculukları listeler (Opsiyonel: `?universityId=1`) |
| `GET` | `/api/rides/search` | Kalkış, varış, tarih ve üniversiteye göre arama yapar |
| `POST` | `/api/rides` | Yeni yolculuk ilanı oluşturur |
| `POST` | `/api/rides/{id}/requests` | İlana katılım talebi gönderir |
| `PUT` | `/api/rides/requests/{talepId}/status` | Sürücünün talebi onaylaması / reddetmesi |
| `POST` | `/api/rides/{id}/reviews` | Yolculuğa puan ve yorum ekleme |
| `POST` | `/api/users/register` | Yeni kullanıcı kaydı |
| `POST` | `/api/users/login` | Kullanıcı girişi |
