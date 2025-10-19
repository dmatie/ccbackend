using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotDefinedCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Name", "NameFr", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("91431d44-531f-48d1-b990-8bd34aa185f4"), "NOT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Not Defined", "Non défini", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("91431d44-531f-48d1-b990-8bd34aa185f4"));
        }
    }
}
