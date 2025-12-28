using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class CourseDbRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _db;

        public CourseDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ===== IRepository =====

        public async Task<IList<Course>> ListAsync()
        {
            return await _db.Course.ToListAsync();
        }

        public async Task<Course> FindAsync(int id)
        {
            return await _db.Course.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> AddAsync(Course entity)
        {
            await _db.Course.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Course> UpdateAsync(Course entity)
        {
            _db.Course.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Course> DeleteAsync(int id)
        {
            var course = await FindAsync(id);
            if (course == null)
                return null;

            _db.Course.Remove(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public IQueryable<Course> Query()
        {
            return _db.Course.AsQueryable();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Course.AnyAsync(c => c.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Course.CountAsync();
        }

        // ===== Course Specific =====

        public async Task<IList<Course>> ListWithRelationsAsync()
        {
            return await _db.Course
                .Include(c => c.Specialization)
                .Include(c => c.Parent)
                .ToListAsync();
        }

        public async Task<Course> FindWithRelationsAsync(int id)
        {
            return await _db.Course
                .Include(c => c.Specialization)
                .Include(c => c.Parent)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
