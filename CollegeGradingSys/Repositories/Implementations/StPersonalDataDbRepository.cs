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
//    public class StPersonalDataDbRepository : ICollegeGradingSysRepository<StPersonalData>
//    {
//        private readonly ApplicationDbContext db;

//        public StPersonalDataDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }

//        public StPersonalData Add(StPersonalData entity)
//        {
//            db.StPersonalData.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public StPersonalData Delete(int id)
//        {
//            var stPersonalData = Find(id);
//            if (stPersonalData != null)
//            {
//                db.StPersonalData.Remove(stPersonalData);
//                SaveChange();
//            }
//            return stPersonalData;
//        }

//        public StPersonalData Find(int id)
//        {
//            return db.StPersonalData.Include(x => x.StHighSchoolData)
//                .Include(x => x.StAcademicDatas)
//                .ThenInclude(x => x.Batch)
//                 .Include(x => x.Birthcountry)
//                .Include(x => x.BirthGovernorate)
//                .Include(x => x.EnrollmentYear)
//                .SingleOrDefault(a => a.AcademicID == id);
//        }

//        public IList<StPersonalData> List()
//        {
//            return db.StPersonalData.Include(x => x.StHighSchoolData)
//                .Include(x => x.Nationality)
//                .Include(x => x.StAcademicDatas)
//                .ThenInclude(x => x.Batch)
//                .Include(x => x.BirthGovernorate)
//                .Include(x => x.EnrollmentYear)
//                .ToList();
//        }

//        public StPersonalData Update(int id, StPersonalData newStPersonalData)
//        {            
//            var college = db.StPersonalData.Attach(newStPersonalData);
//            college.State = EntityState.Modified;
//            SaveChange();
//            return newStPersonalData;
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
    public class StPersonalDataDbRepository : IStPersonalDataRepository
    {
        private readonly ApplicationDbContext _db;

        public StPersonalDataDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        private IQueryable<StPersonalData> QueryFull()
        {
            return _db.StPersonalData
                .Include(x => x.StHighSchoolData)
                .Include(x => x.Nationality)
                .Include(x => x.StAcademicDatas)
                    .ThenInclude(x => x.Batch)
                .Include(x => x.Birthcountry)
                .Include(x => x.BirthGovernorate)
                .Include(x => x.EnrollmentYear);
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
            _db.StPersonalData.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task<StPersonalData> DeleteAsync(int id)
        {
            var e = await FindAsync(id);
            if (e == null) return null;
            _db.StPersonalData.Remove(e);
            await _db.SaveChangesAsync();
            return e;
        }

        public IQueryable<StPersonalData> Query()
        {
            throw new System.NotImplementedException();
        }

     
    }

}

