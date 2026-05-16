using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddMissingAdminPolicies : Migration
    {
        const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // إضافة الصلاحية المفقودة لعرض درجات المواد
            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { ADMIN_USER_GUID, "All StCourseGrade", "عرض درجات الطالب" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // الحذف المخصص لهذه الصلاحية فقط عند التراجع
            migrationBuilder.Sql($"DELETE FROM AspNetUserClaims WHERE UserId='{ADMIN_USER_GUID}' AND ClaimType='All StCourseGrade'");
        }
    }
}