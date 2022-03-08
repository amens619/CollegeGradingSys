using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class CourseGradeIndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم القيد")]
        public int? AcademicID { get; set; }

        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }



        //==========================
        [Display(Name = "حالة الطالب")]
        public string StStatus { get; set; }
        [Display(Name = "الدفعة")]
        public string Batch { get; set; }
        [Display(Name = "التخصص")]
        public string Specialization { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public string Term { get; set; }

        [Display(Name = "المستوى")]
        public string Level { get; set; }
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }

        [Display(Name = "العام الاكاديمي")]
        public string AcademicYear { get; set; }

        [Display(Name = "نوع الفصل")]
        public string CourseType { get; set; }

        public ICollection<CourseGrade>  CourseGrades { get; set; }


    }
}
