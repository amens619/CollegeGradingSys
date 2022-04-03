using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels
{
    public class AcademicYearVM
    {
        public bool IsCurrentYearClosed { get; set; }
        public ICollection<AcademicYear> AcademicYears { get; set; }
    }
}
