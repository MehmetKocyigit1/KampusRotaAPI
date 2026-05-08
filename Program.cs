using KampusRota.Data;
using KampusRota.Models;
using KampusRota.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;         

    var builder = WebApplication.CreateBuilder(args);

     builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

     builder.Services.AddScoped<IYolculukService, YolculukService>();
    builder.Services.AddScoped<IKullaniciService, KullaniciService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }



    app.MapGet("/api/rides", async (IYolculukService yolculukService) =>
    {
        var rides = await yolculukService.TumYolculuklariGetirAsync();
        return Results.Ok(rides);
    });

    app.MapGet("/api/rides/search", async (IYolculukService yolculukService, string kalkis, string varis, DateTime tarih) =>
    {
        var rides = await yolculukService.MusaitYolculuklariGetirAsync(kalkis, varis, tarih);
        return Results.Ok(rides);
    });

    app.MapPost("/api/rides", async (IYolculukService yolculukService, [FromBody] Yolculuk yeniYolculuk, int kullaniciId) =>
    {
        var createdRide = await yolculukService.YolculukEkleAsync(yeniYolculuk, kullaniciId);
        return Results.Created($"/api/rides/{createdRide.Id}", createdRide);
    });

    app.MapPut("/api/rides/{id}", async (IYolculukService yolculukService, int id, [FromBody] Yolculuk guncelYolculuk, int kullaniciId) =>
    {
        var result = await yolculukService.YolculukGuncelleAsync(id, guncelYolculuk, kullaniciId);
        return result != null ? Results.Ok(result) : Results.NotFound("İlan bulunamadı veya silinmiş.");
    });

     app.MapDelete("/api/rides/{id}", async (IYolculukService yolculukService, int id, int silenKullaniciId) =>
    {
        var isDeleted = await yolculukService.YolculukSilAsync(id, silenKullaniciId);
        return isDeleted ? Results.Ok(new { message = "İlan başarıyla silindi." }) : Results.NotFound("İlan bulunamadı veya zaten silinmiş.");
    });


 
    app.MapPost("/api/users/register", async (IKullaniciService kullaniciService, [FromBody] Kullanici yeniKullanici) =>
    {
        var user = await kullaniciService.KayitOlAsync(yeniKullanici);
        return user != null ? Results.Ok(user) : Results.BadRequest("Bu e-posta zaten kullanımda veya hesap önceden silinmiş.");
    });

    app.MapPost("/api/users/login", async (IKullaniciService kullaniciService, string email, string sifre) =>
    {
        var user = await kullaniciService.GirisYapAsync(email, sifre);
        return user != null ? Results.Ok(user) : Results.Unauthorized();
    });

     app.MapPut("/api/users/{id}/change-password", async (IKullaniciService kullaniciService, int id, [FromBody] SifreDegistirmeIstegi istek) =>
    {
         if (istek == null)
            return Results.BadRequest("Geçersiz istek formu.");

         if (istek.YeniSifre != istek.YeniSifreTekrar)
        {
            return Results.BadRequest("Yeni şifreler birbiriyle uyuşmuyor.");
        }

         var success = await kullaniciService.SifreDegistirAsync(id, istek);

        if (success)
        {
            return Results.Ok(new { message = "Şifreniz başarıyla değiştirildi." });
        }
        else
        {
             return Results.BadRequest(new { message = "Mevcut şifre hatalı veya işlem gerçekleştirilemedi." });
        }
    });

    app.Run();