using CollegeGradingSys.Models;
using CollegeGradingSys.ViewModels;
using CollegeGradingSys.ViewModels.Batch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IBatchService : IGenericService<Batch>
    {
        Task<Batch> CreateAsync(BatchCreateDataVM dto);
        Task UpdateBatchAsync(BatchEditVM vm);
        Task DeleteAsync(int id);
        Task<IList<Batch>> GetBatchesAsync(int? specializationId);
        Task<bool> IsBatchNameExistsAsync(string batchName);
    }
}
