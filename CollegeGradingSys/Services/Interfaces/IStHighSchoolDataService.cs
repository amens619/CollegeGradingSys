using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IStHighSchoolDataService
    {
        // ===== CRUD (عبر GenericService) =====
        Task<StHighSchoolData> GetByIdAsync(int id);
        Task<IList<StHighSchoolData>> GetAllAsync();
        Task<StHighSchoolData> CreateAsync(StHighSchoolData entity);
        Task<StHighSchoolData> UpdateAsync(StHighSchoolData entity);
        Task<StHighSchoolData> DeleteAsync(int id);

        // ===== Business / Contextual =====
        Task<StHighSchoolData> GetByAcademicIdAsync(int academicId);
        Task<bool> ExistsAsync(int academicId);
    }
}