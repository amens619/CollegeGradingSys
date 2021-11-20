using CollegeGradingSys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CollegeDbRepository : ICollegeGradingSysRepository<College>
    {
        private readonly ApplicationDbContext db;

        public CollegeDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public College Add(College entity)
        {
            db.College.Add(entity);
            SaveChange();
            return entity;
        }

        public College Delete(int id)
        {
            College college = Find(id);
            if (college != null)
            {
                db.College.Remove(college);
                SaveChange();
            }
            return college;
        }

        public College Find(int id)
        {
            return db.College.SingleOrDefault(a => a.Id == id);
        }

        public IList<College> List()
        {
            return db.College.ToList();
        }

        public College Update(int id, College newCollege)
        {
            var college = db.College.Attach(newCollege);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newCollege;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    


