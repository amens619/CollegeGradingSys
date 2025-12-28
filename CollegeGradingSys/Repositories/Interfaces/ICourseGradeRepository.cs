using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface ICourseGradeRepository : IRepository<CourseGrade>
    {
        Task<IList<CourseGrade>> ListWithRelationsAsync();
        Task<CourseGrade> FindWithRelationsAsync(int id);
    }
}
