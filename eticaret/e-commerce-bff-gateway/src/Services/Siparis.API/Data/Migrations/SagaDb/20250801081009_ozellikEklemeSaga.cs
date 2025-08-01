using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Siparis.API.Data.Migrations.SagaDb
{
    /// <inheritdoc />
    public partial class ozellikEklemeSaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "SagaStates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KullaniciEmail",
                table: "SagaStates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KullaniciRol",
                table: "SagaStates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UserDate",
                table: "SagaStates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "SagaStates");

            migrationBuilder.DropColumn(
                name: "KullaniciEmail",
                table: "SagaStates");

            migrationBuilder.DropColumn(
                name: "KullaniciRol",
                table: "SagaStates");

            migrationBuilder.DropColumn(
                name: "UserDate",
                table: "SagaStates");
        }
    }
}
