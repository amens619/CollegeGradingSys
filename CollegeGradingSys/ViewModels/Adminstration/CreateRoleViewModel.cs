using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Adminstration
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "يجب ادخال الدور او المسؤولية")]
        [Display(Name = "الدور او المسؤولية")]
        public string RoleName { get; set; }
    }
}
