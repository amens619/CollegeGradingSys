using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class SpecializationDbRepository : ICollegeGradingSysRepository<Specialization>
    {
        private readonly ApplicationDbContext db;

        public SpecializationDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public Specialization Add(Specialization entity)
        {
            db.Specialization.Add(entity);
            SaveChange();
            return entity;
        }

        public Specialization Delete(int id)
        {
            var specialization = Find(id);
            if (specialization != null)
            {
                db.Specialization.Remove(specialization);
                SaveChange();
            }
            return specialization;
        }

        public Specialization Find(int id)
        {
            return db.Specialization.Include(a => a.Department).SingleOrDefault(a => a.Id == id);
        }

        public IList<Specialization> List()
        {
            return db.Specialization.Include(a=> a.Department).ToList();
        }

        public Specialization Update(int id, Specialization newSpecialization)
        {            
            var college = db.Specialization.Attach(newSpecialization);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newSpecialization;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






