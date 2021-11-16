using CollegeGradingSys.Data;
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










        public void Add(Department entity)
        {
            db.Department.Add(entity);
            SaveChange();
        }

        public void Delete(int id)
        {
            var department = Find(id);
            db.Department.Remove(department);
            SaveChange();
        }

        public Department Find(int id)
        {
            return db.Department.SingleOrDefault(a => a.Id == id);
        }

        public IList<Department> List()
        {
            return db.Department.ToList();
        }

        public void Update(int id, Department newDepartment)
        {
            db.Department.Update(newDepartment);
            SaveChange();
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






