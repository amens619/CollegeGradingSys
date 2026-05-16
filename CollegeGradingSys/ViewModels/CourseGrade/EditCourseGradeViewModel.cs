using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.CourseGrade
{
    public class EditCourseGradeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "اسم الطالب")]
        public string StudentName { get; set; } // للعرض فقط

        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; } // للعرض فقط

        [Display(Name = "الدرجة الكبرى")]
        public int BigGrade { get; set; } // للعرض والتحقق

        [Display(Name = "الدرجة الصغرى")]
        public int SmallGrade { get; set; } // للعرض

        [Display(Name = "نوع المادة")]
        public bool CourseType { get; set; } // (أساسية/تكميلية)

        // البيانات القابلة للتعديل
        [Display(Name = "العلامة")]
        // 💡 حذفنا [Range] لأن الدرجة الكبرى متغيرة (BigGrade) ولأن النوع String
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "يرجى إدخال رقم صحيح أو عشري صحيح (مثال: 95.5)")]
        public string Grade { get; set; }

        [Display(Name = "حالة الطالب للمادة")]
        public StStatusForCourse StStatusForCourse { get; set; }

        //=================================

        [Display(Name = "المادة الاساسية")]
        public int? ParentId { get; set; }

        [Display(Name = "نوع المادة(فرعية/اساسية)")]
        public bool IsSubCourse { get; set; }

       

        


       
   



    }
}
