using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisbursmentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Disbursements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SapCodeProject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LoanGrantNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    DisbursementTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disbursements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disbursements_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Disbursements_DisbursementTypes_DisbursementTypeId",
                        column: x => x.DisbursementTypeId,
                        principalTable: "DisbursementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disbursements_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disbursements_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementA1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentPurpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BeneficiaryBpNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BeneficiaryCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeneficiaryEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorrespondentBankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorrespondentBankAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CorrespondentBankCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrespondantAccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorrespondentBankSwiftCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SignatoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SignatoryContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SignatoryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SignatoryCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SignatoryEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SignatoryPhone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SignatoryTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementA1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementA1_Countries_BeneficiaryCountryId",
                        column: x => x.BeneficiaryCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementA1_Countries_CorrespondentBankCountryId",
                        column: x => x.CorrespondentBankCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementA1_Countries_SignatoryCountryId",
                        column: x => x.SignatoryCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementA1_Disbursements_DisbursementId",
                        column: x => x.DisbursementId,
                        principalTable: "Disbursements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementA2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReimbursementPurpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Contractor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GoodDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GoodOriginCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractBorrowerReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContractAfDBReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContractValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContractBankShare = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContractAmountPreviouslyPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InvoiceRef = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDateOfPayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentAmountWithdrawn = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentEvidenceOfPayment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementA2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementA2_Countries_GoodOriginCountryId",
                        column: x => x.GoodOriginCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementA2_Disbursements_DisbursementId",
                        column: x => x.DisbursementId,
                        principalTable: "Disbursements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementA3",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodForUtilization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ItemNumber = table.Column<int>(type: "int", nullable: false),
                    GoodDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GoodOriginCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoodQuantity = table.Column<int>(type: "int", nullable: false),
                    AnnualBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdvanceRequested = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateOfApproval = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementA3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementA3_Countries_GoodOriginCountryId",
                        column: x => x.GoodOriginCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementA3_Disbursements_DisbursementId",
                        column: x => x.DisbursementId,
                        principalTable: "Disbursements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementB1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuaranteeDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ConfirmingBank = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IssuingBankName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IssuingBankAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GuaranteeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryBPNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryAFDBContract = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryBankAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BeneficiaryCity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoodDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BeneficiaryLcContractRef = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExecutingAgencyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExecutingAgencyContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExecutingAgencyAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExecutingAgencyCity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExecutingAgencyCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutingAgencyEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExecutingAgencyPhone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementB1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementB1_Countries_BeneficiaryCountryId",
                        column: x => x.BeneficiaryCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementB1_Countries_ExecutingAgencyCountryId",
                        column: x => x.ExecutingAgencyCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DisbursementB1_Disbursements_DisbursementId",
                        column: x => x.DisbursementId,
                        principalTable: "Disbursements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementDocuments_Disbursements_DisbursementId",
                        column: x => x.DisbursementId,
                        principalTable: "Disbursements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisbursementProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisbursementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementProcesses_Disbursements_DisbursementId",
                        column: x => x.DisbursementId,
                        principalTable: "Disbursements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisbursementProcesses_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Name", "Symbol", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("03bb614c-f288-4924-b247-6671b00b94b9"), "GBP", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "British Pound", "£", null, null },
                    { new Guid("51128130-44f4-4243-865d-9be6c530cf3b"), "JPY", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Japanese Yen", "¥", null, null },
                    { new Guid("6e087735-4cab-40fe-aa2e-1482b6ca9452"), "XOF", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Franc CFA", "CFA", null, null },
                    { new Guid("9f66cbd8-a197-40a1-ad5e-6d060f20b400"), "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "US Dollar", "$", null, null },
                    { new Guid("e11e7997-21cf-4310-a4d2-568d20caab63"), "EUR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Euro", "€", null, null }
                });

            migrationBuilder.InsertData(
                table: "DisbursementTypes",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "Name", "NameFr", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("3fcde576-d6e1-4145-a52e-0da14f8e08c1"), "B1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Description for Type 6 Disbursement", "Reimbursement of a guarantee", "Remboursement d'une garantie", null, null },
                    { new Guid("65616f7b-b385-45f7-a5fc-87dbf5a68d3b"), "A1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Description for Type 1 Disbursement", "Direct payment", "Paiement direct", null, null },
                    { new Guid("691ca504-2252-4039-a4cc-86228432ccea"), "A1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Description for Type 2 Disbursement", "Replenishment of a Special Account", "Réapprovisionnement d'un compte spécial", null, null },
                    { new Guid("76f95c38-b800-4f4c-a0c3-cada31703bd5"), "A3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Description for Type 5 Disbursement", "Estimate Budgeted Activities", "Estimation des activités budgétisées", null, null },
                    { new Guid("84e690ed-c29c-4b34-b113-34a5c5d61b6f"), "A1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Description for Type 3 Disbursement", "Justification only", "Justification", null, null },
                    { new Guid("9509bfe3-5ba7-4302-b7fb-11ca7007fa39"), "A2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Description for Type 4 Disbursement", "Declare Expenditures", "Déclarer les dépenses", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA1_BeneficiaryCountryId",
                table: "DisbursementA1",
                column: "BeneficiaryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA1_CorrespondentBankCountryId",
                table: "DisbursementA1",
                column: "CorrespondentBankCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA1_DisbursementId",
                table: "DisbursementA1",
                column: "DisbursementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA1_SignatoryCountryId",
                table: "DisbursementA1",
                column: "SignatoryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA2_DisbursementId",
                table: "DisbursementA2",
                column: "DisbursementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA2_GoodOriginCountryId",
                table: "DisbursementA2",
                column: "GoodOriginCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA3_DisbursementId",
                table: "DisbursementA3",
                column: "DisbursementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementA3_GoodOriginCountryId",
                table: "DisbursementA3",
                column: "GoodOriginCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementB1_BeneficiaryCountryId",
                table: "DisbursementB1",
                column: "BeneficiaryCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementB1_DisbursementId",
                table: "DisbursementB1",
                column: "DisbursementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementB1_ExecutingAgencyCountryId",
                table: "DisbursementB1",
                column: "ExecutingAgencyCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementDocuments_DisbursementId",
                table: "DisbursementDocuments",
                column: "DisbursementId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementProcesses_CreatedByUserId",
                table: "DisbursementProcesses",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementProcesses_DisbursementId",
                table: "DisbursementProcesses",
                column: "DisbursementId");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_CreatedByUserId",
                table: "Disbursements",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_CurrencyId",
                table: "Disbursements",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_DisbursementTypeId",
                table: "Disbursements",
                column: "DisbursementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_ProcessedByUserId",
                table: "Disbursements",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_RequestNumber",
                table: "Disbursements",
                column: "RequestNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementTypes_Code",
                table: "DisbursementTypes",
                column: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisbursementA1");

            migrationBuilder.DropTable(
                name: "DisbursementA2");

            migrationBuilder.DropTable(
                name: "DisbursementA3");

            migrationBuilder.DropTable(
                name: "DisbursementB1");

            migrationBuilder.DropTable(
                name: "DisbursementDocuments");

            migrationBuilder.DropTable(
                name: "DisbursementProcesses");

            migrationBuilder.DropTable(
                name: "Disbursements");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "DisbursementTypes");
        }
    }
}
