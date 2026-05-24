using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KampusRota.Migrations
{
    /// <inheritdoc />
    public partial class RideRequestsAndReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YolculukTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YolculukId = table.Column<int>(type: "int", nullable: false),
                    YolcuId = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalepMesaji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurucuNotu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    SilindiMi = table.Column<bool>(type: "bit", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SilenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YolculukTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YolculukTalepleri_Kullanicilar_YolcuId",
                        column: x => x.YolcuId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YolculukTalepleri_Yolculuklar_YolculukId",
                        column: x => x.YolculukId,
                        principalTable: "Yolculuklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YolculukYorumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YolculukId = table.Column<int>(type: "int", nullable: false),
                    YorumYapanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    PuanlananKullaniciId = table.Column<int>(type: "int", nullable: false),
                    Puan = table.Column<int>(type: "int", nullable: false),
                    Yorum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YorumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    SilindiMi = table.Column<bool>(type: "bit", nullable: false),
                    SilinmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SilenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YolculukYorumlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YolculukYorumlari_Kullanicilar_PuanlananKullaniciId",
                        column: x => x.PuanlananKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YolculukYorumlari_Kullanicilar_YorumYapanKullaniciId",
                        column: x => x.YorumYapanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YolculukYorumlari_Yolculuklar_YolculukId",
                        column: x => x.YolculukId,
                        principalTable: "Yolculuklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YolculukTalepleri_YolcuId",
                table: "YolculukTalepleri",
                column: "YolcuId");

            migrationBuilder.CreateIndex(
                name: "IX_YolculukTalepleri_YolculukId",
                table: "YolculukTalepleri",
                column: "YolculukId");

            migrationBuilder.CreateIndex(
                name: "IX_YolculukYorumlari_PuanlananKullaniciId",
                table: "YolculukYorumlari",
                column: "PuanlananKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_YolculukYorumlari_YolculukId",
                table: "YolculukYorumlari",
                column: "YolculukId");

            migrationBuilder.CreateIndex(
                name: "IX_YolculukYorumlari_YorumYapanKullaniciId",
                table: "YolculukYorumlari",
                column: "YorumYapanKullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YolculukTalepleri");

            migrationBuilder.DropTable(
                name: "YolculukYorumlari");
        }
    }
}
