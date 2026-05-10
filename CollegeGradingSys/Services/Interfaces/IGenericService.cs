using CollegeGradingSys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Interfaces
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<List<SelectItemVM>> GetSelectItemsAsync(
             Expression<Func<TEntity, SelectItemVM>> selector,
             string placeholder = "-- اختر --");

        Task<List<SelectItemVM>> GetSelectItemsAsync(
           Expression<Func<TEntity, SelectItemVM>> selector);
    }
}
