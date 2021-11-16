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

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "التخصص")]
        public string SpecializationName { get; set; }

        [Display(Name = "القسم")]
        public int DepartmentId { get; set; }
        public ICollection<Department>  Departments { get; set; }

    }
}
