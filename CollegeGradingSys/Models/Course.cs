using System;
using System.Collections.Generic;
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
        public Specialization  Specialization { get; set; }
        public ICollection<SubCourse>  SubCourses { get; set; }


    }
}
