/*
  # Add AccessRequestDocuments table

  1. New Tables
    - `AccessRequestDocuments`
      - `Id` (uniqueidentifier, primary key)
      - `AccessRequestId` (uniqueidentifier, foreign key to AccessRequests)
      - `FileName` (nvarchar(500), required) - Name of the attached document
      - `DocumentUrl` (nvarchar(1000), required) - SharePoint URL where the document is stored
      - `CreatedBy` (nvarchar(200), required) - Email of user who uploaded the document
      - `CreatedAt` (datetime2, required) - Upload timestamp

  2. Security
    - Foreign key relationship with AccessRequests table with cascade delete
    - When an AccessRequest is deleted, all associated documents are automatically deleted

  3. Important Notes
    - This table stores metadata for PDF documents attached during access request submission
    - The actual files are stored in SharePoint, this table only stores references
    - Each access request must have at least one document attached at submission
    - Document uploads are restricted to PDF format only
*/

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessRequestDocumentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessRequestDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRequestDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessRequestDocuments_AccessRequests_AccessRequestId",
                        column: x => x.AccessRequestId,
                        principalTable: "AccessRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequestDocuments_AccessRequestId",
                table: "AccessRequestDocuments",
                column: "AccessRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessRequestDocuments");
        }
    }
}
