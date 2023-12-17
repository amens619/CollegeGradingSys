using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAdminStAcademicDataPolicies : Migration
    {
        const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Export StsGradesToExcel", "بيان درجات" }
          );
            migrationBuilder.InsertData(
        table: "AspNetUserClaims",
        columns: new[] { "UserId", "ClaimType", "ClaimValue" },
        values: new object[] { ADMIN_USER_GUID, "Export StAcademicDataToExcel", "كشف المقيدين" }
        );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Export GraduateStToExcel", "كشف الخريجين" }
          );
            migrationBuilder.InsertData(
         table: "AspNetUserClaims",
         columns: new[] { "UserId", "ClaimType", "ClaimValue" },
         values: new object[] { ADMIN_USER_GUID, "Add AcademicDataForAllSts", "تقييد جميع طلاب العام الماضي" }
         );

            migrationBuilder.InsertData(
        table: "AspNetUserClaims",
        columns: new[] { "UserId", "ClaimType", "ClaimValue" },
        values: new object[] { ADMIN_USER_GUID, "All StAcademicData", "السجل كامل" }
        );
            migrationBuilder.InsertData(
         table: "AspNetUserClaims",
         columns: new[] { "UserId", "ClaimType", "ClaimValue" },
         values: new object[] { ADMIN_USER_GUID, "Graduation Certificate", "شهادة التخرج" }
         );

            migrationBuilder.InsertData(
       table: "AspNetUserClaims",
       columns: new[] { "UserId", "ClaimType", "ClaimValue" },
       values: new object[] { ADMIN_USER_GUID, "Graduation Statement", "افادة" }
       );

            migrationBuilder.InsertData(
       table: "AspNetUserClaims",
       columns: new[] { "UserId", "ClaimType", "ClaimValue" },
       values: new object[] { ADMIN_USER_GUID, "Study Confirmation", "تأكيد الدراسة" }
       );

            migrationBuilder.InsertData(
       table: "AspNetUserClaims",
       columns: new[] { "UserId", "ClaimType", "ClaimValue" },
       values: new object[] { ADMIN_USER_GUID, "Statement AfterComprehensive", "افادة بعد الشامل" }
       );

            migrationBuilder.InsertData(
    table: "AspNetUserClaims",
    columns: new[] { "UserId", "ClaimType", "ClaimValue" },
    values: new object[] { ADMIN_USER_GUID, "Statement ThreeYears", "افادة ثلاثة سنوات" }
    );
            migrationBuilder.InsertData(
    table: "AspNetUserClaims",
    columns: new[] { "UserId", "ClaimType", "ClaimValue" },
    values: new object[] { ADMIN_USER_GUID, "Edit StAcademicData", "تعديل السجل الاكاديمي" }
    );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserClaims WHERE UserId='{ADMIN_USER_GUID}'");
        }
    }
}
