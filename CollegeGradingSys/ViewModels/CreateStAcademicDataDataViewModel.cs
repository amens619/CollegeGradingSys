using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{

    public class CreateStAcademicDataDataViewModel
    {
        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }
        //===============
        [Display(Name = "العام الدراسي")]
        public string preAcademicYear { get; set; }
        [Display(Name = "التخصص")]
        public string preSpecialization { get; set; }
        [Display(Name = "المستوى")]
        public string preLevel { get; set; }
        [Display(Name = "حالة الطالب")]
        public string preStStatus { get; set; }
        [Display(Name = "التقدير")]
        public string preValuation { get; set; }
        [Display(Name = "المعدل التراكمي")]
        public string preGPA { get; set; }

        //======================
        public int Id { get; set; }


        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }

        [Display(Name = "نظام الدراسة")]
        public StudyType StudyType { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

        [Display(Name = "المعدل")]
        public string Average { get; set; }
        [Display(Name = "المعدل التراكمي")]
        public string GPA { get; set; }
        [Display(Name = "التقدير")]
        public Valuation Valuation { get; set; }
        //[Display(Name = "الفصل الحالي")]
        //public bool IsCurrentYear { get; set; }
        [Display(Name = "الدفعة")]
        public int BatchId { get; set; }
        [Display(Name = "العام الدراسي")]
        public int AcademicYearId { get; set; }
        //public ICollection<Batch>  Batches { get; set; }
        //public ICollection<AcademicYear>  AcademicYears { get; set; }

    }
}
