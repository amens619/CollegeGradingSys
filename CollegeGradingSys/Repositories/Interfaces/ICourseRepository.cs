using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IList<Course>> ListWithRelationsAsync();
        Task<Course> FindWithRelationsAsync(int id);
    }
}
