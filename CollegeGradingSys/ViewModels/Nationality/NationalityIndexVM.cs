using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Nationality
{
    public class NationalityIndexVM
    {
        public int Id { get; set; }

        [Display(Name = "الدولة")]
        public string CountryName { get; set; }

        [Display(Name = "الجنسية")]
        public string NationalityName { get; set; }
    }
}
