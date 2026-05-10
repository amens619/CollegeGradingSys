using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class StHighSchoolDataService
    : GenericService<StHighSchoolData>, IStHighSchoolDataService
    {
        private readonly IStHighSchoolDataRepository _repo;

        public StHighSchoolDataService(IStHighSchoolDataRepository repo)
            : base(repo)
        {
            _repo = repo;
        }

        public Task<StHighSchoolData> GetByAcademicIdAsync(int academicId)
            => _repo.GetByAcademicIdAsync(academicId);

       
    }

}
