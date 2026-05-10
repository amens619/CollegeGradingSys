using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.ViewModels.CourseGrade;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Batch
{
    public class AllbatchCourseGradeFailedViewModel    {
        

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }
        [Display(Name = "المستوى")]
        public Level? Level { get; set; }
        [Display(Name = "المادة")]
        public int? CourseId { get; set; }

        [Display(Name = "التخصص")]
        public int? SpecializationId { get; set; }

        [Display(Name = "العام")]
        public int? AcademicYearId { get; set; }
        public string courseName { get; set; }
        //==========================       

        // ==========================
        // حقول البحث
        // ==========================
        public string SearchString { get; set; }
        public int? SearchAcademicID { get; set; }

        public bool IsSelectCurrentYear { get; set; }
        public bool isExportBtnEnable { get; set; }
        //===

        // ==========================
        // القوائم المنسدلة 
        // ==========================
        public SelectList AcademicYearsList { get; set; }
        public SelectList SpecializationsList { get; set; }
        public SelectList CourseList { get; set; }


        // ==========================
        // البيانات (VM بدلاً من Entity)
        // ==========================
        public List<CourseGradeVM> CourseGrades { get; set; } = new List<CourseGradeVM>();


    }
}
