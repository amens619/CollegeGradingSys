using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "المديرية/المنطقة")]
        public string DistrictName { get; set; }


        public virtual Governorate Governorate { get; set; }

        public virtual ICollection<City> Cities  { get; set; }
    }
}
