using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IAcademicYearService
    {
        Task<AcademicYearIndexVM> GetIndexViewAsync();

        Task<AcademicYearDetailsVM> GetDetailsAsync(int id);
        Task<List<AcademicYearSelectItemVM>> GetAcademicYearsAsync(
            string placeholder = "— اختر العام الدراسي —");

        Task<bool> IsAcademicYearNameExistsAsync(
            string name, int? excludeId = null);

        Task<bool> IsCurrentYearClosedAsync();

        Task<AcademicYear?> CreateAsync(AcademicYear newYear);

        Task<AcademicYearEditVM> GetEditViewAsync(int id);
        Task UpdateAsync(int id, AcademicYearEditVM vm);

        Task CloseYearAsync(int id);

        Task<AcademicYearDeleteVM?> GetDeleteViewAsync(int id);

        Task DeleteAsync(int id);
    }

}

