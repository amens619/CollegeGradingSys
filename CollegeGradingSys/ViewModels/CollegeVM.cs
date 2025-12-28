using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels
{
    public class CollegeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء كتابة اسم الكلية")]
        [StringLength(60)]
        public string CollegeName { get; set; }
    }
}
