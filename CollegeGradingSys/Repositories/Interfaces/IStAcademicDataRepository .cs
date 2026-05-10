using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    //public interface IStAcademicDataRepository : IRepository<StAcademicData>
    //{
    //    Task<List<StAcademicData>> GetFilteredAsync(
    //        int? academicYearId,
    //        int? batchId,
    //        string nameSearch,
    //        StStatus? status,
    //        Term? term,
    //        StudyType? studyType
    //    );
    //}

    public interface IStAcademicDataRepository
    {
        IQueryable<StAcademicData> Query();
        Task<IList<StAcademicData>> ListAsync();
        Task<StAcademicData> FindAsync(int id);
        IQueryable<StAcademicData> GetFilteredQuery(int? academicYearId, int? batchId, string nameSearch, StStatus? status, Term? term, StudyType? studyType);

        Task<StAcademicData> AddAsync(StAcademicData entity);
        Task<StAcademicData> UpdateAsync(StAcademicData entity);
        Task<StAcademicData> DeleteAsync(int id);
    }
}
