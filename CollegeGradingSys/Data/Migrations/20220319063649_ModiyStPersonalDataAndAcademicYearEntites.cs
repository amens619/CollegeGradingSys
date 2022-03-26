using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class ModiyStPersonalDataAndAcademicYearEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrollmentYearH",
                table: "StPersonalData");

            migrationBuilder.DropColumn(
                name: "EnrollmentYearM",
                table: "StPersonalData");

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentYearId",
                table: "StPersonalData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearNameH",
                table: "AcademicYear",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_EnrollmentYearId",
                table: "StPersonalData",
                column: "EnrollmentYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_StPersonalData_AcademicYear_EnrollmentYearId",
                table: "StPersonalData",
                column: "EnrollmentYearId",
                principalTable: "AcademicYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StPersonalData_AcademicYear_EnrollmentYearId",
                table: "StPersonalData");

            migrationBuilder.DropIndex(
                name: "IX_StPersonalData_EnrollmentYearId",
                table: "StPersonalData");

            migrationBuilder.DropColumn(
                name: "EnrollmentYearId",
                table: "StPersonalData");

            migrationBuilder.DropColumn(
                name: "AcademicYearNameH",
                table: "AcademicYear");

            migrationBuilder.AddColumn<string>(
                name: "EnrollmentYearH",
                table: "StPersonalData",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EnrollmentYearM",
                table: "StPersonalData",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
