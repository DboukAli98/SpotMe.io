using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotMeBackend.Migrations
{
    public partial class migratev5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileName",
                table: "Applicants",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "skills",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "ProfileName",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "skills",
                table: "Applicants");
        }
    }
}
