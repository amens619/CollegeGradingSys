using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StsBatchForTheYear
    {
        public int Id { get; set; }
        public virtual Batch  Batch { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual ICollection<StAcademicData> StAcademicDatas { get; set; }
    }
}
