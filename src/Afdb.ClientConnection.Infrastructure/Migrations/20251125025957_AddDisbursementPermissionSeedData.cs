using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisbursementPermissionSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DisbursementPermissions",
                columns: new[] { "Id", "BusinessProfileId", "CanConsult", "CanSubmit", "CreatedAt", "CreatedBy", "FunctionId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("09756fce-635d-4f08-8b85-1736422cba0c"), new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("0cf86a66-7a42-4e50-b2b7-a61054d12c3f"), new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), true, true, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("1e594e22-e012-45fd-88e3-1daa265f13fa"), new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("26f59882-bcf8-4bcf-8e50-823d19f15b4a"), new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), true, true, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("49521407-96c9-4cc7-b999-b41e19ab9607"), new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("4b48131f-905d-4506-9507-b5d04cdb8838"), new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("4f041a12-bcfb-43fc-a237-a6c4b4ba56bf"), new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("4ff93882-e9fe-4ce8-a05a-d2b580fca08d"), new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("5fba02aa-b580-4063-97f1-35c9e809c376"), new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("8a8a0293-423c-44ec-9232-4425f89fb462"), new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), true, true, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("9854db95-9043-44e6-b434-0bdde60127f0"), new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("a25e56c8-3d75-4f79-a77e-16f68a8aef01"), new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), true, true, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("a447a675-f46e-4861-878b-1bd897afe690"), new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("aa7e6455-4596-40fe-8f70-495d2f4dba08"), new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("ad736711-7595-499c-a8ab-2c2c2cd698d2"), new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("b07960b3-57cf-45cd-9bd9-90072cfa1c49"), new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("c58e8741-5bfe-4073-a136-1688cf0f7ce5"), new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"), null, null },
                    { new Guid("f771857e-fa6b-4f0a-a13c-2d3c032c9d4f"), new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"), null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("09756fce-635d-4f08-8b85-1736422cba0c"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("0cf86a66-7a42-4e50-b2b7-a61054d12c3f"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("1e594e22-e012-45fd-88e3-1daa265f13fa"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("26f59882-bcf8-4bcf-8e50-823d19f15b4a"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("49521407-96c9-4cc7-b999-b41e19ab9607"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("4b48131f-905d-4506-9507-b5d04cdb8838"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("4f041a12-bcfb-43fc-a237-a6c4b4ba56bf"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("4ff93882-e9fe-4ce8-a05a-d2b580fca08d"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("5fba02aa-b580-4063-97f1-35c9e809c376"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("8a8a0293-423c-44ec-9232-4425f89fb462"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("9854db95-9043-44e6-b434-0bdde60127f0"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("a25e56c8-3d75-4f79-a77e-16f68a8aef01"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("a447a675-f46e-4861-878b-1bd897afe690"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("aa7e6455-4596-40fe-8f70-495d2f4dba08"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("ad736711-7595-499c-a8ab-2c2c2cd698d2"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("b07960b3-57cf-45cd-9bd9-90072cfa1c49"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("c58e8741-5bfe-4073-a136-1688cf0f7ce5"));

            migrationBuilder.DeleteData(
                table: "DisbursementPermissions",
                keyColumn: "Id",
                keyValue: new Guid("f771857e-fa6b-4f0a-a13c-2d3c032c9d4f"));
        }
    }
}
