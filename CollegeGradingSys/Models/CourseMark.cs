using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class CourseMark
    {
        public int Id { get; set; }
        public bool CourseType { get; set; }
        public StPersonalData StPersonalData { get; set; }
        public Course Course { get; set; }
        public int Mark { get; set; }
        public StStatusForCourse StStatusForCourse { get; set; }

    }
}
