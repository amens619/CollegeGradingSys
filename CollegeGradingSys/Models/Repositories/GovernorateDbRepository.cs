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










        public void Add(Governorate entity)
        {
            db.Governorate.Add(entity);
            SaveChange();
        }

        public void Delete(int id)
        {
            var governorate = Find(id);
            db.Governorate.Remove(governorate);
            SaveChange();
        }

        public Governorate Find(int id)
        {
            return db.Governorate.Include(a => a.Nationality).SingleOrDefault(a => a.Id == id);
        }

        public IList<Governorate> List()
        {
            return db.Governorate.Include(a => a.Nationality).ToList();
        }

        public void Update(int id, Governorate newGovernorate)
        {
            db.Governorate.Update(newGovernorate);
            SaveChange();
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






