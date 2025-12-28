using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IList<TEntity>> ListAsync();
        Task<TEntity> FindAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(int id);

        IQueryable<TEntity> Query();
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
    }
}
