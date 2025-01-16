using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class PrintGraduatStatementVM
    {
       
        public string PrintConfEnrollDate { get; set; }

      
        public string PrintConfEnrollDateH { get; set; }

        
        public string StName { get; set; }

        public  string Birthcountry { get; set; }

        public string BirthDate { get; set; }
        
        public string AcademicID { get; set; }

       
        public string Nationality { get; set; }
       
        public string StLevel { get; set; }

        public string GraduationYear { get; set; }

        public string GraduationYearH { get; set; }
        public string AcademicYearName { get; set; }

        public StPersonalData StPersonalData { get; set; }


        [Display(Name = "رئيس قسم شئون الطلاب")]
        public string StDepartmentHead { get; set; }

    }
}
