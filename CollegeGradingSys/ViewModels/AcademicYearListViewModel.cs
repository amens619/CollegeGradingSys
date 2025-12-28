using System.Collections.Generic;

namespace CollegeGradingSys.ViewModels
{
    public class AcademicYearListViewModel
    {
        public required List<AcademicYearSelectItemVM> Items { get; set; }

        public int SelectedYearId { get; set; }
    }

}
