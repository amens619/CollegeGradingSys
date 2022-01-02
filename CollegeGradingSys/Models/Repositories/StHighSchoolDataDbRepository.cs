using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StHighSchoolDataDbRepository : ICollegeGradingSysRepository<StHighSchoolData>
    {
        private readonly ApplicationDbContext db;

        public StHighSchoolDataDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public StHighSchoolData Add(StHighSchoolData entity)
        {
            db.StHighSchoolData.Add(entity);
            SaveChange();
            return entity;
        }

        public StHighSchoolData Delete(int id)
        {
            StHighSchoolData stHighSchoolData = Find(id);
            if (stHighSchoolData != null)
            {
                db.StHighSchoolData.Remove(stHighSchoolData);
                SaveChange();
            }
            return stHighSchoolData;
        }

        public StHighSchoolData Find(int id)
        {
            return db.StHighSchoolData.SingleOrDefault(a => a.AcademicID == id);
        }

        public IList<StHighSchoolData> List()
        {
            return db.StHighSchoolData.ToList();
        }

        public StHighSchoolData Update(int id, StHighSchoolData newStHighSchoolData)
        {
            
            var college = db.StHighSchoolData.Attach(newStHighSchoolData);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newStHighSchoolData;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






