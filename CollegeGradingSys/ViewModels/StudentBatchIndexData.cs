using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class StudentBatchIndexData
    {
        [Display(Name = "العام الدراسي")]
        public int? AcademicYearId { get; set; }          

        public ICollection<AcademicYear> AcademicYears { get; set; }
        public ICollection<StudentBatch> StudentBatches { get; set; }
    }
}
