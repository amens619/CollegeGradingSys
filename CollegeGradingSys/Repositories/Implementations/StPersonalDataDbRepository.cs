using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using GemBox.Spreadsheet.Drawing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class StPersonalDataDbRepository : IStPersonalDataRepository
    {
        private readonly ApplicationDbContext _db;

        public StPersonalDataDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        //private IQueryable<StPersonalData> QueryFull()
        //{
        //    return _db.StPersonalData
        //        .AsNoTracking()
        //        .Include(x => x.StHighSchoolData)
        //        .Include(x => x.Nationality)
        //        //.Include(x => x.StAcademicDatas)
        //        //    .ThenInclude(x => x.Batch)
        //            // 💡 هنا التعديل السحري:
        //  .Include(x => x.StAcademicDatas)
        //      .ThenInclude(a => a.Batch) // 1. جلبنا الدفعة
        //          .ThenInclude(b => b.Specialization) // 2. جلبنا التخصص الموجود داخل الدفعة
        //              .ThenInclude(s => s.Department) // 3. جلبنا القسم الموجود داخل التخصصؤا
        //                   .ThenInclude(d => d.College)
        //        .Include(x => x.Birthcountry)
        //        .Include(x => x.BirthGovernorate)
        //        .Include(x => x.EnrollmentYear);
        //}
        private IQueryable<StPersonalData> QueryFull()
        {
            return _db.StPersonalData
                .AsNoTracking()
                .Include(x => x.StHighSchoolData)
                .Include(x => x.Nationality)
                .Include(x => x.Birthcountry)
                .Include(x => x.BirthGovernorate)
                .Include(x => x.EnrollmentYear)

                // 💡 هنا الشجرة المتسلسلة للسجل الأكاديمي وملحقاته
                .Include(x => x.StAcademicDatas)
                    .ThenInclude(a => a.Batch)
                        .ThenInclude(b => b.Specialization)
                            .ThenInclude(s => s.Department)
                                .ThenInclude(d => d.College)

                // 💡 وهنا الشجرة المتسلسلة لجلب الدرجات من داخل السجل الأكاديمي
                .Include(x => x.StAcademicDatas)
                    .ThenInclude(a => a.CourseGrades)
                        .ThenInclude(cg => cg.Course)
                .Include(x => x.StAcademicDatas)
                    .ThenInclude(a => a.AcademicYear);
        }

        public async Task<IList<StPersonalData>> ListFullAsync()
            => await QueryFull().ToListAsync();

        public async Task<StPersonalData> FindFullAsync(int academicId)
            => await QueryFull().SingleOrDefaultAsync(x => x.AcademicID == academicId);

        // CRUD العادي        
        public async Task<IList<StPersonalData>> ListAsync()
        {
            return await _db.StPersonalData.ToListAsync();
        }
       
        public Task<StPersonalData> FindAsync(int id) => _db.StPersonalData.FindAsync(id).AsTask();       
        public Task<bool> ExistsAsync(int id) => _db.StPersonalData.AnyAsync(x => x.AcademicID == id);
        public Task<int> CountAsync() => _db.StPersonalData.CountAsync();
        public async Task<StPersonalData> AddAsync(StPersonalData entity)
        {
            _db.StPersonalData.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
          
        }
        public async Task<StPersonalData> UpdateAsync(StPersonalData entity)
        {           
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task<StPersonalData> DeleteAsync(int id)
        {
            var e = await _db.StPersonalData.FindAsync(id);
            if (e == null) return null;

            _db.StPersonalData.Remove(e);
            await _db.SaveChangesAsync();
            return e;
        }



        public IQueryable<StPersonalData> Query() => _db.StPersonalData.AsNoTracking();
    }

}

