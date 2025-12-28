using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class EditCourseGradeViewModel
    {
        public int Id { get; set; }              
        
        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; }
        [Display(Name = "الكبرى")]
        public int BigGrade { get; set; }

        [Required]
        [Display(Name = "الصغرى")]
        public int SmallGrade { get; set; }

        [Display(Name = "نوع المادة (عام /تكميلي )")]
        public string CourseType { get; set; }

        [Display(Name = "المادة الاساسية")]
        public int? ParentId { get; set; }

        [Display(Name = "نوع المادة(فرعية/اساسية)")]
        public bool IsSubCourse { get; set; }

        [Display(Name = "العلامة")]
        //[ModelBinder(BinderType = typeof(DecimalModelBinder))]
        public string Grade { get; set; }

        [Display(Name = "حالة الطالب للمادة")]
        public StStatusForCourse StStatusForCourse { get; set; }







    }
}
