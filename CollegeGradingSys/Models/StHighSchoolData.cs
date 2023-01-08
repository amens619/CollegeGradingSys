using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{

    public enum CertificateType
    {
        علمي, أدبي
    }
    public class StHighSchoolData
    {
        [Key]
        [ForeignKey("StPersonalData")]
        public int AcademicID { get; set; }

        [Display(Name = "نوع الشهادة")]
        public CertificateType CertificateType { get; set; }
        [Display(Name = "المعدل (النسبة)")]
        public float Average { get; set; }
        [Display(Name = "المصدر")]
        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "الرجاء ادخال المصدر بطول بين  2-60 حرف .")]
        public string Source { get; set; }
        [Display(Name = "رقم الجلوس ")]
        public int SeatNo { get; set; }
        [Display(Name = "سنة الشهادة")]
        public int CertificateYear { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم الثانوية أطول من 40 حرفًا.")]
        [Display(Name = "اسم الثانوية")]
        public string HighSchoolName { get; set; }

        [Display(Name = "ملاحظة")]        
        [StringLength(60, ErrorMessage = "لا يمكن أن يكون الملاحظة أطول من 60 حرفًا.")]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string Note { get; set; }      

       
        public virtual StPersonalData StPersonalData { get; set; }

    }
}
