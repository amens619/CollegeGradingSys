using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StudentBatchDBRepository : ICollegeGradingSysRepository<StudentBatch>
    {
        private readonly ApplicationDbContext db;

        public StudentBatchDBRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public StudentBatch Add(StudentBatch entity)
        {
            db.StudentBatch.Add(entity);
            SaveChange();
            return entity;
        }

        public StudentBatch Delete(int id)
        {
            StudentBatch studentBatch = Find(id);
            if (studentBatch != null)
            {
                db.StudentBatch.Remove(studentBatch);
                SaveChange();
            }
            return studentBatch;
        }

        public StudentBatch Find(int id)
        {
            return db.StudentBatch.Include(a => a.AcademicYear).SingleOrDefault(a => a.Id == id);
        }

        public IList<StudentBatch> List()
        {
            return db.StudentBatch.Include(a=> a.AcademicYear).ToList();
        }

        public StudentBatch Update(int id, StudentBatch newStudentBatch)
        {
            
            var college = db.StudentBatch.Attach(newStudentBatch);
            college.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newStudentBatch;
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






