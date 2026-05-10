using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IGovernorateService : IGenericService<Governorate>
    {       
        Task EnsureCountryCanBeDeletedAsync(int countryId);
        Task CreateWithEnsureGovernorateNameAsync(Governorate governorate);
        Task UpdateEnsureGovernorateNameAsync(Governorate governorate);
        Task DeleteWithValidationAsync(int id);
        Task<bool> IsGovernorateNameExistsAsync(string name, int? excludeId = null);
        Task EnsureGovernorateCanBeDeletedAsync(int governorateId);
    }
}
