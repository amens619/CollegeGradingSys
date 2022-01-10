using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class BatchDbRepository : ICollegeGradingSysRepository<Batch>
    {
        private readonly ApplicationDbContext db;

        public BatchDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public Batch Add(Batch entity)
        {
            db.Batch.Add(entity);
            SaveChange();
            return entity;
        }

        public Batch Delete(int id)
        {
            Batch studentBatch = Find(id);
            if (studentBatch != null)
            {
                db.Batch.Remove(studentBatch);
                SaveChange();
            }
            return studentBatch;
        }

        public Batch Find(int id)
        {
            return db.Batch.SingleOrDefault(a => a.Id == id);
        }

        public IList<Batch> List()
        {
            return db.Batch.ToList();
        }

        public Batch Update(int id, Batch newBatch)
        {
            
            var college = db.Batch.Attach(newBatch);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newBatch;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






