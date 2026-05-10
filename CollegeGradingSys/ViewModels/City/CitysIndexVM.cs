using CollegeGradingSys.Models;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.City
{
    public class CityIndexVM
    {
        public int Id { get; set; }

        [Display(Name = "المدينة")]
        public string CityName { get; set; }

        public string DistrictName { get; set; }
    }
}
