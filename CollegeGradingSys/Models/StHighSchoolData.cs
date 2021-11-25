using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StHighSchoolData
    {
        public int Id { get; set; }
        public string CertificateType { get; set; }
        public int Average { get; set; }
        public string Source { get; set; }
        public string SeatNo { get; set; }
        public int CertificateYear { get; set; }
        public string HighSchoolName { get; set; }

        public string Note { get; set; }


        public int AcademicID { get; set; }

        [ForeignKey(nameof(AcademicID))]
        public StPersonalData StPersonalData { get; set; }

    }
}
