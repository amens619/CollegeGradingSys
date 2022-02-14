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

        [Display(Name = "الفصل الدراسي")]
        public Term Term { get; set; }

               
        [StringLength(40, MinimumLength = 3, ErrorMessage = " يجب إدخال اسم المادة بطول  40 حرفًا على الاكثر.")]
        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; }
        [Display(Name = "الكبرى")]
        public int BigGrade { get; set; }

        [Required]
        [Display(Name = "الصغرى")]
        public int SmallGrade { get; set; }

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
