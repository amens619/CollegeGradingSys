using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<IList<College>> GetAllAsync();
        Task CreateAsync(CollegeCreateVM vm);
        Task<bool> ExistsByNameAsync(string name);
        Task<College?> GetByIdAsync(int id);
        Task UpdateAsync(CollegeVM vm);

        Task<bool> DeleteAsync(int id);
    }
}
