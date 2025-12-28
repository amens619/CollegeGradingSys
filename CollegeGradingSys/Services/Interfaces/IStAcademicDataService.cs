using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IStAcademicDataService
    {
        Task<IList<StAcademicData>> GetAllAsync();

        Task<IList<StAcademicData>> GetFilteredAsync(
            int? academicYearId,
            int? batchId,
            string studentName,
            StStatus? stStatus,
            Term? term,
            StudyType? studyType);

        Task<StAcademicData> GetByIdAsync(int id);

        Task<StAcademicData> AddAsync(StAcademicData entity);
        Task<StAcademicData> UpdateAsync(StAcademicData entity);
        Task<StAcademicData> DeleteAsync(int id);
    }
}
