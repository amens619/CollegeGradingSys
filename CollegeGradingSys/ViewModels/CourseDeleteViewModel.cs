using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels
{
    public class CourseDeleteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "اسم المادة")]
        public string CourseName { get; set; }

        public string ErrorMessage { get; set; }
    }
}

