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
//    public class SpecializationDbRepository : ICollegeGradingSysRepository<Specialization>
//    {
//        private readonly ApplicationDbContext db;

//        public SpecializationDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }

//        public Specialization Add(Specialization entity)
//        {
//            db.Specialization.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public Specialization Delete(int id)
//        {
//            var specialization = Find(id);
//            if (specialization != null)
//            {
//                db.Specialization.Remove(specialization);
//                SaveChange();
//            }
//            return specialization;
//        }

//        public Specialization Find(int id)
//        {
//            return db.Specialization.Include(a => a.Department).SingleOrDefault(a => a.Id == id);
//        }

//        public IList<Specialization> List()
//        {
//            return db.Specialization.Include(a=> a.Department).ToList();
//        }

//        public Specialization Update(int id, Specialization newSpecialization)
//        {            
//            var college = db.Specialization.Attach(newSpecialization);
//            college.State = EntityState.Modified;
//            SaveChange();
//            return newSpecialization;
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
    public class SpecializationDbRepository : IRepository<Specialization>
    {
        private readonly ApplicationDbContext _db;

        public SpecializationDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<Specialization> Query()
        {
            return _db.Specialization
                .Include(s => s.Department)
                .AsQueryable();
        }

        // ========= Read =========
        public async Task<IList<Specialization>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<Specialization> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Specialization.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Specialization.CountAsync();
        }

        // ========= Write =========
        public async Task<Specialization> AddAsync(Specialization entity)
        {
            _db.Specialization.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Specialization> UpdateAsync(Specialization entity)
        {
            _db.Specialization.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Specialization> DeleteAsync(int id)
        {
            var entity = await _db.Specialization.FindAsync(id);
            if (entity == null) return null;

            _db.Specialization.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
