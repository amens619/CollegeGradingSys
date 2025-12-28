using CollegeGradingSys.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels
{
    public class BatchEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم الدفعة")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول اسم الدفعة من 2 إلى 30 حرفًا")]
        public string BatchName { get; set; }

        [Required]
        [Display(Name = "التخصص")]
        public int SpecializationId { get; set; }

        [StringLength(60)]
        [Display(Name = "ملاحظة")]
        public string Note { get; set; }

        public IList<Specialization>? Specializations { get; set; }
    }
}
