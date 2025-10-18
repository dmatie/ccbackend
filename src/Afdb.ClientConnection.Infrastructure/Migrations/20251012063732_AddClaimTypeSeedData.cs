using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimTypeSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClaimTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Name", "NameFr", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("57cf2666-198c-41a9-b828-8f0651da76ed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "Disbursment", "Décaissement", null, null },
                    { new Guid("f671b0d7-ee85-48d8-a58b-0029660b2cad"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "Other", "Autre", null, null },
                    { new Guid("fd97a4df-40ef-4581-b5bf-983d56e70b3d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "Payment not received", "Paiement non reçu", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClaimTypes",
                keyColumn: "Id",
                keyValue: new Guid("57cf2666-198c-41a9-b828-8f0651da76ed"));

            migrationBuilder.DeleteData(
                table: "ClaimTypes",
                keyColumn: "Id",
                keyValue: new Guid("f671b0d7-ee85-48d8-a58b-0029660b2cad"));

            migrationBuilder.DeleteData(
                table: "ClaimTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd97a4df-40ef-4581-b5bf-983d56e70b3d"));
        }
    }
}
