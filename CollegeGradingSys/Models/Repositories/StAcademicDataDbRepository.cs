using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StAcademicDataDbRepository : ICollegeGradingSysRepository<StAcademicData>
    {
        private readonly ApplicationDbContext db;

        public StAcademicDataDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public StAcademicData Add(StAcademicData entity)
        {
            db.StAcademicData.Add(entity);
            SaveChange();
            return entity;
        }

        public StAcademicData Delete(int id)
        {
            var stAcademicData = Find(id);
            if (stAcademicData != null)
            {
                db.StAcademicData.Remove(stAcademicData);
                SaveChange();
            }
            return stAcademicData;
        }

        public StAcademicData Find(int id)
        {
            return db.StAcademicData
                 //.Include(x => x.Batch)
                //.Include(x => x.AcademicYear)
                .Include(x => x.StPersonalData)
                .SingleOrDefault(a => a.Id == id);
        }

        public IList<StAcademicData> List()
        {
            return db.StAcademicData
                //.Include(x => x.Batch)                
                //.Include(x => x.AcademicYear)
                .Include(x => x.StPersonalData)
                .ToList();
        }

        public StAcademicData Update(int id, StAcademicData newStAcademicData)
        {            
            var college = db.StAcademicData.Attach(newStAcademicData);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newStAcademicData;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






