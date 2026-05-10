using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using CollegeGradingSys.ViewModels.Course;
using CollegeGradingSys.ViewModels.CourseGrade;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Batch
{
    public class BatchCourseGradeUploadVM
    {       
        [Display(Name = "حالة الطالب للمادة")]
        public StStatusForCourse? StStatusForCourse { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }
        [Display(Name = "المستوى")]
        public Level? Level { get; set; }

        public bool IsCurrentYear { get; set; }
        public string CourseName { get; set; }
        public CollegeGradingSys.Models.Course Course { get; set; } = null;
        //==========================      

        public IFormFile BatchGrades  { get; set; }

        [Display(Name = "نوع الفصل (تكميلي / عام )")]
        public bool CourseType { get; set; }




        public IList<CourseGradeVM> CourseGrades { get; set; }

        // Metadata
        public int BatchId { get; set; }
        public int CourseId { get; set; }
        //public string CourseName { get; set; }
        public int BigGrade { get; set; }

        // Upload
        public IFormFile BatchGradesFile { get; set; }

        // Preview Data (بعد قراءة الإكسل)
        public List<CourseGradeVM> PreviewGrades { get; set; } = new List<CourseGradeVM>();
        public string ErrorMessage { get; set; }
    }
}

