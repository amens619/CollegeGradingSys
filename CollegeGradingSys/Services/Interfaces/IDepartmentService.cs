using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels.Department;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IDepartmentService : IGenericService<Department>
    {
        // Use Cases
        Task<DepartmentVM> GetCreateViewModelAsync();
        Task<(bool Success, string ErrorMessage)> CreateDepartmentAsync(DepartmentVM model);
        Task<DepartmentVM> GetEditViewModelAsync(int id);
        Task<(bool Success, string ErrorMessage)> UpdateDepartmentAsync(int id, DepartmentVM model);
        Task<(bool CanDelete, string ErrorMessage)> CanDeleteDepartmentAsync(int id);
        Task<bool> IsDepartmentNameExistsAsync(string departmentName, int? excludeId = null);
    }
}

