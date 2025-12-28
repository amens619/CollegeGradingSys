using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class AllbatchCourseGradeViewModel
    {
        [Display(Name = "حالة الطالب للمادة")]
        public StStatusForCourse? StStatusForCourse { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }
        [Display(Name = "المستوى")]
        public Level? Level { get; set; }

        public bool IsCurrentYear { get; set; }
        public string courseName { get; set; }

        public Course  Course { get; set; }
        //==========================      



        [Display(Name = "نوع الفصل (تكميلي / عام )")]
        public bool CourseType { get; set; }
         

        

        public ICollection<CourseGrade>  CourseGrades { get; set; }


    }
}
