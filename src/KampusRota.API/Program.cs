using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KampusRota.Application.Common;
using KampusRota.Application.DTOs;
using KampusRota.Application.Services;
using KampusRota.Domain.Entities;
using KampusRota.Infrastructure.Persistence;
using KampusRota.Infrastructure.Services;
using KampusRota.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application Services
builder.Services.AddScoped<IUniversityService, UniversityService>();
builder.Services.AddScoped<IYolculukService, YolculukService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Kampus Rota API",
        Version = "v1",
        Description = "Üniversite Kampüsleri Arası Yolculuk Paylaşım Platformu API'si"
    });
});

var app = builder.Build();

// Global Exception Handler Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kampus Rota API v1");
    });
}

// Database Initialization & Seed
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await DbInitializer.SeedAsync(dbContext);
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Veritabanı başlatma/seed sırasında bir sorun oluştu.");
}

// ==========================================
// 1. ÜNİVERSİTE & KAMPÜS HARİTA ENDPOINT'LERİ
// ==========================================

app.MapGet("/api/universities", async (IUniversityService universityService) =>
{
    var universities = await universityService.GetAllUniversitiesAsync();
    return Results.Ok(universities);
})
.WithTags("Universities")
.WithSummary("Tüm üniversiteleri koordinat ve durak sayılarıyla listeler");

app.MapGet("/api/universities/{id:int}", async (IUniversityService universityService, int id) =>
{
    var university = await universityService.GetUniversityByIdAsync(id);
    return university != null ? Results.Ok(university) : Results.NotFound("Üniversite bulunamadı.");
})
.WithTags("Universities")
.WithSummary("Belirli bir üniversiteyi ve durak noktalarını getirir");

app.MapGet("/api/universities/{id:int}/locations", async (IUniversityService universityService, int id) =>
{
    var locations = await universityService.GetCampusLocationsAsync(id);
    return Results.Ok(locations);
})
.WithTags("Universities")
.WithSummary("Üniversiteye ait harita durak ve kampüs noktalarını listeler");

app.MapGet("/api/universities/by-email", async (IUniversityService universityService, [FromQuery] string email) =>
{
    var university = await universityService.GetUniversityByEmailAsync(email);
    return university != null ? Results.Ok(university) : Results.NotFound("Bu e-posta uzantısıyla eşleşen üniversite bulunamadı.");
})
.WithTags("Universities")
.WithSummary("E-posta adresine göre üniversiteyi tespit eder");

// ==========================================
// 2. YOLCULUK (RIDE) ENDPOINT'LERİ
// ==========================================

app.MapGet("/api/rides", async (IYolculukService yolculukService, [FromQuery] int? universityId) =>
{
    var rides = await yolculukService.TumYolculuklariGetirAsync(universityId);
    return Results.Ok(rides);
})
.WithTags("Rides")
.WithSummary("Müsait tüm yolculuk ilanlarını listeler (Üniversiteye göre filtrelenebilir)");

app.MapGet("/api/rides/search", async (IYolculukService yolculukService, [FromQuery] string kalkis, [FromQuery] string varis, [FromQuery] DateTime tarih, [FromQuery] int? universityId) =>
{
    var rides = await yolculukService.MusaitYolculuklariGetirAsync(kalkis ?? string.Empty, varis ?? string.Empty, tarih, universityId);
    return Results.Ok(rides);
})
.WithTags("Rides")
.WithSummary("Kalkış, varış, tarih ve opsiyonel üniversiteye göre yolculuk arar");

app.MapGet("/api/rides/{id:int}", async (IYolculukService yolculukService, int id) =>
{
    var ride = await yolculukService.YolculukGetirByIdAsync(id);
    return ride != null ? Results.Ok(ride) : Results.NotFound("İlan bulunamadı.");
})
.WithTags("Rides");

app.MapPost("/api/rides", async (IYolculukService yolculukService, [FromBody] Yolculuk yeniYolculuk, [FromQuery] int kullaniciId) =>
{
    var createdRide = await yolculukService.YolculukEkleAsync(yeniYolculuk, kullaniciId);
    return Results.Created($"/api/rides/{createdRide.Id}", createdRide);
})
.WithTags("Rides")
.WithSummary("Yeni yolculuk ilanı oluşturur");

app.MapPut("/api/rides/{id:int}", async (IYolculukService yolculukService, int id, [FromBody] Yolculuk guncelYolculuk, [FromQuery] int kullaniciId) =>
{
    var result = await yolculukService.YolculukGuncelleAsync(id, guncelYolculuk, kullaniciId);
    return result != null ? Results.Ok(result) : Results.NotFound("İlan bulunamadı veya silinmiş.");
})
.WithTags("Rides")
.WithSummary("Yolculuk ilanını günceller");

app.MapDelete("/api/rides/{id:int}", async (IYolculukService yolculukService, int id, [FromQuery] int silenKullaniciId) =>
{
    var isDeleted = await yolculukService.YolculukSilAsync(id, silenKullaniciId);
    return isDeleted ? Results.Ok(new { message = "İlan başarıyla silindi." }) : Results.NotFound("İlan bulunamadı veya zaten silinmiş.");
})
.WithTags("Rides")
.WithSummary("Yolculuk ilanını siler");

app.MapPost("/api/rides/{id:int}/requests", async (IYolculukService yolculukService, int id, [FromQuery] int yolcuId, [FromBody] YolculukTalebi? talep) =>
{
    var result = await yolculukService.KatilmaTalebiOlusturAsync(id, yolcuId, talep?.TalepMesaji ?? string.Empty);
    return result.Success && result.StatusCode == 201
        ? Results.Created($"/api/rides/requests/{result.Value!.Id}", result.Value)
        : ToHttpResult(result);
})
.WithTags("Ride Requests")
.WithSummary("Yolculuğa katılım talebi gönderir");

