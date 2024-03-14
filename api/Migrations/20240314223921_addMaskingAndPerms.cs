using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addMaskingAndPerms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            USE patientinc;
            ALTER TABLE dbo.AspNetUsers
            ALTER COLUMN PhoneNumber ADD MASKED WITH (FUNCTION = 'partial(2,""xxxx"",0)');
            ALTER TABLE dbo.MedicalRecords
            ALTER COLUMN TreatmentPlan ADD MASKED WITH (FUNCTION = 'partial(2,""xxxx"",0)');

            ALTER TABLE dbo.MedicalRecords
            ALTER COLUMN DiagnosisDetails ADD MASKED WITH (FUNCTION = 'partial(2,""xxxx"",0)');

            CREATE USER helpdesk WITHOUT LOGIN;

            CREATE USER patient WITHOUT LOGIN;

            ALTER ROLE db_datareader ADD MEMBER helpdesk;

            ALTER ROLE db_datareader ADD MEMBER patient;

            GRANT UNMASK ON  dbo.MedicalRecords(DiagnosisDetails) TO patient;

            GRANT UNMASK ON  dbo.AspNetUsers(PhoneNumber) TO patient;

            GRANT UNMASK ON  dbo.MedicalRecords(TreatmentPlan) TO patient;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
