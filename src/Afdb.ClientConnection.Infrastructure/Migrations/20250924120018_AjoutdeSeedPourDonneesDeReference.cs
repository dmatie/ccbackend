using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjoutdeSeedPourDonneesDeReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Functions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "BusinessProfiles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Borrower", true, "Borrower", null, null },
                    { new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Guarantor", true, "Technical Advisor", null, null },
                    { new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Agency responsible for execution of projects", true, "Executing Agency", null, null }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Name", "NameFr", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("034d456d-1003-4b0f-8a10-826d6284a514"), "ZW", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Zimbabwe", "Zimbabwe", null, null },
                    { new Guid("06aac45f-afb8-4e16-bcb6-cb25f50f6c9a"), "KM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Comoros", "Comores", null, null },
                    { new Guid("1109d7e7-2f27-4da3-b3e8-46c2518412a7"), "ZM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Zambia", "Zambie", null, null },
                    { new Guid("143fb95d-f41d-4af3-ac51-10f720f546a8"), "NG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Nigeria", "Nigeria", null, null },
                    { new Guid("16bf844f-81e8-409b-b6c0-066d46ef2047"), "DZ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Algeria", "Algérie", null, null },
                    { new Guid("201e24e7-f4b4-4702-9556-2e2bcb5b2dc0"), "GN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Guinea", "Guinée", null, null },
                    { new Guid("20c99637-afcc-44d1-843e-c5f1866d0dd2"), "SL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Sierra Leone", "Sierra Leone", null, null },
                    { new Guid("262a9bab-e842-4e51-b0c2-c4ab56184e2b"), "GH", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Ghana", "Ghana", null, null },
                    { new Guid("278c7840-dd64-4733-b370-d06ba365e4bd"), "BI", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Burundi", "Burundi", null, null },
                    { new Guid("28f8e661-ef53-4266-baea-f943459ed3a0"), "MW", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Malawi", "Malawi", null, null },
                    { new Guid("2e593d13-82ed-4d18-80ea-26393f4071c2"), "DJ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Djibouti", "Djibouti", null, null },
                    { new Guid("2fdcdcb5-2f19-4232-a8d0-b80fa4e59dd1"), "SC", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Seychelles", "Seychelles", null, null },
                    { new Guid("3170066c-7064-4b49-af7e-dc87d8c9cbf2"), "LR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Liberia", "Liberia", null, null },
                    { new Guid("3239c809-8568-4a03-960f-4b0f568a3ada"), "MR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Mauritania", "Mauritanie", null, null },
                    { new Guid("32bcc345-48b0-4b88-b1b5-bc4120e42943"), "ER", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Eritrea", "Érythrée", null, null },
                    { new Guid("337112f4-e35e-44b8-9659-15dfafe0aa55"), "SN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Senegal", "Sénégal", null, null },
                    { new Guid("3e1e98d5-e342-45af-8919-8d27caeddd11"), "MA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Morocco", "Maroc", null, null },
                    { new Guid("3f9dc2e8-6c08-4320-b408-70b197db214c"), "EG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Egypt", "Égypte", null, null },
                    { new Guid("40d52c17-ddbb-4191-9aad-095dc1f3afec"), "CM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Cameroon", "Cameroun", null, null },
                    { new Guid("44a832f3-9a2c-4360-bd0c-d197d91c7ffc"), "BJ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Benin", "Bénin", null, null },
                    { new Guid("456881e5-2731-447a-8221-41f59cadf64a"), "TG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Togo", "Togo", null, null },
                    { new Guid("45a74a51-ce4c-419e-a897-736c80c98ac3"), "ZA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "South Africa", "Afrique du Sud", null, null },
                    { new Guid("45cb5526-d90f-4fb2-aecc-bea64e04cc61"), "BF", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Burkina Faso", "Burkina Faso", null, null },
                    { new Guid("4ba00dd3-e832-42d1-8db7-d1ddd82a4977"), "ET", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Ethiopia", "Éthiopie", null, null },
                    { new Guid("4ec7011e-9148-4383-8906-9d7ba8ed4aea"), "CI", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Côte d'Ivoire", "Côte d'Ivoire", null, null },
                    { new Guid("50de7424-4bb4-4f18-81d4-8c445f3c1571"), "TN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Tunisia", "Tunisie", null, null },
                    { new Guid("54c29f69-8e0e-4b4c-a937-301a3507c1ac"), "SD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Sudan", "Soudan", null, null },
                    { new Guid("55ffe5b5-0dce-4242-a1c9-6cc9b31c6b48"), "AO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Angola", "Angola", null, null },
                    { new Guid("57d2a2c0-97f2-4d76-8509-335e85deea75"), "NE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Niger", "Niger", null, null },
                    { new Guid("5c82037c-98be-4d65-8fbb-4ae7d615caa2"), "MU", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Mauritius", "Maurice", null, null },
                    { new Guid("5d290047-6a5c-473e-aa21-9fe0a6db6376"), "ST", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Sao Tome and Principe", "Sao Tomé-et-Principe", null, null },
                    { new Guid("6bc83388-ec40-48e0-bc3d-2c6b7111cafa"), "UG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Uganda", "Ouganda", null, null },
                    { new Guid("7f1f72df-fc52-468e-86dd-e10908252ab3"), "CD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Congo (Democratic Republic)", "République démocratique du Congo", null, null },
                    { new Guid("8256266a-c436-4e12-899f-c95781b053e1"), "GA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Gabon", "Gabon", null, null },
                    { new Guid("88a7942f-42da-488d-9cbf-1651744d8fe7"), "SS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "South Sudan", "Soudan du Sud", null, null },
                    { new Guid("8ccc2bc5-8cb0-4e52-aa8f-f296d076f6a0"), "MG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Madagascar", "Madagascar", null, null },
                    { new Guid("8dddc3ca-8919-4c59-84cf-8a6af2f4144e"), "GM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Gambia", "Gambie", null, null },
                    { new Guid("93c5b5b9-a548-4ff2-8077-74f29bb41b78"), "ML", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Mali", "Mali", null, null },
                    { new Guid("999588cd-23b2-4624-8903-7c95adb94377"), "CF", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Central African Republic", "République centrafricaine", null, null },
                    { new Guid("9f95189f-6be4-445e-8bed-11abd207398f"), "CV", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Cabo Verde", "Cap-Vert", null, null },
                    { new Guid("ac9109d0-1c94-46b4-b1c5-95cecf53304e"), "MZ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Mozambique", "Mozambique", null, null },
                    { new Guid("b8951b42-af7f-40e5-bc53-704d1c5cdf57"), "TZ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Tanzania", "Tanzanie", null, null },
                    { new Guid("b8c61296-af91-4e53-89f3-f4d682b0350b"), "KE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Kenya", "Kenya", null, null },
                    { new Guid("bea01a0f-ae39-4d35-a15a-e84dacb23714"), "LY", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Libya", "Libye", null, null },
                    { new Guid("c941d4ab-cd7d-4225-9113-1ec2009bd627"), "NA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Namibia", "Namibie", null, null },
                    { new Guid("c9de1d01-bdf7-4ea2-8493-c7c3de1e8db6"), "GQ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Equatorial Guinea", "Guinée équatoriale", null, null },
                    { new Guid("d62e490e-fc7a-43c8-aa32-05a1d0980110"), "LS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Lesotho", "Lesotho", null, null },
                    { new Guid("ddd70b66-ef0a-40be-9bf9-c601aa5ba34d"), "RW", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Rwanda", "Rwanda", null, null },
                    { new Guid("e64b4cd1-b88c-4d8f-9560-7a2afba4276e"), "SO", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Somalia", "Somalie", null, null },
                    { new Guid("eaa3d3ff-8a69-4207-8b9a-9fee511707a0"), "TD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Chad", "Tchad", null, null },
                    { new Guid("f5b32999-f5f4-43a0-a005-03150707509e"), "BW", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Botswana", "Botswana", null, null },
                    { new Guid("f78bc4c9-5d9d-4f75-89ec-44f40034be09"), "CG", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Congo", "République du Congo", null, null },
                    { new Guid("ff0551d8-5783-4b7a-a064-9bbb454e3c78"), "GW", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, "Guinea-Bissau", "Guinée-Bissau", null, null }
                });

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"), "ADB_DESK_OFFICE", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "African Development Bank Desk Office", true, "ADB Desk Office", null, null },
                    { new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"), "DIRECTOR_FIN_ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Director responsible for finance and administration", true, "Director/Finance and Administrative Manager", null, null },
                    { new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"), "MINISTRY_STAFF", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Staff from the Ministry", true, "Ministry Staff", null, null },
                    { new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"), "PROJECT_ACCOUNTANT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Accountant working on projects", true, "Project Accountant", null, null },
                    { new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"), "PROJECT_COORDINATOR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Coordinator for project activities", true, "Project Coordinator", null, null },
                    { new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"), "OTHER", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Other functions", true, "Other", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Functions_Code",
                table: "Functions",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Functions_Code",
                table: "Functions");

            migrationBuilder.DeleteData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"));

            migrationBuilder.DeleteData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"));

            migrationBuilder.DeleteData(
                table: "BusinessProfiles",
                keyColumn: "Id",
                keyValue: new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("034d456d-1003-4b0f-8a10-826d6284a514"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("06aac45f-afb8-4e16-bcb6-cb25f50f6c9a"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("1109d7e7-2f27-4da3-b3e8-46c2518412a7"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("143fb95d-f41d-4af3-ac51-10f720f546a8"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("16bf844f-81e8-409b-b6c0-066d46ef2047"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("201e24e7-f4b4-4702-9556-2e2bcb5b2dc0"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("20c99637-afcc-44d1-843e-c5f1866d0dd2"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("262a9bab-e842-4e51-b0c2-c4ab56184e2b"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("278c7840-dd64-4733-b370-d06ba365e4bd"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("28f8e661-ef53-4266-baea-f943459ed3a0"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("2e593d13-82ed-4d18-80ea-26393f4071c2"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("2fdcdcb5-2f19-4232-a8d0-b80fa4e59dd1"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("3170066c-7064-4b49-af7e-dc87d8c9cbf2"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("3239c809-8568-4a03-960f-4b0f568a3ada"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("32bcc345-48b0-4b88-b1b5-bc4120e42943"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("337112f4-e35e-44b8-9659-15dfafe0aa55"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("3e1e98d5-e342-45af-8919-8d27caeddd11"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("3f9dc2e8-6c08-4320-b408-70b197db214c"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("40d52c17-ddbb-4191-9aad-095dc1f3afec"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("44a832f3-9a2c-4360-bd0c-d197d91c7ffc"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("456881e5-2731-447a-8221-41f59cadf64a"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("45a74a51-ce4c-419e-a897-736c80c98ac3"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("45cb5526-d90f-4fb2-aecc-bea64e04cc61"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("4ba00dd3-e832-42d1-8db7-d1ddd82a4977"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("4ec7011e-9148-4383-8906-9d7ba8ed4aea"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("50de7424-4bb4-4f18-81d4-8c445f3c1571"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("54c29f69-8e0e-4b4c-a937-301a3507c1ac"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("55ffe5b5-0dce-4242-a1c9-6cc9b31c6b48"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("57d2a2c0-97f2-4d76-8509-335e85deea75"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("5c82037c-98be-4d65-8fbb-4ae7d615caa2"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("5d290047-6a5c-473e-aa21-9fe0a6db6376"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("6bc83388-ec40-48e0-bc3d-2c6b7111cafa"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("7f1f72df-fc52-468e-86dd-e10908252ab3"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("8256266a-c436-4e12-899f-c95781b053e1"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("88a7942f-42da-488d-9cbf-1651744d8fe7"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("8ccc2bc5-8cb0-4e52-aa8f-f296d076f6a0"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("8dddc3ca-8919-4c59-84cf-8a6af2f4144e"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("93c5b5b9-a548-4ff2-8077-74f29bb41b78"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("999588cd-23b2-4624-8903-7c95adb94377"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("9f95189f-6be4-445e-8bed-11abd207398f"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("ac9109d0-1c94-46b4-b1c5-95cecf53304e"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("b8951b42-af7f-40e5-bc53-704d1c5cdf57"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("b8c61296-af91-4e53-89f3-f4d682b0350b"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("bea01a0f-ae39-4d35-a15a-e84dacb23714"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("c941d4ab-cd7d-4225-9113-1ec2009bd627"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("c9de1d01-bdf7-4ea2-8493-c7c3de1e8db6"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("d62e490e-fc7a-43c8-aa32-05a1d0980110"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("ddd70b66-ef0a-40be-9bf9-c601aa5ba34d"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("e64b4cd1-b88c-4d8f-9560-7a2afba4276e"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("eaa3d3ff-8a69-4207-8b9a-9fee511707a0"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("f5b32999-f5f4-43a0-a005-03150707509e"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("f78bc4c9-5d9d-4f75-89ec-44f40034be09"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("ff0551d8-5783-4b7a-a064-9bbb454e3c78"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"));

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Functions");
        }
    }
}
