using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class AcademicYearDbRepository : ICollegeGradingSysRepository<AcademicYear>
    {
        private readonly ApplicationDbContext db;

        public AcademicYearDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public AcademicYear Add(AcademicYear entity)
        {
            db.AcademicYear.Add(entity);
            SaveChange();
            return entity;
        }

        public AcademicYear Delete(int id)
        {
            AcademicYear city = Find(id);
            if (city != null)
            {
                db.AcademicYear.Remove(city);
                SaveChange();
            }
            return city;
        }

        public AcademicYear Find(int id)
        {
            return db.AcademicYear.SingleOrDefault(a => a.Id == id);
        }

        public IList<AcademicYear> List()
        {
            return db.AcademicYear.ToList();
        }

        public AcademicYear Update(int id, AcademicYear newAcademicYear)
        {
            
            var college = db.AcademicYear.Attach(newAcademicYear);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newAcademicYear;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






