using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class Batch
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول اسم الدفعة  من 3 - 30 حرفًا.")]
        [Display(Name = "اسم الدفعة")]
        public string BatchName { get; set; }

        
        [StringLength(60)]
        [Display(Name = "ملاحظة")]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string Note { get; set; }

        public virtual Specialization  Specialization { get; set; }
        public virtual ICollection<StAcademicData>   StAcademicDatas  { get; set; }
       
    }
}
