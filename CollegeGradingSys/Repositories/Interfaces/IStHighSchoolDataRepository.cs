using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface IStHighSchoolDataRepository : IRepository<StHighSchoolData>
    {
        Task<StHighSchoolData> GetByAcademicIdAsync(int academicId);
    }

}
