using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var command = @"CREATE OR ALTER PROCEDURE dbo.GetUserData
                                @role NVARCHAR(50)
                            AS
                            BEGIN
                                IF @role = 'client'
                                BEGIN
                                    EXECUTE AS USER
                                    = 'client';
                                END
                                ELSE
                                BEGIN
                                    EXECUTE AS USER
                                    = 'helpdesk';
                                END
                                SELECT 
                                Id,FullName, Email, PhoneNumber, TreatmentPlan, DiagnosisDetails, AccessCode
                                FROM dbo.AspNetUsers;
                                REVERT;
                            END;
                            GO";
            migrationBuilder.Sql(command);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.GetUserData");
        }
    }
}
