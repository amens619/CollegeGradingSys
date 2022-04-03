using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StAcademicDataVM
    {
       
        public int Id { get; set; }

        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

        [Display(Name = "نظام الدراسة")]
        public StudyType StudyType { get; set; }


        [Display(Name = "المعدل")]
        public float? Average { get; set; }
        [Display(Name = "المعدل التراكمي")]
        public float? GPA { get; set; }
        [Display(Name = "التقدير")]
        public Valuation Valuation { get; set; }
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }

        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Batch Batch { get; set; }

        public virtual StPersonalData StPersonalData { get; set; }
        public virtual ICollection<CourseGrade> CourseGrades { get; set; }

        public bool IsSelected { get; set; }


      
    }
}
