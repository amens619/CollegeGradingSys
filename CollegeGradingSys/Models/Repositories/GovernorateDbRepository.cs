using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class GovernorateDbRepository : ICollegeGradingSysRepository<Governorate>
    {
        private readonly ApplicationDbContext db;

        public GovernorateDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public Governorate Add(Governorate entity)
        {
            db.Governorate.Add(entity);
            SaveChange();
            return entity;
        }

        public Governorate Delete(int id)
        {
            var governorate = Find(id);
            if (governorate != null)
            {
                db.Governorate.Remove(governorate);
                SaveChange();
            }
            return governorate;
        }

        public Governorate Find(int id)
        {
            return db.Governorate.Include(a => a.Nationality).SingleOrDefault(a => a.Id == id);
        }

        public IList<Governorate> List()
        {
            return db.Governorate.Include(a => a.Nationality).ToList();
        }

        public Governorate Update(int id, Governorate newGovernorate)
        {
            var college = db.Governorate.Attach(newGovernorate);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newGovernorate;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






