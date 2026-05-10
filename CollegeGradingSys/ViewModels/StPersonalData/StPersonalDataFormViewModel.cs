using CollegeGradingSys.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollegeGradingSys.ViewModels.StPersonalData
{
    // لعمليات الإضافة والتعديل (Create/Edit)
    public class StPersonalDataFormViewModel
    {
        [Display(Name = "رقم القيد")]
        [Required(ErrorMessage = "رقم القيد مطلوب")]
        public int? AcademicID { get; set; }

        [Display(Name = "الاسم الطالب")]
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string StName { get; set; }

        [Display(Name = "رقم الهوية")]
        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        public string IdentificatioNO { get; set; }

        [Display(Name = "الجنس")]
        public Sex Sex { get; set; }

        [Display(Name = "الجنسية")]
        public int NationalityID { get; set; }

        [Display(Name = "مكان الميلاد (الدولة)")]
        public int BirthcountryID { get; set; }

        [Display(Name = "مكان الميلاد (المحافظة)")]
        public int BirthGovernorateId { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1);

        public int EnrollmentYearId { get; set; }

        // القوائم المنسدلة (لتجنب ViewBag)
        public IEnumerable<SelectListItem> Nationalities { get; set; }
        public IEnumerable<SelectListItem> Governorates { get; set; }
        public IEnumerable<SelectListItem> AcademicYears { get; set; }
    }
}
