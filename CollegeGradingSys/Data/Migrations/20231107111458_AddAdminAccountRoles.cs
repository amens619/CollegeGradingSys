using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAdminAccountRoles : Migration
    {
        const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "AspNetUserRoles",
            columns: new[] { "UserId", "RoleId" },
            values: new object[] { ADMIN_USER_GUID, "164a286e-1bb6-4b36-9938-62b3401ec44e" }
            );
            migrationBuilder.InsertData(
            table: "AspNetUserRoles",
            columns: new[] { "UserId", "RoleId" },
            values: new object[] { ADMIN_USER_GUID, "7fa92891-3765-4664-8cfe-445ed8337304" }
            );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId='{ADMIN_USER_GUID}' AND RoleId='764a286e-1bb6-4b36-9938-62b3401ec44e'");
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId='{ADMIN_USER_GUID}' AND RoleId='7fa92891-3765-4664-8cfe-445ed8337304'");

        }
    }
}
