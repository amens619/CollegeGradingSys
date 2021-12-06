using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "لا يمكن أن يكون اسم المدينة أطول من 60 حرفًا.")]
        [Display(Name = "المدينة")]
        public string CityName { get; set; }

        public District District { get; set; }
    }
}
