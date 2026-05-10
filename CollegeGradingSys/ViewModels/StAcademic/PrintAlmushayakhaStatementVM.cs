using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.StAcademic
{
    public class PrintAlmushayakhaStatementVM
    {
       
        public string PrintConfEnrollDate { get; set; }

        public string StName { get; set; }

        public  string Birthcountry { get; set; }

        public string BirthDate { get; set; }
        
        public string AcademicID { get; set; }

       
        public string Nationality { get; set; }
       
        public string CollegeName { get; set; }

        public string DepartmentName { get; set; }       

        public string GraduationYear { get; set; }

        public string GraduationYearH { get; set; }
      

        public Models.StPersonalData StPersonalData { get; set; }


        [Display(Name = "رئيس قسم شئون الطلاب")]
        public string StDepartmentHead { get; set; }


        [Display(Name = "رئيس فرع الجامعة بحضرموت")]
        public string BranchHeadName { get; set; }

    }
}
