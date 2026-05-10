using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Specialization
{
    public class SpecializationIndexVM
    {
        public int Id { get; set; }
        [Display(Name = "التخصص")]
        public string SpecializationName { get; set; }
        [Display(Name = "القسم")]
        public string DepartmentName { get; set; }
    }
}
