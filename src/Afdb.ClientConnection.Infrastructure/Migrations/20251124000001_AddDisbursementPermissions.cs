using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Afdb.ClientConnection.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisbursementPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisbursementPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanConsult = table.Column<bool>(type: "bit", nullable: false),
                    CanSubmit = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisbursementPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisbursementPermissions_BusinessProfiles_BusinessProfileId",
                        column: x => x.BusinessProfileId,
                        principalTable: "BusinessProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisbursementPermissions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementPermissions_BusinessProfile_Function",
                table: "DisbursementPermissions",
                columns: new[] { "BusinessProfileId", "FunctionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisbursementPermissions_FunctionId",
                table: "DisbursementPermissions",
                column: "FunctionId");

            migrationBuilder.Sql(@"
                -- Get IDs for BusinessProfiles
                DECLARE @ExecutingAgencyId UNIQUEIDENTIFIER = (SELECT Id FROM BusinessProfiles WHERE Name = 'Executing Agency')
                DECLARE @BorrowerId UNIQUEIDENTIFIER = (SELECT Id FROM BusinessProfiles WHERE Name = 'Borrower')
                DECLARE @GarantorId UNIQUEIDENTIFIER = (SELECT Id FROM BusinessProfiles WHERE Name = 'Garantor')

                -- Get IDs for Functions
                DECLARE @AdbDeskId UNIQUEIDENTIFIER = (SELECT Id FROM Functions WHERE Name = 'ADB desk Office')
                DECLARE @DirectorId UNIQUEIDENTIFIER = (SELECT Id FROM Functions WHERE Name = 'Director / Finance and Administrative manager')
                DECLARE @MinistryId UNIQUEIDENTIFIER = (SELECT Id FROM Functions WHERE Name = 'Ministry Staff')
                DECLARE @AccountantId UNIQUEIDENTIFIER = (SELECT Id FROM Functions WHERE Name = 'Project Accountant')
                DECLARE @CoordinatorId UNIQUEIDENTIFIER = (SELECT Id FROM Functions WHERE Name = 'Project Coordinator')
                DECLARE @OthersId UNIQUEIDENTIFIER = (SELECT Id FROM Functions WHERE Name = 'Others')

                -- ADB desk Office permissions
                INSERT INTO DisbursementPermissions (Id, BusinessProfileId, FunctionId, CanConsult, CanSubmit, CreatedDate, CreatedBy)
                VALUES
                    (NEWID(), @ExecutingAgencyId, @AdbDeskId, 1, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @BorrowerId, @AdbDeskId, 1, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @GarantorId, @AdbDeskId, 1, 0, GETUTCDATE(), 'System')

                -- Director / Finance and Administrative manager permissions
                INSERT INTO DisbursementPermissions (Id, BusinessProfileId, FunctionId, CanConsult, CanSubmit, CreatedDate, CreatedBy)
                VALUES
                    (NEWID(), @ExecutingAgencyId, @DirectorId, 1, 1, GETUTCDATE(), 'System'),
                    (NEWID(), @BorrowerId, @DirectorId, 1, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @GarantorId, @DirectorId, 0, 0, GETUTCDATE(), 'System')

                -- Ministry Staff permissions
                INSERT INTO DisbursementPermissions (Id, BusinessProfileId, FunctionId, CanConsult, CanSubmit, CreatedDate, CreatedBy)
                VALUES
                    (NEWID(), @ExecutingAgencyId, @MinistryId, 0, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @BorrowerId, @MinistryId, 1, 1, GETUTCDATE(), 'System'),
                    (NEWID(), @GarantorId, @MinistryId, 1, 0, GETUTCDATE(), 'System')

                -- Project Accountant permissions
                INSERT INTO DisbursementPermissions (Id, BusinessProfileId, FunctionId, CanConsult, CanSubmit, CreatedDate, CreatedBy)
                VALUES
                    (NEWID(), @ExecutingAgencyId, @AccountantId, 1, 1, GETUTCDATE(), 'System'),
                    (NEWID(), @BorrowerId, @AccountantId, 0, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @GarantorId, @AccountantId, 0, 0, GETUTCDATE(), 'System')

                -- Project Coordinator permissions
                INSERT INTO DisbursementPermissions (Id, BusinessProfileId, FunctionId, CanConsult, CanSubmit, CreatedDate, CreatedBy)
                VALUES
                    (NEWID(), @ExecutingAgencyId, @CoordinatorId, 1, 1, GETUTCDATE(), 'System'),
                    (NEWID(), @BorrowerId, @CoordinatorId, 0, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @GarantorId, @CoordinatorId, 0, 0, GETUTCDATE(), 'System')

                -- Others permissions
                INSERT INTO DisbursementPermissions (Id, BusinessProfileId, FunctionId, CanConsult, CanSubmit, CreatedDate, CreatedBy)
                VALUES
                    (NEWID(), @ExecutingAgencyId, @OthersId, 1, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @BorrowerId, @OthersId, 1, 0, GETUTCDATE(), 'System'),
                    (NEWID(), @GarantorId, @OthersId, 1, 0, GETUTCDATE(), 'System')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisbursementPermissions");
        }
    }
}
