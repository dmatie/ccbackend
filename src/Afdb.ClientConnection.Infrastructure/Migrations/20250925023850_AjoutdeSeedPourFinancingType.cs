using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjoutdeSeedPourFinancingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "FinancingTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldDefaultValue: "System");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "FinancingTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "FinancingTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldDefaultValue: "System");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FinancingTypes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.InsertData(
                table: "FinancingTypes",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("1aefe328-1746-457c-9949-cfbaa3390c67"), "OTHER", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Autre type de financement", true, "Other", null, null },
                    { new Guid("27cfc0a1-682f-4e97-8d22-6416617f3706"), "PRIVATE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Financement d'origine privée", true, "Private", null, null },
                    { new Guid("a975c9e2-0cb2-4f83-876c-7078aaf66abd"), "PUBLIC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Financement d'origine publique", true, "Public", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FinancingTypes",
                keyColumn: "Id",
                keyValue: new Guid("1aefe328-1746-457c-9949-cfbaa3390c67"));

            migrationBuilder.DeleteData(
                table: "FinancingTypes",
                keyColumn: "Id",
                keyValue: new Guid("27cfc0a1-682f-4e97-8d22-6416617f3706"));

            migrationBuilder.DeleteData(
                table: "FinancingTypes",
                keyColumn: "Id",
                keyValue: new Guid("a975c9e2-0cb2-4f83-876c-7078aaf66abd"));

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "FinancingTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "System",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "FinancingTypes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "FinancingTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "System",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FinancingTypes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
