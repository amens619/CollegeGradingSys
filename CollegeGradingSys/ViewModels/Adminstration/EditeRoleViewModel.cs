using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Adminstration
{
    public class EditeRoleViewModel
    {
        public EditeRoleViewModel()
        {
            Users = new List<string>();
        }
        [Display(Name = "الرقم")]
        public string Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال الدور او المسؤولية")]
        [Display(Name = "الدور او المسؤولية")]
        public string RoleName { get; set; }


        public List<string> Users { get; set; }
    }
}
