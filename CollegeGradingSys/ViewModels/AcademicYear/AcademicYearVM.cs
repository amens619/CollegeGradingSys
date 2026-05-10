using cloudscribe.Pagination.Models;
using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.ViewModels.AcademicYear
{
    public class AcademicYearVM
    {
        public bool IsCurrentYearClosed { get; set; }

        public List<AcademicYearSelectItemVM> AcademicYears { get; set; } = new();

        public int SelectedAcademicYearId { get; set; }
    }
}
