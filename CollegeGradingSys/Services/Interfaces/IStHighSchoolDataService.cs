using CollegeGradingSys.Models;
using CollegeGradingSys.Services.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IStHighSchoolDataService : IGenericService<StHighSchoolData>
    {
        // ===== Business / Contextual =====
        Task<StHighSchoolData> GetByAcademicIdAsync(int academicId);
    }
}