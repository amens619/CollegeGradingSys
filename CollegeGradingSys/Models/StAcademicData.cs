using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StAcademicData
    {

        public int Id { get; set; }
        
       
        public Specialization Specialization { get; set; }
        public Level StLevel { get; set; }
        public StStatus StStatus { get; set; }

       
        public StudentBatch  StudentBatch { get; set; }

       
       
        public AcademicYear AcademicYear { get; set; }
        public StPersonalData StPersonalData { get; set; }
    }
}
