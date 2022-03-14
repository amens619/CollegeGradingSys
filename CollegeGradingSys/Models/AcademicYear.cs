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

        [DataType(DataType.Date, ErrorMessage = "يجب أن يكون طول اسم الكلية  من 3 - 30 حرفًا.")]              
        [Display(Name = "تاريخ بداية العام")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcademicYearStart { get; set; }

        [DataType(DataType.Date)]
        [Required]        
        [Display(Name = "تاريخ نهاية العام")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcademicYearEnd { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "العام الدراسي")]
        public string AcademicYearName { get; set; }       
      
        [MaxLength(200)]
        [Display(Name = "ملاحظة")]
        [DisplayFormat(NullDisplayText = "لا توجد ملاحظات")]
        public string Note { get; set; }

        public virtual ICollection<StAcademicData>  StAcademicDatas { get; set; }

    }
}
