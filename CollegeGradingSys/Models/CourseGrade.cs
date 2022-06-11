using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class CourseGrade
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "نوع المادة")]
        public bool CourseType { get; set; }

        [Display(Name = "العلامة")]
        [DisplayFormat(NullDisplayText = "لم يتم ادخال العلامة")]
        public float? Grade { get; set; }
       
        [Display(Name = "حالة الطالب للمادة")]
        public  StStatusForCourse StStatusForCourse { get; set; }


        public virtual StAcademicData StAcademicData { get; set; }
         public virtual Course Course { get; set; }


    }
}
