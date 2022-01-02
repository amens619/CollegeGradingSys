using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StPersonalDataDbRepository : ICollegeGradingSysRepository<StPersonalData>
    {
        private readonly ApplicationDbContext db;

        public StPersonalDataDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public StPersonalData Add(StPersonalData entity)
        {
            db.StPersonalData.Add(entity);
            SaveChange();
            return entity;
        }

        public StPersonalData Delete(int id)
        {
            var stPersonalData = Find(id);
            if (stPersonalData != null)
            {
                db.StPersonalData.Remove(stPersonalData);
                SaveChange();
            }
            return stPersonalData;
        }

        public StPersonalData Find(int id)
        {
            return db.StPersonalData.SingleOrDefault(a => a.AcademicID == id);
        }

        public IList<StPersonalData> List()
        {
            return db.StPersonalData.Include(x => x.StHighSchoolData).ToList();
        }

        public StPersonalData Update(int id, StPersonalData newStPersonalData)
        {            
            var college = db.StPersonalData.Attach(newStPersonalData);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newStPersonalData;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






