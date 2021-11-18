using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Governorate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        [Display(Name = "المحافظة/المنطقة")]
        public string GovernorateName { get; set; }


        public Nationality  Nationality { get; set; }

        public ICollection<District> Districts { get; set; }
    }
}
