using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class StPersonalDataService
    : GenericService<StPersonalData>, IStPersonalDataService
    {
        private readonly IStPersonalDataRepository _repo;

        public StPersonalDataService(IStPersonalDataRepository repo)
            : base(repo)
        {
            _repo = repo;
        }

        public Task<StPersonalData> GetFullAsync(int academicId)
            => _repo.FindFullAsync(academicId);

        public Task<IList<StPersonalData>> GetAllFullAsync()
            => _repo.ListFullAsync();
    }

}
