using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class CityDbRepository : IRepository<City>
    {
        private readonly ApplicationDbContext _db;

        public CityDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IList<City>> ListAsync()
        {
            return await _db.City
                .Include(c => c.District)
                .ToListAsync();
        }

        public async Task<City> FindAsync(int id)
        {
            return await _db.City
                .Include(c => c.District)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<City> AddAsync(City entity)
        {
            await _db.City.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<City> UpdateAsync(City entity)
        {
            _db.City.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<City> DeleteAsync(int id)
        {
            var city = await FindAsync(id);
            if (city == null)
                return null;

            _db.City.Remove(city);
            await _db.SaveChangesAsync();
            return city;
        }

        public IQueryable<City> Query()
        {
            return _db.City.AsQueryable();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.City.AnyAsync(c => c.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.City.CountAsync();
        }
    }
}
