using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class DepartmentDbRepository : ICollegeGradingSysRepository<Department>
    {
        private readonly ApplicationDbContext db;

        public DepartmentDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }



        public Department Add(Department entity)
        {
            db.Department.Add(entity);
            SaveChange();
            return entity;
        }

        public Department Delete(int id)
        {
            var department = Find(id);
            if (department != null)
            {
                db.Department.Remove(department);
                SaveChange();
            }
            return department;
        }

        public Department Find(int id)
        {
            return db.Department.Include(a => a.College).SingleOrDefault(a => a.Id == id);
        }

        public IList<Department> List()
        {
            return db.Department.Include(a => a.College).ToList();
        }

        public Department Update(int id, Department newDepartment)
        {
            
            var department = db.Department.Attach(newDepartment);
            department.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newDepartment;
           
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






