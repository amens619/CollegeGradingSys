using CollegeGradingSys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels
{
    public class EditeUserViewModel
    {
        public EditeUserViewModel()
        {
            Claims = new List<UserClaim>();
            Roles = new List<string>();
        }

        public string Id { get; set; }


        [Required(ErrorMessage = "يجب ادخال البريد الالكتروني")]
        [EmailAddress]       
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }

        public List<UserClaim> Claims { get; set; }
        public List<string> Roles { get; set; }
    }
}
