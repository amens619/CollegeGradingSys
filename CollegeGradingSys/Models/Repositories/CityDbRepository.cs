using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CityDbRepository : ICollegeGradingSysRepository<City>
    {
        private readonly ApplicationDbContext db;

        public CityDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public City Add(City entity)
        {
            db.City.Add(entity);
            SaveChange();
            return entity;
        }

        public City Delete(int id)
        {
            City city = Find(id);
            if (city != null)
            {
                db.City.Remove(city);
                SaveChange();
            }
            return city;
        }

        public City Find(int id)
        {
            return db.City.Include(a => a.District).SingleOrDefault(a => a.Id == id);
        }

        public IList<City> List()
        {
            return db.City.Include(a=> a.District).ToList();
        }

        public City Update(int id, City newCity)
        {
            
            var college = db.City.Attach(newCity);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newCity;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






