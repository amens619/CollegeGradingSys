using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class AcademicYear
    {
        public int Id { get; set; }
        public DateTime AcademicYearStart { get; set; }
        public DateTime AcademicYearEnd { get; set; }
        public string AcademicYearName { get; set; }


        public ICollection<StAcademicData>  stAcademicDatas { get; set; }
        public string Note { get; set; }

    }
}
