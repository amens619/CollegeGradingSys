//using CollegeGradingSys.Data;
//using CollegeGradingSys.Models;
//using CollegeGradingSys.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CollegeGradingSys.Repositories.Implementations
//{
//    public class DBSettingsDbRepository : ICollegeGradingSysRepository<DBSettings>
//    {
//        private readonly ApplicationDbContext db;

//        public DBSettingsDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }
//        public DBSettings Add(DBSettings entity)
//        {
//            db.DBSettings.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public DBSettings Delete(int id)
//        {
//            DBSettings dBSettings = Find(id);
//            if (dBSettings != null)
//            {
//                db.DBSettings.Remove(dBSettings);
//                SaveChange();
//            }
//            return dBSettings;
//        }

//        public DBSettings Find(int id)
//        {
//            return db.DBSettings.SingleOrDefault(a => a.Id == id);
//        }

//        public IList<DBSettings> List()
//        {
//            return db.DBSettings.ToList();
//        }

//        public DBSettings Update(int id, DBSettings newDBSettings)
//        {

//            var college = db.DBSettings.Attach(newDBSettings);
//            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//            SaveChange();
//            return newDBSettings;
//        }

//        private void SaveChange()
//        {
//            db.SaveChanges();
//        }
//    }
//}
using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class DBSettingsDbRepository : IRepository<DBSettings>
    {
        private readonly ApplicationDbContext _db;

        public DBSettingsDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IList<DBSettings>> ListAsync()
        {
            return await _db.DBSettings.ToListAsync();
        }

        public async Task<DBSettings> FindAsync(int id)
        {
            return await _db.DBSettings.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DBSettings> AddAsync(DBSettings entity)
        {
            await _db.DBSettings.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<DBSettings> UpdateAsync(DBSettings entity)
        {
            _db.DBSettings.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<DBSettings> DeleteAsync(int id)
        {
            var setting = await FindAsync(id);
            if (setting == null)
                return null;

            _db.DBSettings.Remove(setting);
            await _db.SaveChangesAsync();
            return setting;
        }

        public IQueryable<DBSettings> Query()
        {
            return _db.DBSettings.AsQueryable();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.DBSettings.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.DBSettings.CountAsync();
        }
    }
}
