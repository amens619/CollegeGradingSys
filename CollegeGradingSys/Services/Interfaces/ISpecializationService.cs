using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels.Specialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ISpecializationService : IGenericService<Specialization>
    {
        // Use Cases
        Task<SpecializationVM> GetCreateViewModelAsync();
        Task<(bool Success, string ErrorMessage)> CreateSpecializationAsync(SpecializationVM model);
        Task<SpecializationVM> GetEditViewModelAsync(int id);
        Task<(bool Success, string ErrorMessage)> UpdateSpecializationAsync(int id, SpecializationVM model);
        Task<(bool CanDelete, string ErrorMessage)> CanDeleteSpecializationAsync(int id);
        Task<bool> IsSpecializationNameExistsAsync(string specializationName, int? excludeId = null);
        //Task<List<Department>> GetDepartmentsAsync();
    }
}

