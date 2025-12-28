using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface IStPersonalDataRepository : IRepository<StPersonalData>
    {
        Task<StPersonalData> FindFullAsync(int academicId);
        Task<IList<StPersonalData>> ListFullAsync();
    }

}
