using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class StAcademicDataDbRepository : IStAcademicDataRepository
    {
        private readonly ApplicationDbContext _db;

        public StAcademicDataDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ================= Query =================
        public IQueryable<StAcademicData> Query()
        {
            return _db.StAcademicData
                .AsNoTracking()
                .Include(x => x.Batch)
                    .ThenInclude(y => y.Specialization)
                        .ThenInclude(z => z.Department)
                .Include(x => x.AcademicYear)
                .Include(x => x.StPersonalData)
                .Include(x => x.CourseGrades)
                    .ThenInclude(y => y.Course);
        }

        // ================= Read =================
        public async Task<IList<StAcademicData>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<StAcademicData> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.StAcademicData.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.StAcademicData.CountAsync();
        }

        // ================= Write =================
        public async Task<StAcademicData> AddAsync(StAcademicData entity)
        {
            _db.StAcademicData.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<StAcademicData> UpdateAsync(StAcademicData entity)
        {
            _db.StAcademicData.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<StAcademicData> DeleteAsync(int id)
        {
            var entity = await _db.StAcademicData.FindAsync(id);
            if (entity == null) return null;

            _db.StAcademicData.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
             

        public IQueryable<StAcademicData> GetFilteredQuery(
            int? academicYearId,
            int? batchId,
            string nameSearch,
            StStatus? status,
            Term? term,
            StudyType? studyType)
        {
            var query = Query();

            if (academicYearId.HasValue)
                query = query.Where(x => x.AcademicYear.Id == academicYearId);

            if (batchId.HasValue && batchId != -1)
                query = query.Where(x => x.Batch.Id == batchId);

            if (!string.IsNullOrWhiteSpace(nameSearch))
                query = query.Where(x => x.StPersonalData.StName.Contains(nameSearch));

            if (status.HasValue)
                query = query.Where(x => x.StStatus == status);

            if (term.HasValue)
                query = query.Where(x => x.Term == term);

            if (studyType.HasValue)
                query = query.Where(x => x.StudyType == studyType);

            return query; 
        }

    }
}




