using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherDocumentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtherDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtherDocumentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Year = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SAPCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoanNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherDocuments_OtherDocumentTypes_OtherDocumentTypeId",
                        column: x => x.OtherDocumentTypeId,
                        principalTable: "OtherDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OtherDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OtherDocumentFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtherDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDocumentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherDocumentFiles_OtherDocuments_OtherDocumentId",
                        column: x => x.OtherDocumentId,
                        principalTable: "OtherDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OtherDocumentTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Name", "NameFr", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("25364bf0-87ae-4a5e-9897-c1eb5351ad5f"), new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "System", true, "Other", "Autre", null, null },
                    { new Guid("eae933da-22e9-4735-96c4-4483157bb27f"), new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "System", true, "specimen signature", "Spécimen de signature", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherDocumentFiles_OtherDocumentId",
                table: "OtherDocumentFiles",
                column: "OtherDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherDocuments_OtherDocumentTypeId",
                table: "OtherDocuments",
                column: "OtherDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherDocuments_UserId",
                table: "OtherDocuments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtherDocumentFiles");

            migrationBuilder.DropTable(
                name: "OtherDocuments");

            migrationBuilder.DropTable(
                name: "OtherDocumentTypes");
        }
    }
}
