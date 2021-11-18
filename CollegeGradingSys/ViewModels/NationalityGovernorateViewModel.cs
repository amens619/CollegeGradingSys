using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class NationalityGovernorateViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "المحافظة/المنطقة")]
        public string GovernorateName { get; set; }


        public int NationalityId { get; set; }

        public ICollection<Nationality>  Nationalities { get; set; }
    }
}
