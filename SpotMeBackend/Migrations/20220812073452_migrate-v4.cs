using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotMeBackend.Migrations
{
    public partial class migratev4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Recruiters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileName",
                table: "Recruiters",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Recruiters");

            migrationBuilder.DropColumn(
                name: "ProfileName",
                table: "Recruiters");
        }
    }
}
