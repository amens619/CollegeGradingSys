using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class BatchCreateData
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول اسم الدفعة  من 3 - 30 حرفًا.")]
        [Display(Name = "اسم الدفعة")]
        public string BatchName { get; set; }

        [Display(Name = "التخصص")]
        public int SpecializationId { get; set; }

        [StringLength(60)]
        [Display(Name = "ملاحظة")]
        public string Note { get; set; }

        public virtual Specialization  Specialization { get; set; }

       

       
        
    }
}
