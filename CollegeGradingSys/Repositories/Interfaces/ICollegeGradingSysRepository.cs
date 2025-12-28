using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface ICollegeGradingSysRepository<TEntity>
    {
        IList<TEntity> List();
        TEntity Find(int id);
        TEntity Add(TEntity entity);
        TEntity Update(int id, TEntity entity);
        TEntity Delete(int id);
    }

  
}
