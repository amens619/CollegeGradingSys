using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StudentBatchCreateData
    {
        public int Id { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 2)]
        [Display(Name = "اسم الدفعة")]
        public string StudentBatchName { get; set; }

        [Display(Name = "العام الدراسي")]
        public int AcademicYearId { get; set; }

        [StringLength(60)]
        [Display(Name = "ملاحظة")]
        public string Note { get; set; }

        public virtual AcademicYear AcademicYear { get; set; }

       

       
        
    }
}
