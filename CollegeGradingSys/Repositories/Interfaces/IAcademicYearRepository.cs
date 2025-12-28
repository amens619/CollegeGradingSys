using global::CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface IAcademicYearRepository : IRepository<AcademicYear>
    {
        Task<AcademicYear> GetCurrentYearAsync();
        Task<IReadOnlyList<AcademicYear>> GetAllOrderedByStartDateAsync();
    }
}
