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

    app.MapPost("/api/rides/{id}/requests", async (AppDbContext context, int id, int yolcuId, [FromBody] YolculukTalebi? talep) =>
    {
        var yolculuk = await context.Yolculuklar.FirstOrDefaultAsync(y => y.Id == id && !y.SilindiMi);
        if (yolculuk == null) return Results.NotFound("İlan bulunamadı.");
        if (yolculuk.SurucuId == yolcuId) return Results.BadRequest("Kendi ilanına katılım talebi gönderemezsin.");
        if (!yolculuk.AktifMi || yolculuk.BosKoltukSayisi <= 0) return Results.BadRequest("Bu ilan katılıma uygun değil.");

        var mevcutTalep = await context.YolculukTalepleri
            .FirstOrDefaultAsync(t => t.YolculukId == id && t.YolcuId == yolcuId && !t.SilindiMi && t.Durum != "Reddedildi");
        if (mevcutTalep != null) return Results.Ok(mevcutTalep);

        var yeniTalep = new YolculukTalebi
        {
            YolculukId = id,
            YolcuId = yolcuId,
            TalepMesaji = talep?.TalepMesaji ?? string.Empty,
            Durum = "Bekliyor",
            TalepTarihi = DateTime.UtcNow,
            OlusturulmaTarihi = DateTime.UtcNow,
            OlusturanKullaniciId = yolcuId,
            AktifMi = true,
            SilindiMi = false
        };

        context.YolculukTalepleri.Add(yeniTalep);
        await context.SaveChangesAsync();
        return Results.Created($"/api/rides/requests/{yeniTalep.Id}", yeniTalep);
    });

    app.MapGet("/api/rides/requests/driver/{surucuId}", async (AppDbContext context, int surucuId) =>
    {
        var talepler = await context.YolculukTalepleri
            .Include(t => t.Yolculuk)
            .Include(t => t.Yolcu)
            .Where(t => !t.SilindiMi && t.Yolculuk != null && t.Yolculuk!.SurucuId == surucuId)
            .OrderByDescending(t => t.TalepTarihi)
            .ToListAsync();

        return Results.Ok(talepler);
    });

    app.MapGet("/api/rides/requests/passenger/{yolcuId}", async (AppDbContext context, int yolcuId) =>
    {
        var talepler = await context.YolculukTalepleri
            .Include(t => t.Yolculuk)
            .ThenInclude(y => y!.Surucu)
            .Include(t => t.Yolcu)
            .Where(t => !t.SilindiMi && t.YolcuId == yolcuId)
            .OrderByDescending(t => t.TalepTarihi)
            .ToListAsync();

        return Results.Ok(talepler);
    });

    app.MapPut("/api/rides/requests/{talepId}/status", async (AppDbContext context, int talepId, int surucuId, bool onaylandi, string? not) =>
    {
        var talep = await context.YolculukTalepleri
            .Include(t => t.Yolculuk)
            .FirstOrDefaultAsync(t => t.Id == talepId && !t.SilindiMi);
        if (talep?.Yolculuk == null) return Results.NotFound("Talep bulunamadı.");
        if (talep.Yolculuk.SurucuId != surucuId) return Results.Forbid();
        if (talep.Durum != "Bekliyor") return Results.BadRequest("Bu talep daha önce sonuçlandırılmış.");

        talep.Durum = onaylandi ? "Onaylandı" : "Reddedildi";
        talep.SurucuNotu = not ?? string.Empty;
        talep.OnayTarihi = DateTime.UtcNow;
        talep.GuncellenmeTarihi = DateTime.UtcNow;
        talep.GuncelleyenKullaniciId = surucuId;

        if (onaylandi)
        {
            if (talep.Yolculuk.BosKoltukSayisi <= 0) return Results.BadRequest("Boş koltuk kalmadı.");

            talep.Yolculuk.BosKoltukSayisi -= 1;
            talep.Yolculuk.GuncellenmeTarihi = DateTime.UtcNow;
            talep.Yolculuk.GuncelleyenKullaniciId = surucuId;

            if (talep.Yolculuk.BosKoltukSayisi <= 0)
            {
                talep.Yolculuk.BosKoltukSayisi = 0;
                talep.Yolculuk.AktifMi = false;
                talep.Yolculuk.SilindiMi = true;
                talep.Yolculuk.SilinmeTarihi = DateTime.UtcNow;
                talep.Yolculuk.SilenKullaniciId = surucuId;

                var digerBekleyenTalepler = await context.YolculukTalepleri
                    .Where(t =>
                        t.YolculukId == talep.YolculukId &&
                        t.Id != talep.Id &&
                        !t.SilindiMi &&
                        t.Durum == "Bekliyor")
                    .ToListAsync();

                foreach (var bekleyenTalep in digerBekleyenTalepler)
                {
                    bekleyenTalep.Durum = "Reddedildi";
                    bekleyenTalep.SurucuNotu = "Koltuklar dolduğu için talep otomatik reddedildi.";
                    bekleyenTalep.OnayTarihi = DateTime.UtcNow;
                    bekleyenTalep.GuncellenmeTarihi = DateTime.UtcNow;
                    bekleyenTalep.GuncelleyenKullaniciId = surucuId;
                }
            }
            else
            {
                talep.Yolculuk.AktifMi = true;
            }
        }

        await context.SaveChangesAsync();
        return Results.Ok(talep);
    });

    app.MapPost("/api/rides/{id}/reviews", async (AppDbContext context, int id, int yorumYapanKullaniciId, int puanlananKullaniciId, [FromBody] YolculukYorumu yorum) =>
    {
        if (yorum.Puan < 1 || yorum.Puan > 5) return Results.BadRequest("Puan 1 ile 5 arasında olmalı.");

        var yolculuk = await context.Yolculuklar.FirstOrDefaultAsync(y => y.Id == id && !y.SilindiMi);
        if (yolculuk == null) return Results.NotFound("Yolculuk bulunamadı.");

        var katilimVarMi = yolculuk.SurucuId == yorumYapanKullaniciId ||
            await context.YolculukTalepleri.AnyAsync(t =>
                t.YolculukId == id &&
                t.YolcuId == yorumYapanKullaniciId &&
                t.Durum == "Onaylandı" &&
                !t.SilindiMi);
        if (!katilimVarMi) return Results.BadRequest("Sadece yolculuğa katılan kullanıcılar yorum yapabilir.");

        var mevcutYorum = await context.YolculukYorumlari.FirstOrDefaultAsync(y =>
            y.YolculukId == id &&
            y.YorumYapanKullaniciId == yorumYapanKullaniciId &&
            y.PuanlananKullaniciId == puanlananKullaniciId &&
            !y.SilindiMi);
        if (mevcutYorum != null) return Results.BadRequest("Bu kullanıcı için bu yolculukta zaten yorum yapılmış.");

        var yeniYorum = new YolculukYorumu
        {
            YolculukId = id,
            YorumYapanKullaniciId = yorumYapanKullaniciId,
            PuanlananKullaniciId = puanlananKullaniciId,
            Puan = yorum.Puan,
            Yorum = yorum.Yorum ?? string.Empty,
            YorumTarihi = DateTime.UtcNow,
            OlusturulmaTarihi = DateTime.UtcNow,
            OlusturanKullaniciId = yorumYapanKullaniciId,
            AktifMi = true,
            SilindiMi = false
        };

        context.YolculukYorumlari.Add(yeniYorum);

        var puanlanan = await context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == puanlananKullaniciId && !k.SilindiMi);
        if (puanlanan != null)
        {
            var puanlar = await context.YolculukYorumlari
                .Where(y => y.PuanlananKullaniciId == puanlananKullaniciId && !y.SilindiMi)
                .Select(y => y.Puan)
                .ToListAsync();
            puanlar.Add(yeniYorum.Puan);
            puanlanan.OrtalamaPuan = Math.Round(puanlar.Average(), 1);
            puanlanan.GuncellenmeTarihi = DateTime.UtcNow;
            puanlanan.GuncelleyenKullaniciId = yorumYapanKullaniciId;
        }

        await context.SaveChangesAsync();
        return Results.Created($"/api/rides/{id}/reviews/{yeniYorum.Id}", yeniYorum);
    });

    app.MapGet("/api/rides/{id}/reviews", async (AppDbContext context, int id) =>
    {
        var yorumlar = await context.YolculukYorumlari
            .Include(y => y.YorumYapanKullanici)
            .Include(y => y.PuanlananKullanici)
            .Where(y => y.YolculukId == id && !y.SilindiMi)
            .OrderByDescending(y => y.YorumTarihi)
            .ToListAsync();

        return Results.Ok(yorumlar);
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

    app.MapGet("/api/users/{id}", async (AppDbContext context, int id) =>
    {
        var user = await context.Kullanicilar.FirstOrDefaultAsync(k => k.Id == id && !k.SilindiMi);
        return user != null ? Results.Ok(user) : Results.NotFound("Kullanıcı bulunamadı.");
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



