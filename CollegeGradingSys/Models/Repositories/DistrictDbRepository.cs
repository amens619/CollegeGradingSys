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
        public District Add(District entity)
        {
            db.District.Add(entity);
            SaveChange();
            return entity;
        }

        public District Delete(int id)
        {
            District district = Find(id);
            if (district != null)
            {
                db.District.Remove(district);
                SaveChange();
            }
            return district;
        }

        public District Find(int id)
        {
            return db.District.Include(a => a.Governorate).SingleOrDefault(a => a.Id == id);
        }

        public IList<District> List()
        {
            return db.District.Include(a=> a.Governorate).ToList();
        }

        public District Update(int id, District newDistrict)
        {
            
            var college = db.District.Attach(newDistrict);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newDistrict;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






