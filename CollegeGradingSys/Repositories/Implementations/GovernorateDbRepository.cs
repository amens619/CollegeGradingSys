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
//    public class GovernorateDbRepository : ICollegeGradingSysRepository<Governorate>
//    {
//        private readonly ApplicationDbContext db;

//        public GovernorateDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }

//        public Governorate Add(Governorate entity)
//        {
//            db.Governorate.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public Governorate Delete(int id)
//        {
//            var governorate = Find(id);
//            if (governorate != null)
//            {
//                db.Governorate.Remove(governorate);
//                SaveChange();
//            }
//            return governorate;
//        }

//        public Governorate Find(int id)
//        {
//            return db.Governorate.Include(a => a.Nationality).SingleOrDefault(a => a.Id == id);
//        }

//        public IList<Governorate> List()
//        {
//            return db.Governorate.Include(a => a.Nationality).ToList();
//        }

//        public Governorate Update(int id, Governorate newGovernorate)
//        {
//            var college = db.Governorate.Attach(newGovernorate);
//            college.State = EntityState.Modified;
//            SaveChange();
//            return newGovernorate;
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
    public class GovernorateDbRepository : IRepository<Governorate>
    {
        private readonly ApplicationDbContext _db;

        public GovernorateDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<Governorate> Query()
        {
            return _db.Governorate
                .Include(g => g.Nationality)
                .AsQueryable();
        }

        // ========= Read =========
        public async Task<IList<Governorate>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<Governorate> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Governorate.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Governorate.CountAsync();
        }

        // ========= Write =========
        public async Task<Governorate> AddAsync(Governorate entity)
        {
            _db.Governorate.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Governorate> UpdateAsync(Governorate entity)
        {
            _db.Governorate.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Governorate> DeleteAsync(int id)
        {
            var entity = await _db.Governorate.FindAsync(id);
            if (entity == null) return null;

            _db.Governorate.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}




