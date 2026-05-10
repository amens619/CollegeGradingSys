using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels.College;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ICollegeService : IGenericService<College>
    {
      
        Task CreateAsync(CollegeCreateVM vm);
        Task UpdateAsync(CollegeVM vm);

        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);

    }
}
