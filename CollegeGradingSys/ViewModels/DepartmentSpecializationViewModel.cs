using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class DepartmentSpecializationViewModel
    {
        [Key]
        public int Id { get; set; }

        
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم التخصص  من 3 - 30 حرفًا.")]
        [Display(Name = "التخصص")]
        public string SpecializationName { get; set; }

        [Display(Name = "القسم")]
        public int DepartmentId { get; set; }
        public ICollection<Department>  Departments { get; set; }

    }
}
