using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class HomeIndexVM
    {
        [Display(Name = "العام الدراسي")]
        public string AcademicYearName { get; set; }

        public int StudentsNO { get; set; }

    }
}
