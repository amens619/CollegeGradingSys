using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class AcademicYear
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]              
        [Display(Name = "تاريخ بداية العام")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcademicYearStart { get; set; }

        [DataType(DataType.Date)]
        [Required]        
        [Display(Name = "تاريخ نهاية العام")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcademicYearEnd { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول العام الدراسي  من 3 - 30 حرفًا.")]
        [Display(Name = "العام الدراسي")]
        public string AcademicYearName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول العام الدراسي بالهجري من 3 - 30 حرفًا.")]
        [Display(Name = " العام الدراسي بالهجري")]
        public string AcademicYearNameH { get; set; }

        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }

        [MaxLength(200)]
        [Display(Name = "ملاحظة")]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string Note { get; set; }

        public virtual ICollection<StAcademicData>  StAcademicDatas { get; set; }

    }
}
