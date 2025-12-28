using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ICourseService : IGenericService<Course>
    {       
        Task<Course> GetByIdWithRelationsAsync(int id);
        Task<IList<Course>> GetAllWithRelationsAsync();
        
        // Use Cases
        Task<IList<Course>> GetFilteredCoursesAsync(Term? term, Level? level, int? specializationId);
        Task<bool> IsCourseNameExistsAsync(string courseName, int? excludeId = null);
        Task ValidateCourseNameAsync(string courseName, int? excludeId = null);
        Task ValidateGradesAsync(string bigGrade, string smallGrade);
        Task<(int bigGrade, int smallGrade)> ParseGradesAsync(string bigGrade, string smallGrade);
        Task ValidateSubCourseAsync(bool isSubCourse, int? parentId);
        Task<CourseDetailsViewModel> GetCourseDetailsAsync(int id);
        Task<CreateCourseViewModel> PrepareCreateCourseViewModelAsync();
        Task<EditCourseViewModel> PrepareEditCourseViewModelAsync(int id);
        Task CreateCourseAsync(CreateCourseViewModel viewModel);
        Task UpdateCourseAsync(EditCourseViewModel viewModel);
        Task<CourseDeleteViewModel> PrepareDeleteCourseViewModelAsync(int id);
        Task<CourseDeleteResult> CanDeleteCourseAsync(int id);
        Task DeleteCourseAsync(int id);
        Task<List<SelectItemVM>> GetSpecializationsSelectItemsAsync();
        Task<List<SelectItemVM>> GetParentCoursesSelectItemsAsync();
    }

    public class CourseDeleteResult
    {
        public bool CanDelete { get; set; }
        public string ErrorMessage { get; set; }
    }
}
