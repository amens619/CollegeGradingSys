using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Implementations;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.Batch;
using CollegeGradingSys.ViewModels.Course;
using CollegeGradingSys.ViewModels.CourseGrade;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;

namespace CollegeGradingSys.Services.Implementations
{   
    public class CourseGradeService : ICourseGradeService
    {

        private readonly ICourseGradeRepository _repo;       
        private readonly IRepository<Models.Course> _courseRepo; // لجلب معلومات المادة 
        private readonly IRepository<Batch> _batchRepo;
        private readonly IAcademicYearRepository _academicYearRepo;
        private readonly IStAcademicDataRepository _stAcademicDataRepo;
        private readonly IRepository<Specialization> _specializationRepo;
        public CourseGradeService(
            ICourseGradeRepository repo,
            IBatchService batchService,
            IStAcademicDataRepository stAcademicDataRepo,
            IAcademicYearRepository academicYearRepo,
            IRepository<Batch> batchRepo,
            IRepository<Models.Course> courseRepo,
            IRepository<Specialization> specializationRepo)
        {
            _repo = repo;          
            _stAcademicDataRepo = stAcademicDataRepo;
            _courseRepo = courseRepo;
            _academicYearRepo = academicYearRepo;
            _batchRepo = batchRepo;
            _specializationRepo = specializationRepo;

        }
        // ================== Get Student Grades (Single Student) ==================
        public async Task<CourseGradeIndexViewModel> GetStudentGradesAsync(int stAcademicDataId, bool courseType)
        {
            var stData = await _stAcademicDataRepo.FindAsync(stAcademicDataId) ?? throw new DomainException("بيانات الطالب الأكاديمية غير موجودة");
            var grades = await _repo.Query()
                .Where(x => x.StAcademicData.Id == stAcademicDataId && x.CourseType == courseType)
                .Include(x => x.Course)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return new CourseGradeIndexViewModel
            {
                Id = stAcademicDataId,
                AcademicID = stData.StPersonalData.AcademicID,
                StName = stData.StPersonalData.StName,
                AcademicYear = stData.AcademicYear.AcademicYearName,
                Batch = stData.Batch.BatchName,
                CourseType = courseType ? "عام" : "تكميلي",
                IsCurrentYear = stData.AcademicYear.IsCurrentYear,
                Level = stData.StLevel.ToString(),
                Specialization = stData.Batch.Specialization.SpecializationName,
                StStatus = stData.StStatus.ToString(),
                Term = stData.Term.ToString(),
                CourseGrades = grades
            };
        }

