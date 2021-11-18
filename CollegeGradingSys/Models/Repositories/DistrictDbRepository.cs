using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class DistrictDbRepository : ICollegeGradingSysRepository<District>
    {
        private readonly ApplicationDbContext db;

        public DistrictDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }










        public void Add(District entity)
        {
            db.District.Add(entity);
            SaveChange();
        }

        public void Delete(int id)
        {
            var specialization = Find(id);
            db.District.Remove(specialization);
            SaveChange();
        }

        public District Find(int id)
        {
            return db.District.Include(a => a.Governorate).SingleOrDefault(a => a.Id == id);
        }

        public IList<District> List()
        {
            return db.District.Include(a=> a.Governorate).ToList();
        }

        public void Update(int id, District newDistrict)
        {
            db.District.Update(newDistrict);
            SaveChange();
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






