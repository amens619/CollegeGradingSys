using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Services.Implementations
{
    public class GenericService<TEntity> : IGenericService<TEntity>
        where TEntity : class
    {
        protected readonly IRepository<TEntity> Repository;

        public GenericService(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await Repository.ListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Repository.FindAsync(id);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            return await Repository.AddAsync(entity);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Repository.UpdateAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            if (!await Repository.ExistsAsync(id))
                return false;

            await Repository.DeleteAsync(id);
            return true;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await Repository.ExistsAsync(id);
        }

        public virtual async Task<int> CountAsync()
        {
            return await Repository.CountAsync();
        }

        public async Task<List<SelectItemVM>> GetSelectItemsAsync(
            Func<TEntity, SelectItemVM> selector,
            string placeholder = "-- اختر --")
            {
                var items = await Repository.ListAsync();

                var list = items.Select(selector).ToList();

                list.Insert(0, new SelectItemVM { Id = -1, Name = placeholder });

                return list;
            }

    }
}
