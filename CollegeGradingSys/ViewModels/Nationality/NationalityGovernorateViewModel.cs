using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Nationality
{
    public class NationalityGovernorateViewModel
    {
        [Key]
        public int Id { get; set; }

        
        [StringLength(60, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول اسم المحافظة/المنطقة  من 2 - 60 حرفًا.")]
        [Display(Name = "المحافظة/المنطقة")]
        public string GovernorateName { get; set; }

        [Display(Name ="الدولة")]
        public int NationalityId { get; set; }

        
        public IList<SelectItemVM> Nationalities { get; set; }
    }
}
