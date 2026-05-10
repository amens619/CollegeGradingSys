using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Course
{
    public class CourseIndexViewModel
    {
        [Display(Name = "التخصص")]
        public int? SpecializationId { get; set; }
        public Term? Term { get; set; }
        public Level? Level { get; set; }
        public SelectList SpecializationsList { get; set; }
        public IList<Models.Course>  Courses { get; set; }


    }
}