        // ================== Get Batch Grades (Filter Logic) ==================
        public async Task<AllbatchCourseGradeViewModel> GetBatchGradesFilteredAsync(AllbatchCourseGradeViewModel filter)
        {
            // 1. إعداد القيم الافتراضية
            filter.Term ??= Term.الأول;

            // جلب القوائم
            var years = await _academicYearRepo.GetAllOrderedByStartDateAsync();
            if (!filter.AcademicYearId.HasValue)
            {
                var current = await _academicYearRepo.GetCurrentYearAsync();
                filter.AcademicYearId = current?.Id ?? years.FirstOrDefault()?.Id;
            }

            // جلب الدفعات للسنة المختارة
            var batches = await GetBatchesForYear(filter.AcademicYearId.Value);
            if (!filter.BatchId.HasValue && batches.Any()) filter.BatchId = batches.First().Id;

            // جلب المواد للدفعة المختارة
            List<Course> courses = new List<Course>();
            if (filter.BatchId.HasValue)
            {
                var batch = await _batchRepo.FindAsync(filter.BatchId.Value); // Assuming Find includes Specialization
                // إذا لم يكن Find يجلب التخصص، استخدم Query
                var batchWithSpec = await _batchRepo.Query().Include(x => x.Specialization).FirstOrDefaultAsync(x => x.Id == filter.BatchId);

                if (batchWithSpec != null)
                {
                    filter.Level ??= batchWithSpec.StAcademicDatas?.LastOrDefault()?.StLevel ?? Level.الأول;

                    courses = await _courseRepo.Query()
                        .Where(x => x.Specialization.Id == batchWithSpec.Specialization.Id
                                 && x.Level == filter.Level
                                 && x.Term == filter.Term)
                        .ToListAsync();

                    if (!filter.CourseId.HasValue && courses.Any()) filter.CourseId = courses.First().Id;
                }
            }

            // 2. الاستعلام عن الدرجات
            var query = _repo.Query()
                .Include(x => x.StAcademicData).ThenInclude(y => y.StPersonalData)
                .Include(x => x.Course)
                .Where(x => x.CourseType == filter.CourseType);

            if (filter.AcademicYearId.HasValue) query = query.Where(x => x.StAcademicData.AcademicYear.Id == filter.AcademicYearId);
            if (filter.BatchId.HasValue) query = query.Where(x => x.StAcademicData.Batch.Id == filter.BatchId);
            if (filter.Level.HasValue) query = query.Where(x => x.StAcademicData.StLevel == filter.Level);
            if (filter.Term.HasValue) query = query.Where(x => x.StAcademicData.Term == filter.Term);
            if (filter.CourseId.HasValue) query = query.Where(x => x.Course.Id == filter.CourseId);
            if (filter.StStatusForCourse.HasValue) query = query.Where(x => x.StStatusForCourse == filter.StStatusForCourse);

            var grades = await query.ToListAsync();

            // 3. تعبئة ViewModel
            var model = new AllbatchCourseGradeViewModel
            {
                AcademicYearId = filter.AcademicYearId,
                BatchId = filter.BatchId,
                CourseId = filter.CourseId,
                Term = filter.Term,
                Level = filter.Level,
                CourseType = filter.CourseType,
                StStatusForCourse = filter.StStatusForCourse,

                CourseName = courses.FirstOrDefault(x => x.Id == filter.CourseId)?.CourseName,
                IsCurrentYear = years.FirstOrDefault(x => x.Id == filter.AcademicYearId)?.IsCurrentYear ?? false,

                // تحويل Entities إلى ViewModels بسيطة
                CourseGrades = grades.Select(x => new CourseGradeVM
                {
                    Id = x.Id,
                    AcademicID = x.StAcademicData.StPersonalData.AcademicID,
                    StName = x.StAcademicData.StPersonalData.StName,
                    Grade = x.Grade,
                    StStatusForCourse = x.StStatusForCourse,
                    Course = x.Course,
                    IsCurrentYear = x.StAcademicData.AcademicYear.IsCurrentYear

                }).ToList(),

                // تعبئة القوائم
                AcademicYearsList = new SelectList(years, "Id", "AcademicYearName", filter.AcademicYearId),
                BatchesList = new SelectList(batches, "Id", "BatchName", filter.BatchId),
                CoursesList = new SelectList(courses, "Id", "CourseName", filter.CourseId)
            };

            return model;
        }

        // ================== Edit & Update (Single) ==================
        public async Task<EditCourseGradeViewModel> GetEditFormAsync(int id)
        {
            var entity = await _repo.FindWithRelationsAsync(id);
            if (entity == null) throw new DomainException("الدرجة غير موجودة");

            return new EditCourseGradeViewModel
            {
                Id = entity.Id,
                CourseName = entity.Course.CourseName,
                BigGrade = entity.Course.BigGrade,
                SmallGrade = entity.Course.SmallGrade,
                CourseType = entity.CourseType,
                Grade = entity.Grade?.ToString("N2", CultureInfo.InvariantCulture),
                StStatusForCourse = entity.StStatusForCourse,
                IsSubCourse = entity.Course.IsSubCourse,
                ParentId = entity.Course.ParentId
            };
        }

        public async Task UpdateGradeAsync(EditCourseGradeViewModel model)
        {
            var entity = await _repo.FindWithRelationsAsync(model.Id);
            if (entity == null) throw new DomainException("الدرجة غير موجودة");

            // Validation Logic
            float? newGrade = null;
            if (!string.IsNullOrWhiteSpace(model.Grade))
            {
                if (!float.TryParse(model.Grade, NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedGrade))
                    throw new DomainException("الدرجة يجب أن تكون رقم");

                if (parsedGrade < 0 || parsedGrade > entity.Course.BigGrade)
                    throw new DomainException($"الدرجة يجب أن تكون بين 0 و {entity.Course.BigGrade}");

                newGrade = parsedGrade;
            }

            // Update Entity
            entity.Grade = newGrade;
            entity.StStatusForCourse = model.StStatusForCourse;

            // منطق تحديد الحالة تلقائياً إذا تم إدخال درجة
            if (newGrade.HasValue && (model.StStatusForCourse == StStatusForCourse.ناجح || model.StStatusForCourse == StStatusForCourse.راسب))
            {
                entity.StStatusForCourse = newGrade >= entity.Course.SmallGrade ? StStatusForCourse.ناجح : StStatusForCourse.راسب;
            }

            await _repo.UpdateAsync(entity);

            // منطق المواد الفرعية (SubCourses)
            if (entity.Course.IsSubCourse && entity.Course.ParentId.HasValue)
            {
                await RecalculateParentCourseGradeAsync(entity.StAcademicData.Id, entity.Course.ParentId.Value);
            }
        }

