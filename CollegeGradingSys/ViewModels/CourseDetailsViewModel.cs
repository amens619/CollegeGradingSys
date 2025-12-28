using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; }

        [Display(Name = "المستوى")]
        public Level Level { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term Term { get; set; }

        [Display(Name = "الدرجة الكبرى")]
        public int BigGrade { get; set; }

        [Display(Name = "الدرجة الصغرى")]
        public int SmallGrade { get; set; }

        [Display(Name = "الجنس المخصص للمادة")]
        public Course_sGender Course_sGender { get; set; }

        [Display(Name = "نوع المادة (فرعية/اساسية)")]
        public bool IsSubCourse { get; set; }

        [Display(Name = "المادة الاساسية")]
        public string ParentCourseName { get; set; }

        [Display(Name = "التخصص")]
        public string SpecializationName { get; set; }

        [Display(Name = "ملاحظة")]
        public string Note { get; set; }
    }
}

