using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels.AcademicYear
{
    public class AcademicYearListViewModel
    {
        public required List<AcademicYearSelectItemVM> Items { get; set; }

        public int SelectedYearId { get; set; }
    }

}
