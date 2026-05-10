using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IExcelExportService
    {
        // ترجع المصفوفة النصية (byte[]) للملف لتكون جاهزة للتحميل
        byte[] GenerateAcceptedStudentsExcel(IList<StPersonalData> students, AcademicYear academicYear);

        byte[] GenerateHighSchoolDataExcel(IList<StPersonalData> students);

        Task<byte[]> GenerateStudentTranscriptExcelAsync(int AcademicID);
    }
}
