using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.ViewModels.Batch;
using CollegeGradingSys.ViewModels.Course;
using CollegeGradingSys.ViewModels.CourseGrade;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ICourseGradeService
    {
        // الاستعلامات الأساسية
        Task<CourseGradeIndexViewModel> GetStudentGradesAsync(int stAcademicDataId, bool courseType);

        // استعلامات الدفعة (للفلترة والعرض)
        Task<AllbatchCourseGradeViewModel> GetBatchGradesFilteredAsync(AllbatchCourseGradeViewModel filter);
        Task<AllbatchCourseGradeFailedViewModel> GetFailedGradesFilteredAsync(
    string searchString, int? academicId, Term? term, Level? level,
    int? courseId, int? specializationId, int? academicYearId, bool isSelectCurrentYear);


        // عمليات العرض والتصفية
        Task<AllbatchCourseGradeViewModel> GetBatchGradesAsync(int batchId, int courseId, bool isCurrentYear);

        // التعديل والحذف
        Task<EditCourseGradeViewModel> GetEditFormAsync(int id);
        Task UpdateGradeAsync(EditCourseGradeViewModel model);
        Task<CourseGrade> GetByIdAsync(int id);
        //Task DeleteAsync(int id);

        // عمليات الإكسل المعقدة
        Task<BatchCourseGradeUploadVM> PreviewExcelUploadAsync(IFormFile file, AllbatchCourseGradeViewModel filterContext);
        Task UpdateGradesFromPreviewAsync(List<CourseGradeVM> grades);
        Task<MemoryStream> ExportFailedGradesToExcelAsync(string searchString, int? academicId, Term? term, Level? level, int? courseId, int? specializationId, int? academicYearId);
        // التقارير
        Task<MemoryStream> ExportCourseGradesToExcelAsync(int batchId, int courseId);
    } 
}
