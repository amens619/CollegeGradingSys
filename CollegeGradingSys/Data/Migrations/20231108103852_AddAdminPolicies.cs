using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAdminPolicies : Migration
    {
        const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           


            migrationBuilder.InsertData(
      table: "AspNetUserClaims",
      columns: new[] { "UserId", "ClaimType", "ClaimValue" },
      values: new object[] { ADMIN_USER_GUID, "Create College", "إضافة كلية" }
      );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit College", "تعديل كلية" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete College", "حذف كلية" }
          );




            migrationBuilder.InsertData(
      table: "AspNetUserClaims",
      columns: new[] { "UserId", "ClaimType", "ClaimValue" },
      values: new object[] { ADMIN_USER_GUID, "Create Department", "إضافة قسم" }
      );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit Department", "تعديل قسم" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Department", "حذف قسم" }
          );




            migrationBuilder.InsertData(
      table: "AspNetUserClaims",
      columns: new[] { "UserId", "ClaimType", "ClaimValue" },
      values: new object[] { ADMIN_USER_GUID, "Create Specialization", "إضافة تخصص" }
      );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit Specialization", "تعديل تخصص" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Specialization", "حذف تخصص" }
          );



            migrationBuilder.InsertData(
      table: "AspNetUserClaims",
      columns: new[] { "UserId", "ClaimType", "ClaimValue" },
      values: new object[] { ADMIN_USER_GUID, "Create Nationality", "إضافة دولة" }
      );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit Nationality", "تعديل دولة" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Nationality", "حذف دولة" }
          );



            migrationBuilder.InsertData(
      table: "AspNetUserClaims",
      columns: new[] { "UserId", "ClaimType", "ClaimValue" },
      values: new object[] { ADMIN_USER_GUID, "Create Governorate", "إضافة محافظة" }
      );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit Governorate", "تعديل محافظة" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Governorate", "حذف محافظة" }
          );





            migrationBuilder.InsertData(
           table: "AspNetUserClaims",
           columns: new[] { "UserId", "ClaimType", "ClaimValue" },
           values: new object[] { ADMIN_USER_GUID, "Create AcademicYear", "إضافة عام اكاديمي" }
           );
            migrationBuilder.InsertData(
        table: "AspNetUserClaims",
        columns: new[] { "UserId", "ClaimType", "ClaimValue" },
        values: new object[] { ADMIN_USER_GUID, "Edit AcademicYear", "تعديل عام اكاديمي" }
        );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete AcademicYear", "حذف عام اكاديمي" }
          );
            migrationBuilder.InsertData(
         table: "AspNetUserClaims",
         columns: new[] { "UserId", "ClaimType", "ClaimValue" },
         values: new object[] { ADMIN_USER_GUID, "Details AcademicYear", "عرض تفاصيل اكاديمي" }
         );



            migrationBuilder.InsertData(
      table: "AspNetUserClaims",
      columns: new[] { "UserId", "ClaimType", "ClaimValue" },
      values: new object[] { ADMIN_USER_GUID, "Create Batch", "إضافة دفعة" }
      );
            migrationBuilder.InsertData(
     table: "AspNetUserClaims",
     columns: new[] { "UserId", "ClaimType", "ClaimValue" },
     values: new object[] { ADMIN_USER_GUID, "Edit Batch", "تعديل دفعة" }
     );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Batch", "حذف دفعة" }
          );


                    migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Create Course", "إضافة مادة" }
          );
                migrationBuilder.InsertData(
         table: "AspNetUserClaims",
         columns: new[] { "UserId", "ClaimType", "ClaimValue" },
         values: new object[] { ADMIN_USER_GUID, "Edit Course", "تعديل مادة" }
         );
            migrationBuilder.InsertData(
          table: "AspNetUserClaims",
          columns: new[] { "UserId", "ClaimType", "ClaimValue" },
          values: new object[] { ADMIN_USER_GUID, "Delete Course", "حذف مادة" }
          );

            migrationBuilder.InsertData(
        table: "AspNetUserClaims",
        columns: new[] { "UserId", "ClaimType", "ClaimValue" },
        values: new object[] { ADMIN_USER_GUID, "Details Course", "عرض تفاصيل مادة" }
        );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserClaims WHERE UserId='{ADMIN_USER_GUID}'");
        }
    }
}
