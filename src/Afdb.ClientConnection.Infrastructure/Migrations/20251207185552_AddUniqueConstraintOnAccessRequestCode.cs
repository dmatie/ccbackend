using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintOnAccessRequestCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_Code",
                table: "AccessRequests",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessRequests_Code",
                table: "AccessRequests");
        }
    }
}
