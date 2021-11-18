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










    public void Add(Nationality entity)
    {  
        db.Nationality.Add(entity);
            SaveChange();
    }

    public void Delete(int id)
    {
        var nationality = Find(id);
            db.Nationality.Remove(nationality);
            SaveChange();
    }

    public Nationality Find(int id)
    {
        return db.Nationality.SingleOrDefault(a => a.Id == id);
    }

    public IList<Nationality> List()
    {
        return db.Nationality.ToList();
    }

    public void Update(int id, Nationality newNationality)
    {
            db.Nationality.Update(newNationality);
            SaveChange();
    }

        private void SaveChange()
        {
            db.SaveChanges();
        }
}
}

    


