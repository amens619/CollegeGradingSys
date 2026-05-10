using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.College
{
    public class CollegeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء كتابة اسم الكلية")]
        [StringLength(60)]
        [Display(Name = "الكلية")]
        public string CollegeName { get; set; }
    }
}
