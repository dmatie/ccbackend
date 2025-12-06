using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColonneProjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessProfiles_Name",
                table: "BusinessProfiles");

            migrationBuilder.AddColumn<string>(
                name: "NameFr",
                table: "Functions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameFr",
                table: "FinancingTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "BusinessProfiles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameFr",
                table: "BusinessProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectTitle",
                table: "AccessRequestsProjects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "123456");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AccessRequests",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "123456");

            migrationBuilder.UpdateData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                columns: new[] { "Code", "NameFr" },
                values: new object[] { "BO", "Emprunteur" });

            migrationBuilder.UpdateData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                columns: new[] { "Code", "Name", "NameFr" },
                values: new object[] { "GU", "Guarantor", "Garant" });

            migrationBuilder.UpdateData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                columns: new[] { "Code", "NameFr" },
                values: new object[] { "EA", "Agence d'exécution" });

            migrationBuilder.UpdateData(
                table: "FinancingTypes",
                keyColumn: "Id",
                keyValue: new Guid("1aefe328-1746-457c-9949-cfbaa3390c67"),
                column: "NameFr",
                value: "Autre");

            migrationBuilder.UpdateData(
                table: "FinancingTypes",
                keyColumn: "Id",
                keyValue: new Guid("27cfc0a1-682f-4e97-8d22-6416617f3706"),
                column: "NameFr",
                value: "Privé");

            migrationBuilder.UpdateData(
                table: "FinancingTypes",
                keyColumn: "Id",
                keyValue: new Guid("a975c9e2-0cb2-4f83-876c-7078aaf66abd"),
                column: "NameFr",
                value: "Public");

            migrationBuilder.UpdateData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"),
                column: "NameFr",
                value: "Bureau ADB");

            migrationBuilder.UpdateData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"),
                column: "NameFr",
                value: "Directeur/Directeur financier et administratif");

            migrationBuilder.UpdateData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"),
                column: "NameFr",
                value: "Agent de ministrère");

            migrationBuilder.UpdateData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"),
                column: "NameFr",
                value: "Comptable de projet");

            migrationBuilder.UpdateData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"),
                column: "NameFr",
                value: "Cordinateur de projet");

            migrationBuilder.UpdateData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"),
                column: "NameFr",
                value: "Autre");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfiles_Code",
                table: "BusinessProfiles",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessProfiles_Code",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "NameFr",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "NameFr",
                table: "FinancingTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "NameFr",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "ProjectTitle",
                table: "AccessRequestsProjects");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AccessRequests");

            migrationBuilder.UpdateData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                column: "Name",
                value: "Technical Advisor");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfiles_Name",
                table: "BusinessProfiles",
                column: "Name",
                unique: true);
        }
    }
}
