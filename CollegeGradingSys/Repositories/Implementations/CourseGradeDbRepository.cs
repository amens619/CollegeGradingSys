using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.ViewModels.AcademicYear;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class CourseGradeDbRepository : ICourseGradeRepository
    {
        private readonly ApplicationDbContext _db;

        public CourseGradeDbRepository(ApplicationDbContext db) 

        {
            _db = db;
        }
        public async Task<IList<CourseGrade>> ListAsync()
        {
            return await _db.CourseGrade.ToListAsync();
        }

        public async Task<CourseGrade> FindAsync(int id)
        {
            return await _db.CourseGrade.FindAsync(id);
        }

        public async Task<CourseGrade> AddAsync(CourseGrade entity)
        {
            await _db.CourseGrade.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<CourseGrade> UpdateAsync(CourseGrade entity)
        {
            _db.CourseGrade.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        //public async Task UpdateAsync(CourseGrade entity)
        //{
        //    _db.CourseGrade.Update(entity);
        //    await _db.SaveChangesAsync();
        //}
        public async Task<CourseGrade> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                _db.CourseGrade.Remove(entity);
                await _db.SaveChangesAsync();
            }
            return entity;
        }

        //public IQueryable<CourseGrade> Query()
        //{
        //    return _db.CourseGrade.AsQueryable();
        //}

        public async Task<bool> ExistsAsync(int id)
        {
            // نفترض أن المفتاح الأساسي اسمه Id
            return await _db.CourseGrade.AnyAsync(e => e.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.CourseGrade.CountAsync();
        }
        public IQueryable<CourseGrade> Query()
        {
            return _db.CourseGrade
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.StPersonalData)
                .Include(x => x.Course);
        }

        public async Task<CourseGrade> FindWithRelationsAsync(int id)
        {
            return await Query().FirstOrDefaultAsync(x => x.Id == id);
        }

      

        public async Task<List<CourseGrade>> GetGradesForBatchAsync(int batchId, int courseId)
        {
            return await Query()
                .Where(x => x.StAcademicData.Batch.Id == batchId && x.Course.Id == courseId)
                .Include(y => y.StAcademicData.AcademicYear)
                .OrderBy(x => x.StAcademicData.StPersonalData.StName)
                .ToListAsync();
        }
                
    }
}



