using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Adminstration
{
    public class AdminResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Display(Name = "اسم المستخدم / الإيميل")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال كلمة المرور الجديدة")]
        [StringLength(100, ErrorMessage = "يجب أن تتكون كلمة المرور من {2} أحرف على الأقل.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين.")]
        public string ConfirmPassword { get; set; }
    }
}