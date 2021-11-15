using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StAcademicData
    {
        public StPersonalData StPersonalData { get; set; }
        
        public College College { get; set; }
       
        public Specialization Specialization { get; set; }
        public Level StLevel { get; set; }
        public StStatus StStatus { get; set; }

        public int StudentBatchId { get; set; }
        public StudentBatch  StudentBatch { get; set; }

        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }
}
