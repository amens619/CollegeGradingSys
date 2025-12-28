using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface ICourseGradeService
    {
        Task<IList<CourseGrade>> GetAllAsync();
        Task<IList<CourseGrade>> GetAllWithRelationsAsync();
        Task<CourseGrade> GetByIdAsync(int id);
        Task<CourseGrade> GetByIdWithRelationsAsync(int id);
        Task<CourseGrade> CreateAsync(CourseGrade courseGrade);
        Task<CourseGrade> UpdateAsync(CourseGrade courseGrade);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
