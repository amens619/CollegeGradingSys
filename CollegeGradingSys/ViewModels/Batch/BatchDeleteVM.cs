using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.Batch
{
    public class BatchDeleteVM
    {
        public int Id { get; set; }
        [Display(Name = "اسم الدفعة")]
        public string BatchName { get; set; }
        public string SpecializationName { get; set; }
    }
}
