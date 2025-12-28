using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IBatchService 
    {
        // CRUD عام
        Task<Batch?> GetByIdAsync(int id);
        Task<IList<Batch>> GetAllAsync();       
       

        Task<Batch> CreateAsync(BatchCreateDataVM dto);
        Task UpdateBatchAsync(BatchEditVM vm);

        Task DeleteAsync(int id);
        Task<IList<Batch>> GetBatchesAsync(int? specializationId);

        Task<bool> IsBatchNameExistsAsync(string batchName);

        Task<List<BatchSelectItemVM>> GetBatchsSelectItemAsync(string placeholder );
    }
}
