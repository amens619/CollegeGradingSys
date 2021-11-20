using CollegeGradingSys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class NationalityDbRepository : ICollegeGradingSysRepository<Nationality>
    {
        private readonly ApplicationDbContext db;

        public NationalityDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public Nationality Add(Nationality entity)
        {
            db.Nationality.Add(entity);
            SaveChange();
            return entity;
        }

        public Nationality Delete(int id)
        {
            var nationality = Find(id);
            if (nationality != null)
            {
                db.Nationality.Remove(nationality);
                SaveChange();
            }
            return nationality;
        }

        public Nationality Find(int id)
        {
            return db.Nationality.SingleOrDefault(a => a.Id == id);
        }

        public IList<Nationality> List()
        {
            return db.Nationality.ToList();
        }

        public Nationality Update(int id, Nationality newNationality)
        {
            var college = db.Nationality.Attach(newNationality);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newNationality;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    


