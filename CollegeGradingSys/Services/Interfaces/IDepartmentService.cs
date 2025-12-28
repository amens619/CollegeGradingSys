using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IDepartmentService : IGenericService<Department>
    {
        // Use Cases
        Task<CollegeDepartmentViewModel> GetCreateViewModelAsync();
        Task<(bool Success, string ErrorMessage)> CreateDepartmentAsync(CollegeDepartmentViewModel model);
        Task<CollegeDepartmentViewModel> GetEditViewModelAsync(int id);
        Task<(bool Success, string ErrorMessage)> UpdateDepartmentAsync(int id, CollegeDepartmentViewModel model);
        Task<(bool CanDelete, string ErrorMessage)> CanDeleteDepartmentAsync(int id);
        Task<bool> IsDepartmentNameExistsAsync(string departmentName, int? excludeId = null);
        Task<List<College>> GetCollegesAsync();
    }
}

