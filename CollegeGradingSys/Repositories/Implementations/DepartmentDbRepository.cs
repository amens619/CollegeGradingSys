//using CollegeGradingSys.Data;
//using CollegeGradingSys.Models;
//using CollegeGradingSys.Repositories.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CollegeGradingSys.Repositories.Implementations
//{
//    public class DepartmentDbRepository : ICollegeGradingSysRepository<Department>
//    {
//        private readonly ApplicationDbContext db;

//        public DepartmentDbRepository(ApplicationDbContext _db)
//        {
//            db = _db;
//        }



//        public Department Add(Department entity)
//        {
//            db.Department.Add(entity);
//            SaveChange();
//            return entity;
//        }

//        public Department Delete(int id)
//        {
//            var department = Find(id);
//            if (department != null)
//            {
//                db.Department.Remove(department);
//                SaveChange();
//            }
//            return department;
//        }

//        public Department Find(int id)
//        {
//            return db.Department.Include(a => a.College).SingleOrDefault(a => a.Id == id);
//        }

//        public IList<Department> List()
//        {
//            return db.Department.Include(a => a.College).ToList();
//        }

//        public Department Update(int id, Department newDepartment)
//        {

//            var department = db.Department.Attach(newDepartment);
//            department.State = EntityState.Modified;
//            SaveChange();
//            return newDepartment;

//        }

//        private void SaveChange()
//        {
//            db.SaveChanges();
//        }
//    }
//}

using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class DepartmentDbRepository : IRepository<Department>
    {
        private readonly ApplicationDbContext _db;

        public DepartmentDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========= Query =========
        public IQueryable<Department> Query()
        {
            return _db.Department
                .Include(d => d.College)
                .AsQueryable();
        }

        // ========= Read =========
        public async Task<IList<Department>> ListAsync()
        {
            return await Query().ToListAsync();
        }

        public async Task<Department> FindAsync(int id)
        {
            return await Query().SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Department.AnyAsync(d => d.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Department.CountAsync();
        }

        // ========= Write =========
        public async Task<Department> AddAsync(Department entity)
        {
            _db.Department.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Department> UpdateAsync(Department entity)
        {
            _db.Department.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Department> DeleteAsync(int id)
        {
            var entity = await _db.Department.FindAsync(id);
            if (entity == null) return null;

            _db.Department.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}