app.MapGet("/api/rides/requests/driver/{surucuId:int}", async (IYolculukService yolculukService, int surucuId) =>
{
    var talepler = await yolculukService.SurucuTalepleriniGetirAsync(surucuId);
    return Results.Ok(talepler);
})
.WithTags("Ride Requests")
.WithSummary("Sürücüye gelen talepleri listeler");

app.MapGet("/api/rides/requests/passenger/{yolcuId:int}", async (IYolculukService yolculukService, int yolcuId) =>
{
    var talepler = await yolculukService.YolcuTalepleriniGetirAsync(yolcuId);
    return Results.Ok(talepler);
})
.WithTags("Ride Requests")
.WithSummary("Yolcunun gönderdiği talepleri listeler");

app.MapPut("/api/rides/requests/{talepId:int}/status", async (IYolculukService yolculukService, int talepId, [FromQuery] int surucuId, [FromQuery] bool onaylandi, [FromQuery] string? not) =>
{
    var result = await yolculukService.TalepDurumuGuncelleAsync(talepId, surucuId, onaylandi, not);
    return ToHttpResult(result);
})
.WithTags("Ride Requests")
.WithSummary("Sürücü katılım talebini onaylar veya reddeder");

app.MapPost("/api/rides/{id:int}/reviews", async (IYolculukService yolculukService, int id, [FromQuery] int yorumYapanKullaniciId, [FromQuery] int puanlananKullaniciId, [FromBody] YolculukYorumu yorum) =>
{
    var result = await yolculukService.YolculukYorumuEkleAsync(id, yorumYapanKullaniciId, puanlananKullaniciId, yorum);
    return result.Success && result.StatusCode == 201
        ? Results.Created($"/api/rides/{id}/reviews/{result.Value!.Id}", result.Value)
        : ToHttpResult(result);
})
.WithTags("Reviews")
.WithSummary("Yolculuk sonrası değerlendirme ve puan ekler");

app.MapGet("/api/rides/{id:int}/reviews", async (IYolculukService yolculukService, int id) =>
{
    var yorumlar = await yolculukService.YolculukYorumlariniGetirAsync(id);
    return Results.Ok(yorumlar);
})
.WithTags("Reviews")
.WithSummary("İlana ait yorum ve puanları listeler");

// ==========================================
// 3. KULLANICI (USER) ENDPOINT'LERİ
// ==========================================

app.MapPost("/api/users/register", async (IKullaniciService kullaniciService, [FromBody] Kullanici yeniKullanici) =>
{
    var user = await kullaniciService.KayitOlAsync(yeniKullanici);
    return user != null ? Results.Ok(user) : Results.BadRequest("Bu e-posta zaten kullanımda veya hesap önceden silinmiş.");
})
.WithTags("Users")
.WithSummary("Yeni öğrenci/kullanıcı kaydı oluşturur");

app.MapPost("/api/users/login", async (IKullaniciService kullaniciService, [FromQuery] string email, [FromQuery] string sifre) =>
{
    var user = await kullaniciService.GirisYapAsync(email, sifre);
    return user != null ? Results.Ok(user) : Results.Unauthorized();
})
.WithTags("Users")
.WithSummary("Kullanıcı girişi yapar");

app.MapGet("/api/users/{id:int}", async (IKullaniciService kullaniciService, int id) =>
{
    var user = await kullaniciService.KullaniciGetirAsync(id);
    return user != null ? Results.Ok(user) : Results.NotFound("Kullanıcı bulunamadı.");
})
.WithTags("Users")
.WithSummary("Kullanıcı profil bilgilerini getirir");

app.MapPut("/api/users/{id:int}", async (IKullaniciService kullaniciService, int id, [FromBody] Kullanici guncelKullanici) =>
{
    var user = await kullaniciService.KullaniciGuncelleAsync(id, guncelKullanici);
    return user != null ? Results.Ok(user) : Results.NotFound("Kullanıcı bulunamadı.");
})
.WithTags("Users")
.WithSummary("Profil bilgilerini günceller");

app.MapPut("/api/users/{id:int}/change-password", async (IKullaniciService kullaniciService, int id, [FromBody] SifreDegistirmeIstegi istek) =>
{
    if (istek == null)
        return Results.BadRequest("Geçersiz istek formu.");

    if (istek.YeniSifre != istek.YeniSifreTekrar)
        return Results.BadRequest("Yeni şifreler birbiriyle uyuşmuyor.");

    var success = await kullaniciService.SifreDegistirAsync(id, istek);
    return success 
        ? Results.Ok(new { message = "Şifreniz başarıyla değiştirildi." }) 
        : Results.BadRequest(new { message = "Mevcut şifre hatalı veya işlem gerçekleştirilemedi." });
})
.WithTags("Users")
.WithSummary("Kullanıcı şifresini değiştirir");

app.MapDelete("/api/users/{id:int}", async (IKullaniciService kullaniciService, int id) =>
{
    var isDeleted = await kullaniciService.KullaniciSilAsync(id);
    return isDeleted ? Results.Ok(new { message = "Hesap başarıyla silindi." }) : Results.NotFound("Hesap bulunamadı veya zaten silinmiş.");
})
.WithTags("Users")
.WithSummary("Kullanıcı hesabını siler");

app.Run();

static IResult ToHttpResult<T>(ServiceResult<T> result)
{
    if (result.Success)
    {
        return Results.Ok(result.Value);
    }

    return result.StatusCode switch
    {
        400 => Results.BadRequest(new { error = result.Message }),
        403 => Results.Forbid(),
        404 => Results.NotFound(new { error = result.Message }),
        _ => Results.BadRequest(new { error = result.Message })
    };
}
