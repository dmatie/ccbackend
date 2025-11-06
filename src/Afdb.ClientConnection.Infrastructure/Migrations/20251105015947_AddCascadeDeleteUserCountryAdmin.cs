using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteUserCountryAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryAdmins_Users_UserId",
                table: "CountryAdmins");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryAdmins_Users_UserId",
                table: "CountryAdmins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryAdmins_Users_UserId",
                table: "CountryAdmins");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryAdmins_Users_UserId",
                table: "CountryAdmins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
