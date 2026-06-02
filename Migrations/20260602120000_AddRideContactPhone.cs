using KampusRota.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KampusRota.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260602120000_AddRideContactPhone")]
    public partial class AddRideContactPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IletisimTelefonu",
                table: "Yolculuklar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IletisimTelefonu",
                table: "Yolculuklar");
        }
    }
}
