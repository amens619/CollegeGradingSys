using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
            values: new object[] { "164a286e-1bb6-4b36-9938-62b3401ec44e", "Owner", "Owner".ToUpper(), Guid.NewGuid().ToString() }
            );
            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { "7fa92891-3765-4664-8cfe-445ed8337304", "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }
               );
           
           
            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { "35e9bbe2-c9e1-4a54-ba4a-aca853158dbd", "Editor", "Editor".ToUpper(), Guid.NewGuid().ToString() }
               );

            migrationBuilder.InsertData(
              table: "AspNetRoles",
              columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
              values: new object[] { "440cae50-b5a1-4d4e-a6d0-b6dd36bd09f9", "Employee", "Employee".ToUpper(), Guid.NewGuid().ToString() }
              );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
