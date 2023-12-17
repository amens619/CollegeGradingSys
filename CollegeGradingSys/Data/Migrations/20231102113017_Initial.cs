using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYear",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYearStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcademicYearEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcademicYearName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AcademicYearNameH = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsCurrentYear = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.Id);
                });




            migrationBuilder.CreateTable(
                name: "College",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollegeName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_College", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    NationalityName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationality", x => x.Id);
                });



            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CollegeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_College_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "College",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Governorate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GovernorateName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Governorate_Nationality_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specialization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecializationName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialization_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_Governorate_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StPersonalData",
                columns: table => new
                {
                    AcademicID = table.Column<int>(type: "int", nullable: false),
                    StName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentificatioNO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    NationalityID = table.Column<int>(type: "int", nullable: false),
                    BirthcountryID = table.Column<int>(type: "int", nullable: false),
                    BirthGovernorateId = table.Column<int>(type: "int", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnrollmentYearId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StPersonalData", x => x.AcademicID);
                    table.ForeignKey(
                        name: "FK_StPersonalData_AcademicYear_EnrollmentYearId",
                        column: x => x.EnrollmentYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StPersonalData_Governorate_BirthGovernorateId",
                        column: x => x.BirthGovernorateId,
                        principalTable: "Governorate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StPersonalData_Nationality_BirthcountryID",
                        column: x => x.BirthcountryID,
                        principalTable: "Nationality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StPersonalData_Nationality_NationalityID",
                        column: x => x.NationalityID,
                        principalTable: "Nationality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batch_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BigGrade = table.Column<int>(type: "int", nullable: false),
                    SmallGrade = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsSubCourse = table.Column<bool>(type: "bit", nullable: false),
                    SpecializationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Course_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Course_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StHighSchoolData",
                columns: table => new
                {
                    AcademicID = table.Column<int>(type: "int", nullable: false),
                    CertificateType = table.Column<int>(type: "int", nullable: false),
                    Average = table.Column<float>(type: "real", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SeatNo = table.Column<int>(type: "int", nullable: false),
                    CertificateYear = table.Column<int>(type: "int", nullable: false),
                    HighSchoolName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StHighSchoolData", x => x.AcademicID);
                    table.ForeignKey(
                        name: "FK_StHighSchoolData_StPersonalData_AcademicID",
                        column: x => x.AcademicID,
                        principalTable: "StPersonalData",
                        principalColumn: "AcademicID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StAcademicData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StLevel = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: true),
                    StStatus = table.Column<int>(type: "int", nullable: false),
                    StudyType = table.Column<int>(type: "int", nullable: false),
                    Average = table.Column<float>(type: "real", nullable: true),
                    GPA = table.Column<float>(type: "real", nullable: true),
                    Valuation = table.Column<int>(type: "int", nullable: false),
                    IsTerm = table.Column<bool>(type: "bit", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: true),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    StPersonalDataAcademicID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StAcademicData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StAcademicData_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StAcademicData_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StAcademicData_StPersonalData_StPersonalDataAcademicID",
                        column: x => x.StPersonalDataAcademicID,
                        principalTable: "StPersonalData",
                        principalColumn: "AcademicID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseGrade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseType = table.Column<bool>(type: "bit", nullable: false),
                    Grade = table.Column<float>(type: "real", nullable: true),
                    StStatusForCourse = table.Column<int>(type: "int", nullable: false),
                    StAcademicDataId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseGrade_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseGrade_StAcademicData_StAcademicDataId",
                        column: x => x.StAcademicDataId,
                        principalTable: "StAcademicData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });





            migrationBuilder.CreateIndex(
                name: "IX_Batch_SpecializationId",
                table: "Batch",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_City_DistrictId",
                table: "City",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_ParentId",
                table: "Course",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SpecializationId",
                table: "Course",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGrade_CourseId",
                table: "CourseGrade",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGrade_StAcademicDataId",
                table: "CourseGrade",
                column: "StAcademicDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CollegeId",
                table: "Department",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_District_GovernorateId",
                table: "District",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Governorate_NationalityId",
                table: "Governorate",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialization_DepartmentId",
                table: "Specialization",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StAcademicData_AcademicYearId",
                table: "StAcademicData",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StAcademicData_BatchId",
                table: "StAcademicData",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StAcademicData_StPersonalDataAcademicID",
                table: "StAcademicData",
                column: "StPersonalDataAcademicID");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_BirthcountryID",
                table: "StPersonalData",
                column: "BirthcountryID");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_BirthGovernorateId",
                table: "StPersonalData",
                column: "BirthGovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_EnrollmentYearId",
                table: "StPersonalData",
                column: "EnrollmentYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_NationalityID",
                table: "StPersonalData",
                column: "NationalityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "CourseGrade");

            migrationBuilder.DropTable(
                name: "StHighSchoolData");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "StAcademicData");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "StPersonalData");

            migrationBuilder.DropTable(
                name: "Specialization");

            migrationBuilder.DropTable(
                name: "AcademicYear");

            migrationBuilder.DropTable(
                name: "Governorate");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Nationality");

            migrationBuilder.DropTable(
                name: "College");
        }
    }
}
