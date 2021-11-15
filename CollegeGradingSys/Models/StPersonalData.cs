using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StPersonalData
    {
        public int AcademicID { get; set; }
        public string StName { get; set; }
        public string IdentificatioNO { get; set; }
        public Sex Sex { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public int EnrollmentYearM { get; set; }
        public int EnrollmentYearH { get; set; }

        public College College { get; set; }

        public StHighSchoolData  StHighSchoolData { get; set; }
        public Nationality Nationality { get; set; }
        public Governorate Governorate { get; set; }
    }
}
