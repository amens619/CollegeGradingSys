using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StudentBatch
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "اسم الدفعة")]
        public string StudentBatchName { get; set; }

        
        [StringLength(60)]
        [Display(Name = "ملاحظة")]
        public string Note { get; set; }

        public virtual Specialization  Specialization { get; set; }
        public virtual AcademicYear  AcademicYear { get; set; }
        public virtual ICollection<StAcademicData> StAcademicDatas { get; set; }
    }
}
