using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CollegeGradingSys.ViewModels
{
    public class LoginViewModel
    {       
        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني")]
        [EmailAddress]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب ادخال كلمة المرور")]
        [DataType(DataType.Password)]
        [Display(Name = " كلمة المرور")]
        public string Password { get; set; }


        [Display(Name = "تذكر كلمة المرور")]
        public bool RememberMe { get; set; }
       
    }
}
