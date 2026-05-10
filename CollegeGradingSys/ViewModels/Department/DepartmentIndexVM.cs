using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Department
{
    public class DepartmentIndexVM
    {
        public int Id { get; set; }
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }

        [Display(Name = "الكلية")]
        public string CollegeName { get; set; }
    }
}
