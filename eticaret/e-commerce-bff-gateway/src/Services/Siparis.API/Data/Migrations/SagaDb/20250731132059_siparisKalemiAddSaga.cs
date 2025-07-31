using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Siparis.API.Data.Migrations.SagaDb
{
    /// <inheritdoc />
    public partial class siparisKalemiAddSaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiparisKalemleri",
                table: "SagaStates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiparisKalemleri",
                table: "SagaStates");
        }
    }
}
