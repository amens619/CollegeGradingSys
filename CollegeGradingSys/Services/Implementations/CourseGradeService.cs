using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class CourseGradeService : ICourseGradeService
    {
        private readonly ICourseGradeRepository _courseGradeRepository;

        public CourseGradeService(ICourseGradeRepository courseGradeRepository)
        {
            _courseGradeRepository = courseGradeRepository;
        }

        public async Task<IList<CourseGrade>> GetAllAsync()
        {
            return await _courseGradeRepository.ListAsync();
        }

        public async Task<IList<CourseGrade>> GetAllWithRelationsAsync()
        {
            return await _courseGradeRepository.ListWithRelationsAsync();
        }

        public async Task<CourseGrade> GetByIdAsync(int id)
        {
            return await _courseGradeRepository.FindAsync(id);
        }

        public async Task<CourseGrade> GetByIdWithRelationsAsync(int id)
        {
            return await _courseGradeRepository.FindWithRelationsAsync(id);
        }

        public async Task<CourseGrade> CreateAsync(CourseGrade courseGrade)
        {
            return await _courseGradeRepository.AddAsync(courseGrade);
        }

        public async Task<CourseGrade> UpdateAsync(CourseGrade courseGrade)
        {
            return await _courseGradeRepository.UpdateAsync(courseGrade);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _courseGradeRepository.ExistsAsync(id))
                return false;

            await _courseGradeRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _courseGradeRepository.ExistsAsync(id);
        }
    }
}
