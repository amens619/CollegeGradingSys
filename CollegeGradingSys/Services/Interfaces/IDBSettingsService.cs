using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IDBSettingsService
    {
        Task<IList<DBSettings>> GetAllAsync();
        Task<DBSettings> GetByIdAsync(int id);
        Task<DBSettings> CreateAsync(DBSettings settings);
        Task<DBSettings> UpdateAsync(DBSettings settings);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
