//using CollegeGradingSys.Data;
//using CollegeGradingSys.Models;
//using CollegeGradingSys.Repositories.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CollegeGradingSys.Repositories.Implementations
//{
//    public class CourseGradeDbRepository : ICollegeGradingSysRepository<CourseGrade>
//    {
//        private readonly ApplicationDbContext db;

//        public CourseGradeDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }



//        public CourseGrade Add(CourseGrade entity)
//        {
//            db.CourseGrade.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public CourseGrade Delete(int id)
//        {
//            var courseGrade = Find(id);
//            if (courseGrade != null)
//            {
//                db.CourseGrade.Remove(courseGrade);
//                SaveChange();
//            }
//            return courseGrade;
//        }

//        public CourseGrade Find(int id)
//        {
//            return db.CourseGrade
//                .Include(x => x.StAcademicData)
//                    .ThenInclude(y => y.AcademicYear)
//                .Include(y => y.StAcademicData)
//                    .ThenInclude(y => y.Batch)
//                .Include(y => y.StAcademicData)
//                    .ThenInclude(y => y.StPersonalData)
//                .Include(x => x.Course)
//                .SingleOrDefault(a => a.Id == id);
//        }

//        public IList<CourseGrade> List()
//        {
//            return db.CourseGrade
//               .Include(x => x.StAcademicData)
//                    .ThenInclude(y => y.AcademicYear)
//                .Include(y => y.StAcademicData)
//                    .ThenInclude(y => y.Batch)
//                 .Include(y => y.StAcademicData)
//                    .ThenInclude(y => y.StPersonalData)
//                .Include(x => x.Course)                
//                .ToList();
//        }

//        public CourseGrade Update(int id, CourseGrade newCourseGrade)
//        {

//            var course = db.CourseGrade.Attach(newCourseGrade);
//            course.State = EntityState.Modified;
//            SaveChange();
//            return newCourseGrade;

//        }

//        private void SaveChange()
//        {
//            db.SaveChanges();
//        }
//    }
//}




using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
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

        // ========= IRepository =========

        public async Task<IList<CourseGrade>> ListAsync()
        {
            return await _db.CourseGrade.ToListAsync();
        }

        public async Task<CourseGrade> FindAsync(int id)
        {
            return await _db.CourseGrade.SingleOrDefaultAsync(x => x.Id == id);
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

        public async Task<CourseGrade> DeleteAsync(int id)
        {
            var grade = await FindAsync(id);
            if (grade == null)
                return null;

            _db.CourseGrade.Remove(grade);
            await _db.SaveChangesAsync();
            return grade;
        }

        public IQueryable<CourseGrade> Query()
        {
            return _db.CourseGrade.AsQueryable();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.CourseGrade.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.CourseGrade.CountAsync();
        }

        // ========= CourseGrade Specific =========

        public async Task<IList<CourseGrade>> ListWithRelationsAsync()
        {
            return await _db.CourseGrade
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.AcademicYear)
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.Batch)
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.StPersonalData)
                .Include(x => x.Course)
                .ToListAsync();
        }

        public async Task<CourseGrade> FindWithRelationsAsync(int id)
        {
            return await _db.CourseGrade
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.AcademicYear)
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.Batch)
                .Include(x => x.StAcademicData)
                    .ThenInclude(y => y.StPersonalData)
                .Include(x => x.Course)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}




