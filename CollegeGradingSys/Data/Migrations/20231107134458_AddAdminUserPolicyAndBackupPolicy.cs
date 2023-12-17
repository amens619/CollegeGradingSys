using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAdminUserPolicyAndBackupPolicy : Migration
    {
        const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
       table: "AspNetUserClaims",
       columns: new[] { "UserId", "ClaimType", "ClaimValue" },
       values: new object[] { ADMIN_USER_GUID, "Create User", "إضافة مستخدم" }
       );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit User", "تعديل مستخدم" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete User", "حذف مستخدم" }
          );

            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Manage User Roles", "إضافة/حذف مسؤولية لهذه المستخدم" }
          );

            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Manage User Claims", "إضافة/حذف صلاحيات" }
          );

            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Create Backup", "اجراء نسخة احتياطية جديدة" }
          );

            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Backup", "حذف نسخة احتياطية" }
          );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserClaims WHERE UserId='{ADMIN_USER_GUID}'");
        }
    }
}
