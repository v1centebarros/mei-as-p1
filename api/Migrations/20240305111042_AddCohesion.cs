using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddCohesion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DiagnosisDetails",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MedicalRecordNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TreatmentPlan",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRecordNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnosisDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MedicalRecordId",
                table: "AspNetUsers",
                column: "MedicalRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MedicalRecords_MedicalRecordId",
                table: "AspNetUsers",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MedicalRecords_MedicalRecordId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MedicalRecordId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MedicalRecordId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosisDetails",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalRecordNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentPlan",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
