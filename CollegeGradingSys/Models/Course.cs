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
        [Display(Name = "الكبرى")]
        public int BigGrade { get; set; }
        [Display(Name = "الصغرى")]
        public int SmallGrade { get; set; }
        [Display(Name = "ملاحظة")]
        public string Note { get; set; }

        public bool IsTwoCourse { get; set; }

        
        public virtual Specialization Specialization { get; set; }

        public virtual ICollection<SubCourse> SubCourses { get; set; }
        public virtual ICollection<CourseGrade> CourseGrades  { get; set; }



    }
}
