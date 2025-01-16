using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddDataToGeneralInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                       table: "GeneralInfo",
                       columns: new[] { "StDepartmentHead", "BranchHeadName", "BackupPath" },
                       values: new object[] { "د / أحمد سقاف العيدروس", "د / أنور رمضان مسيعد"," " }
                       );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [GeneralInfo]");
        }
    }
}
