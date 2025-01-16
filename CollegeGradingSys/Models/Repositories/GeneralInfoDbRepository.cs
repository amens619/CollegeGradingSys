using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class GeneralInfoDbRepository : ICollegeGradingSysRepository<GeneralInfo>
    {
        private readonly ApplicationDbContext db;

        public GeneralInfoDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public GeneralInfo Add(GeneralInfo entity)
        {
            db.GeneralInfo.Add(entity);
            SaveChange();
            return entity;
        }

        public GeneralInfo Delete(int id)
        {
            GeneralInfo generalInfo = Find(id);
            if (generalInfo != null)
            {
                db.GeneralInfo.Remove(generalInfo);
                SaveChange();
            }
            return generalInfo;
        }

        public GeneralInfo Find(int id)
        {
            return db.GeneralInfo.SingleOrDefault(a => a.Id == id);
        }

        public IList<GeneralInfo> List()
        {
            return db.GeneralInfo.ToList();
        }

        public GeneralInfo Update(int id, GeneralInfo newGeneralInfo)
        {
            
            var college = db.GeneralInfo.Attach(newGeneralInfo);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newGeneralInfo;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






