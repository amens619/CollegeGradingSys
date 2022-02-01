using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StPersonalDataVM
    {
        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }

        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }  
       
        public bool IsSelected { get; set; }


        [DisplayFormat(NullDisplayText = "لا توجد بيانات أكاديمة")]
        public StAcademicData StAcademicData { get; set; }
    }
}
