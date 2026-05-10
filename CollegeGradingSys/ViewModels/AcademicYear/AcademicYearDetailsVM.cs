using System;

namespace CollegeGradingSys.ViewModels.AcademicYear
{
    public class AcademicYearDetailsVM
    {
        public int Id { get; set; }
        public string AcademicYearName { get; set; }
        public DateTime AcademicYearStart { get; set; }
        public DateTime AcademicYearEnd { get; set; }
        public bool IsClosed { get; set; }
        public bool IsCurrentYear { get; set; }
    }

}
