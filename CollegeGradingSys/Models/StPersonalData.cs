using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StPersonalData
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2)]
        [Display(Name = "الرقم الوطني (الهوية )")]
        public string IdentificatioNO { get; set; }

        [Required]        
        [Display(Name = "الجنس")]
        public Sex Sex { get; set; }

        public int NationalityID { get; set; }

        [Required]
        [Display(Name = "الجنسية")]
        [ForeignKey("NationalityID")]
        public virtual Nationality Nationality { get; set; }

        public int BirthcountryID { get; set; }
        [Required]
        [Display(Name = "مكان الميلاد (الدولة ) ")]
        [ForeignKey("BirthcountryID")]
        public virtual Nationality Birthcountry { get; set; }

        [Required]
        [Display(Name = "مكان الميلاد (المحافظة )")]
        public Governorate BirthGovernorate { get; set; }

        [Required]
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 2)]
        [Display(Name = "سنة الالتحاق ميلادي")]
        public string EnrollmentYearM { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 2)]
        [Display(Name = "سنة الالتحاق هجري")]
        public string EnrollmentYearH { get; set; }



        [DisplayFormat(NullDisplayText = "لا توجد بيانات")]
        public virtual StHighSchoolData  StHighSchoolData { get; set; }        
       
        public virtual ICollection<StAcademicData>  StAcademicDatas { get; set; }
    }
}
