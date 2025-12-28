using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class CollegeDbRepository : IRepository<College>
    {
        private readonly ApplicationDbContext _db;

        public CollegeDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IList<College>> ListAsync()
        {
            return await _db.College.ToListAsync();
        }

        public async Task<College> FindAsync(int id)
        {
            return await _db.College.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<College> AddAsync(College entity)
        {
            await _db.College.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<College> UpdateAsync(College entity)
        {
            _db.College.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<College> DeleteAsync(int id)
        {
            var college = await FindAsync(id);
            if (college == null)
                return null;

            _db.College.Remove(college);
            await _db.SaveChangesAsync();
            return college;
        }

        public IQueryable<College> Query()
        {
            return _db.College.AsQueryable();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.College.AnyAsync(c => c.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.College.CountAsync();
        }
    }
}
