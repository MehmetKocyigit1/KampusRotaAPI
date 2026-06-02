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
        try
        {
            var createdRide = await yolculukService.YolculukEkleAsync(yeniYolculuk, kullaniciId);
            return Results.Created($"/api/rides/{createdRide.Id}", createdRide);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    });

    app.MapPut("/api/rides/{id}", async (IYolculukService yolculukService, int id, [FromBody] Yolculuk guncelYolculuk, int kullaniciId) =>
    {
        try
        {
            var result = await yolculukService.YolculukGuncelleAsync(id, guncelYolculuk, kullaniciId);
            return result != null ? Results.Ok(result) : Results.NotFound("İlan bulunamadı veya silinmiş.");
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    });

    app.MapDelete("/api/rides/{id}", async (IYolculukService yolculukService, int id, int silenKullaniciId) =>
    {
        var isDeleted = await yolculukService.YolculukSilAsync(id, silenKullaniciId);
        return isDeleted ? Results.Ok(new { message = "İlan başarıyla silindi." }) : Results.NotFound("İlan bulunamadı veya zaten silinmiş.");
    });

    app.MapPost("/api/rides/{id}/requests", async (IYolculukService yolculukService, int id, int yolcuId, [FromBody] YolculukTalebi? talep) =>
    {
        var result = await yolculukService.KatilmaTalebiOlusturAsync(id, yolcuId, talep?.TalepMesaji ?? string.Empty);
        return result.Success && result.StatusCode == 201
            ? Results.Created($"/api/rides/requests/{result.Value!.Id}", result.Value)
            : ToHttpResult(result);
    });

    app.MapGet("/api/rides/requests/driver/{surucuId}", async (IYolculukService yolculukService, int surucuId) =>
    {
        var talepler = await yolculukService.SurucuTalepleriniGetirAsync(surucuId);
        return Results.Ok(talepler);
    });

    app.MapGet("/api/rides/requests/passenger/{yolcuId}", async (IYolculukService yolculukService, int yolcuId) =>
    {
        var talepler = await yolculukService.YolcuTalepleriniGetirAsync(yolcuId);
        return Results.Ok(talepler);
    });

    app.MapPut("/api/rides/requests/{talepId}/status", async (IYolculukService yolculukService, int talepId, int surucuId, bool onaylandi, string? not) =>
    {
        var result = await yolculukService.TalepDurumuGuncelleAsync(talepId, surucuId, onaylandi, not);
        return ToHttpResult(result);
    });

    app.MapPost("/api/rides/{id}/reviews", async (IYolculukService yolculukService, int id, int yorumYapanKullaniciId, int puanlananKullaniciId, [FromBody] YolculukYorumu yorum) =>
    {
        var result = await yolculukService.YolculukYorumuEkleAsync(id, yorumYapanKullaniciId, puanlananKullaniciId, yorum);
        return result.Success && result.StatusCode == 201
            ? Results.Created($"/api/rides/{id}/reviews/{result.Value!.Id}", result.Value)
            : ToHttpResult(result);
    });

    app.MapGet("/api/rides/{id}/reviews", async (IYolculukService yolculukService, int id) =>
    {
        var yorumlar = await yolculukService.YolculukYorumlariniGetirAsync(id);
        return Results.Ok(yorumlar);
    });


 
    app.MapPost("/api/users/register", async (IKullaniciService kullaniciService, [FromBody] Kullanici yeniKullanici) =>
    {
        try
        {
            var user = await kullaniciService.KayitOlAsync(yeniKullanici);
            return user != null ? Results.Ok(user) : Results.BadRequest("Bu e-posta zaten kullanımda veya hesap önceden silinmiş.");
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    });

    app.MapPost("/api/users/login", async (IKullaniciService kullaniciService, string email, string sifre) =>
    {
        var user = await kullaniciService.GirisYapAsync(email, sifre);
        return user != null ? Results.Ok(user) : Results.Unauthorized();
    });

    app.MapGet("/api/users/{id}", async (IKullaniciService kullaniciService, int id) =>
    {
        var user = await kullaniciService.KullaniciGetirAsync(id);
        return user != null ? Results.Ok(user) : Results.NotFound("Kullanıcı bulunamadı.");
    });

    app.MapPut("/api/users/{id}", async (IKullaniciService kullaniciService, int id, [FromBody] Kullanici guncelKullanici) =>
    {
        try
        {
            var user = await kullaniciService.KullaniciGuncelleAsync(id, guncelKullanici);
            return user != null ? Results.Ok(user) : Results.NotFound("Kullanıcı bulunamadı.");
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
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

    app.MapDelete("/api/users/{id}", async (IKullaniciService kullaniciService, int id) =>
    {
        var isDeleted = await kullaniciService.KullaniciSilAsync(id);
        return isDeleted ? Results.Ok(new { message = "Hesap başarıyla silindi." }) : Results.NotFound("Hesap bulunamadı veya zaten silinmiş.");
    });

    app.Run();

    static IResult ToHttpResult<T>(ServiceResult<T> result)
    {
        if (result.Success)
        {
            return Results.Ok(result.Value);
        }

        return result.StatusCode switch
        {
            400 => Results.BadRequest(result.Message),
            403 => Results.Forbid(),
            404 => Results.NotFound(result.Message),
            _ => Results.BadRequest(result.Message)
        };
    }
