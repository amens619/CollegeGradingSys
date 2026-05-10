using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
// CreateStAcademicDataDataViewModel.cs


namespace CollegeGradingSys.ViewModels.StAcademic
{
    public class CreateStAcademicDataDataViewModel
    {
        // ================== بيانات الطالب (للعرض) ==================
        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Display(Name = "اسم الطالب")]
        public string StName { get; set; }

        // ================== بيانات السجل السابق (للعرض فقط) ==================
        [Display(Name = "العام الدراسي السابق")]
        public string preAcademicYear { get; set; }

        [Display(Name = "التخصص السابق")]
        public string preSpecialization { get; set; }

        [Display(Name = "المستوى السابق")]
        public string preLevel { get; set; }

        [Display(Name = "حالة الطالب السابقة")]
        public string preStStatus { get; set; }

        [Display(Name = "التقدير السابق")]
        public string preValuation { get; set; }

        [Display(Name = "المعدل التراكمي السابق")]
        public string preGPA { get; set; }

        // ================== بيانات السجل الحالي (للإدخال) ==================
        public int Id { get; set; }

        [Required(ErrorMessage = "حقل المستوى مطلوب")]
        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }

        [Display(Name = "نظام الدراسة")]
        public StudyType StudyType { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

        [Display(Name = "المعدل")]
        public string Average { get; set; } // String لتسهيل التعامل مع الإدخال

        [Display(Name = "المعدل التراكمي")]
        public string GPA { get; set; }

        [Display(Name = "التقدير")]
        public Valuation Valuation { get; set; }

        [Required(ErrorMessage = "يجب اختيار الدفعة")]
        [Display(Name = "الدفعة")]
        public int BatchId { get; set; }

        [Required(ErrorMessage = "يجب اختيار العام الدراسي")]
        [Display(Name = "العام الدراسي")]
        public int AcademicYearId { get; set; }

        // ================== القوائم المنسدلة (بديل ViewBag) ==================
        public SelectList AcademicYearsList { get; set; }
        public SelectList BatchesList { get; set; }
    }
}
