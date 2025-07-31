using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odeme.API.Migrations
{
    /// <inheritdoc />
    public partial class addCorrelaitonidOdeme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiparisId",
                table: "OdemeKayitlari");

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "OdemeKayitlari",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "OdemeKayitlari");

            migrationBuilder.AddColumn<int>(
                name: "SiparisId",
                table: "OdemeKayitlari",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
