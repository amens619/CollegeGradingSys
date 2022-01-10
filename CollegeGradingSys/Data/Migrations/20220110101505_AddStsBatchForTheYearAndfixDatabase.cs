using Microsoft.EntityFrameworkCore.Migrations;

namespace CollegeGradingSys.Data.Migrations
{
    public partial class AddStsBatchForTheYearAndfixDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StAcademicData_AcademicYear_AcademicYearId",
                table: "StAcademicData");

            migrationBuilder.DropForeignKey(
                name: "FK_StAcademicData_StudentBatch_StudentBatchId",
                table: "StAcademicData");

            migrationBuilder.DropTable(
                name: "StudentBatch");

            migrationBuilder.DropIndex(
                name: "IX_StAcademicData_AcademicYearId",
                table: "StAcademicData");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "StAcademicData");

            migrationBuilder.RenameColumn(
                name: "StudentBatchId",
                table: "StAcademicData",
                newName: "StsBatchForTheYearId");

            migrationBuilder.RenameIndex(
                name: "IX_StAcademicData_StudentBatchId",
                table: "StAcademicData",
                newName: "IX_StAcademicData_StsBatchForTheYearId");

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
                name: "StsBatchForTheYear",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StsBatchForTheYear", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StsBatchForTheYear_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StsBatchForTheYear_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batch_SpecializationId",
                table: "Batch",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_StsBatchForTheYear_AcademicYearId",
                table: "StsBatchForTheYear",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StsBatchForTheYear_BatchId",
                table: "StsBatchForTheYear",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_StAcademicData_StsBatchForTheYear_StsBatchForTheYearId",
                table: "StAcademicData",
                column: "StsBatchForTheYearId",
                principalTable: "StsBatchForTheYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StAcademicData_StsBatchForTheYear_StsBatchForTheYearId",
                table: "StAcademicData");

            migrationBuilder.DropTable(
                name: "StsBatchForTheYear");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.RenameColumn(
                name: "StsBatchForTheYearId",
                table: "StAcademicData",
                newName: "StudentBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_StAcademicData_StsBatchForTheYearId",
                table: "StAcademicData",
                newName: "IX_StAcademicData_StudentBatchId");

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "StAcademicData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYearId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: true),
                    StudentBatchName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_StAcademicData_AcademicYearId",
                table: "StAcademicData",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatch_AcademicYearId",
                table: "StudentBatch",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatch_SpecializationId",
                table: "StudentBatch",
                column: "SpecializationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StAcademicData_AcademicYear_AcademicYearId",
                table: "StAcademicData",
                column: "AcademicYearId",
                principalTable: "AcademicYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StAcademicData_StudentBatch_StudentBatchId",
                table: "StAcademicData",
                column: "StudentBatchId",
                principalTable: "StudentBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
