using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctionCountryBusinessProfileEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessJustification",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AccessRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessProfileEntityId",
                table: "AccessRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryEntityId",
                table: "AccessRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FunctionEntityId",
                table: "AccessRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_BusinessProfileEntityId",
                table: "AccessRequests",
                column: "BusinessProfileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_CountryEntityId",
                table: "AccessRequests",
                column: "CountryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_FunctionEntityId",
                table: "AccessRequests",
                column: "FunctionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfiles_Name",
                table: "BusinessProfiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_Name",
                table: "Functions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequests_BusinessProfiles_BusinessProfileEntityId",
                table: "AccessRequests",
                column: "BusinessProfileEntityId",
                principalTable: "BusinessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequests_Countries_CountryEntityId",
                table: "AccessRequests",
                column: "CountryEntityId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequests_Functions_FunctionEntityId",
                table: "AccessRequests",
                column: "FunctionEntityId",
                principalTable: "Functions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequests_BusinessProfiles_BusinessProfileEntityId",
                table: "AccessRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequests_Countries_CountryEntityId",
                table: "AccessRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequests_Functions_FunctionEntityId",
                table: "AccessRequests");

            migrationBuilder.DropTable(
                name: "BusinessProfiles");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_AccessRequests_BusinessProfileEntityId",
                table: "AccessRequests");

            migrationBuilder.DropIndex(
                name: "IX_AccessRequests_CountryEntityId",
                table: "AccessRequests");

            migrationBuilder.DropIndex(
                name: "IX_AccessRequests_FunctionEntityId",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "BusinessProfileEntityId",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "CountryEntityId",
                table: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "FunctionEntityId",
                table: "AccessRequests");

            migrationBuilder.AddColumn<string>(
                name: "BusinessJustification",
                table: "AccessRequests",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AccessRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "AccessRequests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AccessRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
