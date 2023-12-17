
using CollegeGradingSys.Helper;
using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class CreateCourseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "المستوى")]
        public Level Level { get; set; }

        [Display(Name = "الجنس المخصص للمادة")]
        public Course_sGender Course_sGender { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term Term { get; set; }

        [Required(ErrorMessage = "يجب إدخال اسم المادة بطول  40 حرفًا على الاكثر.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = " يجب إدخال اسم المادة بطول  40 حرفًا على الاكثر.")]
        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = " الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.")]
        [Display(Name = "الكبرى")]
        public string BigGrade { get; set; }

        [Required(ErrorMessage = " الرجاء إدخال  الدرجة الكبرى رقماً صحيحا بين 0 - 100.")]            
        [Display(Name = "الصغرى")]
        public string SmallGrade { get; set; }

        [Display(Name = "ملاحظة")]
        public string Note { get; set; }
        [Display(Name = "المادة الاساسية")]
        public int? ParentId { get; set; }

        [Display(Name = "نوع المادة(فرعية/اساسية)")]
        public bool IsSubCourse { get; set; }

        [Display(Name = "التخصص")]
        public int SpecializationId { get; set; }
       
        public virtual Specialization Specialization { get; set; }

       

    }
}
