using CollegeGradingSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StAcademicDataIndexViewModel
    {

        public int id { get; set; }
        public StStatus?  StStatus { get; set; }
        public Term? Term { get; set; }
        public Level? Level { get; set; }

        [Display(Name = "نظام الدراسة")]
        public StudyType? StudyType { get; set; }
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }

        public bool IsStEnrollmentClosed { get; set; }
        public bool IsCurrentYearClosed { get; set; }
        public IList<StAcademicDataVM> StAcademicDataVMs { get; set; }


    }
}
