using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotMeBackend.Migrations
{
    public partial class migratev7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_Applicants_ApplicantId",
                table: "Education");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Education",
                table: "Education");

            migrationBuilder.RenameTable(
                name: "Education",
                newName: "Educations");

            migrationBuilder.RenameIndex(
                name: "IX_Education_ApplicantId",
                table: "Educations",
                newName: "IX_Educations_ApplicantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Educations",
                table: "Educations",
                column: "EducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Applicants_ApplicantId",
                table: "Educations",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Applicants_ApplicantId",
                table: "Educations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Educations",
                table: "Educations");

            migrationBuilder.RenameTable(
                name: "Educations",
                newName: "Education");

            migrationBuilder.RenameIndex(
                name: "IX_Educations_ApplicantId",
                table: "Education",
                newName: "IX_Education_ApplicantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Education",
                table: "Education",
                column: "EducationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Applicants_ApplicantId",
                table: "Education",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
