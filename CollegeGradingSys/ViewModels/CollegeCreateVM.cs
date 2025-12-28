using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels
{
    public class CollegeCreateVM
    {
        [Required(ErrorMessage = "الرجاء كتابة اسم الكلية")]
        [StringLength(60)]
        public string CollegeName { get; set; }
    }
}
