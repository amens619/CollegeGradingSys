using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class RegisterViewModel
    {
      
        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني")]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب ادخال كلمة المرور")]
        [DataType(DataType.Password)]
        [Display(Name = " كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }


       
    }
}
