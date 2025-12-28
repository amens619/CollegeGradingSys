using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IStPersonalDataService : IGenericService<StPersonalData>
    {
        Task<StPersonalData> GetFullAsync(int academicId);
        Task<IList<StPersonalData>> GetAllFullAsync();
    }
}
