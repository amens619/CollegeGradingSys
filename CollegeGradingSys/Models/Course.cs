using CollegeGradingSys.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Course
    {
        public int Id { get; set; }
       
        [Display(Name = "المستوى")]
        public Level Level { get; set; }
        
        [Display(Name = "الفصل الدراسي")]
        public Term Term { get; set; }

        [Required]
        [Display(Name = "اسم المادة")]
        [StringLength(50, MinimumLength = 2)]
        public string CourseName { get; set; }
        [Display(Name = "الدرجة الكبرى")]
        public int BigGrade { get; set; }

        [Required] 
        [Display(Name = "الدرجة الصغرى")]
        public int SmallGrade { get; set; }

        [Display(Name = "ملاحظة")]
        [StringLength(100)]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string Note { get; set; }


        public Course_sGender Course_sGender { get; set; }

        [Display(Name = "المادة الاساسية")]
        public int? ParentId { get; set; }

        [Display(Name = "نوع المادة (فرعية/اساسية)")]      
        public bool IsSubCourse { get; set; }

        public virtual Course Parent { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Course> SubCourses { get; set; }
        public virtual ICollection<CourseGrade> CourseGrades  { get; set; }



    }
}
