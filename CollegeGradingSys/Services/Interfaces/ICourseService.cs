using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ICourseService : IGenericService<Course>
    {       
        Task<Course> GetByIdWithRelationsAsync(int id);
        Task<IList<Course>> GetAllWithRelationsAsync();
        
        // Use Cases
        Task<CourseIndexViewModel> GetIndexViewModelAsync(Term? term, Level? level, int? specializationId);
        Task<CourseDetailsViewModel> GetDetailsViewModelAsync(int id);
        CreateCourseViewModel GetCreateViewModelAsync();
        Task<(bool Success, string ErrorMessage)> CreateCourseAsync(CreateCourseViewModel model);
        Task<EditCourseViewModel> GetEditViewModelAsync(int id);
        Task<(bool Success, string ErrorMessage)> UpdateCourseAsync(EditCourseViewModel model);
        Task<(bool CanDelete, string ErrorMessage)> CanDeleteCourseAsync(int id);
        Task<bool> IsCourseNameExistsAsync(string courseName, int? excludeId = null);
        Task<List<Course>> GetParentCoursesAsync();
        Task<List<Specialization>> GetSpecializationsAsync();
        Task<List<SelectItemVM>> GetCoursesByBatchAsync(int batchId, Level level, Term term);
        Task<List<SelectItemVM>> GetCoursesBySpecializationAsync(int specializationId, Level? level, Term? term);
    }
}
