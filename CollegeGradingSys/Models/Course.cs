using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Course
    {
        public int Id { get; set; }
        public Level Level { get; set; }
        public Term Term { get; set; }

        public string CourseName { get; set; }

        public bool IsTwoCourse { get; set; }

        
        public virtual Specialization Specialization { get; set; }

        public virtual ICollection<SubCourse> SubCourses { get; set; }
        public virtual ICollection<CourseGrade> CourseGrade  { get; set; }



    }
}
