//using cloudscribe.Pagination.Models;
//using CollegeGradingSys.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CollegeGradingSys.ViewModels
//{
//    public class StHighSchoolDataVM
//    {
//        [Display(Name = "رقم القيد")]
//        public int AcademicID { get; set; }

//        [Display(Name = "نوع الشهادة")]
//        public CertificateType CertificateType { get; set; }

//        [Required(ErrorMessage = " الرجاء إدخال المعدل رقماً  بين 0 - 100.")]
//        [Display(Name = "المعدل (النسبة)")]
//        public string Average { get; set; }




//        [Display(Name = "المصدر")]
//        [Required(ErrorMessage = " الرجاء إدخال المصدر .")]
//        [StringLength(60, MinimumLength = 2, ErrorMessage = "الرجاء ادخال المصدر بطول بين  2-60 حرف .")]
//        public string Source { get; set; }

//        [Required(ErrorMessage = " الرجاء ادخال رقم الجلوس .")]
//        [Display(Name = "رقم الجلوس ")]
//        public string SeatNo { get; set; }

//        [Required(ErrorMessage = "الرجاء إدخال سنة الشهادة.")]
//        [Display(Name = "سنة الشهادة")]
//        public string CertificateYear { get; set; }


//        [Required(ErrorMessage = "الرجاء إدخال اسم الثانوية.")]
//        [StringLength(40, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم الثانوية أطول من 40 حرفًا.")]
//        [Display(Name = "اسم الثانوية")]
//        public string HighSchoolName { get; set; }

//        [Display(Name = "ملاحظة")]
//        [StringLength(60, ErrorMessage = "لا يمكن أن يكون الملاحظة أطول من 60 حرفًا.")]
//        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
//        public string Note { get; set; }


//        public virtual Models.StPersonalData StPersonalData { get; set; }

//    }
//}
using CollegeGradingSys.Models;
using System;
using System.ComponentModel.DataAnnotations;
// تم إزالة الاستدعاءات غير الضرورية لتخفيف الكلاس

namespace CollegeGradingSys.ViewModels.StHighSchoolData
{
    public class StHighSchoolDataVM
    {
        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Display(Name = "نوع الشهادة")]
        [Required(ErrorMessage = "الرجاء تحديد نوع الشهادة.")]
        public CertificateType CertificateType { get; set; } // افترضت أن CertificateType هو Enum

        // 💡 التعديل 1: استخدام float? مع [Range]
        [Required(ErrorMessage = "الرجاء إدخال المعدل.")]
        [Range(0.0, 100.0, ErrorMessage = "الرجاء إدخال المعدل رقماً بين 0 - 100.")]
        [Display(Name = "المعدل (النسبة)")]
        public float? Average { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال المصدر.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "الرجاء إدخال المصدر بطول بين 2-60 حرف.")]
        [Display(Name = "المصدر")]
        public string Source { get; set; } = string.Empty;

        // 💡 التعديل 2: استخدام int? 
        [Required(ErrorMessage = "الرجاء إدخال رقم الجلوس.")]
        [Display(Name = "رقم الجلوس")]
        public int? SeatNo { get; set; }

        // 💡 التعديل 3: استخدام int? مع إضافة نطاق منطقي للسنوات
        [Required(ErrorMessage = "الرجاء إدخال سنة الشهادة.")]
        [Range(1950, 2100, ErrorMessage = "الرجاء إدخال سنة صحيحة (مثال: 2023).")]
        [Display(Name = "سنة الشهادة")]
        public int? CertificateYear { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم الثانوية.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "يجب أن يكون اسم الثانوية من 3 إلى 40 حرفًا.")]
        [Display(Name = "اسم الثانوية")]
        public string HighSchoolName { get; set; }

        [Display(Name = "ملاحظة")]
        [StringLength(60, ErrorMessage = "لا يمكن أن تكون الملاحظة أطول من 60 حرفًا.")]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string Note { get; set; }

      
    }
}