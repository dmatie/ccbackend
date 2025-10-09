using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FinancingTypeEntityId",
                table: "AccessRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinancingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "System"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "System")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancingTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_FinancingTypeEntityId",
                table: "AccessRequests",
                column: "FinancingTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancingTypes_IsActive",
                table: "FinancingTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FinancingTypes_Name",
                table: "FinancingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequests_FinancingTypes_FinancingTypeEntityId",
                table: "AccessRequests",
                column: "FinancingTypeEntityId",
                principalTable: "FinancingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequests_FinancingTypes_FinancingTypeEntityId",
                table: "AccessRequests");

            migrationBuilder.DropTable(
                name: "FinancingTypes");

            migrationBuilder.DropIndex(
                name: "IX_AccessRequests_FinancingTypeEntityId",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "FinancingTypeEntityId",
                table: "AccessRequests");
        }
    }
}
