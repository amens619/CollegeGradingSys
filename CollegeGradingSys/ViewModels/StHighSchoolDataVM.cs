using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StHighSchoolDataVM
    {
        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Display(Name = "نوع الشهادة")]
        public CertificateType CertificateType { get; set; }

        [Required(ErrorMessage = " الرجاء إدخال المعدل رقماً  بين 0 - 100.")]
        [Display(Name = "المعدل (النسبة)")]
        public string Average { get; set; }

        


        [Display(Name = "المصدر")]
        [Required(ErrorMessage = " الرجاء إدخال المصدر .")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "الرجاء ادخال المصدر بطول بين  2-60 حرف .")]
        public string Source { get; set; }

        [Required(ErrorMessage = " الرجاء ادخال رقم الجلوس .")]
        [Display(Name = "رقم الجلوس ")]
        public string SeatNo { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال سنة الشهادة.")]
        [Display(Name = "سنة الشهادة")]
        public string CertificateYear { get; set; }


        [Required(ErrorMessage = "الرجاء إدخال اسم الثانوية.")]
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