        //// ================== Delete ==================
        //public async Task DeleteAsync(int id)
        //{
        //    var entity = await _repo.FindWithRelationsAsync(id);
        //    if (entity == null) throw new DomainException("غير موجود");

        //    if (entity.CourseType == true)
        //        throw new DomainException("لا يمكن حذف درجات المادة الأساسية");

        //    // Assuming repo has delete by ID
        //    await _repo.DeleteAsync(entity);
        //}

        // عمليات الإكسل المعقدة
        // ================== Excel Upload Logic (Preview) ==================
        public async Task<BatchCourseGradeUploadVM> PreviewExcelUploadAsync(IFormFile file, AllbatchCourseGradeViewModel filterContext)
        {
            if (file == null || file.Length == 0) throw new DomainException("يجب اختيار ملف");

            var result = new BatchCourseGradeUploadVM
            {
                BatchId = filterContext.BatchId ?? 0,
                CourseId = filterContext.CourseId ?? 0
            };

            // نحتاج معلومات المادة للتحقق من الدرجة العظمى
            var course = await _courseRepo.FindAsync(result.CourseId);
            if (course == null) throw new DomainException("المادة غير محددة");
            result.BigGrade = course.BigGrade;
            result.CourseName = course.CourseName;

            using (var stream = file.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null) throw new DomainException("ملف الإكسل فارغ");

                var rowCount = worksheet.Dimension.Rows;
                for (var row = 7; row <= rowCount; row++) // Start from row 7 per logic
                {
                    try
                    {
                        var academicIDObj = worksheet.Cells[row, 3].Value;
                        if (academicIDObj == null) continue;

                        int academicID = Convert.ToInt32(academicIDObj.ToString());
                        var gradeStr = worksheet.Cells[row, 4].Value?.ToString();

                        float? grade = null;
                        string error = null;

                        if (gradeStr != null)
                        {
                            if (float.TryParse(gradeStr, out float parsedGrade) && parsedGrade >= 0 && parsedGrade <= course.BigGrade)
                            {
                                grade = parsedGrade;
                            }
                            else
                            {
                                error = $"قيمة خطأ: يجب أن تكون بين 0 و {course.BigGrade}";
                            }
                        }

                        // البحث عن الطالب في قاعدة البيانات لمطابقة الـ ID
                        // ملاحظة: هذا قد يكون بطيئاً داخل اللوب، الأفضل جلب درجات الدفعة في الذاكرة أولاً
                        var dbGrade = await _repo.Query()
                            .Include(x => x.StAcademicData.StPersonalData)
                            .FirstOrDefaultAsync(x => x.StAcademicData.StPersonalData.AcademicID == academicID
                                                   && x.Course.Id == result.CourseId);

                        if (dbGrade != null)
                        {
                            result.PreviewGrades.Add(new CourseGradeVM
                            {
                                Id = dbGrade.Id,
                                AcademicID = academicID,
                                StName = dbGrade.StAcademicData.StPersonalData.StName,
                                Grade = grade,
                                Note = error,
                                StStatusForCourse = (grade.HasValue && grade >= course.SmallGrade) ? StStatusForCourse.ناجح : StStatusForCourse.راسب,
                                IsGradeChange = true // Flag for update
                            });
                        }
                    }
                    catch { /* Log error or continue */ }
                }
            }

            // Check duplicates logic from controller
            if (result.PreviewGrades.GroupBy(x => x.AcademicID).Any(g => g.Count() > 1))
            {
                throw new DomainException("يوجد تكرار في أرقام القيد في الملف.");
            }

            return result;
        }

