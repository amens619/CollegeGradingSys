using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Governorate
{
    public class GovernorateDistrictViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "المديرية/المنطقة")]
        public string DistrictName { get; set; }

        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }
        public ICollection<Models.Governorate>  Governorates { get; set; }
    }
}
