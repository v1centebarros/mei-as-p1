using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class getUserDataByIdST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procedure = @"
            CREATE OR ALTER PROCEDURE dbo.GetUserById
    @role NVARCHAR(50),
    @id NVARCHAR(50)
AS
BEGIN
    IF @role = 'patient'
    BEGIN
        EXECUTE AS USER
        = 'patient';
    END
    ELSE
    BEGIN
        EXECUTE AS USER
        = 'helpdesk';
    END
    

    SELECT 
        [dbo].[Patients].Id,
        [dbo].[Patients].FullName, 
        [dbo].[AspNetUsers].Email,
        [dbo].[AspNetUsers].[PhoneNumber],
        [dbo].[MedicalRecords].DiagnosisDetails, 
        [dbo].[MedicalRecords].MedicalRecordNumber, 
        [dbo].[MedicalRecords].TreatmentPlan
    FROM [dbo].[Patients] INNER JOIN [dbo].[MedicalRecords] ON
        [dbo].[Patients].MedicalRecordId = [dbo].[MedicalRecords].Id
        INNER JOIN [dbo].[AspNetUsers] ON
        [dbo].[Patients].[Id] = [dbo].[AspNetUsers].[Id]
        WHERE [dbo].[AspNetUsers].[Id] = @id
    REVERT;
END;
GO
";
            migrationBuilder.Sql(procedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcedure = @"DROP PROCEDURE IF EXISTS dbo.GetUserById";
            migrationBuilder.Sql(dropProcedure);
        }
    }
}
