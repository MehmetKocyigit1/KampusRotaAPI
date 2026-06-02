using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KampusRota.Migrations
{
    public partial class FixDecimalPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yolculuklar_Kullanicilar_SurucuId",
                table: "Yolculuklar");

            migrationBuilder.AddForeignKey(
                name: "FK_Yolculuklar_Kullanicilar_SurucuId",
                table: "Yolculuklar",
                column: "SurucuId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yolculuklar_Kullanicilar_SurucuId",
                table: "Yolculuklar");

            migrationBuilder.AddForeignKey(
                name: "FK_Yolculuklar_Kullanicilar_SurucuId",
                table: "Yolculuklar",
                column: "SurucuId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
