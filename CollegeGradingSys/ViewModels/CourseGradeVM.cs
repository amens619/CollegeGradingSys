using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class CourseGradeVM
    {
        public int Id { get; set; }
        public int AcademicID { get; set; }
       
        public string StName { get; set; }

       
       
        public float? Grade { get; set; }

       
        public StStatusForCourse StStatusForCourse { get; set; }
        public bool IsGradeChange { get; set; }

        public virtual Course Course { get; set; }
        public string Note { get; set; }
    }
}
