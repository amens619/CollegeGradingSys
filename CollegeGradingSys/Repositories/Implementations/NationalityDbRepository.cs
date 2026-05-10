using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class NationalityDbRepository : IRepository<Nationality>
    {
        private readonly ApplicationDbContext _db;

        public NationalityDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<Nationality> Query()
        {
            return _db.Nationality.AsQueryable();
        }

        // ========= Read =========
        public async Task<IList<Nationality>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<Nationality> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Nationality.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Nationality.CountAsync();
        }

        // ========= Write =========
        public async Task<Nationality> AddAsync(Nationality entity)
        {
            _db.Nationality.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Nationality> UpdateAsync(Nationality entity)
        {
            _db.Nationality.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Nationality> DeleteAsync(int id)
        {
            var entity = await _db.Nationality.FindAsync(id);
            if (entity == null) return null;

            _db.Nationality.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}


