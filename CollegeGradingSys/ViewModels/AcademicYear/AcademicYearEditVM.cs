using System;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.AcademicYear
{
    public class AcademicYearEditVM
    {
        public int Id { get; set; }

        [Display(Name = "تاريخ بداية العام")]
        [Required(ErrorMessage = "الرجاء إدخال تاريخ بداية العام")]
        [DataType(DataType.Date)]
        public DateTime AcademicYearStart { get; set; }

        [Display(Name = "تاريخ نهاية العام")]
        [Required(ErrorMessage = "الرجاء إدخال تاريخ نهاية العام")]
        [DataType(DataType.Date)]
        public DateTime AcademicYearEnd { get; set; }

        [Display(Name = "العام الجامعي")]
        [Required(ErrorMessage = "الرجاء إدخال العام الجامعي")]
        [StringLength(50)]
        public string AcademicYearName { get; set; } = string.Empty;

        [Display(Name = "العام الجامعي (هجري)")]
        [Required(ErrorMessage = "الرجاء إدخال العام الجامعي الهجري")]
        [StringLength(50)]
        public string AcademicYearNameH { get; set; } = string.Empty;

        [Display(Name = "ملاحظات")]
        [StringLength(500)]
        public string Note { get; set; }

        /// <summary>
        /// يستخدم فقط لمنع تعديل العام الحالي
        /// وليس للتعديل من الواجهة
        /// </summary>
        public bool IsCurrentYear { get; set; }
        public bool IsClosed { get; set; }
    }
}
