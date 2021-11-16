using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class CollegeDepartmentViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }

        [Display(Name = "الكلية")]
        public int CollegeId { get; set; }
        public ICollection<College> Colleges { get; set; }

        public ICollection<Specialization> Specializations { get; set; }
    }
}
