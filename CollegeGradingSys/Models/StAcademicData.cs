using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public enum Valuation
    {
        ممتاز ,جيدجدا ,جيد ,مقبول, ضعيف
    }
    public class StAcademicData
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term?  Term { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

        [Display(Name = "المعدل")]
        public float Average { get; set; }
        [Display(Name = "المعدل التراكمي")]
        public float GPA { get; set; }
        [Display(Name = "التقدير")]
        public Valuation Valuation { get; set; }

        public bool IsCurrentYear { get; set; }

        public virtual StudentBatch  StudentBatch { get; set; }       
       
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual StPersonalData StPersonalData { get; set; }
    }
}
