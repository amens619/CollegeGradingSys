using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StPersonalDataViewModel
    {
        [Key]
        [Required]
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

       
        [Display(Name = "الجنسية")]
        public int NationalityId { get; set; }
        public ICollection<Nationality>  Nationalities { get; set; }


        [Required]
        [Display(Name = "مكان الميلاد (الدولة ) ")]
        public int BirthPlaceId { get; set; }
       

        [Display(Name = "مكان الميلاد (المحافظة ) ")]
        public int GovernorateId { get; set; }
        public ICollection<Governorate>  Governorates { get; set; }

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




        public StHighSchoolData StHighSchoolData { get; set; }



        public ICollection<StAcademicData> StAcademicDatas { get; set; }
    }
}