        // ================== Excel Export Logic ==================
        public async Task<MemoryStream> ExportFailedGradesToExcelAsync(string searchString, int? academicId, Term? term, Level? level, int? courseId, int? specializationId, int? academicYearId)
        {
            var courseGrades = _repo.Query()
                            .Where(x => x.CourseType == false);
           var courses = await _courseRepo.Query()
                       .Where(x => x.Specialization.Id == specializationId
                                && x.Level == level
                                && x.Term == term)
                       .ToListAsync();
            courses = courses.Where(x => x.IsSubCourse == false).ToList();
            // 1. Fetch Data (Reuse existing logic or create specific query)
            var stream = new MemoryStream();

            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add(" كشف التكميلية طلاب");
                worksheet.View.RightToLeft = true;
                worksheet.Cells.Style.Font.Bold = true;
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 5;
                const int startColumn = 3;
                var row = startRow;

                worksheet.Column(1).Width = 18.43;
                worksheet.Column(2).Width = 41.71;
                worksheet.Column(3).Width = 101.71;
                int coursesNo = 1;
                for (; coursesNo <= courses.Count(); coursesNo++)
                {
                    worksheet.Column(coursesNo + startColumn).Width = 24.86;
                }



                worksheet.Column(coursesNo + startColumn).Width = 73.71;
                //==========================
                worksheet.Row(1).Height = 45;
                worksheet.Row(2).Height = 45;
                worksheet.Row(3).Height = 45;
                worksheet.Row(4).Height = 30.5;
                worksheet.Row(5).Height = 144;




                //Create Headers and format them
                worksheet.Cells["B1:C1"].Value = "       الجمهورية اليمنية";
                using (var r = worksheet.Cells["B1:C1"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells["B2:c2"].Value = "       جامعة الإيمان";
                using (var r = worksheet.Cells["B2:C2"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells["B3:C3"].Value = "       فرع حضرموت";
                using (var r = worksheet.Cells["B3:C3"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //=============================

                worksheet.Cells["A5:N5"].Style.Font.Size = 28;
                worksheet.Cells["B1:B3"].Style.Font.Size = 36;

                worksheet.Cells["A5:N5"].Style.Font.Name = "Khalid Art bold";

                //=============================
                var academicYear = await _academicYearRepo.FindAsync(academicId?? 0);
                worksheet.Cells[4, startColumn + 1, 4, coursesNo + 1].Style.Font.Size = 28;

                worksheet.Cells[4, startColumn + 1, 4, coursesNo + 1].Value = academicYear != null ? (" نتيجة إمتحانات       " + "المستوى: " + level + "       الفصل: " + term + "    للعام الجامعي: " + academicYear.AcademicYearNameH + " الموافق " + academicYear.AcademicYearName) : (" نتيجة إمتحانات       " + "المستوى: " + level + "       الفصل: " + term);
                using (var r = worksheet.Cells[4, startColumn + 1, 4, coursesNo + 1])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                ////////============================
                worksheet.Cells[1, coursesNo + startColumn - 1, 1, coursesNo + startColumn].Value = "Republic of Yemen";
                using (var r = worksheet.Cells[1, coursesNo + startColumn - 1, 1, coursesNo + startColumn])
                {
                    r.Merge = true;
                    r.Style.Font.Name = "Times New Roman";
                    r.Style.Font.Size = 36;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells[2, coursesNo + startColumn - 1, 2, coursesNo + startColumn].Value = "AL - Eman university";
                using (var r = worksheet.Cells[2, coursesNo - 1 + startColumn, 2, coursesNo + startColumn])
                {
                    r.Merge = true;
                    r.Style.Font.Name = "Stenc";
                    r.Style.Font.Size = 36;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                worksheet.Cells[3, coursesNo + startColumn - 1, 3, coursesNo + startColumn].Value = "Hadhramout branch";

                using (var r = worksheet.Cells[3, coursesNo + startColumn - 1, 3, coursesNo + startColumn])
                {
                    r.Merge = true;
                    r.Style.Font.Name = "Times New Roman";
                    r.Style.Font.Size = 36;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                }
                //=====================================
                //////var academicYear = GetCurrentYear();
                //////worksheet.Cells["I2"].Value = academicYear.AcademicYearName;
                Color colGradFromHex = System.Drawing.ColorTranslator.FromHtml("#BFBFBF");
                Color LightYellowFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                Color BrownFromHex = System.Drawing.ColorTranslator.FromHtml("#974706");

                //================================

                int rowIndex = 1;
                int colIndex = 7;
                int PixelTop = 17;
                int PixelLeft = 1721;
                int Height = 153;
                int Width = 100;
                Image img = Image.FromFile(@"wwwroot/images/CollegeIcon.jpg");

                OfficeOpenXml.Drawing.ExcelPicture pic = worksheet.Drawings.AddPicture("Sample", img);
                pic.SetPosition(PixelTop, PixelLeft);


                using (var r = worksheet.Cells["A5:C5"])
                {
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                row = startRow;
                //================================
                worksheet.Cells["A5"].Value = "م";
                //================================
                worksheet.Cells["B5"].Value = "الرقم الاكاديمي";
                //================================
                worksheet.Cells["C5"].Value = "الاسم";
                //================================
                var Column = startColumn;
                foreach (var course in courses)
                {
                    Column++;
                    using (var r = worksheet.Cells[row, Column])
                    {
                        r.Style.WrapText = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);

                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Top.Color.SetColor(Color.Black);

                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Color.SetColor(Color.Black);

                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Color.SetColor(Color.Black);

                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }

                    worksheet.Cells[row, Column].Value = course.CourseName;
                }


                //================================
                Column++;
                using (var r = worksheet.Cells[row, Column])
                {
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(colGradFromHex);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                worksheet.Cells[row, Column].Value = "ملاحظات";



                var stsCourseGrade = (await courseGrades.ToListAsync()).GroupBy(x => x.StAcademicData.StPersonalData);



                var no = 1;
                foreach (var OneSt in stsCourseGrade)
                {
                    row++;
                    worksheet.Row(row).Height = 40;
                    worksheet.Cells[row, 1].Value = no;

                    DrowCell(worksheet, row, 1, Color.White);


                    //=================================================
                    worksheet.Cells[row, 2].Value = OneSt.Key.AcademicID;
                    DrowCell(worksheet, row, 2, colGradFromHex);
                    //================================================
                    worksheet.Cells[row, 3].Value = OneSt.Key.StName;
                    DrowCell(worksheet, row, 3, Color.White);
                    //================================================
                    Column = 3;
                    foreach (var course in courses)
                    {
                        Column++;
                        var courseGradeTemp = OneSt.FirstOrDefault(x => x.Course.Id == course.Id);
                        if (courseGradeTemp != null)

                            worksheet.Cells[row, Column].Value = courseGradeTemp.Grade != null ? courseGradeTemp.Grade : 0;
                        else
                            worksheet.Cells[row, Column].Value = "-";
                        DrowCell(worksheet, row, Column, colGradFromHex);
                    }
                    //=================================================
                    worksheet.Cells[row, Column + 1].Value = " ";
                    DrowCell(worksheet, row, Column + 1, Color.White);

                    no++;
                }
                for (; no <= 24; no++)
                {
                    row++;
                    worksheet.Row(row).Height = 40;
                    worksheet.Cells[row, 1].Value = no;

                    DrowCell(worksheet, row, 1, Color.White);


                    //=================================================
                    worksheet.Cells[row, 2].Value = "";
                    DrowCell(worksheet, row, 2, colGradFromHex);
                    //================================================
                    worksheet.Cells[row, 3].Value = "";
                    DrowCell(worksheet, row, 3, Color.White);
                    //================================================
                    Column = 3;
                    foreach (var course in courses)
                    {
                        Column++;
                        worksheet.Cells[row, Column].Value = "";
                        DrowCell(worksheet, row, Column, colGradFromHex);
                    }
                    //=================================================
                    worksheet.Cells[row, Column + 1].Value = " ";
                    DrowCell(worksheet, row, Column + 1, Color.White);


                }
                worksheet.Row(row + 1).Height = 201.75;
                worksheet.Cells[row + 1, 1, row + 1, Column + 1].Style.Font.Size = 36;
                worksheet.Cells[row + 1, 1, row + 1, Column + 1].Style.Font.Name = "Khalid Art bold";
                worksheet.Cells[row + 1, 1, row + 1, Column + 1].Value = "مدير القبول والتسجيل                                                             نائب رئيس الفرع لشئون الطلاب                                                نائب رئيس الفرع للشئون العلمية";
                using (var r = worksheet.Cells[row + 1, 1, row + 1, Column + 1])
                {
                    r.Merge = true;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.White);

                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Color.SetColor(Color.Black);

                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Color.SetColor(Color.Black);

                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Color.SetColor(Color.Black);

                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
                // set some core property values
                xlPackage.Workbook.Properties.Title = "User List";
                xlPackage.Workbook.Properties.Author = "";
                xlPackage.Workbook.Properties.Subject = "User List";
                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }

            //using (var package = new ExcelPackage(stream))
            //{
            //    var sheet = package.Workbook.Worksheets.Add("FailedGrades");
            //    sheet.Cells[1, 1].Value = "تقرير الدرجات";
            //    // Implement the drawing logic here...
            //    package.Save();
            //}

            stream.Position = 0;
            return stream;
        }

        public Task<CourseGrade> GetByIdAsync(int id) => _repo.FindWithRelationsAsync(id);
        //public Task<AllbatchCourseGradeFailedViewModel> GetFailedGradesFilteredAsync(string searchString, int? academicId, int? courseId, int? specializationId, int? academicYearId, bool isSelectCurrentYear) => throw new NotImplementedException();
        public async Task<AllbatchCourseGradeFailedViewModel> GetFailedGradesFilteredAsync(
    string searchString, int? academicId, Term? term, Level? level,
    int? courseId, int? specializationId, int? academicYearId, bool isSelectCurrentYear)
        {
            // 1. إنشاء الـ ViewModel المبدئي
            var model = new AllbatchCourseGradeFailedViewModel
            {
                SearchString = searchString,
                SearchAcademicID = academicId,
                Term = term,
                Level = level,
                CourseId = courseId ?? -1,
                SpecializationId = specializationId,
                IsSelectCurrentYear = isSelectCurrentYear
            };

            // 2. منطق العام الدراسي الحالي
            if (isSelectCurrentYear)
            {
                var currentYear = await _academicYearRepo.GetCurrentYearAsync();
                if (currentYear != null)
                {
                    academicYearId = currentYear.Id;
                }
            }
            else if (!academicYearId.HasValue)
            {
                academicYearId = -1;
            }
            model.AcademicYearId = academicYearId;

            // 3. بناء الاستعلام للدرجات التكميلية (الرسوب)
            // نستخدم Query() لضمان الفلترة داخل SQL وليس في الذاكرة (Memory)        
            var query = _repo.Query()
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.StPersonalData)
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.AcademicYear) // 👈 حل المشكلة 1
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.Batch)
                        .ThenInclude(b => b.Specialization)
                .Include(x => x.Course)
                .Where(x => x.CourseType == false); // المواد التكميلية
            // 4. تطبيق الفلاتر الديناميكية (Dynamic Filtering)
            if (academicYearId.HasValue && academicYearId != -1)
                query = query.Where(x => x.StAcademicData.AcademicYear.Id == academicYearId);

            if (specializationId.HasValue && specializationId != -1)
                query = query.Where(x => x.StAcademicData != null && x.StAcademicData.Batch != null && x.StAcademicData.Batch.Specialization.Id == specializationId);

            if (level.HasValue)
                query = query.Where(x => x.StAcademicData.StLevel == level);

            if (term.HasValue)
                query = query.Where(x => x.StAcademicData.Term == term);

            if (courseId.HasValue && courseId != -1)
                query = query.Where(x => x.Course.Id == courseId);

           
            if (!string.IsNullOrEmpty(searchString))               
            query = query.Where(x => x.StAcademicData.StPersonalData.StName.Contains(searchString));
           
            if (academicId.HasValue)
                query = query.Where(x => x.StAcademicData != null &&
                                         x.StAcademicData.StPersonalData != null &&
                                         x.StAcademicData.StPersonalData.AcademicID == academicId);
            // 5. جلب بيانات الدرجات وتحويلها إلى ViewModel
            var grades = await query.ToListAsync();
            model.CourseGrades = grades.Select(x => new CourseGradeVM
            {
                Id = x.Id,
                AcademicID = x.StAcademicData?.StPersonalData?.AcademicID ?? 0,
                StName = x.StAcademicData?.StPersonalData?.StName ?? "غير معروف",
                Grade = x.Grade,
                StStatusForCourse = x.StStatusForCourse,
                Course = x.Course
            }).ToList();

            // 6. منطق زر التصدير
            model.isExportBtnEnable = term.HasValue && level.HasValue && specializationId.HasValue && specializationId != -1;

            // 7. إعداد القوائم المنسدلة (SelectLists)
            var academicYears = await _academicYearRepo.GetAllOrderedByStartDateAsync();
            model.AcademicYearsList = new SelectList(academicYears, "Id", "AcademicYearName", academicYearId);

            // جلب التخصصات (وإضافة خيار --الكل--)
            var specs = await _specializationRepo.Query().ToListAsync();
            specs.Insert(0, new Specialization { Id = -1, SpecializationName = "-- الكل --" });
            model.SpecializationsList = new SelectList(specs, "Id", "SpecializationName", specializationId ?? -1);

            // جلب المواد الخاصة بالتخصص والمستوى والترم
            var coursesQuery = _courseRepo.Query().AsQueryable();
            if (specializationId.HasValue && specializationId != -1)
                coursesQuery = coursesQuery.Where(x => x.Specialization.Id == specializationId);
            if (level.HasValue)
                coursesQuery = coursesQuery.Where(x => x.Level == level);
            if (term.HasValue)
                coursesQuery = coursesQuery.Where(x => x.Term == term);

            var courses = await coursesQuery.ToListAsync();
            model.CourseList = new SelectList(courses, "Id", "CourseName", model.CourseId);

            // تعيين اسم المادة إذا تم اختيارها
            if (model.CourseId != -1)
            {
                model.courseName = courses.FirstOrDefault(x => x.Id == model.CourseId)?.CourseName;
            }

            return model;
        }
        public async Task UpdateGradesFromPreviewAsync(List<CourseGradeVM> grades)
        {
            // 1. تصفية القائمة للحصول فقط على الدرجات التي تم تعديلها
            var changedGrades = grades?.Where(x => x.IsGradeChange).ToList();

            if (changedGrades == null || !changedGrades.Any())
                return;

            foreach (var vm in changedGrades)
            {
                // 2. جلب السجل من قاعدة البيانات مع بيانات المادة
                var entity = await _repo.Query()
                    .Include(x => x.Course)
                    .Include(x => x.StAcademicData)
                    .FirstOrDefaultAsync(x => x.Id == vm.Id);

                if (entity == null)
                    throw new DomainException($"لم يتم العثور على سجل الدرجة للطالب: {vm.StName}");

                // 3. تحديث البيانات الأساسية
                entity.Grade = vm.Grade;
                entity.StStatusForCourse = vm.StStatusForCourse;

                // 4. حفظ التحديث الأساسي
                await _repo.UpdateAsync(entity);

                // 5. منطق المواد الفرعية (إعادة احتساب درجة المادة الأساسية)
                // نعتمد على بيانات المادة القادمة من قاعدة البيانات لضمان الموثوقية
                if (entity.Course != null && entity.Course.IsSubCourse && entity.Course.ParentId.HasValue)
                {
                    await RecalculateParentCourseGradeAsync(entity.StAcademicData.Id, entity.Course.ParentId.Value);
                }
            }
        }
        // ================== Private Helpers ==================
        private async Task<List<Batch>> GetBatchesForYear(int yearId)
        {
            var year = await _academicYearRepo.Query()
                .Include(x => x.StAcademicDatas).ThenInclude(x => x.Batch)
                .FirstOrDefaultAsync(x => x.Id == yearId);

            if (year == null) return new List<Batch>();

            return year.StAcademicDatas
                .Select(x => x.Batch)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToList();
        }      
        private async Task RecalculateParentCourseGradeAsync(int stAcademicDataId, int parentCourseId)
        {
            var parentGradeRecord = await _repo.Query()
                .Include(x => x.Course).ThenInclude(c => c.SubCourses)
                .FirstOrDefaultAsync(x => x.StAcademicData.Id == stAcademicDataId && x.Course.Id == parentCourseId);

            if (parentGradeRecord == null || !parentGradeRecord.Course.SubCourses.Any())
                return;

            // 1. استخراج أرقام المواد الفرعية في قائمة (List)
            var subCourseIds = parentGradeRecord.Course.SubCourses.Select(sc => sc.Id).ToList();

            // 2. جلب كل الدرجات دفعة واحدة (استعلام واحد فقط!)
            var subCourseGrades = await _repo.Query()
                .Where(x => x.StAcademicData.Id == stAcademicDataId && subCourseIds.Contains(x.Course.Id))
                .Select(x => x.Grade)
                .ToListAsync();

            // 3. حساب المجموع
            float sum = 0;
            foreach (var grade in subCourseGrades)
            {
                sum += grade ?? 0; // إذا كانت الدرجة null نعتبرها 0
            }

            // 4. التحديث
            if (parentGradeRecord.Course.SubCourses.Count > 0)
            {
                parentGradeRecord.Grade = sum / parentGradeRecord.Course.SubCourses.Count;

                parentGradeRecord.StStatusForCourse = parentGradeRecord.Grade >= parentGradeRecord.Course.SmallGrade
                    ? StStatusForCourse.ناجح
                    : StStatusForCourse.راسب;

                await _repo.UpdateAsync(parentGradeRecord);
            }
        }
        private void DrowCell(OfficeOpenXml.ExcelWorksheet worksheet, int row, int Column, Color backgroundColor)
        {
            worksheet.Cells[row, Column].Style.Font.Size = 28;
            worksheet.Cells[row, Column].Style.Font.Name = "Khalid Art bold";
            using (var r = worksheet.Cells[row, Column])
            {
                r.Style.WrapText = true;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(backgroundColor);

                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Top.Color.SetColor(Color.Black);

                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Left.Color.SetColor(Color.Black);

                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Right.Color.SetColor(Color.Black);

                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Bottom.Color.SetColor(Color.Black);
            }
        }
        public async Task<AllbatchCourseGradeViewModel> GetBatchGradesAsync(int batchId, int courseId, bool isCurrentYear)
        {
            //var grades = await _repo.GetGradesForBatchAsync(batchId, courseId);
            //var course = await _courseRepo.Query().FirstOrDefaultAsync(x => x.Id == courseId);

            //if (course == null) throw new DomainException("المادة غير موجودة");

            //// تحويل البيانات إلى ViewModel
            //return new AllbatchCourseGradeViewModel
            //{
            //    CourseGrades = grades.Select(x => new CourseGradeVM
            //    {
            //        Id = x.Id,
            //        AcademicID = x.StAcademicData.StPersonalData.AcademicID,
            //        StName = x.StAcademicData.StPersonalData.StName,
            //        Grade = x.Grade,
            //        StStatusForCourse = x.StStatusForCourse,
            //        Course = x.Course
            //    }).ToList(),
            //    Course = course,
            //    CourseName = course.CourseName,
            //    Level = course.Level,
            //    Term = course.Term,
            //    IsCurrentYear = isCurrentYear,
            //    // يمكنك إضافة منطق إضافي هنا لحساب نسب النجاح مثلاً
            //};
            // 1. جلب البيانات من الريبوزيتوري
            var grades = await _repo.GetGradesForBatchAsync(batchId, courseId);
            var course = await _courseRepo.Query().FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null) throw new DomainException("المادة غير موجودة");

            // 2. تجهيز القوائم المنسدلة (لتجنب استخدام ViewBag في الكنترولر)
            // ملاحظة: نحتاج لاستدعاء سيرفس السنوات والدفعات هنا أو الريبوزيتوري الخاص بهم
            var years = await _academicYearRepo.GetAllOrderedByStartDateAsync();
            // أو استخدام academicYearService.GetSelectItemsAsync() حسب المتاح لديك

            var batches = await _batchRepo.Query().ToListAsync(); // أو دالة مخصصة لجلب الدفعات
            var courses = await _courseRepo.Query()
                .Where(x => x.Specialization.Id == course.Specialization.Id && x.Level == course.Level && x.Term == course.Term)
                .ToListAsync();

            // 3. بناء الـ ViewModel
            return new AllbatchCourseGradeViewModel
            {
                // === البيانات الأساسية ===
                BatchId = batchId,
                CourseId = courseId,
                AcademicYearId = grades.FirstOrDefault()?.StAcademicData.AcademicYear.Id, // محاولة جلب السنة من البيانات
                CourseName = course.CourseName,
                Level = course.Level,
                Term = course.Term,
                IsCurrentYear = isCurrentYear,
                Course = course, // يفضل إبقاؤه للعرض فقط أو استبداله بخصائص بسيطة

                // === تحويل قائمة الدرجات (تعديلك الممتاز) ===
                CourseGrades = grades.Select(x => new CourseGradeVM
                {
                    Id = x.Id,
                    // استخدام ?. لتجنب NullReferenceException في حال نقص البيانات
                    AcademicID = x.StAcademicData?.StPersonalData?.AcademicID ?? 0,
                    StName = x.StAcademicData?.StPersonalData?.StName ?? "غير معروف",
                    Grade = x.Grade,
                    StStatusForCourse = x.StStatusForCourse,
                    IsGradeChange = false, // القيمة الافتراضية
                    Course = x.Course,                       
                    IsCurrentYear = x.StAcademicData?.AcademicYear?.IsCurrentYear ?? false

                }).ToList(),

                // === تعبئة القوائم المنسدلة (بديل ViewBag) ===
                AcademicYearsList = new SelectList(years, "Id", "AcademicYearName"),
                BatchesList = new SelectList(batches, "Id", "BatchName", batchId),
                CoursesList = new SelectList(courses, "Id", "CourseName", courseId)
            };
        }

      
      
        public async Task<MemoryStream> ExportCourseGradesToExcelAsync(int batchId, int courseId)
        {
            var data = await _repo.GetGradesForBatchAsync(batchId, courseId);
            if (!data.Any()) throw new DomainException("لا توجد بيانات للتصدير");

            var course = data.First().Course;

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("كشف الدرجات");
                worksheet.View.RightToLeft = true;

                // --- تصميم الهيدر (تم نقله من الكونترولر) ---
                worksheet.Cells[1, 1].Value = "كشف درجات مادة: " + course.CourseName;
                worksheet.Cells[1, 1, 1, 4].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // رؤوس الأعمدة
                worksheet.Cells[3, 1].Value = "م";
                worksheet.Cells[3, 2].Value = "رقم القيد";
                worksheet.Cells[3, 3].Value = "اسم الطالب";
                worksheet.Cells[3, 4].Value = "الدرجة";
                worksheet.Cells[3, 5].Value = "الحالة";

                int row = 4;
                int count = 1;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = count++;
                    worksheet.Cells[row, 2].Value = item.StAcademicData.StPersonalData.AcademicID;
                    worksheet.Cells[row, 3].Value = item.StAcademicData.StPersonalData.StName;
                    worksheet.Cells[row, 4].Value = item.Grade;
                    worksheet.Cells[row, 5].Value = item.StStatusForCourse.ToString();
                    row++;
                }

                worksheet.Cells.AutoFitColumns();
                package.Save();
            }

            stream.Position = 0;
            return stream;
        }

    }
}
