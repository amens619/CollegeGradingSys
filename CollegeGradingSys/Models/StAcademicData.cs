using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models
{
    public class StAcademicData
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "المستوى")]
        public Level StLevel { get; set; }

        [Display(Name = "حالة الطالب")]
        public StStatus StStatus { get; set; }

       
        public virtual StudentBatch  StudentBatch { get; set; }

        
       
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual StPersonalData StPersonalData { get; set; }
    }
}
