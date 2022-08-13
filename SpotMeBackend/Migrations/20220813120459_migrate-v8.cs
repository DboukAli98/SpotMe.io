using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotMeBackend.Migrations
{
    public partial class migratev8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EducationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    InstitutionAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyDomain = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK_Educations_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_ApplicantId",
                table: "Educations",
                column: "ApplicantId");
        }
    }
}
