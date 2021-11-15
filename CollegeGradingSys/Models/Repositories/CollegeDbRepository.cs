using CollegeGradingSys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CollegeDbRepository: ICollegeGradingSysRepository<College>
    {
        private readonly ApplicationDbContext db;

        public CollegeDbRepository(ApplicationDbContext _db)
    {
            db = _db;
        }










    public void Add(College entity)
    {  
        db.College.Add(entity);
            SaveChange();
    }

    public void Delete(int id)
    {
        var college = Find(id);
            db.College.Remove(college);
            SaveChange();
    }

    public College Find(int id)
    {
        return db.College.SingleOrDefault(a => a.Id == id);
    }

    public IList<College> List()
    {
        return db.College.ToList();
    }

    public void Update(int id, College newCollege)
    {
            db.College.Update(newCollege);
            SaveChange();
    }

        private void SaveChange()
        {
            db.SaveChanges();
        }
}
}

    


