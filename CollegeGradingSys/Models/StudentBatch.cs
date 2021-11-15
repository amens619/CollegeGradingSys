using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StudentBatch
    {
        public int Id { get; set; }
        public string StudentBatchName { get; set; }

        public ICollection<StAcademicData>  StAcademicDatas { get; set; }
        public string Note { get; set; }
    }
}
