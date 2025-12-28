using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ISpecializationService : IGenericService<Specialization>
    {
        // Use Cases
        Task<DepartmentSpecializationViewModel> GetCreateViewModelAsync();
        Task<(bool Success, string ErrorMessage)> CreateSpecializationAsync(DepartmentSpecializationViewModel model);
        Task<DepartmentSpecializationViewModel> GetEditViewModelAsync(int id);
        Task<(bool Success, string ErrorMessage)> UpdateSpecializationAsync(int id, DepartmentSpecializationViewModel model);
        Task<(bool CanDelete, string ErrorMessage)> CanDeleteSpecializationAsync(int id);
        Task<bool> IsSpecializationNameExistsAsync(string specializationName, int? excludeId = null);
        Task<List<Department>> GetDepartmentsAsync();
    }
}

