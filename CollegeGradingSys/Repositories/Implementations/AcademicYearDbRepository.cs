using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using GemBox.Spreadsheet.Drawing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class AcademicYearDbRepository : IAcademicYearRepository
    {
        private readonly ApplicationDbContext _db;

        public AcademicYearDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // --------------------------------------------------------
        // BASE QUERY (Reusable Includes)
        // --------------------------------------------------------
        private IQueryable<AcademicYear> BaseQuery =>
            _db.AcademicYear
               .Include(x => x.StAcademicDatas)
                    .ThenInclude(y => y.Batch);

        // --------------------------------------------------------
        // LIST
        // --------------------------------------------------------
        public async Task<IList<AcademicYear>> ListAsync()
        {
            return await BaseQuery
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        // --------------------------------------------------------
        // FIND
        // --------------------------------------------------------
        public async Task<AcademicYear> FindAsync(int id)
        {
            return await BaseQuery
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // --------------------------------------------------------
        // ADD
        // --------------------------------------------------------
        public async Task<AcademicYear> AddAsync(AcademicYear entity)
        {
            await _db.AcademicYear.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        // --------------------------------------------------------
        // UPDATE
        // --------------------------------------------------------
        public async Task<AcademicYear> UpdateAsync(AcademicYear entity)
        {
            _db.AcademicYear.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        // --------------------------------------------------------
        // DELETE
        // --------------------------------------------------------
        public async Task<AcademicYear> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            if (entity == null)
                return null;

            _db.AcademicYear.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        // --------------------------------------------------------
        // EXTRA: EXISTS
        // --------------------------------------------------------
        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.AcademicYear
                .AnyAsync(x => x.Id == id);
        }

        // --------------------------------------------------------
        // EXTRA: COUNT
        // --------------------------------------------------------
        public async Task<int> CountAsync()
        {
            return await _db.AcademicYear.CountAsync();
        }

        // --------------------------------------------------------
        // EXTRA: QUERY
        // --------------------------------------------------------
        public IQueryable<AcademicYear> Query()
        {
            return BaseQuery;
        }

        // --------------------------------------------------------
        // CUSTOM: GET CURRENT YEAR
        // --------------------------------------------------------
        public async Task<AcademicYear> GetCurrentYearAsync()
        {
            return await _db.AcademicYear
                .FirstOrDefaultAsync(x => x.IsCurrentYear == true);
        }

        public async Task<IReadOnlyList<AcademicYear>> GetAllOrderedByStartDateAsync()
        {
            return await _db.AcademicYear
                .OrderByDescending(x => x.AcademicYearStart)
                .ToListAsync();
        }

       


    }
}
