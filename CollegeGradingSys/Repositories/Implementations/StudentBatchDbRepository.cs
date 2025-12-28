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
//    public class BatchDbRepository : ICollegeGradingSysRepository<Batch>
//    {
//        private readonly ApplicationDbContext db;

//        public BatchDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }
//        public Batch Add(Batch entity)
//        {
//            db.Batch.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public Batch Delete(int id)
//        {
//            Batch studentBatch = Find(id);
//            if (studentBatch != null)
//            {
//                db.Batch.Remove(studentBatch);
//                SaveChange();
//            }
//            return studentBatch;
//        }

//        public Batch Find(int id)
//        {
//            return db.Batch.Include(x => x.Specialization).SingleOrDefault(a => a.Id == id);
//        }

//        public IList<Batch> List()
//        {
//            return db.Batch.Include(x => x.Specialization).ToList();
//        }

//        public Batch Update(int id, Batch newBatch)
//        {

//            var college = db.Batch.Attach(newBatch);
//            college.State = EntityState.Modified;
//            SaveChange();
//            return newBatch;
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
    public class BatchDbRepository : IRepository<Batch>
    {
        private readonly ApplicationDbContext _db;

        public BatchDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ================= Query =================
        public IQueryable<Batch> Query()
        {
            return _db.Batch
                      .Include(x => x.Specialization)
                      .AsNoTracking();
        }

        // ================= Read =================
        public async Task<IList<Batch>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<Batch> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Batch.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Batch.CountAsync();
        }

        // ================= Write =================
        public async Task<Batch> AddAsync(Batch entity)
        {
            _db.Batch.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Batch> UpdateAsync(Batch entity)
        {
            _db.Batch.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Batch> DeleteAsync(int id)
        {
            var batch = await _db.Batch.FindAsync(id);
            if (batch == null) return null;

            _db.Batch.Remove(batch);
            await _db.SaveChangesAsync();
            return batch;
        }
    }
}




