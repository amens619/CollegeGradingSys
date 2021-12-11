using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddAllEntityToDatabase : Migration
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
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTwoCourse = table.Column<bool>(type: "bit", nullable: false),
                    SpecializationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
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
                    EnrollmentYearM = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EnrollmentYearH = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StPersonalData", x => x.AcademicID);
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
                name: "StudentBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentBatchName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentBatch_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentBatch_Specialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCourse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCourseName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BigGrade = table.Column<int>(type: "int", nullable: false),
                    SmallGrade = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
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
                    StStatus = table.Column<int>(type: "int", nullable: false),
                    StudentBatchId = table.Column<int>(type: "int", nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_StAcademicData_StPersonalData_StPersonalDataAcademicID",
                        column: x => x.StPersonalDataAcademicID,
                        principalTable: "StPersonalData",
                        principalColumn: "AcademicID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StAcademicData_StudentBatch_StudentBatchId",
                        column: x => x.StudentBatchId,
                        principalTable: "StudentBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseGrade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseType = table.Column<bool>(type: "bit", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true),
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
                name: "IX_StAcademicData_AcademicYearId",
                table: "StAcademicData",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StAcademicData_StPersonalDataAcademicID",
                table: "StAcademicData",
                column: "StPersonalDataAcademicID");

            migrationBuilder.CreateIndex(
                name: "IX_StAcademicData_StudentBatchId",
                table: "StAcademicData",
                column: "StudentBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_BirthcountryID",
                table: "StPersonalData",
                column: "BirthcountryID");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_BirthGovernorateId",
                table: "StPersonalData",
                column: "BirthGovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_StPersonalData_NationalityID",
                table: "StPersonalData",
                column: "NationalityID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatch_AcademicYearId",
                table: "StudentBatch",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatch_SpecializationId",
                table: "StudentBatch",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCourse_CourseId",
                table: "SubCourse",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseGrade");

            migrationBuilder.DropTable(
                name: "StHighSchoolData");

            migrationBuilder.DropTable(
                name: "SubCourse");

            migrationBuilder.DropTable(
                name: "StAcademicData");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "StPersonalData");

            migrationBuilder.DropTable(
                name: "StudentBatch");

            migrationBuilder.DropTable(
                name: "AcademicYear");
        }
    }
}
