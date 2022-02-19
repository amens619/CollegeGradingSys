using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CourseGradeDbRepository : ICollegeGradingSysRepository<CourseGrade>
    {
        private readonly ApplicationDbContext db;

        public CourseGradeDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }



        public CourseGrade Add(CourseGrade entity)
        {
            db.CourseGrade.Add(entity);
            SaveChange();
            return entity;
        }

        public CourseGrade Delete(int id)
        {
            var course = Find(id);
            if (course != null)
            {
                db.CourseGrade.Remove(course);
                SaveChange();
            }
            return course;
        }

        public CourseGrade Find(int id)
        {
            return db.CourseGrade.SingleOrDefault(a => a.Id == id);
        }

        public IList<CourseGrade> List()
        {
            return db.CourseGrade.ToList();
        }

        public CourseGrade Update(int id, CourseGrade newCourseGrade)
        {
            
            var course = db.CourseGrade.Attach(newCourseGrade);
            course.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newCourseGrade;
           
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






