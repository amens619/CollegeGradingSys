using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class DBSettingsService : IDBSettingsService
    {
        private readonly IRepository<DBSettings> _dbSettingsRepository;

        public DBSettingsService(IRepository<DBSettings> dbSettingsRepository)
        {
            _dbSettingsRepository = dbSettingsRepository;
        }

        public async Task<IList<DBSettings>> GetAllAsync()
        {
            return await _dbSettingsRepository.ListAsync();
        }

        public async Task<DBSettings> GetByIdAsync(int id)
        {
            return await _dbSettingsRepository.FindAsync(id);
        }

        public async Task<DBSettings> CreateAsync(DBSettings settings)
        {
            return await _dbSettingsRepository.AddAsync(settings);
        }

        public async Task<DBSettings> UpdateAsync(DBSettings settings)
        {
            return await _dbSettingsRepository.UpdateAsync(settings);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _dbSettingsRepository.ExistsAsync(id))
                return false;

            await _dbSettingsRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSettingsRepository.ExistsAsync(id);
        }
    }
}
