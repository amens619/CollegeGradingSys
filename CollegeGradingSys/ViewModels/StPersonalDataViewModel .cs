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
        
        [Display(Name = "رقم القيد")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول رقم القيد من 2 - 8 رقم.")]
        public string AcademicID { get; set; }

       
        [StringLength(100, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول اسم القسم  من 2 - 100 حرفًا.")]
        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }

       
        [StringLength(15, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول الرقم الوطني من 2 - 15 رقم.")]
        [Display(Name = "الرقم الوطني (الهوية )")]
        public string IdentificatioNO { get; set; }

        
        [Display(Name = "الجنس")]
        public Sex Sex { get; set; }

       
        [Display(Name = "الجنسية")]
        public int NationalityId { get; set; }
        public ICollection<Nationality>  Nationalities { get; set; }


        
        [Display(Name = "مكان الميلاد (الدولة ) ")]
        public int BirthPlaceId { get; set; }
       

        [Display(Name = "مكان الميلاد (المحافظة ) ")]
        public int GovernorateId { get; set; }
        public ICollection<Governorate>  Governorates { get; set; }

       
       
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        
        public int EnrollmentYearId { get; set; }

        [Display(Name = "سنة الالتحاق ميلادي")]
        public string EnrollmentYearM { get; set; }

        [Display(Name = "سنة الالتحاق هجري")]
        public string EnrollmentYearH { get; set; }

        public StHighSchoolData StHighSchoolData { get; set; }
        public ICollection<StAcademicData> StAcademicDatas { get; set; }

        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "الفصل الدراسي")]
        public Term? Term { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

        [Display(Name = "نظام الدراسة")]
        public StudyType StudyType { get; set; }

        [Display(Name = "المعدل")]
        public float? Average { get; set; }
        [Display(Name = "المعدل التراكمي")]
        public float? GPA { get; set; }
        [Display(Name = "التقدير")]
        public Valuation Valuation { get; set; }
        [Display(Name = "الفصل الحالي")]
        public bool IsCurrentYear { get; set; }
        [Display(Name = "الدفعة")]
        public int BatchId { get; set; }
        [Display(Name = "العام الدراسي")]
        public int AcademicYearId { get; set; }

       
    }
}
