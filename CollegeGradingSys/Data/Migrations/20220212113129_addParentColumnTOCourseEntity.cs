using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class addParentColumnTOCourseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Course_ParentId",
                table: "Course",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Course_ParentId",
                table: "Course",
                column: "ParentId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Course_ParentId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_ParentId",
                table: "Course");
        }
    }
}
