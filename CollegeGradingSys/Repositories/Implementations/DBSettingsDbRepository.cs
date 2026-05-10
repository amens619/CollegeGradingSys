
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
