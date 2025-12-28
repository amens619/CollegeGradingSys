using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace CollegeGradingSys.Services.Implementations
{
    public class StAcademicDataService : IStAcademicDataService
    {
        private readonly IRepository<StAcademicData> _repository;

        public StAcademicDataService(IRepository<StAcademicData> repository)
        {
            _repository = repository;
        }

        public async Task<IList<StAcademicData>> GetAllAsync()
        {
            return await _repository.ListAsync();
        }

        public async Task<StAcademicData> GetByIdAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task<IList<StAcademicData>> GetFilteredAsync(
            int? academicYearId,
            int? batchId,
            string studentName,
            StStatus? stStatus,
            Term? term,
            StudyType? studyType)
        {
            var query = _repository.Query();

            if (academicYearId.HasValue)
                query = query.Where(x => x.AcademicYear.Id == academicYearId);

            if (batchId.HasValue && batchId != -1)
                query = query.Where(x => x.Batch.Id == batchId);

            if (!string.IsNullOrWhiteSpace(studentName))
                query = query.Where(x => x.StPersonalData.StName.Contains(studentName));

            if (stStatus.HasValue)
                query = query.Where(x => x.StStatus == stStatus);

            if (term.HasValue)
                query = query.Where(x => x.Term == term);

            if (studyType.HasValue)
                query = query.Where(x => x.StudyType == studyType);

            return await query.ToListAsync();
        }

        public async Task<StAcademicData> AddAsync(StAcademicData entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<StAcademicData> UpdateAsync(StAcademicData entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<StAcademicData> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
