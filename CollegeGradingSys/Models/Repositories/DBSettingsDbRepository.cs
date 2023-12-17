using CollegeGradingSys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class DBSettingsDbRepository : ICollegeGradingSysRepository<DBSettings>
    {
        private readonly ApplicationDbContext db;

        public DBSettingsDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public DBSettings Add(DBSettings entity)
        {
            db.DBSettings.Add(entity);
            SaveChange();
            return entity;
        }

        public DBSettings Delete(int id)
        {
            DBSettings dBSettings = Find(id);
            if (dBSettings != null)
            {
                db.DBSettings.Remove(dBSettings);
                SaveChange();
            }
            return dBSettings;
        }

        public DBSettings Find(int id)
        {
            return db.DBSettings.SingleOrDefault(a => a.Id == id);
        }

        public IList<DBSettings> List()
        {
            return db.DBSettings.ToList();
        }

        public DBSettings Update(int id, DBSettings newDBSettings)
        {

            var college = db.DBSettings.Attach(newDBSettings);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newDBSettings;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}