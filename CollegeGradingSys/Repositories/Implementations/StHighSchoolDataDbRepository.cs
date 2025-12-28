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
//    public class StHighSchoolDataDbRepository : ICollegeGradingSysRepository<StHighSchoolData>
//    {
//        private readonly ApplicationDbContext db;

//        public StHighSchoolDataDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }
//        public StHighSchoolData Add(StHighSchoolData entity)
//        {
//            db.StHighSchoolData.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public StHighSchoolData Delete(int id)
//        {
//            StHighSchoolData stHighSchoolData = Find(id);
//            if (stHighSchoolData != null)
//            {
//                db.StHighSchoolData.Remove(stHighSchoolData);
//                SaveChange();
//            }
//            return stHighSchoolData;
//        }

//        public StHighSchoolData Find(int id)
//        {
//            return db.StHighSchoolData
//                .Include(s => s.StPersonalData)
//                .SingleOrDefault(a => a.AcademicID == id);
//        }

//        public IList<StHighSchoolData> List()
//        {
//            return db.StHighSchoolData.Include(s => s.StPersonalData).ToList();
//        }

//        public StHighSchoolData Update(int id, StHighSchoolData newStHighSchoolData)
//        {

//            var college = db.StHighSchoolData.Attach(newStHighSchoolData);
//            college.State = EntityState.Modified;
//            SaveChange();
//            return newStHighSchoolData;
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
    public class StHighSchoolDataDbRepository
        : IStHighSchoolDataRepository
    {
        private readonly ApplicationDbContext _db;

        public StHighSchoolDataDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<StHighSchoolData> Query()
        {
            return _db.StHighSchoolData
                .Include(s => s.StPersonalData)
                .AsQueryable();
        }

        // ========= Custom =========
        public async Task<StHighSchoolData> GetByAcademicIdAsync(int academicId)
        {
            return await Query()
                .SingleOrDefaultAsync(x => x.AcademicID == academicId);
        }

        // ========= Read =========
        public async Task<IList<StHighSchoolData>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<StHighSchoolData> FindAsync(int id)
        {
            return await _db.StHighSchoolData.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int academicID)
        {
            return await _db.StHighSchoolData.AnyAsync(x => x.AcademicID == academicID);
        }

        public async Task<int> CountAsync()
        {
            return await _db.StHighSchoolData.CountAsync();
        }

        // ========= Write =========
        public async Task<StHighSchoolData> AddAsync(StHighSchoolData entity)
        {
            _db.StHighSchoolData.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<StHighSchoolData> UpdateAsync(StHighSchoolData entity)
        {
            _db.StHighSchoolData.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<StHighSchoolData> DeleteAsync(int id)
        {
            var entity = await _db.StHighSchoolData.FindAsync(id);
            if (entity == null) return null;

            _db.StHighSchoolData.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}






