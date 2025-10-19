using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimsStatusColonneTableClaimProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ClaimProcesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ClaimProcesses");
        }
    }
}
