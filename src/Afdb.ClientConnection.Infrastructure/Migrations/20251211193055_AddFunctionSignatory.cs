using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctionSignatory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "NameFr", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("1ea69a03-75fe-40b5-84c4-734e079d8e3c"), "SIGNATORY", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Other functions", true, "Signatory", "Saignataire", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1ea69a03-75fe-40b5-84c4-734e079d8e3c"));
        }
    }
}
