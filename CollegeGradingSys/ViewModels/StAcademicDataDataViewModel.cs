using CollegeGradingSys.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{

    public class StAcademicDataDataViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم القيد")]
        public int AcademicID { get; set; }
        
        [Display(Name = "الاسم الطالب")]
        public string StName { get; set; }
        public bool IsCanRegisterInCurrentYear { get; set; }
        public ICollection<StAcademicData>  StAcademicDatas { get; set; }

       
    }
}
