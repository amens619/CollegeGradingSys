using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Nationality
{
    public class NationalityVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم الدولة")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم الدولة من 3 - 60 حرفًا.")]
        [Display(Name = "الدولة")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم الجنسية")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم الجنسية من 3 - 60 حرفًا.")]
        [Display(Name = "الجنسية")]
        public string NationalityName { get; set; }
    }
}
