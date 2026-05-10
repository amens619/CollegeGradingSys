using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني")]
        [EmailAddress]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }
    }
}
