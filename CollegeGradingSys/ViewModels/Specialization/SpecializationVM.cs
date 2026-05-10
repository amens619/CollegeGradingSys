using CollegeGradingSys.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Specialization
{
    public class SpecializationVM
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم القسم")]       
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم التخصص  من 3 - 30 حرفًا.")]
        [Display(Name = "التخصص")]
        public string SpecializationName { get; set; }

        [Required(ErrorMessage = "الرجاء تحديد القسم التابع لها التخصص")]
        [Display(Name = "القسم")]
        public int DepartmentId { get; set; }
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }
        public IEnumerable<SelectListItem> DepartmentsList { get; set; }

    }
}
