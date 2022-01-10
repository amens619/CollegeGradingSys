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

        public int Id { get; set; }


        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

        [Display(Name = "المعدل")]
        public float? Average { get; set; }
        [Display(Name = "المعدل التراكمي")]
        public float? GPA { get; set; }
        [Display(Name = "التقدير")]
        public Valuation Valuation { get; set; }
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }
        [Display(Name = "الدفعة")]
        public int BatchId { get; set; }
        [Display(Name = "العام الدراسي")]
        public int AcademicYearId { get; set; }
        public ICollection<Batch>  Batches { get; set; }
        public ICollection<AcademicYear>  AcademicYears { get; set; }

    }
}
