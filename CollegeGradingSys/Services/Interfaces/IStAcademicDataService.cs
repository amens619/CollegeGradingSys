using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.StAcademic;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
     public interface IStAcademicDataService
    {
        // العرض والاستعلام
        Task<StAcademicDataUnifiedVM> GetIndexDataAsync(StAcademicDataUnifiedVM filter, int pageNumber , int pageSize );
        Task<StAcademicDataDataViewModel> GetStudentAcademicHistoryAsync(int academicID);

        // الإضافة (Create)
        Task<CreateStAcademicDataDataViewModel> GetCreateFormAsync(int academicId);
        Task CreateAsync(CreateStAcademicDataDataViewModel model);

        // التعديل (Edit)
        Task<CreateStAcademicDataDataViewModel> GetEditFormAsync(int id);
        Task EditAsync(CreateStAcademicDataDataViewModel model);

        // الحذف
        Task DeleteAsync(int id);

        // عمليات خاصة
        Task PromoteAllStudentsToNextYearAsync(); // ترحيل الطلاب
        Task FillListsForModelAsync(CreateStAcademicDataDataViewModel model); // إعادة تعبئة القوائم عند الخطأ

        // الطباعة
        Task<PrintConfEnrollVM> GetPrintConfEnrollAsync(int id);
        Task<PrintConfEnrollVM> GetPrintGradeReportAsync(int id);
        Task<PrintGraduatStatementVM> GetPrintGraduatStatementAsync(int id);
        Task<PrintAlmushayakhaStatementVM> GetPrintAlmushayakhaStatementAsync(int id);

        // التصدير
        Task<MemoryStream> ExportStAcademicDataToExcelAsync(StAcademicDataFilterVM filter);
        Task<MemoryStream> ExportGraduateStToExcelAsync(StAcademicDataFilterVM filter);
        Task<StudentGradesFilterVM> GetStudentGradesReportAsync(int academicId, int? level, int? term, int? yearId);

        Task<SingleTermGradesVM> GetSingleTermGradesAsync(int stAcademicDataId);
    }
}