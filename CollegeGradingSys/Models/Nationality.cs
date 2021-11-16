using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Nationality
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "اسم الدولة")]
        public string CountryName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "الجنسية")]
        public string NationalityName { get; set; }
    }
}
