using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class GeneralInfoDbRepository : IRepository<GeneralInfo>
    {
        private readonly ApplicationDbContext _db;

        public GeneralInfoDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<GeneralInfo> Query()
        {
            return _db.GeneralInfo.AsQueryable();
        }

        // ========= Read =========
        public async Task<IList<GeneralInfo>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<GeneralInfo> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.GeneralInfo.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.GeneralInfo.CountAsync();
        }

        // ========= Write =========
        public async Task<GeneralInfo> AddAsync(GeneralInfo entity)
        {
            _db.GeneralInfo.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<GeneralInfo> UpdateAsync(GeneralInfo entity)
        {
            _db.GeneralInfo.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<GeneralInfo> DeleteAsync(int id)
        {
            var entity = await _db.GeneralInfo.FindAsync(id);
            if (entity == null) return null;

            _db.GeneralInfo.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}





