using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering; // ضروري للقوائم المنسدلة
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.CourseGrade
{
    public class AllbatchCourseGradeViewModel
    {
        // ==========================
        // 1. فلاتر البحث (IDs) - الخصائص الناقصة للفلترة
        // ==========================
        [Display(Name = "العام الدراسي")]
        public int? AcademicYearId { get; set; }

        [Display(Name = "الدفعة")]
        public int? BatchId { get; set; }

        [Display(Name = "المادة")]
        public int? CourseId { get; set; }

        [Display(Name = "حالة الطالب للمادة")]
        public StStatusForCourse? StStatusForCourse { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }

        [Display(Name = "المستوى")]
        public Level? Level { get; set; }

        [Display(Name = "نوع الفصل (تكميلي / عام )")]
        public bool CourseType { get; set; }

        // ==========================
        // 2. بيانات العرض (Display Data)
        // ==========================
        public bool IsCurrentYear { get; set; }

        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; }

        // يفضل استخدام ViewModel بسيط للمادة بدلاً من Entity كامل، لكن يمكن إبقاؤه للعرض فقط
        public Models.Course Course { get; set; }

        // ==========================
        // 3. قوائم الاختيار (SelectLists) - بديل ViewBag
        // ==========================
        public SelectList AcademicYearsList { get; set; }
        public SelectList BatchesList { get; set; }
        public SelectList CoursesList { get; set; }

        // ==========================
        // 4. قائمة البيانات (Data List)
        // ==========================
        // يفضل استخدام CourseGradeVM بدلاً من CourseGrade Entity لفصل الطبقات
        public List<CourseGradeVM> CourseGrades { get; set; } = new List<CourseGradeVM>();      
    }   
}