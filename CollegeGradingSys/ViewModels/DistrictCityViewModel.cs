using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class DistrictCityViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم المدينة أطول من 60 حرفًا واصغر من 3 حرفًا.")]
        [Display(Name = "المدينة")]
        public string CityName { get; set; }

        [Display(Name = "المديرية")]
        public int DistrictId { get; set; }
        public IList<SelectItemVM> DistrictsSelectItems { get; set; }
    }
}
