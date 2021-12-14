using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class ModifyStAcademicDataEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Average",
                table: "StAcademicData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GPA",
                table: "StAcademicData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentYear",
                table: "StAcademicData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Term",
                table: "StAcademicData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Valuation",
                table: "StAcademicData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Average",
                table: "StAcademicData");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "StAcademicData");

            migrationBuilder.DropColumn(
                name: "IsCurrentYear",
                table: "StAcademicData");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "StAcademicData");

            migrationBuilder.DropColumn(
                name: "Valuation",
                table: "StAcademicData");
        }
    }
}
