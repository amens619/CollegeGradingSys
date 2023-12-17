using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAdminPersonalDataPolicies : Migration
    {
        const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
           table: "AspNetUserClaims",
           columns: new[] { "UserId", "ClaimType", "ClaimValue" },
           values: new object[] { ADMIN_USER_GUID, "Create StPersonalData", "إضافة طالب" }
           );
            migrationBuilder.InsertData(
        table: "AspNetUserClaims",
        columns: new[] { "UserId", "ClaimType", "ClaimValue" },
        values: new object[] { ADMIN_USER_GUID, "Edit StPersonalData", "تعديل طالب" }
        );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete StPersonalData", "حذف طالب" }
          );
            migrationBuilder.InsertData(
         table: "AspNetUserClaims",
         columns: new[] { "UserId", "ClaimType", "ClaimValue" },
         values: new object[] { ADMIN_USER_GUID, "Details StPersonalData", "عرض تفاصيل طالب" }
         );

            migrationBuilder.InsertData(
        table: "AspNetUserClaims",
        columns: new[] { "UserId", "ClaimType", "ClaimValue" },
        values: new object[] { ADMIN_USER_GUID, "Export AcceptedStToExcel", "تصدير المقبولين" }
        );
            migrationBuilder.InsertData(
         table: "AspNetUserClaims",
         columns: new[] { "UserId", "ClaimType", "ClaimValue" },
         values: new object[] { ADMIN_USER_GUID, "Export SthighSchoolToExcel", "تصدير من واقع الثانوية العامة" }
         );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserClaims WHERE UserId='{ADMIN_USER_GUID}'");
        }
    }
}
