using CollegeGradingSys.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CourseDbRepository : ICollegeGradingSysRepository<Course>
    {
        private readonly ApplicationDbContext db;

        public CourseDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }



        public Course Add(Course entity)
        {
            db.Course.Add(entity);
            SaveChange();
            return entity;
        }

        public Course Delete(int id)
        {
            var course = Find(id);
            if (course != null)
            {
                db.Course.Remove(course);
                SaveChange();
            }
            return course;
        }

        public Course Find(int id)
        {
            return db.Course
                .Include(x => x.Specialization)
                .Include(x => x.Parent)
                .SingleOrDefault(a => a.Id == id);
        }

        public IList<Course> List()
        {
            return db.Course.Include(x => x.Specialization)
                 .Include(x => x.Parent)
                 .ToList();
        }

        public Course Update(int id, Course newCourse)
        {
            
            var course = db.Course.Attach(newCourse);
            course.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            SaveChange();
            return newCourse;
           
        }

        private void SaveChange()
        {
            db.SaveChanges();
        }
    }
}

    






