using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{   
    public interface IStPersonalDataService : IGenericService<StPersonalData>
    {
        Task<StPersonalData> GetFullAsync(int academicId);
        Task<IList<StPersonalData>> GetAllFullAsync();
        Task<IEnumerable<StPersonalData>> GetByEnrollmentYearAsync(int enrollmentYearId);
        Task EnsureCountryCanBeDeletedAsync(int countryId);
        Task<bool> IsAcademicIdAvailableAsync(int academicId);

        // ✨ الدالة الجديدة: تسجيل طالب وإنشاء سجلاته الأكاديمية ودرجاته تلقائياً
        Task RegisterNewStudentAsync(StPersonalData student, int batchId);
    }
}
