using CollegeGradingSys.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Department
{
    public class DepartmentVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم القسم")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم القسم من 3 - 30 حرفًا.")]
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "الرجاء تحديد الكلية التابع لها القسم")]
        [Display(Name = "الكلية")]
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public IEnumerable<SelectListItem> CollegesList { get; set; }
        //public ICollection<Specialization> Specializations { get; set; }
    }
}
