using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class CourseIndexViewModel
    {
        [Display(Name = "التخصص")]
        public int? SpecializationId { get; set; }
        public Term? Term { get; set; }
        public Level? Level { get; set; }
       
        public IList<Course>  Courses { get; set; }


    }
}
