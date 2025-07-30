using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Rapor.API.Migrations
{
    /// <inheritdoc />
    public partial class ilkRaporlar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MusteriSiparisRaporlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    UserCreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderCount = table.Column<int>(type: "integer", nullable: false),
                    RefundCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusteriSiparisRaporlari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SiparisDetayRaporlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    UserRole = table.Column<string>(type: "text", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<double>(type: "double precision", nullable: false),
                    OrderStatus = table.Column<string>(type: "text", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    ProductPrice = table.Column<double>(type: "double precision", nullable: false),
                    ProductQuantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDetayRaporlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrunPerformansRaporlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    ProductCreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderedCount = table.Column<int>(type: "integer", nullable: false),
                    RefundedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunPerformansRaporlari", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusteriSiparisRaporlari");

            migrationBuilder.DropTable(
                name: "SiparisDetayRaporlari");

            migrationBuilder.DropTable(
                name: "UrunPerformansRaporlari");
        }
    }
}
