using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class CourseRepository : ICollegeGradingSysRepository<Course>
    {
        IList<Course> courses;
        public CourseRepository()
        {
            courses = new List<Course>()
            {
                new Course { Id=1, Specialization=new Specialization { Id=1},  CourseName ="تفسير (101(", Level= Level.الأول, Term=Term.الأول},
                new Course { Id=1, Specialization=new Specialization { Id=1},  CourseName ="حديث (101(", Level=Level.الأول, Term=Term.الأول},
                 new Course { Id=1,Specialization=new Specialization { Id=1},  CourseName ="التزكية (101(",Level=Level.الأول,Term=Term.الأول}

            };
        }
        public Course Add(Course entity)
        {
            var college = courses.FirstOrDefault();
            entity.Id = college != null ? courses.Max(b => b.Id) + 1 : 1;    
            courses.Add(entity);
            return entity;
        }

        public Course Delete(int id)
        {
            var course = Find(id);
            if (course != null)
            {
                courses.Remove(course);
            }
            return course;
        }

        public Course Find(int id)
        {
           return courses.SingleOrDefault(a => a.Id == id);
        }

        public IList<Course> List()
        {
            return courses;
        }

        public Course Update(int id, Course newCourses)
        {
            var oldCourse = Find(id);
            if (oldCourse != null)
            {
                oldCourse.Level = newCourses.Level;
                oldCourse.CourseName = newCourses.CourseName;
                //oldCourse.SubCourses = newCourses.SubCourses;
                //foreach (var newSubCourse in newCourses.SubCourses)
                //{
                //    var oldSubCourse = oldCourse.SubCourses.SingleOrDefault(a => a.Id == newSubCourse.Id);
                //    oldSubCourse.SmallMark = newSubCourse.SmallMark;
                //    oldSubCourse.BigMark = newSubCourse.BigMark;
                //    oldSubCourse.Note = newSubCourse.Note;
                //}

                oldCourse.Specialization = newCourses.Specialization;
                oldCourse.Term = newCourses.Term;
            }
            return newCourses;

        }

       
    }
}
