using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class SubCourse
    {
        public int Id { get; set; }
        public string SubCourseName { get; set; }
        public int BigMark { get; set; }
        public int SmallMark { get; set; }
        public string Note { get; set; }

        public int CourseIdfk { get; set; }
        public Course  Course { get; set; }
        
    }
}
