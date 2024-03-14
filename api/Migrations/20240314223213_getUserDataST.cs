using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class getUserDataST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procedure = @"
        CREATE OR ALTER PROCEDURE dbo.GetUserData
            @role NVARCHAR(50)
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
            REVERT;
        END;
        GO";

            migrationBuilder.Sql(procedure);
            migrationBuilder.DropTable(
                name: "WeatherForecast");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcedure = @"DROP PROCEDURE IF EXISTS dbo.GetUserData";
            migrationBuilder.Sql(dropProcedure);
            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemperatureC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast", x => x.Id);
                });
        }
    }
}
