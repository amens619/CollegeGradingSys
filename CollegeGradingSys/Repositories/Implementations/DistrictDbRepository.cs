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
//    public class DistrictDbRepository : ICollegeGradingSysRepository<District>
//    {
//        private readonly ApplicationDbContext db;

//        public DistrictDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }
//        public District Add(District entity)
//        {
//            db.District.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public District Delete(int id)
//        {
//            District district = Find(id);
//            if (district != null)
//            {
//                db.District.Remove(district);
//                SaveChange();
//            }
//            return district;
//        }

//        public District Find(int id)
//        {
//            return db.District.Include(a => a.Governorate).SingleOrDefault(a => a.Id == id);
//        }

//        public IList<District> List()
//        {
//            return db.District.Include(a=> a.Governorate).ToList();
//        }

//        public District Update(int id, District newDistrict)
//        {

//            var college = db.District.Attach(newDistrict);
//            college.State = EntityState.Modified;
//            SaveChange();
//            return newDistrict;
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
    public class DistrictDbRepository : IRepository<District>
    {
        private readonly ApplicationDbContext _db;

        public DistrictDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<District> Query()
        {
            return _db.District
                .Include(d => d.Governorate)
                .AsQueryable();
        }

        // ========= Read =========
        public async Task<IList<District>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<District> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.District.AnyAsync(d => d.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.District.CountAsync();
        }

        // ========= Write =========
        public async Task<District> AddAsync(District entity)
        {
            _db.District.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<District> UpdateAsync(District entity)
        {
            _db.District.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<District> DeleteAsync(int id)
        {
            var entity = await _db.District.FindAsync(id);
            if (entity == null) return null;

            _db.District.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}






