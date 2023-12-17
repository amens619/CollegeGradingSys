using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAdminAccount : Migration
    {
        
            const string ADMIN_USER_GUID = "9512fbee-0dd3-4fe1-83a8-28c875393a26";
          
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<IdentityUser>();
            var passwordhash = hasher.HashPassword(null, "save4964!");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{ADMIN_USER_GUID}'");
            sb.AppendLine(",'Admin@Admin.com'");
            sb.AppendLine(",'ADMIN@ADMIN.COM'");
            sb.AppendLine(",'Admin@Admin.com'");
            sb.AppendLine(",'ADMIN@ADMIN.COM'");
            sb.AppendLine(",'False'");
            sb.AppendLine($",'{passwordhash}'");
            sb.AppendLine(",''");
            sb.AppendLine(",''");
            sb.AppendLine(",NULL");
            sb.AppendLine(",'False'");
            sb.AppendLine(",'False'");
            sb.AppendLine(",NULL");
            sb.AppendLine(",'True'");
            sb.AppendLine(",0");           
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id='{ADMIN_USER_GUID}'");

        }
    }
}
