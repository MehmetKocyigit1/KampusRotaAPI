using System;
using KampusRota.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KampusRota.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260524004812_RideRequestsAndReviews")]
    partial class RideRequestsAndReviews
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KampusRota.Models.Kullanici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<string>("Biyografi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cinsiyet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("GuncellenmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GuncelleyenKullaniciId")
                        .HasColumnType("int");

                    b.Property<string>("OgrenciNumarasi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OlusturanKullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturulmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<double>("OrtalamaPuan")
                        .HasColumnType("float");

                    b.Property<string>("ProfilFotografiUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SifreHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SilenKullaniciId")
                        .HasColumnType("int");

                    b.Property<bool>("SilindiMi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("SilinmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonNumarasi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("KampusRota.Models.Yolculuk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<int>("BosKoltukSayisi")
                        .HasColumnType("int");

                    b.Property<DateTime?>("GuncellenmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GuncelleyenKullaniciId")
                        .HasColumnType("int");

                    b.Property<string>("KalkisNoktasi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("KalkisZamani")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("KisiBasiUcret")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OlusturanKullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturulmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<bool>("SadeceKadinlarMi")
                        .HasColumnType("bit");

                    b.Property<int?>("SilenKullaniciId")
                        .HasColumnType("int");

                    b.Property<bool>("SilindiMi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("SilinmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("SurucuId")
                        .HasColumnType("int");

                    b.Property<string>("VarisNoktasi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SurucuId");

                    b.ToTable("Yolculuklar");
                });

            modelBuilder.Entity("KampusRota.Models.YolculukTalebi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<string>("Durum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("GuncellenmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GuncelleyenKullaniciId")
                        .HasColumnType("int");

                    b.Property<int>("OlusturanKullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturulmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("OnayTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SilenKullaniciId")
                        .HasColumnType("int");

                    b.Property<bool>("SilindiMi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("SilinmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<string>("SurucuNotu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TalepMesaji")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TalepTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("YolcuId")
                        .HasColumnType("int");

                    b.Property<int>("YolculukId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("YolcuId");

                    b.HasIndex("YolculukId");

                    b.ToTable("YolculukTalepleri");
                });

            modelBuilder.Entity("KampusRota.Models.YolculukYorumu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("GuncellenmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GuncelleyenKullaniciId")
                        .HasColumnType("int");

                    b.Property<int>("OlusturanKullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturulmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("Puan")
                        .HasColumnType("int");

                    b.Property<int>("PuanlananKullaniciId")
                        .HasColumnType("int");

                    b.Property<int?>("SilenKullaniciId")
                        .HasColumnType("int");

                    b.Property<bool>("SilindiMi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("SilinmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("YolculukId")
                        .HasColumnType("int");

                    b.Property<string>("Yorum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("YorumTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("YorumYapanKullaniciId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PuanlananKullaniciId");

                    b.HasIndex("YolculukId");

                    b.HasIndex("YorumYapanKullaniciId");

                    b.ToTable("YolculukYorumlari");
                });

            modelBuilder.Entity("KampusRota.Models.Yolculuk", b =>
                {
                    b.HasOne("KampusRota.Models.Kullanici", "Surucu")
                        .WithMany()
                        .HasForeignKey("SurucuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Surucu");
                });

            modelBuilder.Entity("KampusRota.Models.YolculukTalebi", b =>
                {
                    b.HasOne("KampusRota.Models.Kullanici", "Yolcu")
                        .WithMany()
                        .HasForeignKey("YolcuId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KampusRota.Models.Yolculuk", "Yolculuk")
                        .WithMany()
                        .HasForeignKey("YolculukId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Yolcu");

                    b.Navigation("Yolculuk");
                });

            modelBuilder.Entity("KampusRota.Models.YolculukYorumu", b =>
                {
                    b.HasOne("KampusRota.Models.Kullanici", "PuanlananKullanici")
                        .WithMany()
                        .HasForeignKey("PuanlananKullaniciId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KampusRota.Models.Yolculuk", "Yolculuk")
                        .WithMany()
                        .HasForeignKey("YolculukId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KampusRota.Models.Kullanici", "YorumYapanKullanici")
                        .WithMany()
                        .HasForeignKey("YorumYapanKullaniciId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PuanlananKullanici");

                    b.Navigation("Yolculuk");

                    b.Navigation("YorumYapanKullanici");
                });
#pragma warning restore 612, 618
        }
    }
}
