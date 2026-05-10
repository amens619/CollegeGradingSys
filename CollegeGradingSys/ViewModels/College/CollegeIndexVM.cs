using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.College
{
    public class CollegeIndexVM
    {
        public int Id { get; set; }
       
        [Display(Name = "الكلية")]
        public string CollegeName { get; set; }
     
    }
}
