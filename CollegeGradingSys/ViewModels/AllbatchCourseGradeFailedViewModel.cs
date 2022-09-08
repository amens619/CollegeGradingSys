using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class AllbatchCourseGradeFailedViewModel    {
        

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }
        [Display(Name = "المستوى")]
        public Level? Level { get; set; }

        [Display(Name = "التخصص")]
        public int SpecializationId { get; set; }

        [Display(Name = "العام")]
        public int AcademicYearId { get; set; }
        public string courseName { get; set; }
        //==========================       

        //===
        public bool IsSelectCurrentYear { get; set; }
        public bool isExportBtnEnable { get; set; }
        //===


        public ICollection<CourseGrade>  CourseGrades { get; set; }


    }
}
