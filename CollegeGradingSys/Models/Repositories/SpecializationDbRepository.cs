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










        public void Add(Specialization entity)
        {
            db.Specialization.Add(entity);
            SaveChange();
        }

        public void Delete(int id)
        {
            var specialization = Find(id);
            db.Specialization.Remove(specialization);
            SaveChange();
        }

        public Specialization Find(int id)
        {
            return db.Specialization.Include(a => a.Department).SingleOrDefault(a => a.Id == id);
        }

        public IList<Specialization> List()
        {
            return db.Specialization.Include(a=> a.Department).ToList();
        }

        public void Update(int id, Specialization newSpecialization)
        {
            db.Specialization.Update(newSpecialization);
            SaveChange();
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






